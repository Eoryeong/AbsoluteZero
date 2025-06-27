using UnityEngine;

public enum ItemTypes
{
    Food,
    Weapon,
    Tool,
    Clothing,
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
    public float grillTime;
    public float eatSoundType;

    [Header("Weapon")]
    public float attackDamage;
    public float attackSpeed;
    public float weaponRange;
    public int weaponDurability;

    [Header("Tool")]
    public int toolEfficiency;
    public int toolDurability;

}
