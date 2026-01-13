using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectGrabbing : MonoBehaviour
{
    public bool PlayerPickedUp = false;
    public GameObject thePlayersGrabObjectPoint;
    private Rigidbody TheRigidbody;
    public float throwPower = 20f; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TheRigidbody = GetComponent<Rigidbody>();
        TheRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

    public void PickUp()
    {
        
        if (PlayerPickedUp == false) {
            
            TheRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            Vector3 ThrowVec = this.transform.position - thePlayersGrabObjectPoint.transform.parent.transform.position;
            this.gameObject.transform.SetParent(null, true);
            TheRigidbody.AddForce(ThrowVec * throwPower, ForceMode.Impulse);
            Debug.Log("Dropping");
        }
        else if (PlayerPickedUp == true)
        {


            StartCoroutine(MoveObjectToHoldPosition());
        }
        
        
    }

    IEnumerator MoveObjectToHoldPosition()
    {
        
        
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 0.2f)  // 0.2f is time until lerp stopps
        {
            transform.position = Vector3.Lerp(this.transform.position, thePlayersGrabObjectPoint.transform.position, Time.time - startTime); // lerp from A to B 
            transform.rotation = Quaternion.Lerp(this.transform.rotation, thePlayersGrabObjectPoint.transform.rotation, Time.time - startTime); //Match Rotation
            yield return 1; // wait for next frame
        }
        //At the end of coroutine parent this object to the players object holding empty
        this.gameObject.transform.SetParent(thePlayersGrabObjectPoint.transform, true);
        this.gameObject.transform.localPosition = Vector3.zero; 
        TheRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }
}
