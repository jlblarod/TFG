using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public int maxHealth;
    public int currentHealth;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
    }
    public void UpdateCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}