using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public static bool isRespawning = false;
    public Slider healthBar;

    public void changeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.value = currentHealth;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isRespawning = true;
            SaveManager.Instance.hasLoadedData = true;
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
    void Start()
    {
        if(SaveManager.Instance != null && SaveManager.Instance.hasLoadedData)
        {
            currentHealth = SaveManager.Instance.data.health;
        }
        else
        {
            currentHealth = maxHealth;
        }
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}
