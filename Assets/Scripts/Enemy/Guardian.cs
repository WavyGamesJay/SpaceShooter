using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : Enemy
{
    [SerializeField] private bool _moveRight = true;
    [SerializeField] private GameObject _explosion;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void CalculateMovement() {
        base.CalculateMovement();
        if(_moveRight == true) {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            if(transform.position.x >= 9.45f) {
                _moveRight = false;
            }
        } else {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            if(transform.position.x <= -9.45f) {
                _moveRight = true;
            }
        }
       
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {

            if(_player != null) {
                if(_explosion != null) {
                    Instantiate(_explosion, transform.position, Quaternion.identity);

                }

                _player.Damage(3);
                _speed = 0f;
                _audioSource.Play();
                Destroy(other.gameObject);
                Destroy(this.gameObject, 0.2f);
                
            }
        }

        if(other.tag == "Laser") {
            Destroy(other.gameObject);
            _lives--;

        }

        if(_lives <= 0) {
            if(_explosion != null) {
                Instantiate(_explosion, transform.position, Quaternion.identity);
            }

            _player.IncreaseScore(30);

            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.2f);
            _isAlive = false;
        }

        _lives = Mathf.Clamp(_lives, 0, 4);
    }
}
