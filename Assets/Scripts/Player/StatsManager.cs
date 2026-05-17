using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    //public TextMeshProUGUI healthText;

    [Header("Health")]
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
        //healthText.text = $"HP: {currentHealth} / {maxHealth}";
    }
    public void UpdateCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        //healthText.text = $"HP: {currentHealth} / {maxHealth}";
    }
}
