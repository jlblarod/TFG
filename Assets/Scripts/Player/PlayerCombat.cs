using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public float cooldownTime = 0.5f;
    public float timeSinceLastAttack = 0f;
    public Transform attackPoint;
    public float weaponRange = 1f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public float knnockbackForce = 5f;
    public float stunDuration = 0.3f;
    public float knockbackDuration = 0.15f;

    private void Update()
    {
        if(timeSinceLastAttack > 0f)
        {
            timeSinceLastAttack -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        ApplyAttack(false);
    }

    public void AttackWithKnockback()
    {
        ApplyAttack(true);
    }

    private void ApplyAttack(bool shouldKnockback)
    {
        if(timeSinceLastAttack <= 0f)
        {
            animator.SetBool("isAttacking", true);
            DealDamage(shouldKnockback);
            timeSinceLastAttack = cooldownTime;
        }
    }

    public void DealDamage(bool shouldKnockback)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.isTrigger)
            {
                continue;
            }

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.ChangeHealth(-attackDamage);
            }

            if (shouldKnockback)
            {
                EnemyKnockback enemyKnockback = enemy.GetComponent<EnemyKnockback>();
                if (enemyKnockback != null)
                {
                    enemyKnockback.Knockback(transform, knnockbackForce, knockbackDuration, stunDuration);
                }
            }

            break;
        }
    }

    public void stopAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}
