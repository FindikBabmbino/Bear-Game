using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerHurt : MonoBehaviour
{
    private WeaponBehaviour weaponBehaviour;
    private void Start()
    {
        weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            StartCoroutine(weaponBehaviour.MeleeFire(collision.gameObject));
        }
    }
}