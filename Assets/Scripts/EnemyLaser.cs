using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
   [SerializeField] private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    void OnTriggerEnter2D (Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if(player != null) {
                player.Damage(1);
            }
            
            Destroy(this.gameObject);
        }
            
    }
}
