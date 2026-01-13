using UnityEngine;

public class ObjectMaster : MonoBehaviour
    //The Object master sits atop any object interactions on the object side and is the only script directly communicating to the player via PlayerToObjectInteraction

{
    [Header("Write a ToolTip \n describing how to interact with this object \n and it will be displayed to players when the see it")]
    public string ToolTip = "";
    [Header("Check IsOnlyToolTip if you only want to use this as a tooltip \n  with no extra object Behaviours")]
    [SerializeField]
    private bool IsOnlyToolTip = false;

    [Header("Don't set these in inspector it should happen automatically")]
    public ObjectGrabbing ObjectGrabbing;
    public ObjectSwitch ObjectSwitch;
    public ObjectClimbPoint ObjectClimbPoint;
    public ObjectNarrowGap ObjectNarrowGap;

    public bool hasObjectGrabbing = false;
    public bool hasObjectSwitch = false;
    public bool hasObjectClimbPoint = false;    
    public bool hasObjectNarrowGap = false;

    private void Start()
    {
        if(IsOnlyToolTip == false)
        {
            if (this.GetComponent<ObjectGrabbing>() != null)
            {
                ObjectGrabbing = this.GetComponent<ObjectGrabbing>();
                hasObjectGrabbing = true;
            }

            if (this.GetComponent<ObjectSwitch>() != null)
            {
                ObjectSwitch = this.GetComponent<ObjectSwitch>();
                hasObjectSwitch = true;
            }

            if (this.GetComponent<ObjectClimbPoint>() != null)
            {
                ObjectClimbPoint = this.GetComponent<ObjectClimbPoint>();
                hasObjectClimbPoint = true;
            }

            if (this.GetComponent<ObjectNarrowGap>() != null)
            {
                ObjectNarrowGap = this.GetComponent<ObjectNarrowGap>();
                hasObjectNarrowGap = true;
            }
        }
       
    }
}
