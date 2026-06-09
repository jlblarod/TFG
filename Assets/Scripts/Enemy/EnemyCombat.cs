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

    // Update is called once per frame
    void Update()
    {
        
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
        
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if(hitPlayers.Length > 0)
        {
            hitPlayers[0].GetComponent<PlayerHealth>().changeHealth(-damage);
            if (shouldKnockback)
            {
                hitPlayers[0].GetComponent<PlayerMovement>().Knockback(transform, knockbackForce, stunDuration);
            }
        }
    }
}
