using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickUpItem : MonoBehaviour
{

    [Header("An item is something that changes variables on the PlayerStatus Eg health, ammo, weapons, keys etc...\n ")]

    //"Since I only forsee there being a dozen or so items in this game in total  " 
    //"This script will house all reference to all items, equipment etc... that can be found on PlayerStatus "
   
    [SerializeField]
    private PlayerStatus ThePlayerStatus;
    [Header("This Script requires A TriggerEvent to Proc the picking up - and therefor a trigger collider aswell")]
    [SerializeField]
    private TriggerEvent TheTriggerEvent;


    
    public List<string> ItemReferences;

    [Header("\nCopy the item you want's Key from the PlayerStatus.cs Dictionary and paste into to ChosenItem \n Eg. The key may be a string called SniperRifle \nyou can also see them at runtime in ItemReferences" )]
    public string ChosenItem;

    [Header("\nHere is the value you want to pass with the chosenItem\n 0 = false, 1 = true or you may use any other int \n Eg. (SniperRifle , 1) is the same as SniperRifle is true, or (Ammo , 100) gives 100 ammo")]
    public int ChosenItemValue = 0;
    void Start()
    {
        ThePlayerStatus = GameObject.FindWithTag("Player").transform.GetComponent<PlayerStatus>();
        TheTriggerEvent = gameObject.GetComponent<TriggerEvent>();
        foreach(KeyValuePair<string, int> pair in ThePlayerStatus.items)
        {
            ItemReferences.Add(pair.Key);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TheTriggerEvent.EventIsTriggered == true) 
        {
            SetItem();
        }
    }

    void SetItem()
    {
        ThePlayerStatus.items[ChosenItem] = ChosenItemValue;
        //Debug.Log(ThePlayerStatus.items[ChosenItem]);
    }
}
