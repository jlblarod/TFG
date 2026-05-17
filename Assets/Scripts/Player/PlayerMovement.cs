using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public int facingDirection = 1;
    private Rigidbody2D rb; 
    float horizontal;
    float vertical;

    public Animator animator;
    private bool isKnockback = false;

    public PlayerCombat playerCombat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerCombat.Attack();
        }
    }

    void FixedUpdate()
    {
        if (!isKnockback)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            animator.SetFloat("horizontal", Mathf.Abs(horizontal));
            animator.SetFloat("vertical", Mathf.Abs(vertical));

            Vector2 movement = new Vector2(horizontal, vertical).normalized;
            rb.linearVelocity = movement * speed;
        }
        
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Knockback(Transform enemy, float knockbackForce, float stunDuration)
    {
        isKnockback = true;
        Vector2 knockbackDirection = (transform.position - enemy.position).normalized;
        rb.linearVelocity = knockbackDirection * knockbackForce;
        StartCoroutine(KnockbackCounter(stunDuration));
    }
    IEnumerator KnockbackCounter(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }
}
