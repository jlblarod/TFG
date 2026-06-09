using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;
    public bool isBoss = false;
    public ItemSO itemDrop;
    public GameObject lootPrefab;
    public SceneChanger sceneChanger;
    
    private void Start()        
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (currentHealth > 0) return;
        
        if (itemDrop != null && !isBoss)
        {
            int goldAmount = Random.Range(1, 6);
            Loot loot = Instantiate(lootPrefab, transform.position, Quaternion.identity).GetComponent<Loot>();
            loot.Initialize(itemDrop, goldAmount, true);
        }
        if (isBoss)
        {
            foreach (EnemyHealth enemy in FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None))
            {
                if (!enemy.isBoss)
                {
                    Destroy(enemy.gameObject);
                }
            }

            Destroy(gameObject);
            sceneChanger.StartEndGameSequence();
            return;
        }

        Destroy(gameObject);
    }
}