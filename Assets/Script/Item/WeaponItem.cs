using UnityEngine;

public class WeaponItem : ItemBehaviour
{
    public WeaponItem(PickupItemData _data) : base(_data)
    {
    }

    public override void UseItem()
    {
        Debug.Log(data.itemName + " ÀåÂø");
    }
}