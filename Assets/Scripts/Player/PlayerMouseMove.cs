using UnityEngine;

public class PlayerMouseMove : MonoBehaviour
{
    private Vector3 mouseLook;
    private Vector3 currentMousePlacement;
    private bool bCrossedMouse;
    private Vector2 direction;
    private float angle;
    [Header("Other Scripts")]
    [SerializeField] Health health;
    
    void Update()
    {
        health = GetComponentInParent<Health>();
        LookAtMouse();
        CheckBool();
    }

    private void LookAtMouse()
    {
        if (health.ReturnIsDead) return;
        mouseLook = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //if (bCrossedMouse) return;
        currentMousePlacement = mouseLook;
        direction = currentMousePlacement - transform.position;
        angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
    private void CheckBool() 
    {
        if (direction.magnitude < 1)
        {
            bCrossedMouse = true;
        }
        if(mouseLook != currentMousePlacement) 
        {
            bCrossedMouse = false;
        }
    }
}