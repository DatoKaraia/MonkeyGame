using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTakingDamage : MonoBehaviour
{
    [Header("This should be on the gameobject that has the collider and is tagged enemy")]
    private EnemyDamage TheEnemiesDamage;
    [SerializeField]
    private PlayerStatus ThePlayerStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ThePlayerStatus.items["Health"] == 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer == 8)
        {
            if (collision.gameObject.GetComponent<EnemyDamage>() != null) {
                var Damagetaken = collision.gameObject.GetComponent<EnemyDamage>().DamageValue;
                ThePlayerStatus.items["Health"] -= Damagetaken;
            }
            
        }
    }
}
