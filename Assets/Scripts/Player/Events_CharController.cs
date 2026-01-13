using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Events_CharController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump");
        //Debug.Log($"Jump : {context.interaction} ");
        if (context.performed && context.interaction is PressInteraction)
        {
            Debug.Log("Jump press");
            //Debug.Log($"Jump press : {context.interaction}");
        }

        if (context.started && context.interaction is HoldInteraction)
        {
            Debug.Log("Jump hold started");
            //Debug.Log($"Jump hold started : {context.interaction}");
        }

        if (context.canceled && context.interaction is HoldInteraction)
        {
            Debug.Log("Jump hold ended");
            //Debug.Log($"Jump hold ended : {context.interaction}");
        }

  


    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
            Vector2 moveInput = context.ReadValue<Vector2>();
            Debug.Log($"Move : {moveInput} ");
        
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        Debug.Log("Reset");
    }


}
