using UnityEngine;

public class ObjectDestruction : MonoBehaviour
{
    [Header("ObjectDestruction sets an object to inactive after its health reaches 0\nUseage:\n This script shoud be on an empty,\nbeneath which on child: is the objects art,EnemyHealth.cs, and the collider to be shot at")]
    [SerializeField]
    private EnemyHealth _TheEnemyHealth;

    [Header("\nPut the objects to be set inactive into _TheChildObjects")]
    
    private bool Test; // there is a bug where headers do not appear above arrays 
    [SerializeField]
    private GameObject[] _theChildObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _TheEnemyHealth = GetComponentInChildren<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_TheEnemyHealth.HP <= 0)
        {
            for (int i = 0; i < _theChildObjects.Length; i++)
            {
                _theChildObjects[i].SetActive(false);
            }
        }
        
    }
}
