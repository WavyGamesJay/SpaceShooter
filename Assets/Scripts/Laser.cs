﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField] private float _speed = 8;
    [SerializeField] private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update() {
        if(_isEnemyLaser == false) {
            MoveUp();
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