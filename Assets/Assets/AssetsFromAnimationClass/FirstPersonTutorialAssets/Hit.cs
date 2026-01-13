using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Hit : MonoBehaviour
{
    private Animator theAnimator;

    private void Start()
    {
        theAnimator = GetComponent<Animator>();
    }
    public void OnHit(InputValue value)
    {
        theAnimator.SetTrigger("hit");
    }
    public void OnChange(InputValue value)
    {
        theAnimator.SetTrigger("change");
    }
}