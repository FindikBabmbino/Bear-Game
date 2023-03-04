using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private void Update()
    {
        if (GameManager.instance.CanSpawn) 
        {
            if (GameManager.instance.EnemySpawnCounter < GameManager.instance.CurrentWaveEnemyCount()) SpawnFunction();
        }
    }

    private void SpawnFunction()
    {
       GameObject gameObject = PoolManager.instance.GetFroomPool(PoolManager.instance.BunnyEnemyPool,transform.position,Quaternion.identity,PoolManager.instance.BunnyEnemyPrefab);
       GameManager.instance.AddToSpawnedEnemiesList(gameObject);
    }

}