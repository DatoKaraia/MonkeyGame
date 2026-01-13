using UnityEngine;
using UnityEngine.AI;

public class MonkeyBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EnemyHealth EnemyHealth;
    private SpriteRenderer TheSpriteRenderer;
    private NavMeshAgent TheNavAgent;
    [SerializeField]
    private Sprite Monkey1;
    [SerializeField]    
    private Sprite Monkey4;
    void Start()
    {
        TheNavAgent = gameObject.GetComponent<NavMeshAgent>();

        TheNavAgent.speed = 5.5f;
        TheSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        EnemyHealth = gameObject.GetComponentInChildren<EnemyHealth>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyHealth.HP < EnemyHealth.MaxHP) {
            TheSpriteRenderer.sprite = Monkey4;
            TheNavAgent.speed = 9f;
        }
    }
}
