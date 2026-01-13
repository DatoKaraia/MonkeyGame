using System.Collections;
using UnityEngine;

public class ObjectNarrowGap : MonoBehaviour
{
    public bool IsSqueeze = false;
    public CapsuleCollider PlayerCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void Squeezing()
    {
        if (IsSqueeze == true)
        {
            

            StartCoroutine(DeflationLerp());

        }
    }
    IEnumerator DeflationLerp()
    {


    float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 1f)  // 1f is Time of lerp duration
         {
             PlayerCollider.radius = Mathf.Lerp(0.5f, 0.1f, Time.time - startTime); // lerp from A to B 

              yield return 1; // wait for next frame
              
         }
        PlayerCollider.radius = Mathf.Lerp(0.5f, 0.1f, 1);
    }

    IEnumerator ReflationLerp()
    {


        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 1f)  // 1f is Time of lerp duration
        {
            PlayerCollider.radius = Mathf.Lerp(0.1f, 0.5f, Time.time - startTime); // lerp from A to B 

            yield return 1; // wait for next frame

        }
        PlayerCollider.radius = Mathf.Lerp(0.1f, 0.5f, 1);
    }





    void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerExit");
        if (other.gameObject.CompareTag("Player") == true)
        {
            IsSqueeze = false;
            StartCoroutine(ReflationLerp());
        }
       
    }

}
