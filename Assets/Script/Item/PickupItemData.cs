using UnityEngine;

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
    public ItemTypes itemType;
    public string itemName;
    public int itemCode;
    public string itemLore;
    public GameObject previewPrefab;

}
