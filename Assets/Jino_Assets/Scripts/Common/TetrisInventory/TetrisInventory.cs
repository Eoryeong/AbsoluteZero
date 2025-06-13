using UnityEngine;

public class TetrisInventory : BaseUI
{
    public static TetrisInventory instanceTetris;

    private void Awake()
    {
        instanceTetris = this;
    }

    public int numberSlots;
}
