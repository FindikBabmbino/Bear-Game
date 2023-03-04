using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float attackDistance;
    [SerializeField] private int moveSpeed;

    [Header("Target")]
    [SerializeField] private Transform targetToMoveTo;

    [SerializeField]private List<GameObject> enemies;

    private Health health;

    private Animator animator;

    private GameObject weaponTrigger;

    private enum EnemyState
    {
        CHASE,
        ATTACK
    }

    private EnemyState enemyState;


    private void Start()
    {
        if (!targetToMoveTo) targetToMoveTo = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<Health>();

        animator = GetComponent<Animator>();

        weaponTrigger = transform.Find("bunnyweapon").gameObject;
        weaponTrigger.SetActive(false);


        enemyState = EnemyState.CHASE;
    }





    private void Update()
    {
        if (!health.ReturnIsDead) 
        {
            SearchForEnemies();
            CheckFriendList();
            CalculateEnemyDistance();
            StateChanger();
        }

        else
        {
            enemies.Clear();
            moveSpeed= 0;
        } 

    }

    private void StateChanger() 
    {
        switch (enemyState) 
        {
            case EnemyState.CHASE:
                ChaseState();
                break;
            case EnemyState.ATTACK:
                AttackState();
                break;
            default: break;
        }
    }


    private void ChaseState() 
    {
        if(GetDistanceFromTarget() > attackDistance) 
        {
            Vector3 moveDirection = targetToMoveTo.position - transform.position;

            if (moveDirection.x > 0) GetComponent<SpriteRenderer>().flipX = false;
            else GetComponent<SpriteRenderer>().flipX = true;
           
            MoveEnemy();

            animator.SetBool("EnemyWalk", true);
        }

        else 
        {
            animator.SetBool("EnemyWalk", false);
            enemyState = EnemyState.ATTACK;
        }
    }

    private void MoveEnemy() 
    {
        transform.position += (targetToMoveTo.transform.position - transform.position).normalized * moveSpeed *Time.deltaTime;
    }

    private float GetDistanceFromTarget() 
    {
        return (targetToMoveTo.position - transform.position).magnitude;
    }

    private void AttackState() 
    {
        if (GetDistanceFromTarget() <= attackDistance) 
        {
            weaponTrigger.SetActive(true);
        }
        else 
        {
            weaponTrigger.SetActive(false);
            enemyState = EnemyState.CHASE;
        }
    }

    private void SearchForEnemies() 
    {
        foreach (EnemyAI enemy in GameObject.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        {
            if (!enemies.Contains(enemy.gameObject) && enemy.gameObject != this.gameObject) 
            {
                enemies.Add(enemy.gameObject);
            }
        }
    }

    private void CheckFriendList() 
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null) continue;
            if (enemies[i].GetComponent<Health>().ReturnIsDead) enemies.RemoveAt(i);
        }
    }

    private void CalculateEnemyDistance() 
    {
        foreach (GameObject gameObject in enemies)
        {
            if (gameObject == null) return;
            if((gameObject.transform.position - transform.position).magnitude < 2.5f) 
            {
                MoveEnemyAwayFromFriend(gameObject);
            }
        }
    }

    private void MoveEnemyAwayFromFriend(GameObject gameObject) 
    {
        Vector3 direction = transform.position - gameObject.transform.position;
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
    }
}