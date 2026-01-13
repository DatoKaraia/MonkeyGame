using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AimAndShoot MyAimAndShoot;
    public GameObject ArmPivot;
    public GameObject Arms; //The art element of arms
    public PlayerStatus ThePlayerStatus;
    public bool RaiseArmAction = false;
    private float speed = 5f;
    private float timeCount = 0.0f;
    public float AimIn_TimeCount = 0.0f;
    public Vector3 ArmUpRotatePos;
    public Vector3 ArmDownRotatePos;
    public Vector3 ArmAimingPos;
    public Vector3 ArmNotAimingPos;
    private quaternion startPosition;
    private quaternion endPosition;
    public bool HasEquipedWeapon = false;
    
    
    

    void Start()
    {
        
        ArmPivot = this.gameObject;
        
        startPosition = Quaternion.Euler(ArmUpRotatePos.x, ArmUpRotatePos.y, ArmUpRotatePos.z);
        endPosition = Quaternion.Euler(ArmDownRotatePos.x, ArmDownRotatePos.y, ArmDownRotatePos.z);


    }

    // Update is called once per frame
    void Update()
    {
        DropArm();
        AimingTransition();
        if (ThePlayerStatus.items["SniperRifle"] == 1)
        {
            HasEquipedWeapon = true;

           
            DropArm();
        }
    }
    

    public void OnNumb1(InputAction.CallbackContext context)
    {
        
        if (context.started) {

            timeCount = 0.0f;
            RaiseArmAction = true;


        }

      

    }
    public void DropArm()
    {
        if (RaiseArmAction == false && HasEquipedWeapon == true)
        {

            ArmPivot.transform.localRotation = startPosition;
        }
        else 
        {
            ArmPivot.transform.localRotation = endPosition;
        }

        if (RaiseArmAction == true)
        {

            ArmPivot.transform.localRotation = Quaternion.Lerp(endPosition, startPosition, (timeCount * speed));

            timeCount = timeCount + Time.deltaTime;

            if (timeCount > 0.8f)
            {
                RaiseArmAction = false;
            }
        }

    }

    public void AimingTransition()
    {
        if (RaiseArmAction == false)
        {

            if (MyAimAndShoot.aim == true)
            {
                ArmPivot.transform.localPosition = Vector3.Lerp(ArmNotAimingPos, ArmAimingPos, (AimIn_TimeCount * speed));
                if (AimIn_TimeCount > 0.2f)
                {
                    AimIn_TimeCount = 1f;
                    
                    MyAimAndShoot.ScopedIn = true;
                }
                else
                {
                    AimIn_TimeCount = AimIn_TimeCount + Time.deltaTime;
                }
                    
                                 
                
            }
            //Aim Out needs figuring out.

            else if (MyAimAndShoot.aim == false)
            {
                
                ArmPivot.transform.localPosition = Vector3.Lerp(ArmNotAimingPos, ArmAimingPos, (AimIn_TimeCount - 0.5f));

                if (AimIn_TimeCount < 0.2f)
                {
                    
                    AimIn_TimeCount = 0f;
                }
                else
                {
                    AimIn_TimeCount = AimIn_TimeCount - Time.deltaTime;
                }
                    
                MyAimAndShoot.ScopedIn = false;
            }

        }
    }

   

}
