using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public int gold;
    public TextMeshProUGUI goldText;
    public InventorySlot[] inventorySlots;
    public UseItem useItem;
    public GameObject lootPrefab;
    public Transform lootSpawnPoint;

    private void Start()
    {
        foreach (var slot in inventorySlots)
        {
            slot.UpdateUI();
        }
    }
    private void OnEnable()
    {
        Loot.OnLootPickedUp += AddItemToInventory;
    }
    private void OnDisable()
    {
        Loot.OnLootPickedUp -= AddItemToInventory;
    }

    public void AddItemToInventory(ItemSO item, int quantity)
    {
        if (item.isGold)
        {
            gold += quantity;
            goldText.text = gold.ToString("D2");
            return;
        }
        foreach (var slot in inventorySlots)
        {
            if (slot.item == item && slot.quantity < item.stackSize)
            {
                int availableSpace = item.stackSize - slot.quantity;
                int quantityToAdd = Mathf.Min(availableSpace, quantity);
                
                slot.quantity += quantityToAdd;
                quantity -= quantityToAdd;
                
                slot.UpdateUI();

                if(quantity <= 0) return;
            }
        }
        foreach (var slot in inventorySlots)
        {
            if (slot.item == null)
            {
                int quantityToAdd = Mathf.Min(item.stackSize, quantity);
                slot.item = item;
                slot.quantity = quantity;
                slot.UpdateUI();
                return;
            }
        }
        if(quantity > 0)
        {
            DropLoot(item, quantity);
        }
    }

    public void DropItem(InventorySlot slot)
    {
        DropLoot(slot.item, 1);
        slot.quantity--;
        if (slot.quantity <= 0)        
        {
            slot.item = null;
        }
        slot.UpdateUI();
    }

    private void DropLoot(ItemSO item, int quantity)
    {
        Loot loot = Instantiate(lootPrefab, lootSpawnPoint.position, Quaternion.identity).GetComponent<Loot>();
        loot.Initialize(item, quantity, false);
    }

    public void UserItem(InventorySlot slot)
    {
        if (slot.item != null && slot.quantity > 0)
        {
            PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
            if (slot.item.currentHealth > 0 && playerHealth != null &&playerHealth.currentHealth >= playerHealth.maxHealth) return;

            useItem.ApplyItemEffect(slot.item);
            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.item = null;
            }
            slot.UpdateUI();
        }
    }
}