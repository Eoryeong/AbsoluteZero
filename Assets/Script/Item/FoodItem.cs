using UnityEngine;

public class FoodItem : ItemBehaviour
{
    public FoodItem(PickupItemData _data) : base(_data)
    {
    }

    public override void UseItem()
    {
        Debug.Log(data.itemName + " 먹기");

        PlayerStatusManager.Instance.Heal(data.healAmount);
        PlayerStatusManager.Instance.AddCurrentHunger(data.hungerAmount);
        PlayerStatusManager.Instance.AddCurrentThirst(data.thirstAmount);
    }
}
