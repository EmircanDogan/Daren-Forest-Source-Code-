using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class Enemy : SimpleFlashExample
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    private SimpleFlash simpleFlash;
    public Transform attackPoint;
    public float attackRange = 1f; // Ayarlayýn
    public LayerMask playerLayer;
    public int attackDamage = 20;
    public float movementSpeed = 3f; // Ayarlayýn
    public float stoppingDistance = 2f;
    public float attackCooldown = 2f; // Saldýrýlarýn arasýndaki bekleme süresi
    private Transform player;
    public float knockbackForce = 5f; // Knockback kuvveti
    private bool canAttack = true;

    void Start()
    {
        currentHealth = maxHealth;
        simpleFlash = GetComponent<SimpleFlash>();
        player = GameObject.FindGameObjectWithTag("gutz").transform; // Oyuncuyu bul
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetBool("IsMoving", false);
            if (canAttack)
            {
                Attack();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        FlipTowardsPlayer(direction);

        animator.SetBool("IsMoving", true);
    }

    void FlipTowardsPlayer(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            
        }
        else
        {
            simpleFlash.Flash();
        }
    }

    

    void Attack()
    {
        animator.SetTrigger("inRange");

        Collider2D gutz = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (gutz != null)
        {
            gutz.GetComponent<PlayerMovement>().TakeDamageg(attackDamage);

            canAttack = false;
            Invoke("ResetAttackCooldown", attackCooldown);
            // Knockback'u çaðýr
            Vector2 knockbackDirection = (gutz.transform.position - transform.position).normalized;
            gutz.GetComponent<PlayerMovement>().ApplyKnockback(knockbackDirection, knockbackForce);
        }
    }

    void ResetAttackCooldown()
    {
        // Saldýrý arasýndaki bekleme süresini sýfýrla
        canAttack = true;
    }

    // Animation Event olarak çaðrýlacak fonksiyon
    public void EnableAttack()
    {
        // Animasyon süresi kadar bekledikten sonra saldýrýyý tekrar et
        if (canAttack)
        {
            Attack();
        }
    }

    void DestroyEnemy()
    {
        
        Destroy(gameObject);
    }
}
