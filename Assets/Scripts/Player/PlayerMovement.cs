using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Speed")]
    [SerializeField]private float moveSpeed;
    [Header("Animators")]
    [SerializeField] private Animator legAnimator;
    [Header("Other scripts")]
    [SerializeField] private Health health;
    [Header("Visual")]
    [SerializeField] private GameObject[] bloodPrints;
    private int index;

    private Rigidbody2D rigid2D;
    private Vector2 movementDirection;
    private float hor,ver;
    private bool bLeaveBloodPrints;

    private bool dontMove;

    void Start()
    {
        legAnimator = GetComponent<Animator>();
        Cursor.lockState=CursorLockMode.Confined;
        index = 0;
        health = GetComponent<Health>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MovementInput();
        UpdateWalkAnimation();
        if (bLeaveBloodPrints) Invoke("DisableBloodPrints", 10.0f);
    }

    private void FixedUpdate()
    {
        MovementCalculation();
    }

    private void MovementInput()
    {
        if (dontMove) 
        {
            movementDirection = Vector2.zero;
        }
        else 
        {
            movementDirection.x = hor = Input.GetAxis("Horizontal");
            movementDirection.y = ver = Input.GetAxis("Vertical");
        }
        //Old calculation used to be vector 3
        //movementDirection = -transform.up * hor + transform.right * ver;
    }

    private void MovementCalculation()
    {
        rigid2D.MovePosition(rigid2D.position + movementDirection.normalized*moveSpeed*Time.deltaTime);
        //Old one-used to be vector 3
        //rigid2D.MovePosition(transform.position+(movementDirection.normalized*moveSpeed*Time.deltaTime));
    }

    private void UpdateWalkAnimation()
    {
        if (legAnimator == null) return;
        if (movementDirection != Vector2.zero) 
        {
            legAnimator.SetBool("PlayerWalk", true);
            rigid2D.isKinematic = false;
        }
        else 
        {
            legAnimator.SetBool("PlayerWalk", false);
            rigid2D.isKinematic = true;
        }
    }

    public void SpawnBloodPrints() 
    {
        Instantiate(bloodPrints[index], transform.position, transform.rotation);
        if (index == 0) index++;
        else index = 0;
    }

    private void DisableBloodPrints() 
    {
        bLeaveBloodPrints = false;
    }

    public void EnableBloodPrints()
    {
       bLeaveBloodPrints = true;
    }

    public void SetDontMove(bool var)
    {
        dontMove = var;
    }

    public bool ReturnLeaveBloodPrints => bLeaveBloodPrints;    
    public bool DontMove=>dontMove;
}