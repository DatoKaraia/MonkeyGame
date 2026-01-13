using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

//This script funnels the initial player input to object interaction when the "E" key is pressed
//to the objectMaster script attached to the gameObject in question.
//That objectMaster hold reference to other object behaviour scripts attached on the object - objectSwitch , objectgrabbable etc..
//The goal of this code structure is to separate the player input from the object behaviour,
//In otherwards pressing the E key does whats on the Gameobject not whats on the player input script and we can change what is on the gameobject

//In summery
//ObjectInteraction on the player passes inputs, info, raycasts, triggers etc... to objectMaster on the gameObject which inturn talks to various object behaviours
//

//    [Player]      Connects between   [Object]
//ObjectInteraction         <->      ObjectMaster
//                                        |
//                               (Connects up to)
//                                       |
//                              ObjectSwitch, ObjectGrabbing, Any other object behaviours etc... 
//
//

public class PlayerToObjectInteraction : MonoBehaviour
{
    public CinemachineCamera FP_Camera;
    public LayerMask ObjectsLayerMask;
    public ObjectMaster TheObjectMaster; //Master node on the object side for subsequnt objectInteraction components
    public GameObject TheGrabObject_Point; //The point in space where Objects are held by the player
    public GameObject theDisplayText; //Where tooltips will be displayed for player to see.
    public Rigidbody playerRigidbody; // Modified when climbing to a point
    public CapsuleCollider PlayerCollider; // Modified for squeezing in narrow gaps
    private string TheObjectToolTip;
    private TextMeshPro theDisplayTextMeshPro;
    private bool IsInteracting = false;
    //private ObjectGrabbing TheGrabbedObject;


    void Start()
    {
        
        theDisplayText.SetActive(false);
        theDisplayTextMeshPro = theDisplayText.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        TheScanRay();
        
    }
   

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            IsInteracting = true;

           
        }
        

        if (context.canceled)
        {
            IsInteracting = false;

            if(TheObjectMaster != null && TheObjectMaster.hasObjectClimbPoint) 
            {
                TheObjectMaster.ObjectClimbPoint.IsVaulting = true;
            }
        }
    }
    void TheScanRay()
    {

        RaycastHit hit;
        

            if (Physics.Raycast(FP_Camera.transform.localPosition, FP_Camera.transform.TransformDirection(Vector3.forward), out hit, 2.5f, ObjectsLayerMask))
            {
            

                 //Check for Object ToolTip
                 //Object ToolTip is the master component for all subsequent object componenets
                if (hit.collider.transform.GetComponent<ObjectMaster>() != null)
                {
                   
                    
                     TheObjectMaster = hit.collider.transform.GetComponent<ObjectMaster>();
                     //Get the tooltip text from the object
                     TheObjectToolTip = TheObjectMaster.ToolTip;

                    //Set the Text to be displayed = to the tooltip stored on the object
                    theDisplayTextMeshPro.text = TheObjectToolTip;
                    //Display the text
                    theDisplayText.SetActive(true);


                    

                }



                //All these "is the object a..."  checks could be consolidated maybe?
                //But I don't know yet if these will all be similar in the end?
                //Another later problem 
           
                //Is the object a grabbable?
                if (TheObjectMaster.hasObjectGrabbing == true)
                {

                     var TheGrabbedObject = TheObjectMaster.ObjectGrabbing;
                    Grabbing(TheGrabbedObject);


                }

                //Is the object a switch?
                if (TheObjectMaster.hasObjectSwitch == true)
                {
                    //Debug.Log("Did Hit ObjectSwitch");
                    var ObjectSwitch = TheObjectMaster.ObjectSwitch;

                    if (IsInteracting == true)
                    {
                        ObjectSwitch.FlipSwitch = !ObjectSwitch.FlipSwitch;
                    IsInteracting = false; //this stops interaction as soon as the first interaction tick happens
                    }
                    

                }

                //Is the object a Climbing Point
                if (TheObjectMaster.hasObjectClimbPoint == true)
                 {
                var ObjectClimbPoint = TheObjectMaster.ObjectClimbPoint;
                ObjectClimbPoint.PlayerRigidbody = playerRigidbody; 
                    if (IsInteracting == true)
                {
                    ObjectClimbPoint.IsVaulting = false;

                    ObjectClimbPoint.PlayerIsClimbing = !ObjectClimbPoint.PlayerIsClimbing;
                    ObjectClimbPoint.Climbing();
                    IsInteracting = false;

                   
                    }
                }

                   //Is the object a NarrowGap?
                 if (TheObjectMaster.hasObjectNarrowGap == true)
                 {
                    Debug.Log("Did Hit ObjectNarrowGap");
                var ObjectNarrowGap = TheObjectMaster.ObjectNarrowGap;
                ObjectNarrowGap.PlayerCollider = PlayerCollider;
                    if (IsInteracting == true)
                    {
                        ObjectNarrowGap.IsSqueeze = !ObjectNarrowGap.IsSqueeze;
                    ObjectNarrowGap.Squeezing();
                        IsInteracting = false;
                    }

                 }
            }

        
            else //Hide tooltips when ray is not on an objectlayer, clear held object
            {
            
            theDisplayText.SetActive(false);

            }
            //the problem, When it is unseen it must be dropped. 
            // if you are holding something drop it




    }

    

    public void Grabbing(ObjectGrabbing TheGrabbedObject)
    {
        
            TheGrabbedObject.thePlayersGrabObjectPoint = TheGrabObject_Point;
            if (IsInteracting == true)
            {
                TheGrabbedObject.PlayerPickedUp = !TheGrabbedObject.PlayerPickedUp;
                TheGrabbedObject.PickUp();
                IsInteracting = false;
                
            }
        
        
    }
    
}
