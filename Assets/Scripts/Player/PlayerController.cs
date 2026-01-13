using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public CinemachineCamera FPCamera;
    public Animator theAnimator;


    private Rigidbody myRigidbody;
    public bool Grounded = false;
    private float MaxSlope;
    public float SlopeOfValidGroundedCheck = 60f;

    private float MovementSpeed = 5f;

    public Vector2 moveVector;
    private Vector3 ForceToAdd;
    public Vector3 moveWithCameraDirection;

    public Collider myCollider;
    public PhysicsMaterial airPhysicMaterial;
    public PhysicsMaterial groundPhysicMaterial;
    public PhysicsMaterial CrouchPhysicsMaterial;

    public float MaxSprintSpeed = 5f;
    public bool IsSprinting = false;
    private float SprintSpeed = 0f;
    public float GroundedMovementSpeed = 5f;
    public float AirMovementSpeed = 2f;
    public float GroundedDrag = 10f;
    public float AirDrag = 0f;
    public float TapJumpSpeed = 250f;
    public float HoldJumpSpeed = 250f;
    public int NumOfJumps = 2;
    private float fallSpeed = -10f;
    public float MinFallSpeed = -10f;
    public float MaxFallSpeed = -20f;
    private Vector3 StandingHeight;
    private Vector3 CrouchingHeight;
    public bool isCrouching = false;
    private float CrouchLerpRatio = 0f;
    public bool PlayerHeadIsObstructed = false;

    public Vector3 SurfaceNormals;
    public Vector3 SlopedForceToAdd;




    void Start()
    {
        StandingHeight = new Vector3(1, 1, 1);
        CrouchingHeight = new Vector3 (1, 0.5f, 1);
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        MaxSlope = ((SlopeOfValidGroundedCheck) * Mathf.Deg2Rad); //converting the degrees to radians
        MovementSpeed = GroundedMovementSpeed;
    }


    void Update()
    {

        
    }

    private void FixedUpdate()
    {
        GroundedToAir();
        walking();
        Crouching();
        headCollisionCheck();
        AnimationTranstions();
    }


    //NOTE: Sqrt (desired height * 2.0 * gravity) to get the velocity required to jump to a desired height,
    //may require inverting gravity if you store gravity as a negetive number, hence * -2;
    public void OnJump(InputAction.CallbackContext context)
    {
        ///Jump removed, feature not needed for prototype
        if (Grounded == true)
        //if (Grounded == true || NumOfJumps > 0)  //for use with double jumps
        {
            //This is just the basic jump which I like over the more complex ones commented out.
            if (context.started)
            {

                myRigidbody.AddForce(new Vector3(0, HoldJumpSpeed, 0), ForceMode.Impulse);
                //Debug.Log($"Jump hold started : {context.interaction}");
            }



            ////This is for double jumps
            //if (context.performed)
            //{
            //    NumOfJumps--;
            //    Debug.Log($"Jump hold{context.duration}");
            //    myRigidbody.AddForce(new Vector3(0, HoldJumpSpeed, 0), ForceMode.Impulse);

            //}
            //if (context.started && context.interaction is HoldInteraction)



            //This is for having variable jump heights based on how long jump is held
            //if (context.canceled)
            //{
            //    double j = context.duration;
            //    double k = context.duration;
            //    //Debug.Log($"Jump canceled {j}");
            //    if (k < 0.2)
            //    {
            //        j = 1;
            //    }
            //    if (k > 0.2)
            //    {
            //        j = 0;

            //    }
            //    NumOfJumps--;
            //    Debug.Log($"Jump tapped{(float)j * 10}");
            //    myRigidbody.AddForce(new Vector3(0, (TapJumpSpeed ) * (float)j, 0), ForceMode.Impulse);


            //}

        }
        







    }
   
    public void OnMove(InputAction.CallbackContext context)
    {

        //Send this to walking
        moveVector = context.ReadValue<Vector2>();
        

    }

  
    public void walking()
    {
        //Aligns movement to camera facing directions
        moveWithCameraDirection = (FPCamera.transform.forward * moveVector.y + FPCamera.transform.right * moveVector.x);
        moveWithCameraDirection.y = 0f;

        //I subtract accumulatedForce which is the current speed this frame, from the added speed in ForceToAdd.
        //This is because I only want to add enough force to maintain a constant speed. 
        //I do not want the ramping up effect.
        
        var accumulatedForce = myRigidbody.linearVelocity.magnitude;

        //The Force to add to the Rigidbody via add force, SprintSpeed here is 0 when not in use.
        ForceToAdd = moveWithCameraDirection.normalized * Mathf.Clamp(((MovementSpeed + SprintSpeed) - accumulatedForce), SprintSpeed, MovementSpeed + SprintSpeed);
        

        //Changes force to add to account for sloped surfaces.
        SlopedForceToAdd = Quaternion.FromToRotation(Vector3.up, SurfaceNormals) * ForceToAdd;

        //AddForce
        myRigidbody.AddForce(SlopedForceToAdd, ForceMode.Impulse);
        
       
        

    }

    public void OnReload(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            Debug.Log("Reload");
        }

        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            SprintSpeed = MaxSprintSpeed;
            SprintSpeed = 0f;
            IsSprinting = true;
            theAnimator.SetBool("Sprint", IsSprinting);
        }
        if (context.canceled)
        {
            SprintSpeed = 0f;
            IsSprinting = false;
            theAnimator.SetBool("Sprint", IsSprinting);
        }

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {

           
           isCrouching = true;
            
            theAnimator.SetBool("CrouchSlide", isCrouching);

            IsSprinting = false;
            theAnimator.SetBool("Sprint", IsSprinting);

        }

        if (context.canceled)
        {
            
            isCrouching = false;

            theAnimator.SetBool("CrouchSlide", isCrouching);
        }
    }

    public void Crouching()
    {
        
        if (isCrouching == true && CrouchLerpRatio < 1) {
            CrouchLerpRatio += 0.1f;
            myCollider.material = CrouchPhysicsMaterial;
        }
        else if (isCrouching == false && PlayerHeadIsObstructed == false && CrouchLerpRatio > 0) {
            CrouchLerpRatio -= 0.2f;
        }
        myCollider.transform.localScale = Vector3.Lerp(StandingHeight, CrouchingHeight, CrouchLerpRatio);
        


    }

    public void AnimationTranstions()
    {
        
        if (IsSprinting == true && isCrouching == true)
        {
            
            Debug.Log("Slide");
            myRigidbody.linearDamping = AirDrag;
            theAnimator.SetBool("Sprint", IsSprinting);
            theAnimator.SetBool("CrouchSlide", isCrouching);
            IsSprinting = false;

        }

        if (IsSprinting == true && moveVector.y < 0)
        {
            IsSprinting = false;

            theAnimator.SetBool("Sprint", IsSprinting);
        }

         
    }

    void headCollisionCheck()
    {
        RaycastHit hit;

        Vector3 p1 = myCollider.transform.position;


        // Cast a sphere from character up, if the normal of the hit is negetive we are head bumping
        
        if (Physics.SphereCast(p1, myCollider.transform.lossyScale.z / 3, transform.up, out hit, 1))
        {
            //Debug.Log($"HitNormal {hit.normal.y}");
            if (hit.normal.y < 0f)
            {
                PlayerHeadIsObstructed = true;
            }
           
        }
        else
        {
            PlayerHeadIsObstructed = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        if (context.started && context.control.name == "leftButton")
        {
           Debug.Log($"Shoot : {context}");

        }

        if (context.started && context.control.name == "rightButton")
        {
            Debug.Log($"Aim : {context}");

        }

    }

    //The properties changed when on ground vs in air.
    private void GroundedToAir()
    {
        if (Grounded == false)
        {
            
            SprintSpeed = 0f;
            myCollider.material = airPhysicMaterial;
            myRigidbody.linearDamping = AirDrag;
            MovementSpeed = AirMovementSpeed;
            if (fallSpeed < MaxFallSpeed)
            {
                fallSpeed++; // fall speed is unused -> for creating a replacement gravity.
            }

        }
        else
        {
            NumOfJumps = 1;
            myCollider.material = groundPhysicMaterial;
            myRigidbody.linearDamping = GroundedDrag;
            MovementSpeed = GroundedMovementSpeed; 
            fallSpeed = MinFallSpeed;

        }
    }

    

    
    void OnCollisionStay(Collision other)
    {
        // Print how many points are colliding with this transform
        //Debug.Log("Points colliding: " + other.contacts.Length);

        // Print the normal of the first point in the collision.
        // Debug.Log("Normal of the first point: " + other.contacts[0].normal);

        // Draw a different colored ray for every normal in the collision
        //foreach (var item in other.contacts)
        //{
        //    Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
        //}

        //Check each collision contact
    for (int i = 0; i < other.contacts.Length; i++) {
            //if the normal of the collision contact, is less than a set angle: Max Slope ==then=> we are grounded.
        if (Mathf.Acos(Vector3.Dot(other.contacts[i].normal, transform.up) / ((other.contacts[i].normal.magnitude) * (transform.up.magnitude))) < MaxSlope)
        {
            Grounded = true;
            SurfaceNormals = other.contacts[i].normal;
            return;
        }

        
       
    }

    }
    void OnCollisionExit(Collision other)
    {
        if (other.contacts.Length == 0)
        {
            Grounded = false;
        }
        

    }
}
