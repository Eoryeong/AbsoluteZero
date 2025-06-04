using System.Collections.Generic;
using UnityEngine;

public class TetrisSlot : MonoBehaviour
{
    public static TetrisSlot instanceSlot;

    private void Awake()
    {
        instanceSlot = this;
    }

    public int[,] grid;

    TetrisInventory Inventory;

    public List<TetrisItemSlot> itensInBag = new List<TetrisItemSlot>();

    public int maxGridX;
    public int maxGridY;

    [SerializeField] TetrisItemSlot prefabSlot;
    Vector2 cellSize = new Vector2(70f, 70f);

    List<Vector2> posItemNaBag = new List<Vector2>();

    private void Start()
    {
        Inventory = TetrisInventory.instanceTetris;

        maxGridX = 10;
        maxGridY = (int)(Inventory.numberSlots + 1) / maxGridX;

        grid = new int[maxGridX, maxGridY];
    }


}
