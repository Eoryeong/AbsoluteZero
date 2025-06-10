using UnityEngine;

public class TetrisUI : MonoBehaviour
{
    TetrisInventory Inventory;

    [SerializeField] GameObject slotPrefab;

    void Start()
    {
        Inventory = TetrisInventory.instanceTetris;

        for (int i = 0; i < Inventory.numberSlots; i++)
        {
            var itemUI = Instantiate(slotPrefab, transform);
        }
    }

}
