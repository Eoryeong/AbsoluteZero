using UnityEngine;

public class ToolItem : ItemBehaviour
{
    public ToolItem(PickupItemData _data) : base(_data)
    {
    }

    public override void UseItem()
    {
        Debug.Log(data.itemName + " »ç¿ë");
    }
}
