using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Bullet stats")]
    [SerializeField] private int bulletDamage;
    [SerializeField] private bool bAvoidCollisionWithOwner;
    [Header("Misc")]
    [SerializeField] private bool bDontDestroyOnLand;
    [SerializeField] private bool bRotate;
    [SerializeField]private float rotateSpeed=100.0f;
    private bool bStartDestruction;
    private bool bDamageOnce;
    private GameObject bulletOwner;


    private void Start()
    {
        bDamageOnce = true;
    }

    private void Update()
    {
        if (bStartDestruction)
        {
            StartCoroutine(DestroyBullet());
        }
        if(bRotate)
        {
            RotateBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == bulletOwner) 
        {
            if (bAvoidCollisionWithOwner) Physics2D.IgnoreCollision(bulletOwner.GetComponent<BoxCollider2D>(), this.GetComponent<BoxCollider2D>());
            return;
        }
        if (bulletOwner!=null)
        {
            //bulletOwner.GetComponent<EnemyAI>() != null
            if (collision.gameObject.tag == "Enemy")
            {
                if (bAvoidCollisionWithOwner) Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), this.GetComponent<BoxCollider2D>());
                return;
            }
        }
        if (collision.gameObject.tag == "Bullet") 
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(),this.GetComponent<BoxCollider2D>());
            return;
        }
        if (collision.gameObject.GetComponent<Health>()&&bDamageOnce) 
        {
            bDamageOnce = false;
            collision.gameObject.GetComponent<Health>().HealAndDecreaseHealth(bulletDamage, true);
        }
        bStartDestruction = true;
    }

    private void RotateBullet()
    {
        transform.rotation=Quaternion.Euler(transform.rotation.x,transform.rotation.y,rotateSpeed*Time.time);
    }

    private IEnumerator DestroyBullet() 
    {
        yield return new WaitForSeconds(3f);
        while (true) 
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            this.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(5f);
            break;
        }
        Destroy(this.gameObject);
    }

    public void SetOwner(GameObject gO)
    {
        bulletOwner = gO;
    }
}