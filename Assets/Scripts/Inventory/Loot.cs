using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO item;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public int quantity;
    public bool canBePickedUp = true;
    public static event System.Action<ItemSO, int> OnLootPickedUp;

    private void OnValidate()
    {
        if (item == null)return;
        UpdateSprite();
    }

    public void Initialize(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
        canBePickedUp = false;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = item.itemIcon;
        this.name = item.itemName;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canBePickedUp)
        {
            animator.Play("LootPickup");
            OnLootPickedUp?.Invoke(item, quantity);
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canBePickedUp = true;
        }
    }
}
