using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    [SerializeField] private int howManyWaves;
    [SerializeField] private int[] enemySpawnAmountDependingOnWave;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip endMusic;
    [SerializeField] private List<GameObject> spawnedEnemies;


    private int enemySpawnCounter;
    [SerializeField]private int currentWaveIndex;
    private int currentEnemyCount;

    private GameUIScripts gameUIScripts;

    private bool canSpawn;
    private bool settingUpWave;
    private bool endingGame;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        gameUIScripts = GameObject.FindAnyObjectByType<GameUIScripts>();

        StartCoroutine(SetUpNewWave());

    }


    private void Update()
    {
        UpdateSpawnedEnemiesList();
        CheckNextWave();
    }


    private void IncreaseSpawnCounter() 
    {
        enemySpawnCounter++;
        currentEnemyCount++;
    }

    private void DecreaseCurrentEnemyCount() 
    {
        currentEnemyCount--;
    }

    private void CheckNextWave() 
    {
        if (currentEnemyCount <= 0 && currentWaveIndex <= howManyWaves && !settingUpWave)
        {
            StartCoroutine(SetUpNewWave());
        }
        else if (currentWaveIndex > howManyWaves && !endingGame)
        {
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator SetUpNewWave() 
    {
        settingUpWave = true;
        canSpawn = false;
        gameUIScripts.EnableDisableGetReadyMessage();
        yield return new WaitForSeconds(2);
        currentWaveIndex++;
        if (currentWaveIndex > howManyWaves) yield return null;
        enemySpawnCounter = 0;
        if (currentWaveIndex <= howManyWaves) 
        {
            gameUIScripts.EnableDisableGetReadyMessage();
            gameUIScripts.UpdateCurrentWaveCount(currentWaveIndex.ToString());
            gameUIScripts.UpdateMaxWaveCount(howManyWaves.ToString());
            int i = currentWaveIndex - 1;
            gameUIScripts.UpdateEnemyCount(enemySpawnAmountDependingOnWave[i].ToString());

            canSpawn = true;
        }
        settingUpWave = false;

        yield return null;
    }

    private IEnumerator EndGame() 
    {
        endingGame = true;
        musicPlayer.clip = endMusic;
        musicPlayer.Play();
        gameUIScripts.EnableEndGameTitle();
        yield return new WaitForSeconds(3f);
        gameUIScripts.EnableEndGameButton();
        yield return null;
    }



    public void AddToSpawnedEnemiesList(GameObject gameObject) 
    {
        spawnedEnemies.Add(gameObject);
        IncreaseSpawnCounter();
    }

    private void UpdateSpawnedEnemiesList() 
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (spawnedEnemies[i].GetComponent<Health>().ReturnIsDead) 
            {
                spawnedEnemies.RemoveAt(i);
                DecreaseCurrentEnemyCount();   
                gameUIScripts.UpdateEnemyCount(currentEnemyCount.ToString());
            }
        }
    }

    public bool CanSpawn => canSpawn;

    public int EnemySpawnCounter => enemySpawnCounter;

    public int CurrentWaveEnemyCount() 
    {
        int i = currentWaveIndex - 1;
        //For some reason doing enemySpawnAmountDependingOnWave[currentWaveIndex--] does not work.
       return enemySpawnAmountDependingOnWave[i];
    
    } 
}