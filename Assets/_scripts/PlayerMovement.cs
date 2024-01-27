using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Animator animator;
    public Rigidbody2D rb;
    private bool isKnockbackActive = false;
    private float knockbackDuration = 1.0f; // Adjust this duration as needed.

    private float knockbackTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private bool isJumping = false;
    private float jumpStartTime;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar hb;
    private bool isDead = false;
    public int CharacterHealth = 100;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackRange2 = 1f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;
    private bool isBlocking = false;
    private bool canTakeDamage = true;



    private bool IsGrounded()
    {
        Vector2 raycastOrigin = transform.position + new Vector3(0f, 0.1f, 0f); // Yükseklik ayarýný ihtiyaca göre deðiþtirin
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.1f);
        return hit.collider != null;
    }

    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        // Knockback uygula
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
    public static PlayerMovement instance;

    public PlayerMovement(bool isBlocking)
    {
        this.isBlocking = isBlocking;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        hb.SetMaxHealth(maxHealth);
        


    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Attack1();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Attack2();
        }






        if (isDashing)
        {
            return;
        }
        if (isKnockbackActive)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= knockbackDuration)
            {
                // Knockback duration has passed, reset the flag.
                isKnockbackActive = false;
                knockbackTimer = 0f;
            }
            return; // Skip player input during knockback.
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(horizontalInput, 0f).normalized * speed;
        animator.SetBool("isMoving", movement.magnitude > 0.1f);
        animator.SetFloat("velocityX", movement.x);
        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        bool grounded = IsGrounded();


        if (!isJumping && IsGrounded())
        {
            animator.SetBool("isJumping", false); // Set the "isJumping" parameter to false
                                                  // Transition to run or idle animations as needed.
        }
        
        


        

        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }



        // Rest of your code...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }

        if (isJumping && Time.time - jumpStartTime >= 1f)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) &! canDash)
        {
            StartCoroutine(Dash());
            Debug.Log("Ýnputtakla");
        }
       
    
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
            
    }

    private void grass()
    {
        AudioManager.instance.Play("grass");
    }
    private void dodgedone()
    {
        canTakeDamage=true;
    }



    void Jump()
    {
        AudioManager.instance.Play("Jump");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("zýpladý");
        isJumping = true;
        jumpStartTime = Time.time;
        animator.SetBool("isJumping", true);
        // Stop the walk sound
    }

    void Attack1()
    {
        //Play an attack animation
        animator.SetTrigger("at1");
        AudioManager.instance.Play("At1");
        //Detecet enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    
    void Attack2()
    {
        //Play an attack animation
        animator.SetTrigger("at2");
        AudioManager.instance.Play("At2");
        //Detecet enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange2, enemyLayers);
        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("aðýr hasar");
            enemy.GetComponent<Enemy>().TakeDamage(100);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    public void TakeDamageg(int damage = 20)
    {
        if (Input.GetKey(KeyCode.Q))
        {
            canTakeDamage = false;
            isBlocking = true;
            if (isBlocking == true & canTakeDamage == false)
            {
                damage = 0;
            }
            animator.SetTrigger("isBlocking");

        }
        else { 
        currentHealth -= damage;
        
        hb.SetHealth(currentHealth);
        StartCoroutine(DamageCooldown()); // Hasar alýmýna bir saniyelik bekleme süresi ekler
        Debug.Log("hasar aldý");
        AudioManager.instance.Play("ghurt");
        animator.SetTrigger("takehit");
        }
        // You can add additional logic here, like checking for death.
        if (currentHealth <= 0)
            isDead = true;
        if (isDead == true)
        {
            animator.SetTrigger("die");
        }

    }
    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false; // Hasar alýmýný engelle
        yield return new WaitForSeconds(2f); // 1 saniye bekle
        canTakeDamage = true; // Hasar alýmýný tekrar aktif hale getir
    }

    public void StartKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        // Apply the knockback force to the Rigidbody2D.
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // Set the knockback flag to true.
        isKnockbackActive = true;
    }
    private IEnumerator Dash()
    {
        Debug.Log("takla");
        animator.SetTrigger("tp");
        canDash = true;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        // Determine the dash direction based on the character's facing direction
        float dashDirection = spriteRenderer.flipX ? -1f : 1f;
        

        rb.velocity = new Vector2(dashDirection * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = false;

    }




}
