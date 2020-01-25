using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerups; 
     


    private bool _stopSpawning = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    
    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false) {
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform; 
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            float rand = Random.Range(0, 101);

            //5% chance to spawn a rare powerup
            if(rand >= 95) {
                int rarePowerup = Random.Range(5, 6);
                Instantiate(_powerups[rarePowerup], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }
            else {
                int normalPowerup = Random.Range(0, 5);
                Instantiate(_powerups[normalPowerup], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }
            
        }
        
    }

    public void OnPlayerDeath() {
        _stopSpawning = true; 
    }
}
