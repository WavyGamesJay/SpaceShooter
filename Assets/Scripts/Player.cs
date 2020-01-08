﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private float _canFire = -1f;

    [SerializeField] private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _uIManager;
    private AudioSource _audioSource;

    [SerializeField] private bool tripleShotActive = false;
    [SerializeField] private bool speedBoostActive = false;
    [SerializeField] private bool shieldActive = false;

    [SerializeField] private int _score;
    

    [SerializeField] GameObject _rightEngine;
    [SerializeField] GameObject _leftEngine;

    [SerializeField] private AudioClip _laserShotClip;
    


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if(_spawnManager == null) {
            Debug.LogError("Spawn Manager is NULL");
        }

        if(_uIManager == null) {
            Debug.LogError("UI Manager is NULL");
        }

        if(_audioSource == null) {
            Debug.LogError("Audio Source is NULL: " + this.gameObject.name);
        }
        else {
            _audioSource.clip = _laserShotClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLaser();
        
    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(speedBoostActive == false) {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        } 
        else {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_speed * _speedMultiplier) * Time.deltaTime);
        }
        

        //Clamp y axis movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11) {
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        } else if (transform.position.x < -11) {
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
    }

    void FireLaser() {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) {
            _canFire = Time.time + _fireRate;
            if (tripleShotActive) {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity); 
            }
            else {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            _audioSource.Play();
            
        }
    }

    public void Damage() {
        if(shieldActive == true) {
            shieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        _uIManager.UpdateLives(_lives);

        if(_lives == 2) {
            _leftEngine.SetActive(true);
        }
        else if(_lives == 1) {
            _rightEngine.SetActive(true);
        }

        if(_lives < 1) {
            _spawnManager.OnPlayerDeath(); 
            Destroy(this.gameObject);
            _uIManager.GameOver(true);
        }
    }

    public void ActivateTripleShot() {
        tripleShotActive = true;
        StartCoroutine(TSPowerDown()); 
    }

    public void ActivateSpeedBoost() {
        speedBoostActive = true;
        StartCoroutine(SBPowerDown());
    }

    public void ActivateShield() {
        shieldActive = true;
        _shieldVisualizer.SetActive(true); 
    }

    public void IncreaseScore(int points) {
        _score += points;
        _uIManager.UpdateScore(_score);
    }
    //Communicate with the UI to update score

    IEnumerator TSPowerDown() {
        yield return new WaitForSeconds(5f);
        tripleShotActive = false;

    }

    IEnumerator SBPowerDown() {
        yield return new WaitForSeconds(5f);
        speedBoostActive = false;
    }


}