using UnityEngine;
using System.Collections;

public class UseItem : MonoBehaviour
{
    public void ApplyItemEffect(ItemSO item)
    {
        if (item == null)
        {
            return;
        }

        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (item.currentHealth > 0 && playerHealth != null)
        {
            playerHealth.changeHealth(item.currentHealth);
        }

        if (item.maxHealth > 0)
        {
            if (playerHealth != null)
            {
                playerHealth.maxHealth += item.maxHealth;
                playerHealth.changeHealth(0);
            }
        }

        if(item.duration > 0)
        {
            StartCoroutine(EffectTimer(item, item.duration));
        }
    }

    private IEnumerator EffectTimer(ItemSO item, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (item.currentHealth > 0)
        {
            // Healing is immediate and not reverted.
        }
        if (item.maxHealth > 0)
        {
            PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.maxHealth -= item.maxHealth;
                if (playerHealth.currentHealth > playerHealth.maxHealth)
                {
                    playerHealth.currentHealth = playerHealth.maxHealth;
                }
                playerHealth.changeHealth(0);
            }
        }
    }
}
