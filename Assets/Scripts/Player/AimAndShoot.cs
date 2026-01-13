using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class AimAndShoot : MonoBehaviour
{

    public Transform weapon;
    public Transform handMarker;
    public Transform holsterMarker;
    public PlayerStatus ThePlayerStatus;


    public float lerpSpeed = 1f;
    public bool DrawingWeapon = false;
    public bool WeaponInHand = false;
    public PlayerController ThePlayerController;
    public Animator theAnimator;
    public ArmMovement theArmMovement;
    public bool aim;
    public bool ScopedIn = false; //This variable is modified in ArmMovement.cs
    public bool shoot;
    private bool isShootingAnimationState = false;
    public bool isReloading = false;
    private bool isReloadingAnimationState = false;
    public CinemachineCamera FP_Camera;
    public Camera Weapon_Camera;
    public int ZoomFov = 30;
    public int StandardFov = 80;
    public LayerMask EnemiesLayerMask;
    public LayerMask EnemiesHeadShotLayerMask;
    [Header("Shooting Line Configuration")]
    private LineRenderer lineRenderer;
    public float lineWidth = 0.3f;
    public UnityEngine.Color StartLineColor = UnityEngine.Color.yellow;
    public UnityEngine.Color EndLineColor = UnityEngine.Color.whiteSmoke;
    public Material lineMaterial;
    public GameObject LineStartPoint;

    public ParticleSystem TheParticleSystem;

    public int ammoCount = 8;
    public GameObject AmmoCountObject;
    public TextMeshProUGUI AmmoCountTMP;



    // Start is called before the first frame update
    void Start()
    {
        AmmoCountTMP = AmmoCountObject.GetComponent<TextMeshProUGUI>();
        aim = false;
        shoot = false;
        weapon.parent = holsterMarker;
        AmmoCountTMP.text = "Ammo:" + ammoCount.ToString();
        //TheParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //theAnimator = GetComponent<Animator>();
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        
        if (context.started && ammoCount > 0 && ThePlayerStatus.items["SniperRifle"] == 1 && ThePlayerController.IsSprinting == false && isReloadingAnimationState == false && isShootingAnimationState == false)
        {
            shoot = true;
            Debug.Log("shoot");
            TheBullet();
            ammoCount--;
            if (ammoCount == 0) 
            { 
                AmmoCountTMP.text = "[R] - reload"; 
            }
            else
            {
                AmmoCountTMP.text = "Ammo:" + ammoCount.ToString();
            }
            
            theAnimator.SetBool("Shoot", shoot);
            TheParticleSystem.Play();




        }

        if (context.canceled && context.control.name == "leftButton")
        {
            shoot = false;
            theAnimator.SetBool("Shoot", shoot);
            



        }

       

    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Aim");
            aim = true;

            //theAnimator.SetBool("Aim", aim);


        }

        if (context.canceled)
        {
            Debug.Log("Aim end");
            aim = false;
            theArmMovement.AimIn_TimeCount = 1f;
            //theAnimator.SetBool("Aim", aim);
            zoom();

        }
    }

    void zoom()
    {
       if( aim == true)
        {
            FP_Camera.Lens.FieldOfView = ZoomFov;
            Weapon_Camera.nearClipPlane = 1.2f;
        }

        if (aim == false)
        {
            FP_Camera.Lens.FieldOfView = StandardFov;
            Weapon_Camera.nearClipPlane = 0.1f;
        }
    }

    void TheBullet()
    {
        
        RaycastHit hit;
        //BodyShots
        if (Physics.Raycast(FP_Camera.transform.localPosition, FP_Camera.transform.TransformDirection(Vector3.forward), out hit, 1000, EnemiesLayerMask))
        {
            

            CreateLine(hit);
            var HitEnemy = hit.collider.transform.GetComponent<EnemyHealth>();
            HitEnemy.HP--;
            Debug.Log($"Did Hit {hit.distance}");
        }
        //HeadShots
        else if (Physics.Raycast(FP_Camera.transform.localPosition, FP_Camera.transform.TransformDirection(Vector3.forward), out hit, 1000, EnemiesHeadShotLayerMask))
        {


            CreateLine(hit);
            var HitEnemy = hit.collider.transform.GetComponentInChildren<EnemyHealth>();
            HitEnemy.HP = 0;
            Debug.Log($"HeadShot");
        }
        else
        {
            
            CreateLine(hit);
            Debug.Log("Did not Hit");
        }
    }
    void CreateLine(RaycastHit shootHit)
    {
        
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        lineRenderer.enabled = true;
        lineWidth = 0.3f;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = StartLineColor;
        lineRenderer.endColor = EndLineColor;

        // Set line positions
        List<Vector3> linePositions = new List<Vector3>();
        linePositions.Add(LineStartPoint.transform.TransformPoint(0,0,0));
        if (shootHit.collider == null)
        {
            linePositions.Add(FP_Camera.transform.forward * 5000f);
        }
        else if (shootHit.collider != null)
        {
            linePositions.Add(shootHit.point);
        }

//Use this to verifiy the Raycast correctness
//Debug.DrawRay(LineStartPoint.transform.TransformPoint(0, 0, 0), FP_Camera.transform.TransformDirection(Vector3.forward) * 1000f, UnityEngine.Color.green , 100f);

lineRenderer.SetPositions(linePositions.ToArray());
        linePositions.Clear();

        
        //lineRenderer.enabled = false;

    }

    public void HideLine()
    {
        if (lineRenderer != null)
        {
            if (lineWidth > 0f)
            {
                lineWidth -= 0.01f;
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
            }
        }
      
    }
  

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
                
                isReloading = true;
                
                theAnimator.SetBool("Reload", isReloading);
           
                
            
            
             
        }
        if (context.performed)
        {
            isReloading = false;
            theAnimator.SetBool("Reload", isReloading);
            ReloadingAnimationStateCheck();
            
        }
            





    }

    void ReloadingAnimationStateCheck()
    {
        
        if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            isReloadingAnimationState = true;
            ammoCount = 8;
            AmmoCountTMP.text = "Ammo:" + ammoCount.ToString();
        }
        else
        {
            isReloadingAnimationState = false;
        }
        
    }

    void ShootingAnimationStateCheck()
    {

        if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
        {
            isShootingAnimationState = true;
           
        }
        else
        {
            isShootingAnimationState = false;
        }

    }

    void Update()
    {
        

        //Not used in final project
        ////Here we Lerp weapon's position and rotation with it's parent slot to be sure our weapon will always match the slot's position
        //if (weapon.parent != null)
        //{
        //    if ((weapon.position - weapon.parent.position).sqrMagnitude > 0.0001f)
        //    {
        //        weapon.position = Vector3.Lerp(weapon.position, weapon.parent.position, Time.deltaTime * lerpSpeed);
        //        weapon.rotation = Quaternion.Lerp(weapon.rotation, weapon.parent.rotation, Time.deltaTime * lerpSpeed);
        //    }
        //}


        if (ScopedIn == true) //This variable is modified in ArmMovement.cs
        {
            
            zoom();
        }


    }

    private void FixedUpdate()
    {
        ShootingAnimationStateCheck();
        ReloadingAnimationStateCheck();
        HideLine();
    }

    //private void LateUpdate()
    //{
    //    shoot = false;
    //}

}
