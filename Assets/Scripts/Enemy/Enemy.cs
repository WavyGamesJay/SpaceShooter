using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [SerializeField] protected float _speed = 4f;
    [SerializeField] protected float _lives;

    protected Player _player;

    protected AudioSource _audioSource;
    protected bool _isAlive = true;



    public virtual void Init() {
        
        
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null) {
            Debug.LogError("Player is NULL");
        }

        if(_audioSource == null) {
            Debug.LogError("Audio Source is NULL: " + this.gameObject.name);
        }


    }

    private void Start() {
        Init();
        
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CalculateMovement();

    }

    public virtual void CalculateMovement() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f) {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }

    public bool GetIsAlive() {
        if (_isAlive) {
            return true;
        }
        else {
            return false;
        }
    }



    public abstract void OnTriggerEnter2D(Collider2D other);

}
