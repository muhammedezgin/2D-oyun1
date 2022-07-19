using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerEnemyControl : MonoBehaviour
{


    Rigidbody2D enemyBody2D;
    public float enemySpeed;
    EnemyHealth enemyHealth;
    Animator flowerEnemyAnim;

    //duvar� bulma
    [Tooltip("karakterin duvara de�ip de�medi�ini belirler")]
     bool isGrounded;
    Transform grounCheck;
    const float GroundCheckRadius = 0.2f;
    [Tooltip("duvar�n ne oldu�unu belirler")]
    public LayerMask groundLayer;

    public bool moveRight;

    // yere d���mesini bulma
    bool onEdge;
    Transform edgeCheck;


    // Start is called before the first frame update
    void Start()
    {
        
        enemyBody2D = GetComponent<Rigidbody2D>();
        grounCheck = transform.Find("GroundCheck");
        edgeCheck = transform.Find("EdgeCheck");
        flowerEnemyAnim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
    
        // duvara de�iyormu bak
        isGrounded = Physics2D.OverlapCircle(grounCheck.position, GroundCheckRadius, groundLayer);
        onEdge = Physics2D.OverlapCircle(edgeCheck.position, GroundCheckRadius, groundLayer);

        if (isGrounded || !onEdge)
            moveRight = !moveRight;
     
       // enemyBody2D.velocity = new Vector2(enemySpeed, 0);
        enemyBody2D.velocity = (moveRight) ? new Vector2(enemySpeed, 0) : new Vector2(-enemySpeed, 0);
        transform.localScale = (moveRight) ? new Vector2(-1, 1) : new Vector2(1, 1);
        
       
    }
}
