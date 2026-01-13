using System.Collections;
using UnityEngine;

public class ObjectLerpToPoint : MonoBehaviour
{
    [Header("An empty transform, to move to")]
    public GameObject EndPoint;
    [Header("The object with the ObjectFlipSwitch script to activate this")]
    public ObjectSwitch TheActivationSwitch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TheActivationSwitch.FlipSwitch == true)
        {
            StartCoroutine(LerpObject());
        }
        
    }

    IEnumerator LerpObject()
    {
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 1000000f)  // 0.2f is time until lerp stopps
        {
            transform.position = Vector3.Lerp(this.transform.position, EndPoint.transform.position, (Time.time - startTime)/100000f); // lerp from A to B 
            
            if(Vector3.Distance(this.transform.position, EndPoint.transform.position) < 0.01f)
            {
                yield break;
            }

            yield return 1; // wait for next frame
        }
        this.gameObject.transform.SetParent(EndPoint.transform, true);
    }

}
