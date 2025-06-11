using UnityEngine;

public class FoodItem : ItemBehaviour
{
    public FoodItem(PickupItemData _data) : base(_data)
    {
    }

    public override void UseItem()
    {
        Debug.Log(data.itemName + " 먹기");
    }

    void Heal(PlayerStatus playerStatus)
    {
        playerStatus.Heal(data.healAmount);
    }

    void Eat(PlayerStatus playerStatus)
    {
        playerStatus.Eat(data.hungerAmount);
    }

    void EatBad(PlayerStatus playerStatus)
    {
        playerStatus.Eat(data.hungerAmount);
        playerStatus.HungerDebuff();

    }

    void Drink(PlayerStatus playerStatus)
    {
        playerStatus.Drink(data.thirstAmount);
    }

    void DrinkBad(PlayerStatus playerStatus)
    {
        playerStatus.Drink(data.thirstAmount);
        playerStatus.ThirstDebuff();
    }
}
