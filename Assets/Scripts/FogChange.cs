using System.Collections;

using UnityEngine;

public class FogChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private TriggerEvent triggerEvent;
    private float fogValue = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerEvent.EventIsTriggered == true)
        {
            LerpFog();
        }
    }

    IEnumerator LerpFog() { 
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        while (Time.time - startTime <= 500f)  // 0.2f is time until lerp stopps
        {
            fogValue = Mathf.Lerp(0.1f, 0.05f, (Time.time - startTime) / 500f); // lerp from A to B 
            RenderSettings.fogDensity = fogValue;
            yield return 1; // wait for next frame
            RenderSettings.fogDensity = fogValue;
        }
       
    }
}
