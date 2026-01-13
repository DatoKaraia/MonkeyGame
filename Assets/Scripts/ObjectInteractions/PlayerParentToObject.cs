using UnityEngine;

public class PlayerParentToObject : MonoBehaviour
{
    [Header("This script Childs the player to the scripts gameobject \n The script should be attached to a box collider Trigger and a rigidbody. \n the Rigidbody settings: \n isKinematic = true \n Use gravity false \n Interpolate false \n Collision detection continous")]
    public Transform player;
    public Rigidbody PlayerRigidbody;
    public Transform parentTransform;
    private RigidbodyConstraints originalConstraints;
    public Joint TheJoint;
    private void Awake()
    {
        originalConstraints = PlayerRigidbody.constraints;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player != null)
            {
               player.parent = parentTransform;
                PlayerRigidbody.interpolation = RigidbodyInterpolation.None;
                //PlayerRigidbody.isKinematic = true;
                PlayerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                //PlayerRigidbody.useGravity = false;
                
                //PlayerRigidbody.constraints = originalConstraints | RigidbodyConstraints.FreezePositionY;
                //TheJoint.connectedBody = PlayerRigidbody;
                
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
       // PlayerRigidbody.AddForce(new Vector3(0, 1, 0) * 5f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player != null)
            {

                player.parent = null;
                PlayerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                PlayerRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                //PlayerRigidbody.isKinematic = false;
                //PlayerRigidbody.useGravity = true;
                //TheJoint.connectedBody = null;
                //PlayerRigidbody.constraints = originalConstraints;
            }
        }
        
    }
}
