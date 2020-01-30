using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField] private float _speed = 3f;
    //0 = TripleShot 1 = Speed 2 = Shield
    [SerializeField] private int powerupID;
    [SerializeField] private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -6f ) {
            Destroy(this.gameObject);
        }

    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if(player != null) {
                switch (powerupID) {
                    // Common Powerups
                    case 0:
                        player.RestoreAmmo();
                        break;
                    //Rare Powerups
                    case 1:
                        player.ActivateTripleShot();
                        break;
                    case 2:
                        player.ActivateSpeedBoost();
                        break;
                    case 3:
                        player.ActivateShield();
                        break;
                    //Rare Powerups
                    case 4:
                        player.RestoreHealth();
                        break;
                    //Legendary Powerups
                    case 5:
                        player.ActiaveHomingMissile();
                        break;

                }
            }
            Destroy(this.gameObject);
        }
    }
 
}
