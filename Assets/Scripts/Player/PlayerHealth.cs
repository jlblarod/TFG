using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public Slider healthBar;

    public void changeHealth(int amount)
    {
        currentHealth += amount;
        healthBar.value = currentHealth;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            gameObject.SetActive(false);
        }
        //currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
