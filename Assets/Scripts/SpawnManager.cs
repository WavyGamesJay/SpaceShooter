﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject[] _powerups;

    [SerializeField] private int _wave;
    [SerializeField] private int _enemiesSpawned = 0;
    [SerializeField] private int _enemiesPerWave;
    [SerializeField] private float _timeBetweenSpawns = 5f;

    public UIManager _uiManager;

    private bool _stopSpawning = false; 

    // Start is called before the first frame update
    void Start()
    {
        _wave = 0;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null) {
            Debug.LogError("UI Manager is NULL");
        }
        
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void UpdateWave() {
        _wave++;
        _enemiesSpawned = 0;
        _enemiesPerWave = _wave * 7;
        _timeBetweenSpawns = _timeBetweenSpawns * .95f;
        _uiManager.NewWave(_wave);
    }

    IEnumerator SpawnEnemyRoutine() {
        UpdateWave();
        yield return new WaitForSeconds(4.0f);

        while (_enemiesSpawned < _enemiesPerWave) {
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesSpawned++;
            yield return new WaitForSeconds(_timeBetweenSpawns);  

            if(_enemiesSpawned >= _enemiesPerWave) {
                UpdateWave();
            }
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
