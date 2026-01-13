using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Collider Collider;
    public bool TakenDamage = false;
    public int MaxHP = 1;
    public int HP = 1;
    private GameObject TheArtAssets;
    private SpriteRenderer TheSpriteRenderer;

    void Start()
    {
        TheSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HP < MaxHP)
        {
            TakenDamage = true;
            TheSpriteRenderer.color = Color.red;
        }
    }
}
