using UnityEngine;

public class FoodItem : ItemBehaviour
{
    public FoodItem(PickupItemData _data) : base(_data)
    {
    }

    public override void UseItem()
    {
        Debug.Log(data.itemName + " ¸Ô±â");
    }
}
