using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float detectionRange = 5f;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private float attackTimer;
    private int facingDirection = -1;
    private EnemyState enemyState;
    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            } else if (enemyState == EnemyState.Attacking)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void Chase()
    {
        if(player.position.x > transform.position.x && facingDirection == -1 || player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void CheckForPlayer()
 {
     Collider2D hits = Physics2D.OverlapCircle(detectionPoint.position, detectionRange, playerLayer);

     if (hits is not null)
     {
         player = hits.transform;

         if (Vector2.Distance(transform.position, player.position) <= attackRange && attackTimer <= 0)
         {
             attackTimer = attackCooldown;
             ChangeState(EnemyState.Attacking);
         }

         else if (Vector2.Distance(transform.position, player.position) >= attackRange && enemyState != EnemyState.Attacking)
         {
             ChangeState(EnemyState.Chasing);
         }
     }
     else
     {
         rb.linearVelocity = Vector2.zero;
         ChangeState(EnemyState.Idle);
     }
 }

    public void ChangeState(EnemyState newState)
    {
        enemyState = newState;
        animator.SetBool("isIdle", newState == EnemyState.Idle);
        animator.SetBool("isChasing", newState == EnemyState.Chasing);
        animator.SetBool("isAttacking", newState == EnemyState.Attacking);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback
}