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

    List<Vector2> posItemNaBag = new List<Vector2>(); // matrix of bag size

    private void Start()
    {
        Inventory = TetrisInventory.instanceTetris;

        maxGridX = 10;
        maxGridY = (int)(Inventory.numberSlots + 1) / maxGridX;

        grid = new int[maxGridX, maxGridY];
    }

    public bool addInFirstSpace(TetrisItem item)
    {
        int contX = (int)item.itemSize.x;
        int contY = (int)item.itemSize.y;

        for (int i = 0; i < maxGridX; i++)
        {
            for (int j = 0; j < maxGridY; j++)
            {
                if (posItemNaBag.Count != (contX * contY))
                {
                    for (int sizeY = 0; sizeY < contY; sizeY++)
                    {
                        for (int sizeX = 0; sizeX < contX; sizeX++)
                        {
                            if ((i + sizeX) < maxGridX && (j + sizeY) < maxGridY && grid[i + sizeX, j + sizeY] != 1)
                            {
                                Vector2 pos;
                                pos.x = i + sizeX;
                                pos.y = j + sizeY;
                                posItemNaBag.Add(pos);
                            }
                            else
                            {
                                sizeX = contX;
                                sizeY = contY;
                                posItemNaBag.Clear();
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        if (posItemNaBag.Count == (contX * contY))  //이미 아이템 있을 때
        {
            return true;
        }

        return false;
    }
}
