using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public GameObject player;
    public string saveFile;
    public GameData data = new GameData();
    public bool hasLoadedData;
    public static string spawnPointName = "";

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
        if (hasLoadedData)
        {
            StartCoroutine(ApplyDataDelayed());
            hasLoadedData = false;
        }
        else
        {
            StartCoroutine(SaveInitialState());
        }
    }

    IEnumerator ApplyDataDelayed()
    {
        yield return null;
        ApplyData();
    }

    IEnumerator SaveInitialState()
    {
        yield return null;
        SaveData();
    }

    public void LoadData()
    {
        if (File.Exists(saveFile))
        {
            string content = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<GameData>(content);
            spawnPointName = data.lastSpawnPoint;
            hasLoadedData = true;
            SceneManager.LoadScene(data.lastScene);
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
        Transform spawnTransform = null;

        if (!string.IsNullOrEmpty(spawnPointName))
        {
            GameObject specific = GameObject.Find(spawnPointName);
            if (specific != null && specific.CompareTag("Respawn"))
            {
                spawnTransform = specific.transform;
                Debug.Log($"Using named spawn point: {spawnPointName}");
            }
            else
            {
                Debug.LogWarning($"Named spawn point '{spawnPointName}' not found or missing 'Respawn' tag.");
            }
        }

        if (spawnTransform == null)
        {
            GameObject fallback = GameObject.FindGameObjectWithTag("Respawn");
            if (fallback != null)
                spawnTransform = fallback.transform;
        }

        if (spawnTransform != null)
            player.transform.position = spawnTransform.position;
        else
            Debug.LogWarning("No spawn point found – player position unchanged.");

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            if (PlayerHealth.isRespawning)
            {
                health.currentHealth = health.maxHealth;
                PlayerHealth.isRespawning = false;
                health.changeHealth(0);
            }
            else
            {
                health.currentHealth = data.health;
                health.healthBar.value = data.health;
            }
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

        player.GetComponent<PlayerMovement>().enabled = true;
        SaveData();
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
            inventory = new SavedItem[inventory.inventorySlots.Length],
            lastScene = SceneManager.GetActiveScene().buildIndex,
            lastSpawnPoint = spawnPointName
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

    public void DeleteSaveData()
    {
        if (File.Exists(saveFile)) File.Delete(saveFile);
        data = new GameData();
        hasLoadedData = false;
    }

    public bool HasSave() => File.Exists(saveFile);

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}