using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectActivate : MonoBehaviour
{
    [Header("Usage: \n This script uses an objectSwitch or EventsTrigger to set objects active/inactive \n it should be on a empty parent object holding the gameObject you want turned on and off \n Setup Example\n PARENT: GameObject{empty} + ObjectAtivate.cs -> CHILD: a point light set inactive in inspector \n")]

    [Header("Use a ObjectSwitch or an EventsTrigger")]
    [SerializeField]
    private ObjectSwitch _TheSwitch;
    
    [SerializeField]
    private TriggerEvent _TheTrigger;

    [Header("The Child object is assumed inactive at start - switch will flip to opposite \n if you want to start with an active check StartActive to invert behavior")]

    [SerializeField]
    private bool _StartActive;
    [SerializeField]
    private GameObject[] _theChildObjects;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_TheSwitch != null)
        {
            if (_TheSwitch.FlipSwitch == true && _StartActive == false )
            {
                SetActive();
            }
            else if(_TheSwitch.FlipSwitch == false && _StartActive == false || _TheSwitch.FlipSwitch == true && _StartActive == true)
            {
                SetInactive();
            }
            
           
        }

        if (_TheTrigger != null)
        {
            if (_TheTrigger.EventIsTriggered == true && _StartActive == false )
            {
                SetActive();
            }
            else if (_TheTrigger.EventIsTriggered == false && _StartActive == false)
            {
                SetInactive();
            }
            else if (_TheTrigger.EventIsTriggered == true && _StartActive == true)
            {
                SetInactive();
            }
        }

    }

        void SetActive()
        {
            for (int i = 0; i < _theChildObjects.Length; i++)
            {
                _theChildObjects[i].SetActive(true);
            }
        }

        void SetInactive()
        {
            for (int i = 0; i < _theChildObjects.Length; i++)
            {
                _theChildObjects[i].SetActive(false);
            }
        }
}
