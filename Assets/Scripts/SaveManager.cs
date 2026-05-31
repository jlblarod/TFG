using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public GameObject player;
    public string saveFile;
    public GameData data = new GameData();
    public bool hasLoadedData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        saveFile = Application.dataPath + "/savefile.json";
        player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyData();
    }

    public void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string content = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<GameData>(content);
            hasLoadedData = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            Debug.Log("Data loaded. Health: " + data.health);
        }
        else
        {
            Debug.LogWarning("Save file not found. Starting with default data.");
        }
    }

    public void ApplyData()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.currentHealth = data.health;
            health.healthBar.value = data.health;
        }

        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
        if (inventory == null || data.inventory == null) return;

        inventory.gold = data.gold;
        inventory.goldText.text = data.gold.ToString("D2");

        for (int i = 0; i < inventory.inventorySlots.Length; i++)
        {
            if (i >= data.inventory.Length || string.IsNullOrEmpty(data.inventory[i].itemName))
            {
                inventory.inventorySlots[i].item = null;
                inventory.inventorySlots[i].quantity = 0;
            }
            else
            {
                inventory.inventorySlots[i].item = Resources.Load<ItemSO>("Items/" + data.inventory[i].itemName);
                inventory.inventorySlots[i].quantity = data.inventory[i].quantity;
            }
            inventory.inventorySlots[i].UpdateUI();
        }
    }

    public void SaveData()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();

        if (player == null || inventory == null) return;

        GameData newData = new GameData()
        {
            health = player.GetComponent<PlayerHealth>().currentHealth,
            gold = inventory.gold,
            inventory = new SavedItem[inventory.inventorySlots.Length]
        };

        for (int i = 0; i < inventory.inventorySlots.Length; i++)
        {
            newData.inventory[i] = new SavedItem
            {
                itemName = inventory.inventorySlots[i].item != null ? inventory.inventorySlots[i].item.itemName : "",
                quantity = inventory.inventorySlots[i].quantity
            };
        }

        string json = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFile, json);
        data = newData;
        hasLoadedData = true;
        Debug.Log("Data saved. Health: " + newData.health + " Gold: " + newData.gold);
    }

    public bool HasSave() => File.Exists(saveFile);

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}