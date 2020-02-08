using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Enemy
{
    [SerializeField] private GameObject _enemyLaser;
    private Animator _anim;

    private float _fireRate = 3.0f;
    private float _canFire;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        _anim = GetComponent<Animator>();

        if(_anim == null) {
            Debug.LogError("Animator is NULL");
        }
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        FireLaser();
    }

    private void FireLaser() {
        if (Time.time > _canFire && _isAlive == true) {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            //Debug.Break();
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++) {
                lasers[i].AssignEnemyLaser();
            }



        }
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {

            if (_player != null) {
                _player.Damage(2);
                _lives--;
            }

        }

        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            _lives--;

            if (_player != null) {
                _player.IncreaseScore(10);
            }



        }

        if (_lives <= 0) {
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
            _isAlive = false;
        }
    }
}
