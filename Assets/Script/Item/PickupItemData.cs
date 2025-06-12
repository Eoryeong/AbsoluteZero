using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public enum ItemTypes
{
    Food,
    Weapon,
    Tool,
    Material,
    Etc,
}

[CreateAssetMenu(menuName = "Pickup Item/Create a new Pickup Item")]
public class PickupItemData : ScriptableObject
{
    [Header("Basic Info")]
    public ItemTypes itemType;
    public string itemName;
    public int itemCode;
    public string itemLore;
    public GameObject previewPrefab;
    public float itemWeight;

    [Header("Inventory Variables")]
    public Sprite itemIcon;
    public Vector2 itemSize;
    public bool stackable = false;
    public int maxStackSize = 1;

    [Header("Food")]
    public float healAmount;
    public float hungerAmount;
    public float thirstAmount;
    public float foodDurability;
    public float hungerDecreaseRate;
    public float thirstDecreaseRate;

    [Header("Weapon")]
    public float attackDamage;
    public float attackSpeed;
    public float weaponRange;
    public int weaponDurability;

    [Header("Tool")]
    public int toolEfficiency;
    public int toolDurability;

}
