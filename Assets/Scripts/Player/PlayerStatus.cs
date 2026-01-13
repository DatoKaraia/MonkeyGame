using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerStatus : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI HealthTMP;

    [Header("This script will house all player stats to reference such as hp ammo, what weapons you have etc... \n Note this is a really nonsensical implemetations, there is no reason to be a dictionary")]
    //This Dictionary should be populated with the variables
    

    public Dictionary<string, int> items = new()
        {
               {"SniperRifle", 0},
               {"Health", 10} 

        };


    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HealthTMP.text = "HEALTH" + items["Health"];
    }
}
