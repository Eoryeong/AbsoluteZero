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

    public bool addInFirstSpace(PickupItemData item)
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
            TetrisItemSlot myItem = Instantiate(prefabSlot);
            myItem.startPosition = new Vector2(posItemNaBag[0].x, posItemNaBag[0].y);
            myItem.item = item;
            myItem.icon.sprite = item.itemIcon;
            myItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            myItem.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
            myItem.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            myItem.transform.SetParent(this.GetComponent<RectTransform>(), false);
            myItem.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            myItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(myItem.startPosition.x * cellSize.x, -myItem.startPosition.y * cellSize.y);
            itensInBag.Add(myItem);

            for (int k = 0; k < posItemNaBag.Count; k++) //upgrade matrix
            {
                int posToAddX = (int)posItemNaBag[k].x;
                int posToAddY = (int)posItemNaBag[k].y;
                grid[posToAddX, posToAddY] = 1;
            }
            posItemNaBag.Clear();

            return true;
        }

        return false;
    }
}
