using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 300f;
    Enemy enemy;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
       

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() {

        _rigidbody.velocity = transform.up * _speed;
        if (transform.position.y > 8) {
            //check if this object has a parent
            //destroy the parent too!
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            _target = other.GetComponent<GameObject>();

            if (_target != null) {
                //Check if Target is Alive
                if (enemy.GetIsAlive()) {
                    Vector2 direction = _target.transform.position - transform.position;
                    Debug.DrawRay(transform.position, direction, Color.green);
                    direction.Normalize();

                    float rotation = Vector3.Cross(direction, transform.up).z;

                    _rigidbody.angularVelocity = -rotation * _rotationSpeed;

                }


            }

            _rigidbody.velocity = transform.up * _speed;
        }
        

    }
}
