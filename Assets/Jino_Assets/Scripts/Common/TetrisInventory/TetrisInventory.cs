using UnityEngine;

public class TetrisInventory : MonoBehaviour
{
    public static TetrisInventory instanceTetris;

    private void Awake()
    {
        instanceTetris = this;
    }

    public int numberSlots;
}
