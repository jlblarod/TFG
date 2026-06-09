using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemSO item;
    public int quantity;

    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    public InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || quantity <= 0 || inventoryManager == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryManager.UserItem(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            inventoryManager.DropItem(this);
        }
    }

    public void UpdateUI()
    {
        if(item != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.gameObject.SetActive(true);
            quantityText.text = quantity.ToString("D2");
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
            quantityText.text = "";
        }
    }
}