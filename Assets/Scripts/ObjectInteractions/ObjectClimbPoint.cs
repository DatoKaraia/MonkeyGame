using System.Collections;

using UnityEngine;

public class ObjectClimbPoint : MonoBehaviour
{
    [Header("Make sure the collider of the climb point is large enough to be interacted from inside")]
    public bool PlayerIsClimbing;
    public Rigidbody PlayerRigidbody;
    [SerializeField]
    private float JumpOffPower = 400f;
    [SerializeField]
    private float snapSpeed = 0.2f;
    
    public bool OverrideScan = false;

    [Header("IsVaulting = true makes it so that you \n immediately hop off the climb point once you reach it \n Otherwise Isvaulting false means holding E will hold you in place ")]
    [SerializeField]
    public bool IsVaulting = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Climbing()
    {
        if (PlayerIsClimbing == false)
        {
            if (IsVaulting == false)
            {
                PlayerRigidbody.transform.SetParent(null, true);
                PlayerRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
                PlayerRigidbody.AddForce(Vector3.up * JumpOffPower, ForceMode.Impulse);
            }
        }
        else if (PlayerIsClimbing == true)
        {
            //if (IsVaulting == true)
            //{
                StartCoroutine(ClimbingCoroutine());
            //}

            //if (IsVaulting == false) 
            //{
            //    StartClimbing = true;
            //    startClimbTime = 0f;
            //}

        }
    }

    //private void ClimbingAndHold()
    //{
    //    if (StartClimbing == true)
    //    {
    //        startClimbTime+= 0.1f;
    //        if(startClimbTime < 1)
    //        {
    //            PlayerRigidbody.transform.position = Vector3.Lerp(PlayerRigidbody.transform.position, this.gameObject.transform.position, startClimbTime);
    //        }
    //        else
    //        {
    //            PlayerRigidbody.transform.SetParent(this.gameObject.transform, true);
    //        }
            

    //    }
        
    //}
    IEnumerator ClimbingCoroutine()
    {


        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= snapSpeed)  // time for lerp to happen within
        {
            PlayerRigidbody.transform.position = Vector3.Lerp(PlayerRigidbody.transform.position, this.gameObject.transform.position, Time.time - startTime); // lerp from A to B in one second
            
            yield return 1; // wait for next frame
        }
        //At the end of coroutine parent player to climb point
        
        PlayerRigidbody.transform.SetParent(this.gameObject.transform, true);
        PlayerRigidbody.transform.localPosition = Vector3.zero;

        //constraints hold you in place 
        PlayerRigidbody.constraints = PlayerRigidbody.constraints | RigidbodyConstraints.FreezePosition;

        while (IsVaulting == false) //We stall coroutine Until the input is released
        {
            yield return null;
        }

        PlayerRigidbody.constraints -= RigidbodyConstraints.FreezePosition; 


        if (IsVaulting == true) // Vaulting is what bumps you off at the end
        {
            PlayerIsClimbing = false;

            PlayerRigidbody.transform.SetParent(null, true);

            PlayerRigidbody.AddForce(Vector3.up * JumpOffPower, ForceMode.Impulse);
        }

       

    }
}
