using UnityEngine;

public abstract class ItemBehaviour
{
    public PickupItemData data;

    public ItemBehaviour(PickupItemData _data)
    {
        this.data = _data;
    }

    public abstract void UseItem();
}
