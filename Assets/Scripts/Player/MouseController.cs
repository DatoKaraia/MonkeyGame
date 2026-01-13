using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineInputAxisController axisController;
    public float MouseXSensitivity = 1f;
    public float MouseYSensitivity = 1f; //this should be minus unless inverted
    private void Awake()
    {


    }

    void Start()
    {
        axisController = GetComponentInParent<CinemachineInputAxisController>();
        foreach (var c in axisController.Controllers)
        {
            if (c.Name == "Look Y (Tilt)")
            {
                c.Input.Gain = -MouseYSensitivity;
            }
            if (c.Name == "Look X (Pan)")
            {
                c.Input.Gain = MouseXSensitivity;

            }
           
            
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;


    }

}
