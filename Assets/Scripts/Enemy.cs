using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private GameObject _enemyLaser;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire;
    private bool _isAlive = true;



    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null) {
            Debug.LogError("Player is NULL");
        }

        if(_anim == null) {
            Debug.LogError("Animator is NULL");
        }

        if(_audioSource == null) {
            Debug.LogError("Audio Source is NULL: " + this.gameObject.name);
        }


    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire && _isAlive == true) { 
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            //Debug.Break();
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for(int i = 0; i < lasers.Length; i++ ) {
                lasers[i].AssignEnemyLaser();
            }


            
        }
    }

    void CalculateMovement() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f) {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }



    private void OnTriggerEnter2D(Collider2D other) {

        

        if (other.tag == "Player") {
            //Damage Player
            Player player = other.transform.GetComponent<Player>();

            if(player != null) {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            if(_player != null) {
                _player.IncreaseScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        _isAlive = false;

    }

}
