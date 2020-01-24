using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField] private float _speed = 8;
    [SerializeField] private bool _isEnemyLaser = false;
    [SerializeField] private bool _isHomingLaser = false;
    Vector3 dir;
    private GameObject _homingTarget;

    private void Start() {
        if (_isHomingLaser) {
            _homingTarget = GameObject.FindGameObjectWithTag("Enemy");
        }
    }
    // Update is called once per frame
    void Update() {
        if(_isEnemyLaser == false) {
            if(_isHomingLaser == false) {
                MoveUp();
            } else {
                AcquireTarget();
            }
            
        }
        else {
            MoveDown();
        }
    }

    private void MoveUp() {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 8) {
            //check if this object has a parent
            //destroy the parent too!
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void AcquireTarget() {
        
        if (_homingTarget != null) {
            Debug.Log("Target Acquired !");
            Vector3.MoveTowards(transform.position, _homingTarget.transform.position, 20f); 

        } else {
            Debug.Log("Target not found");
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y > 8) {
                //check if this object has a parent
                //destroy the parent too!
                if (transform.parent != null) {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void MoveDown() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8) {
            //check if this object has a parent
            //destroy the parent too!
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser() {
        _isEnemyLaser = true; 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && _isEnemyLaser) {
            Player player = other.GetComponent<Player>();
            if(player != null) {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
