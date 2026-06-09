using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 10;
    public Transform attackPoint;
    public float attackRange;
    public float knockbackForce;
    public float stunDuration;
    public LayerMask playerLayer;
    private EnemyHealth enemyHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void Attack()
    {
        ApplyDamage(false);
    }

    public void AttackWithKnockback()
    {
        ApplyDamage(true);
    }

    private void ApplyDamage(bool shouldKnockback)
    {
        
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if(hitPlayer != null)
        {
            hitPlayer.GetComponent<PlayerHealth>().changeHealth(-damage);
            if (shouldKnockback)
            {
                hitPlayer.GetComponent<PlayerMovement>().Knockback(transform, knockbackForce, stunDuration);
            }
        }
    }
}
