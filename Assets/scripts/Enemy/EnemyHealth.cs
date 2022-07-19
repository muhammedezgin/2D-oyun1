using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxEnemyHealth;
    public  float currentEnemyHealth;
    internal bool gotDamage;
    public float damage;
    public GameObject deadthParticle;
    SpriteRenderer graph;
    CircleCollider2D cir2D;
    Rigidbody2D body2D;
    Player player;
   

    // Start is called before the first frame update
    void Start()
    {
        //graph = GameObject.Find("DeadParticle").GetComponent<SpriteRenderer>();
        graph = GetComponent<SpriteRenderer>();
        currentEnemyHealth = maxEnemyHealth;
        player = FindObjectOfType<Player>();
        cir2D = GetComponent<CircleCollider2D>();
        body2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemyHealth <= 0)
        {
            graph.enabled = false;
            cir2D.enabled = false;
            deadthParticle.SetActive(true);
            body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            Destroy(gameObject, 1);

        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerItem" && player.canDamage)
        {
         currentEnemyHealth -= damage;
            Destroy(gameObject, 1);
        }

    }
}
