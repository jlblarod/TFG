[System.Serializable]
public class GameData
{
    public int health;
    public int gold;
    public SavedItem[] inventory;
}

[System.Serializable]
public class SavedItem
{
    public string itemName;
    public int quantity;
}