using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoEnemySpawner : MonoBehaviour
{
    

    [SerializeField] private Transform spawnPoint;

    

    [SerializeField] private LevelInfo _levelInfo;
    //[SerializeField] private float respawnRate = 10;

    //[SerializeField] private float initialSpawnDelay;
    
    //[SerializeField] private int totalNumberToSpawn;
    
    private int spawnedCount = 0;

    private void Start()
    {
        StartCoroutine(LevelCoroutine());
  
    }

    private IEnumerator LevelCoroutine()
    {
        foreach (var i in _levelInfo.GetInfo())
        {
            yield return new WaitForSeconds(i.waitTime);

            Enemy prefab = EnemyConfig.GetEnemyPrefab(i.EnemyType);

            if (prefab == null)
            {
                continue;
            }

            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
