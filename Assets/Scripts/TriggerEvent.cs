using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [Header("This Script requires being on a object with a trigger collider \n It sets a bool called EventsIsTriggered when the player enters the trigger collider \n")]
    [Header("If IsThisRepeatEvent is toggled true: event bool will reset to false when you exit trigger area")]
    [SerializeField]
    private bool IsThisARepeatEvent = false; 
    public bool EventIsTriggered = false;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == true) {

            EventIsTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (IsThisARepeatEvent == true) {

            if (other.gameObject.CompareTag("Player") == true)
            {

                EventIsTriggered = true;
            }
        }
      

    }





}
