using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    [Header("Shell pool")]
    [SerializeField] private Queue<GameObject> shellPool = new Queue<GameObject>();
    [SerializeField] private int shellSizePool;
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private Queue<GameObject> shellCassingPool = new Queue<GameObject>();
    [SerializeField] private GameObject shellCassingPrefab;

    [Header("9MMPool")]
    [SerializeField] private Queue<GameObject> ninemmPool = new Queue<GameObject>();
    [SerializeField] private int ninemmSizeOfPool;
    [SerializeField] private GameObject ninemmPrefab;
    [SerializeField] private Queue<GameObject> ninemmCassingPool = new Queue<GameObject>();
    [SerializeField] private GameObject ninemmCassingPrefab;


    [Header("PaperPool")]
    [SerializeField]private Queue<GameObject> paperPool=new Queue<GameObject>();
    [SerializeField] private int paperSizeOfPool;
    [SerializeField] private GameObject paperPreefab;

    [Header("SmgRelated")]
    [SerializeField] private Queue<GameObject> smgMagPool = new Queue<GameObject>();
    [SerializeField] private int smgMagPoolSize;
    [SerializeField] private GameObject smgMagPrefab;

    [Header("Blood Splatters")]
    [SerializeField] private Queue<GameObject> bloodSplattersPool = new Queue<GameObject>();
    [SerializeField] private int bloodPoolSize;
    [SerializeField] private GameObject[] bloodSplattersPrefabs;

    [Header("Enemy")]
    [SerializeField] private Queue<GameObject> bunnyEnemyPool = new Queue<GameObject>();
    [SerializeField] private GameObject bunnyEnemyPrefab;


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
        if (shellPrefab != null) 
        {
            IntialisePool(shellPool, shellSizePool, shellPrefab);
        }
    }

    private void IntialisePool(Queue<GameObject>gameObjects,int poolSize,GameObject gameObjectToInstantiate) 
    {
        for (int i=0;i<poolSize;i++) 
        {
            GameObject newObj = Instantiate(gameObjectToInstantiate);
            newObj.SetActive(false);
            gameObjects.Enqueue(newObj);
        }
    }

    public GameObject GetFroomPool(Queue<GameObject>gameObjects,Vector3 pos,Quaternion rot,GameObject prefabToLazy=null,GameObject[] prefabsToLazy=null) 
    {
        if (gameObjects.Count <= 0) 
        {
            if (prefabToLazy != null) 
            {
                GameObject addToQueue = Instantiate(prefabToLazy);
                addToQueue.SetActive(false);
                gameObjects.Enqueue(addToQueue);
            }
            else if (prefabsToLazy!=null) 
            {
                for (int i = 0; i < prefabsToLazy.Length; i++)
                {
                    GameObject addToQueue = Instantiate(prefabsToLazy[i]);
                    addToQueue.SetActive(false);
                    gameObjects.Enqueue(addToQueue);
                }
            }
        }
        GameObject objectToSpawn = gameObjects.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rot;
        if (objectToSpawn.GetComponent<Rigidbody2D>()) objectToSpawn.GetComponent<Rigidbody2D>().isKinematic = false;
        return objectToSpawn;
    }

    public void ClearEverything()
    {
        shellPool.Clear();
        shellCassingPool.Clear();
        ninemmCassingPool.Clear();
        ninemmPool.Clear();
        paperPool.Clear();
        bloodSplattersPool.Clear();
    }

    public void ReturnToPool(Queue<GameObject> gameObjects,GameObject gameObjectToReturn) 
    {
        gameObjectToReturn.SetActive(false);
        gameObjects.Enqueue(gameObjectToReturn);
    }

    public Queue<GameObject> ReturnShellQueue() 
    {
        return shellPool;
    }

    public Queue<GameObject> ReturnShellCassingQueue() 
    {
        return shellCassingPool;
    }

    public GameObject ReturnShellPrefab() 
    {
        return shellPrefab;
    }

    public GameObject ReturnShellCassingPrefab() 
    {
        return shellCassingPrefab;
    }

    public Queue<GameObject> ReturnNineMMQueue() 
    {
        return ninemmPool;
    }

    public Queue<GameObject> ReturnNineMMCassingQueue()
    {
        return ninemmCassingPool;
    }

    public GameObject ReturnNineMMPrefab() 
    {
        return ninemmPrefab;
    }

    public GameObject ReturnNineMMCasingPrefab()
    {
        return ninemmCassingPrefab;
    }

    public Queue<GameObject> ReturnSmgMagPool() 
    {
        return smgMagPool;
    }
    
    public GameObject ReturnSmgMagPrefab() 
    {
        return smgMagPrefab;
    }

    public Queue<GameObject> ReturnBloodSplattersPool() 
    {
        return bloodSplattersPool;
    }

    public GameObject[] ReturnBloodSplatterPrefabs() 
    {
        return bloodSplattersPrefabs;
    }

    public Queue<GameObject> ReturnPaperQueue()
    {
        return paperPool;
    }
    public GameObject ReturnPaperPrefab()
    {
        return paperPreefab;
    }

    public GameObject BunnyEnemyPrefab => bunnyEnemyPrefab;
    public Queue<GameObject> BunnyEnemyPool => bunnyEnemyPool;

}