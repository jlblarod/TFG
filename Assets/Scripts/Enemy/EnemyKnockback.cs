using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyMovement enemyMovement;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    public void Knockback(Transform playerTransform, float knockbackForce, float knockbackDuration, float stunDuration)
    {
     enemyMovement.ChangeState(EnemyState.Knockback);
     StartCoroutine(stunTimer(knockbackDuration, stunDuration));   
        Vector2 knockbackDirection = (transform.position - playerTransform.position).normalized;   
     rb.linearVelocity = knockbackDirection * knockbackForce;
    }
    IEnumerator stunTimer(float knockbackDuration, float stunDuration)
    {
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunDuration);
        enemyMovement.ChangeState(EnemyState.Idle);
    }
}
