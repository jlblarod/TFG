using UnityEngine;

[CreateAssetMenu(fileName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite itemIcon;

    public bool isGold;
    public int stackSize = 64;

    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public int speed;
    public int attackDamage;

    [Header("Temporary Items")]
    public float duration;
}
