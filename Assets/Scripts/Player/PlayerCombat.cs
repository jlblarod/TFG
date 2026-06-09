using NUnit.Framework;
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
    public float knockbackForce = 5f;
    public float stunDuration = 0.3f;
    public float knockbackDuration = 0.15f;

    private bool firstHitDone = false;
    private bool secondHitDone = false;

    private void Update()
    {
        if(timeSinceLastAttack > 0f)
            timeSinceLastAttack -= Time.deltaTime;
    }

    public void Attack()
    {
        if (!firstHitDone && timeSinceLastAttack <= 0f)
        {
            firstHitDone = true;
            animator.SetBool("isAttacking", true);

            DealDamage(false);
        }
    }

    public void AttackWithKnockback()
    {
        if (!secondHitDone)
        {
            secondHitDone = true;
            timeSinceLastAttack = cooldownTime;
            DealDamage(true);
        }
    }

    public void DealDamage(bool shouldKnockback)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.isTrigger) continue;

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.ChangeHealth(-attackDamage);
            }

            if (shouldKnockback)
            {
                EnemyKnockback enemyKnockback = enemy.GetComponent<EnemyKnockback>();
                if (enemyKnockback != null)
                    enemyKnockback.Knockback(transform, knockbackForce, knockbackDuration, stunDuration);
            }
            break;
        }
    }

    public void stopAttack()
    {
        firstHitDone = false;
        secondHitDone = false;
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}