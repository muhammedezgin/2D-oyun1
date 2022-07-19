using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]


public class Player : MonoBehaviour
{
    Rigidbody2D body2D;
    /// <summary>
    /// bu varlar karakterin h�z� ve z�plama g�c�n� belirlerler.
    /// </summary>
    [Tooltip("karakterin nekadar h�zl� gidece�ini belirler")]
    [Range(0, 20)]
    public float playerSpeed;
    BoxCollider2D box2D;
    CircleCollider2D cir2D;


    //z�plama
    [Tooltip("karakterin nekadar y�kse�e z�playaca��n� belirler")]
    [Range(0, 1500)]
    public float jumpPower;


    [Tooltip("karakterin 2.z�plamada nekadar y�kse�e z�playaca��n� belirler")]
    [Range(0, 30)]
    public float doubleJumpPower;

    internal bool canDoubleJump;

    internal bool canDamage;

    // karakteri d�nd�rme
    bool facingRight = true;



    //yeri bulma
    [Tooltip("karakterin yere de�ip de�medi�ini belirler")]
    public bool isGrounded;
    Transform grounCheck;
    const float GroundCheckRadius = 0.2f;
    [Tooltip("yerin ne oldu�unu belirler")]
    public LayerMask groundLayer;

    //animator vcontrol animasyonlar� kontrol etmeye yarar
    Animator playerAnimController;

    // oyuncu can�
    internal int maxPlayerHealth = 100;
    public int currentPlayerHealth;
    internal bool isHurt;
    GiveDamage giveDamage;

    //oyuncuyu �ld�r  
    internal bool isDead;
    public float deadForce;

    private float knockBackForce;

    void Start()
    {
        // rigidbody ayarlar�
        body2D = GetComponent<Rigidbody2D>(); //rigidbody ula�ma imkan� ald�k
        body2D.gravityScale = 5;
        body2D.freezeRotation = true;
        body2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        grounCheck = transform.Find("GroundCheck");
        // colladir lar� al
        box2D = GetComponent<BoxCollider2D>();
        cir2D = GetComponent<CircleCollider2D>();


        // animator al
        playerAnimController = GetComponent<Animator>();
        // can� max cana e�itle
        giveDamage = FindObjectOfType<GiveDamage>();
        currentPlayerHealth = maxPlayerHealth;

    }

    void Update()
    {
        UpdateAnimations();
        ReduceHealth();
        // e�er can max candsn y�kse se can� max cana e�itle
        // if (currentPlayerHealth > maxPlayerHealth)
        //      currentPlayerHealth = maxPlayerHealth; 

        isDead = currentPlayerHealth <= 0;
        if (isDead)
            KillPlayer();

    }



    // oyunu olu�turan karelerden ba��ms�z olarak �al���r.fizik ile ilgili kodlar� buraya yazacan
    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(grounCheck.position, GroundCheckRadius, groundLayer);


        float h = Input.GetAxis("Horizontal");// hareket ettirme edit projet setting input
        /* float v = Input.GetAxis("Vertical"); w bas�ld���nda yukar� ��kmas�n */

        body2D.velocity = new Vector2(h * playerSpeed, body2D.velocity.y/*v * playerSpeed*/);
        flip(h);

        if(isGrounded)
            canDamage = false;  

    }



    public void Jump()
    {
        //dikey y�n�nde g�� uygular
        //      body2D.velocity = new Vector2(0, jumpPower);
        body2D.AddForce(new Vector2(0, jumpPower));

    }

    public void DoubleJump()
    {
        //dikey y�n�nde ani g�� uygular
        //      body2D.velocity = new Vector2(0, jumpPower);
        body2D.AddForce(new Vector2(0, doubleJumpPower), ForceMode2D.Impulse);

        canDamage = true;

    }

    // karakteri d�nd�rme fonk
    void flip(float h)
    {
        if (h > 0 && !facingRight || h < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

        }
    }

    // animatoru yenileme fonk
    void UpdateAnimations()
    {

        playerAnimController.SetFloat("VelocityX", Mathf.Abs(body2D.velocity.x));
        playerAnimController.SetBool("isGrounded", isGrounded);
        playerAnimController.SetFloat("VelocityY", body2D.velocity.y);
        playerAnimController.SetBool("isDead", isDead);
        if (isHurt)
            playerAnimController.SetTrigger("isHurt");


    }

    // can azaltma fonk
    void ReduceHealth()
    {
        if (isHurt && !isDead)
        {
            // e�er can�m�z 100 ise ozaman can�m�zdan zarar kadar ��kar - zarar 
            // e�er bu kondiyon do�ru ise can - zarar = yeni can
            currentPlayerHealth -= giveDamage.damage;
            isHurt = false;
            // e�er havadaysak sol sa�a dikey y�n�nde g�� uygula
            if (facingRight && !isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce,500), ForceMode2D.Force);
            else if (facingRight && !isGrounded)
                body2D.AddForce(new Vector2(knockBackForce,500), ForceMode2D.Force);
            // e�er yerdeysek sol veya sa�a ani g��  uygu
            if (facingRight && isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, 0), ForceMode2D.Force);
            else if (!facingRight && isGrounded)
            body2D.AddForce(new Vector2(knockBackForce, 0), ForceMode2D.Force);
        }

    }

    // oyuncuyu �ld�rme fonk
    void KillPlayer()
    {

        
        isHurt = false;
        body2D.AddForce(new Vector2(0, deadForce), ForceMode2D.Impulse);
        body2D.drag += Time.deltaTime * 50;
        deadForce -= Time.deltaTime * 20;
        body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        box2D.enabled = false;
        cir2D.enabled = false;
    }
  
}
