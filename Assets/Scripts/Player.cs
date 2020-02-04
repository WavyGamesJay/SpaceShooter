using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _thrusterSpeed = 8.75f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _homingMissilePrefab;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private float _shieldStrength;
    [SerializeField] private float _ammoCount = 15f;
    [SerializeField] private float _thrusterPower = 10f;
    [SerializeField] private bool _thrusterCooldown;

    [SerializeField] private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _uIManager;
    private AudioSource _audioSource;
    private CameraShake _cameraShake;

    [SerializeField] private bool tripleShotActive = false;
    [SerializeField] private bool homingMissileActive = false;
    [SerializeField] private bool speedBoostActive = false;
    [SerializeField] private bool shieldActive = false;
    [SerializeField] private bool speedDownActive = false;

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
        _cameraShake = Camera.main.GetComponent<CameraShake>();

        if(_spawnManager == null) {
            Debug.LogError("Spawn Manager is NULL");
        }

        if(_uIManager == null) {
            Debug.LogError("UI Manager is NULL");
        }

        if(_cameraShake == null) {
            Debug.LogError("Camera Shake is NULL");
        }
        if(_audioSource == null) {
            Debug.LogError("Audio Source is NULL: " + this.gameObject.name);
        }
        else {
            _audioSource.clip = _laserShotClip;
        }

        _thrusterCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLaser();
        _uIManager.UpdateThrusterUI(_thrusterPower);
        
        

    }

    void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(speedBoostActive == false && speedDownActive == false) {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        } 
        else if(speedDownActive == true) {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_speed / 2) * Time.deltaTime);
        }
        else {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_speed * _speedMultiplier) * Time.deltaTime);
        }

        //Thruster Logic
        //if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterCooldown == false) {
        //   _speed *= 1.75f;
        //}

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterCooldown == false) {
            _speed = _thrusterSpeed;
            _thrusterPower -= 2 * Time.deltaTime;
            
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            _speed = 5f;

        } else {
            _thrusterPower += 2 * Time.deltaTime;
        }

        if(_thrusterPower <= 0) {
            _thrusterCooldown = true;
            _speed = 5f;
            _uIManager.StartRefuel();
        }
        if(_thrusterPower >= 10f) {
            _thrusterCooldown = false;
            _uIManager.EndRefuel();
        }

       _thrusterPower = Mathf.Clamp(_thrusterPower, 0f, 10f);

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
            if(_ammoCount > 0) {
                _canFire = Time.time + _fireRate;
                if (tripleShotActive && !homingMissileActive) {
                    Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                    _ammoCount--;
                } else if (homingMissileActive) {
                    Instantiate(_homingMissilePrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                } else {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                    _ammoCount--;
                }
       
                _audioSource.Play();
                
            }

            _uIManager.UpdateAmmoCount(_ammoCount);

        }
    }

    public void Damage() {
        if(shieldActive == true) {
            _shieldStrength--;

            if(_shieldStrength <= 0f) {
                shieldActive = false;
                _shieldVisualizer.SetActive(false);
            }

            SpriteRenderer shield = _shieldVisualizer.GetComponent<SpriteRenderer>();
            if (_shieldStrength == 2) {
                shield.color = new Color(1f, 1f, 1f, .75f);
            }
            else if(_shieldStrength == 1) {
                shield.color = new Color(1f, 1f, 1f, .25f);
            }

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

        _cameraShake.CameraShaking();
    }

    public void RestoreHealth() {
        if(_lives < 3) {
            _lives++;
            _uIManager.UpdateLives(_lives);

            if (_lives == 2) {
                _rightEngine.SetActive(false);
            } else if (_lives == 3) {
                _leftEngine.SetActive(false);
            }
        }
       
    }

    public void RestoreAmmo() {
        _ammoCount = 15f;
        _uIManager.UpdateAmmoCount(_ammoCount);

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
        _shieldStrength = 3f;
        _shieldVisualizer.SetActive(true); 
    }

    public void IncreaseScore(int points) {
        _score += points;
        _uIManager.UpdateScore(_score);
    }

    public void ActiaveHomingMissile() {
        homingMissileActive = true;
        StartCoroutine(HMPowerDownRoutine());
    }
    
    public void SpeedDown() {
        speedDownActive = true;
        StartCoroutine(SpeedDownRoutine());
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

    IEnumerator HMPowerDownRoutine() {
        yield return new WaitForSeconds(5f);
        homingMissileActive = false;
    }

    IEnumerator SpeedDownRoutine() {
        yield return new WaitForSeconds(5f);
        speedDownActive = false;
    }


}
