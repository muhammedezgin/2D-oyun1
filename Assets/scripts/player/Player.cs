using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]


public class Player : MonoBehaviour
{
    Rigidbody2D body2D;
    /// <summary>
    /// bu varlar karakterin hýzý ve zýplama gücünü belirlerler.
    /// </summary>
    [Tooltip("karakterin nekadar hýzlý gideceðini belirler")]
    [Range(0, 20)]
    public float playerSpeed;
    BoxCollider2D box2D;
    CircleCollider2D cir2D;


    //zýplama
    [Tooltip("karakterin nekadar yükseðe zýplayacaðýný belirler")]
    [Range(0, 1500)]
    public float jumpPower;


    [Tooltip("karakterin 2.zýplamada nekadar yükseðe zýplayacaðýný belirler")]
    [Range(0, 30)]
    public float doubleJumpPower;

    internal bool canDoubleJump;

    internal bool canDamage;

    // karakteri döndürme
    bool facingRight = true;



    //yeri bulma
    [Tooltip("karakterin yere deðip deðmediðini belirler")]
    public bool isGrounded;
    Transform grounCheck;
    const float GroundCheckRadius = 0.2f;
    [Tooltip("yerin ne olduðunu belirler")]
    public LayerMask groundLayer;

    //animator vcontrol animasyonlarý kontrol etmeye yarar
    Animator playerAnimController;

    // oyuncu caný
    internal int maxPlayerHealth = 100;
    public int currentPlayerHealth;
    internal bool isHurt;
    GiveDamage giveDamage;

    //oyuncuyu öldür  
    internal bool isDead;
    public float deadForce;

    private float knockBackForce;

    void Start()
    {
        // rigidbody ayarlarý
        body2D = GetComponent<Rigidbody2D>(); //rigidbody ulaþma imkaný aldýk
        body2D.gravityScale = 5;
        body2D.freezeRotation = true;
        body2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        grounCheck = transform.Find("GroundCheck");
        // colladir larý al
        box2D = GetComponent<BoxCollider2D>();
        cir2D = GetComponent<CircleCollider2D>();


        // animator al
        playerAnimController = GetComponent<Animator>();
        // caný max cana eþitle
        giveDamage = FindObjectOfType<GiveDamage>();
        currentPlayerHealth = maxPlayerHealth;

    }

    void Update()
    {
        UpdateAnimations();
        ReduceHealth();
        // eðer can max candsn yükse se caný max cana eþitle
        // if (currentPlayerHealth > maxPlayerHealth)
        //      currentPlayerHealth = maxPlayerHealth; 

        isDead = currentPlayerHealth <= 0;
        if (isDead)
            KillPlayer();

    }



    // oyunu oluþturan karelerden baðýmsýz olarak çalýþýr.fizik ile ilgili kodlarý buraya yazacan
    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(grounCheck.position, GroundCheckRadius, groundLayer);


        float h = Input.GetAxis("Horizontal");// hareket ettirme edit projet setting input
        /* float v = Input.GetAxis("Vertical"); w basýldýðýnda yukarý çýkmasýn */

        body2D.velocity = new Vector2(h * playerSpeed, body2D.velocity.y/*v * playerSpeed*/);
        flip(h);

        if(isGrounded)
            canDamage = false;  

    }



    public void Jump()
    {
        //dikey yönünde güç uygular
        //      body2D.velocity = new Vector2(0, jumpPower);
        body2D.AddForce(new Vector2(0, jumpPower));

    }

    public void DoubleJump()
    {
        //dikey yönünde ani güç uygular
        //      body2D.velocity = new Vector2(0, jumpPower);
        body2D.AddForce(new Vector2(0, doubleJumpPower), ForceMode2D.Impulse);

        canDamage = true;

    }

    // karakteri döndürme fonk
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
            // eðer canýmýz 100 ise ozaman canýmýzdan zarar kadar çýkar - zarar 
            // eðer bu kondiyon doðru ise can - zarar = yeni can
            currentPlayerHealth -= giveDamage.damage;
            isHurt = false;
            // eðer havadaysak sol saða dikey yönünde güç uygula
            if (facingRight && !isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce,500), ForceMode2D.Force);
            else if (facingRight && !isGrounded)
                body2D.AddForce(new Vector2(knockBackForce,500), ForceMode2D.Force);
            // eðer yerdeysek sol veya saða ani güç  uygu
            if (facingRight && isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, 0), ForceMode2D.Force);
            else if (!facingRight && isGrounded)
            body2D.AddForce(new Vector2(knockBackForce, 0), ForceMode2D.Force);
        }

    }

    // oyuncuyu öldürme fonk
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
