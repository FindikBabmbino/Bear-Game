using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class WeaponBehaviour : MonoBehaviour
{
    private enum WeaponType
    {
        SHOTGUN,
        SMG,
        PAPER,
        NONE
    }
    [Header("Has gun")]
    [SerializeField] private bool bHasGun;
    [Header("Weapon Stats")]
    [SerializeField] private WeaponType currentType;
    [SerializeField] private int maxAmmoCount;
    private int currentAmmoCount;
    [SerializeField] private int maxSpareAmmoCount;
    private int currentSpareAmmoCount;
    [SerializeField] private float fireRate;
    private float fireRateTimer;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float cassingSpeed;

    [Header("Melee Stats")]
    [SerializeField]private float meleeRange;
    [SerializeField]private LayerMask layerMask;
    [SerializeField]private bool hasInfiniteAmmo;
    [SerializeField]private float meleeDamage;
    [SerializeField] private GameObject meleeObj;
    private bool meleeAttacking;

    [Header("Misc")]
    [SerializeField] private bool isPlayer;
    [SerializeField] private GameObject parentGameObject;
    [SerializeField] private Transform[] shootPoint;
    [SerializeField] private Transform[] cassingPoint;
    [SerializeField] private Transform magPoint;
    [SerializeField] private GameObject gunArm;
    private GameObject cloneBullet;
    private GameObject cloneCassing;
    private GameObject cloneMag;
    //This will keep the time it looped through creating clones of cassings to avoid creating more
    private int cassingCloneLimit;
    private bool isReloading;

    [Header("Audio")]
    [SerializeField] private AudioSource weaponAudioSource;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip reloadClip;

    [Header("Other scripts")]
    //UIManager uIManager;
    [SerializeField] private Health health;

    [Header("Animators")]
    [SerializeField] private Animator shotgunAnimator;
    [SerializeField] private Animator smgAnimator;

    private PlayerMovement cantMove;

    private bool gunCanBeUsed;


    private void Start()
    {
        //cantMove=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        //if(isPlayer)uIManager=GameObject.Find("UI").GetComponent<UIManager>();
        currentAmmoCount = maxAmmoCount;
        currentSpareAmmoCount = maxSpareAmmoCount;


        if (!isPlayer)
        {
            gunCanBeUsed = true;
            currentAmmoCount = maxAmmoCount;
            currentSpareAmmoCount = maxSpareAmmoCount;
        }

        gunCanBeUsed = true;
    }

    private void Update()
    {
        if(gunCanBeUsed)
        {
            if (fireRateTimer < fireRate) 
            {
                fireRateTimer += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (currentAmmoCount > 0)
                {
                    CallFireWeapon();
                }
                else if(currentAmmoCount<=0&&currentSpareAmmoCount>0)
                {
                    ReloadWeapon();
                }
            }
            else if (Input.GetKeyDown(KeyCode.R) && currentSpareAmmoCount>0) ReloadWeapon();
        }
    }

    private void CallFireWeapon()
    {
       
        FireWeapon();
    }

    private void FireWeapon() 
    {
        if (fireRateTimer < fireRate) return;
        if (weaponAudioSource&&fireClip) PlayWeaponAudio(fireClip);
        for (int i = 0; i < shootPoint.Length; i++)
        {
            if (currentType == WeaponType.SHOTGUN) CreateCloneBullet(PoolManager.instance.ReturnShellQueue(), PoolManager.instance.ReturnShellPrefab(), i);
            else if (currentType == WeaponType.SMG) CreateCloneBullet(PoolManager.instance.ReturnNineMMQueue(), PoolManager.instance.ReturnNineMMPrefab(), i);
            else if (currentType==WeaponType.PAPER) CreateCloneBullet(PoolManager.instance.ReturnPaperQueue(),PoolManager.instance.ReturnPaperPrefab(),i);
            while (cassingCloneLimit < cassingPoint.Length)
            {
                if (cassingCloneLimit == cassingPoint.Length) break;
                if (currentType == WeaponType.SMG) CreateCloneCassing(PoolManager.instance.ReturnNineMMCassingQueue(), PoolManager.instance.ReturnNineMMCasingPrefab(), cassingCloneLimit);
                cassingCloneLimit++;
            }
        }
        if(currentType==WeaponType.NONE)
        {
          
           
        }
        if (currentType == WeaponType.SHOTGUN) shotgunAnimator.SetTrigger("Fire");
        if (currentType == WeaponType.SMG && smgAnimator) smgAnimator.SetTrigger("Fire");
        cassingCloneLimit = 0;
        if(!hasInfiniteAmmo)currentAmmoCount--;
        fireRateTimer = 0;


    }

    public void EnemyFire() 
    {
        FireWeapon();
    }

    public IEnumerator MeleeFire(GameObject gameObject)
    {
        yield return new WaitForSeconds(1.5f);
        if (meleeAttacking) yield return null;
        Debug.Log(gameObject.transform.gameObject);
        meleeAttacking = true;
        gameObject.GetComponent<Health>().HealAndDecreaseHealth(meleeDamage, true);
        meleeObj.SetActive(true);
        meleeObj.SetActive(false);
        meleeAttacking = false;
    }

    private void ReloadWeapon() 
    {
        if (currentSpareAmmoCount < 0||currentAmmoCount>=maxAmmoCount) return;
        if (weaponAudioSource && reloadClip) PlayWeaponAudio(reloadClip);
        isReloading = true;
        if (currentType == WeaponType.SHOTGUN&&isPlayer) shotgunAnimator.SetBool("ReloadGun", true);
        if (currentType == WeaponType.SMG) smgAnimator.SetBool("ReloadGun", true);

        int ammoToDeduct = 0;

        if(currentAmmoCount<=0)
        {
            if(maxAmmoCount<=currentSpareAmmoCount) ammoToDeduct = maxAmmoCount;
            else ammoToDeduct = currentSpareAmmoCount;
        }
        else
        {
            ammoToDeduct=maxAmmoCount-currentAmmoCount;
        }

        currentSpareAmmoCount -= ammoToDeduct;
        currentAmmoCount += ammoToDeduct;
        currentAmmoCount = Mathf.Clamp(currentAmmoCount, 0, maxAmmoCount);

    }

    public void ReplenishAmmo(int var) 
    {
        if (currentSpareAmmoCount >= maxSpareAmmoCount) return;
        currentSpareAmmoCount += var;
        currentSpareAmmoCount = Mathf.Clamp(var, 0, maxSpareAmmoCount);
    }

    private void CreateCloneBullet(Queue<GameObject> bulletQueue,GameObject bulletPrefab,int var) 
    {
        cloneBullet = PoolManager.instance.GetFroomPool(bulletQueue, shootPoint[var].position, shootPoint[var].rotation * Quaternion.Euler(0, 0, -90), bulletPrefab);
        cloneBullet.GetComponent<BulletBehaviour>().SetOwner(parentGameObject);
        cloneBullet.GetComponent<Rigidbody2D>().AddForce(shootPoint[var].transform.right * projectileSpeed);
    } 

    private void CreateCloneCassing(Queue<GameObject>cassingQueue,GameObject cassingPrefab,int var) 
    {
        cloneCassing = PoolManager.instance.GetFroomPool(cassingQueue, cassingPoint[var].position, cassingPoint[var].rotation, cassingPrefab);
        cloneCassing.GetComponent<BulletBehaviour>().SetOwner(parentGameObject);
        cloneCassing.GetComponent<Rigidbody2D>().AddForce(cassingPoint[var].transform.right * cassingSpeed);
    }


    public void SetIsReloadingFalse() 
    {
        isReloading = false;
        if (currentType == WeaponType.SHOTGUN) shotgunAnimator.SetBool("ReloadGun", false);
        else if (currentType == WeaponType.SMG) smgAnimator.SetBool("ReloadGun", false);
    }

    public void RecieveAmmo(int var, ref bool usedAmmo) 
    {
        if (currentSpareAmmoCount == maxSpareAmmoCount) return;
        currentSpareAmmoCount += var;
        usedAmmo = true;
        if (currentSpareAmmoCount > maxSpareAmmoCount) 
        {
            currentSpareAmmoCount = maxSpareAmmoCount;
        }
    }

    public string ReturnCurrentType() 
    {
        return currentType.ToString();
    }

    private void PlayWeaponAudio(AudioClip var) 
    {
        weaponAudioSource.clip = var;
        if (!weaponAudioSource.isPlaying) weaponAudioSource.Play();
        else AudioSource.PlayClipAtPoint(var, transform.position,1f);
    }

    public void UpdateGunHas(bool var,ref bool usedGun) 
    {
        if (!gunArm.activeSelf) gunArm.SetActive(true);
        bHasGun = var;
        usedGun = true;
        this.gameObject.SetActive(bHasGun);
    }

    public void SetGunCanBeUsed(bool var)
    {
        gunCanBeUsed = var;
    }

    public bool GunCanBeUsed => gunCanBeUsed;

    public bool ReturnIsReloading => isReloading;

    public bool HasGun => bHasGun;

    public void SetHasGun(bool var)
    {
        bHasGun=var;
    }
}