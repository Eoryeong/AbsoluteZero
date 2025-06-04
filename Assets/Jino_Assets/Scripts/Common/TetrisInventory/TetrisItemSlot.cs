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



            }
        }
    }
}
