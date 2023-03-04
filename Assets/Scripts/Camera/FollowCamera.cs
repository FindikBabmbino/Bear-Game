using System.Collections;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]private Transform targetTransform;    
    //Smooth will be used to smooth out the lerp
    [SerializeField]private float smooth;
    [Header("CameraShake")]
    [SerializeField] private GameObject gameObjectToParentTo;
    private bool bCameraIsShaking;

    // Update is called once per frame
    void Update()
    {
        if (!bCameraIsShaking) 
        {
            transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
        }
    }

    void LateUpdate()
    {
    }

    private void CalculateCameraMovement()
    {
        //Check if the cameras transform does not match the target transform if so do a lerp
        if(transform.position!=targetTransform.position)
        {
            //First create targetpos to keep the cameras original z as the players z screws it up
            Vector3 targetPos=new Vector3(targetTransform.position.x,targetTransform.position.y,transform.position.z);
            transform.position=Vector3.Lerp(transform.position,targetPos,smooth*Time.deltaTime);
        }
    }



    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        if (bCameraIsShaking) yield return null;
        while (elapsed < duration)
        {
            bCameraIsShaking = true;
            //Bit hacky but it works the idea is parenting the camera to the player when we shake if we don't do this it likes to go to the origin of the map(you can guess why)
            //Then we de-parent at the end 
            transform.SetParent(gameObjectToParentTo.transform);
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = Vector3.Slerp(transform.position, new Vector3(x, y, transform.localPosition.z), 0.1f * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        bCameraIsShaking = false;
        transform.position = originalPos;
        transform.SetParent(null);
    }

    public bool ReturnIsCameraInAParent()
    {
        if (this.transform.parent != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}