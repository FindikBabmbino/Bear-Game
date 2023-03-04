using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Health stats")]
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private bool bIsDead;
    [Header("Misc")]
    [SerializeField] private GameObject[] characterGameObjectsToTurnOff;
    [SerializeField] private GameObject[] ammoToSpawn;
    [SerializeField] private GameObject[] healthToSpawn;
    [SerializeField] private bool bIsEnemy;

    [Header("Death stuff")]
    [SerializeField] private GameObject dieExplosion;
    [SerializeField] private GameObject[] bloodSplatter;
    [SerializeField] private AudioClip[] dieAudios;

    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Other scripts")]
    //[SerializeField] UIManager uIManager;
    [SerializeField] FollowCamera cameraShake;
    private GameUIScripts gameUIScripts;

    private bool bSpawnedItems;



    private void Start()
    {
        animator = GetComponent<Animator>();
        //if(!PlayerStatsTracker.instance.UseHealthPersist()&&!bIsEnemy)currentHealth = maxHealth;
        //else if(PlayerStatsTracker.instance.UseHealthPersist()&&!bIsEnemy) currentHealth=PlayerStatsTracker.instance.PersistHealth();
        //else if(bIsEnemy)currentHealth=maxHealth;
        //if(!bIsEnemy)uIManager=GameObject.Find("UI").GetComponent<UIManager>();
        //UpdateHealthUI();

        currentHealth = maxHealth;
        gameUIScripts = GameObject.FindAnyObjectByType<GameUIScripts>();
        if(!bIsEnemy) UpdateHealthUI();
    }

    public void HealAndDecreaseHealth(float var,bool hurt=false) 
    {
        switch (hurt) 
        {

            case false:
                if (bIsDead) break;
                currentHealth += var;
                if (currentHealth >= maxHealth)
                {
                    currentHealth = maxHealth;
                    break;
                }
                break;
            case true:
                //PoolManager.instance.GetFroomPool(PoolManager.instance.ReturnBloodSplattersPool(), CreateRandomPos(0, 3), Quaternion.identity, null, PoolManager.instance.ReturnBloodSplatterPrefabs());
                currentHealth = currentHealth - var;
                break;
        }
        CheckIfDead();
        if(!bIsEnemy)UpdateHealthUI();
        //if(!bIsEnemy)PlayerStatsTracker.instance.SetHealthToPersist(currentHealth);
    }

    private void CheckIfDead() 
    {
        if (currentHealth <= 0) 
        {
            TurnOnAndOffGameObjects(false);
            bIsDead = true;
            gameObject.layer = 7;
            GetComponent<SpriteRenderer>().sortingOrder = -2;
            StartCoroutine(DieEffect());

            //SpawnHealthAndAmmo();

            if (!bIsEnemy)
            {
                GetComponent<PlayerMovement>().SetDontMove(true);
                SceneManager.LoadScene("PlayLevel");
            }
            else
            {
                //GetComponent<AIPath>().canMove = false;
                //GetComponent<EnemyAI>().StopMoveCo();
            } 
        }
        else 
        {
            TurnOnAndOffGameObjects(true);
            bIsDead = false;
            if (bIsEnemy) gameObject.layer = 6;
            else gameObject.layer = 3;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            if(!bIsEnemy)
            {
                GetComponent<PlayerMovement>().SetDontMove(false);
            }
        }
    }

    //private void UpdateHealthUI() 
    //{
    //    if (uIManager == null) return;
    //    uIManager.SetHealthValue(currentHealth);
    //}

    private void TurnOnAndOffGameObjects(bool var) 
    {
        if (characterGameObjectsToTurnOff.Length <= 0) return;
        for (int i = 0; i < characterGameObjectsToTurnOff.Length; i++)
        {
            characterGameObjectsToTurnOff[i].SetActive(var);
        }
    }

    private void SpawnHealthAndAmmo() 
    {
        if (!bIsEnemy||bSpawnedItems) return;
        for (int i = 0; i < ammoToSpawn.Length; i++)
        {
            GameObject gO = Instantiate(ammoToSpawn[i],CreateRandomPos(1,2),Quaternion.identity);
        }
        int var = Random.Range(0, healthToSpawn.Length);
        Instantiate(healthToSpawn[var], CreateRandomPos(1,2), Quaternion.identity);
        bSpawnedItems = true;
    }

    private Vector3 CreateRandomPos(int min,int max) 
    {
        Vector3 pos;
        return  pos = transform.position + new Vector3(Random.Range(min, max), Random.Range(min, max));
    }

    private IEnumerator DieEffect() 
    {
        GetComponent<SpriteRenderer>().enabled = false;
        dieExplosion.SetActive(true);
        AudioSource.PlayClipAtPoint(dieAudios[Random.Range(0, dieAudios.Length)], transform.position,2f);
        GetComponent<BoxCollider2D>().enabled = false;
        GameObject gameObject = Instantiate(bloodSplatter[Random.Range(0,bloodSplatter.Length)],transform.position,Quaternion.identity);
        yield return new WaitForSeconds(1);
        dieExplosion.SetActive(false);
        yield return new WaitForSecondsRealtime(5);
        Destroy(this.gameObject);
        yield return null;
    }

    public void SetCurrentHealth(float var)
    {
        currentHealth=var;
    }

    private void UpdateHealthUI() 
    {
        if (bIsEnemy && bIsDead) return;

        gameUIScripts.UpdatePlayerHealth(currentHealth.ToString());
    }

    public bool ReturnIsDead => bIsDead;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

}