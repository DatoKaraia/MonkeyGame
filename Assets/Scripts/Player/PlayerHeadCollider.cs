using UnityEngine;

public class PlayerHeadCollider : MonoBehaviour
{

    public bool PlayerHeadIsObstructed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null) {
            PlayerHeadIsObstructed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null)
        {
            PlayerHeadIsObstructed = false;
        }
    }
}
