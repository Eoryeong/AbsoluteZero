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


        if (data.hungerAmount > 0)
        {
            GameRecode.instance.AddRecord(GameRecordEvent.EatFood, data.hungerAmount);
        }
        else if (data.thirstAmount > 0)
        {
            GameRecode.instance.AddRecord(GameRecordEvent.DrinkWater, data.thirstAmount);
        }

        switch (data.eatSoundType)
        {
            case 1:
                SoundManager.Instance.PlayFoodSound(SoundManager.FoodType.Eating);
                break;
            case 2:
                SoundManager.Instance.PlayFoodSound(SoundManager.FoodType.Drinking);
                break;
            case 3:
                SoundManager.Instance.PlayFoodSound(SoundManager.FoodType.Crunchy);
                break;
        }
    }
}
