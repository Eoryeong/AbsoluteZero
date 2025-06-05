using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TetrisItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Vector2 size = new Vector2(70f, 70f);
    public TetrisItem item;

    public Vector2 startPosition;
    public Vector2 oldPosition;
    public Image icon;

    TetrisSlot slots;


    void Start()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * size.y);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * size.x);

        foreach (RectTransform child in transform)
        {
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * child.rect.height);
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * child.rect.width);

            foreach (RectTransform iconChild in child)
            {
                iconChild.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * iconChild.rect.height);
                iconChild.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * iconChild.rect.width);
                iconChild.localPosition = new Vector2(child.localPosition.x + child.rect.width / 2, child.localPosition.y + child.rect.height / 2 * -1f);
            }

        }

        slots = TetrisSlot.instanceSlot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = transform.GetComponent<RectTransform>().anchoredPosition;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        for (int i = 0; i < item.itemSize.y; i++)
        {
            for (int j = 0; j < item.itemSize.x; j++)
            {
                slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;

            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = GetComponent<RectTransform>().anchoredPosition;

            Vector2 finalSlot;
            finalSlot.x = Mathf.Floor(finalPos.x / size.x);
            finalSlot.y = Mathf.Floor(-finalPos.y / size.y);

            if (((int)(finalSlot.x) + (int)(item.itemSize.x) - 1) < slots.maxGridX && ((int)(finalSlot.y) + (int)(item.itemSize.y) - 1) < slots.maxGridY && ((int)(finalSlot.x)) >= 0 && (int)finalSlot.y >= 0)
            {
                List<Vector2> newPosItem = new List<Vector2>();
                bool fit = false;

                for (int sizeY = 0; sizeY < item.itemSize.y; sizeY++)
                {
                    for (int sizeX = 0; sizeX < item.itemSize.x; sizeX++)
                    {
                        if (slots.grid[(int)finalSlot.x + sizeX, (int)finalSlot.y + sizeY] != 1)
                        {
                            Vector2 pos;
                            pos.x = (int)finalSlot.x + sizeX;
                            pos.y = (int)finalSlot.y + sizeY;
                            newPosItem.Add(pos);
                            fit = true;
                        }
                        else
                        {
                            fit = false;
                            Debug.Log("nao deu" + startPosition);

                            this.transform.GetComponent<RectTransform>().anchoredPosition = oldPosition; //back to old pos
                            sizeX = (int)item.itemSize.x;
                            sizeY = (int)item.itemSize.y;
                            newPosItem.Clear();

                        }

                    }
                }

                if (fit)
                {
                    for (int i = 0; i < item.itemSize.y; i++) //through item Y
                    {
                        for (int j = 0; j < item.itemSize.x; j++) //through item X
                        {
                            slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0; //clean old pos

                        }
                    }

                    for (int i = 0; i < newPosItem.Count; i++)
                    {
                        slots.grid[(int)newPosItem[i].x, (int)newPosItem[i].y] = 1; // add new pos
                    }

                    this.startPosition = newPosItem[0]; // set new start position
                    transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosItem[0].x * size.x, -newPosItem[0].y * size.y);
                    Debug.Log("Position: " + transform.GetComponent<RectTransform>().anchoredPosition);
                }
                else //item voltou pra mesma posição da bag e marca com 1
                {
                    for (int i = 0; i < item.itemSize.y; i++) //through item Y
                    {
                        for (int j = 0; j < item.itemSize.x; j++) //through item X
                        {
                            slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 1; //back to position 1;

                        }
                    }
                }

            }
            else
            { // out of index, back to the old pos
                this.transform.GetComponent<RectTransform>().anchoredPosition = oldPosition;
            }
        }
        else
        {
            //템 떨구기
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
