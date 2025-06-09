using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TetrisItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Vector2 size = new Vector2(70f, 70f);
    public PickupItemData item;

    public Vector2 startPosition;
    public Vector2 oldPosition;
    public Image icon;

    TetrisSlot slots;

    private bool isDragging = false;
    private bool isRotated = false;


    void Start()
    {

        UpdateItemSize();

        slots = TetrisSlot.instanceSlot;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (isDragging && Input.GetKeyDown(KeyCode.R))
        {
            // x와 y 크기를 교환
            float tempSize = item.itemSize.x;
            item.itemSize.x = item.itemSize.y;
            item.itemSize.y = tempSize;

            // UI 크기 업데이트
            UpdateItemSize();
        }
    }


    public void UpdateItemSize()
    {
        // 기본 크기 설정
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * size.y);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * size.x);

        foreach (RectTransform child in transform)
        {
            // 자식 오브젝트의 원래 크기를 저장
            float originalHeight = size.y;
            float originalWidth = size.x;

            // 크기 설정 시 원래 크기에 itemSize만 곱함
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * originalHeight);
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * originalWidth);

            if (!isDragging)
            {
                foreach (RectTransform iconChild in child)
                {
                    iconChild.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * iconChild.rect.height);
                    iconChild.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * iconChild.rect.width);
                    iconChild.localPosition = new Vector2(child.localPosition.x + child.rect.width / 2, child.localPosition.y + child.rect.height / 2 * -1f);
                }
            }
            else
            {
                foreach (RectTransform iconChild in child)
                {
                    if (!isRotated)
                    {
                        // 아이콘 90도 회전
                        iconChild.rotation = Quaternion.Euler(0, 0, 90);
                        isRotated = true;
                    }
                    else
                    {
                        // 원래 상태로 되돌리기
                        iconChild.rotation = Quaternion.Euler(0, 0, 0);
                        isRotated = false;
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = transform.GetComponent<RectTransform>().anchoredPosition;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        // 배열 범위 체크 추가
        int maxX = (int)startPosition.x + (int)item.itemSize.x;
        int maxY = (int)startPosition.y + (int)item.itemSize.y;

        if (maxX <= slots.maxGridX && maxY <= slots.maxGridY)
        {
            for (int i = 0; i < item.itemSize.y; i++)
            {
                for (int j = 0; j < item.itemSize.x; j++)
                {
                    if ((int)startPosition.x + j >= 0 && (int)startPosition.y + i >= 0)
                    {
                        slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = GetComponent<RectTransform>().anchoredPosition;

            Vector2 finalSlot;
            finalSlot.x = Mathf.Floor(finalPos.x / size.x);
            finalSlot.y = Mathf.Floor(-finalPos.y / size.y);

            // 배열 범위 체크를 더 엄격하게 수정
            bool isValidPosition = ((int)finalSlot.x + (int)item.itemSize.x) <= slots.maxGridX &&
                                 ((int)finalSlot.y + (int)item.itemSize.y) <= slots.maxGridY &&
                                 (int)finalSlot.x >= 0 &&
                                 (int)finalSlot.y >= 0;

            if (isValidPosition)
            {
                List<Vector2> newPosItem = new List<Vector2>();
                bool fit = true; // 초기값을 true로 변경

                // 새로운 위치가 유효한지 먼저 확인
                for (int sizeY = 0; sizeY < item.itemSize.y && fit; sizeY++)
                {
                    for (int sizeX = 0; sizeX < item.itemSize.x && fit; sizeX++)
                    {
                        if (slots.grid[(int)finalSlot.x + sizeX, (int)finalSlot.y + sizeY] != 1)
                        {
                            Vector2 pos;
                            pos.x = (int)finalSlot.x + sizeX;
                            pos.y = (int)finalSlot.y + sizeY;
                            newPosItem.Add(pos);
                        }
                        else
                        {
                            fit = false;
                            newPosItem.Clear();
                        }
                    }
                }

                if (fit && newPosItem.Count == (item.itemSize.x * item.itemSize.y))
                {
                    // 이전 위치 초기화 전에 범위 체크
                    bool canClearOld = (int)startPosition.x >= 0 &&
                                     (int)startPosition.y >= 0 &&
                                     ((int)startPosition.x + (int)item.itemSize.x) <= slots.maxGridX &&
                                     ((int)startPosition.y + (int)item.itemSize.y) <= slots.maxGridY;

                    if (canClearOld)
                    {
                        // 이전 위치 초기화
                        for (int i = 0; i < item.itemSize.y; i++)
                        {
                            for (int j = 0; j < item.itemSize.x; j++)
                            {
                                slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;
                            }
                        }
                    }

                    // 새 위치에 아이템 배치
                    foreach (Vector2 pos in newPosItem)
                    {
                        slots.grid[(int)pos.x, (int)pos.y] = 1;
                    }

                    startPosition = newPosItem[0];
                    transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosItem[0].x * size.x, -newPosItem[0].y * size.y);
                }
                else
                {
                    // 원래 위치로 되돌리기
                    transform.GetComponent<RectTransform>().anchoredPosition = oldPosition;
                    // 원래 위치의 grid 상태 복원
                    if ((int)startPosition.x >= 0 &&
                        (int)startPosition.y >= 0 &&
                        ((int)startPosition.x + (int)item.itemSize.x) <= slots.maxGridX &&
                        ((int)startPosition.y + (int)item.itemSize.y) <= slots.maxGridY)
                    {
                        for (int i = 0; i < item.itemSize.y; i++)
                        {
                            for (int j = 0; j < item.itemSize.x; j++)
                            {
                                slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 1;
                            }
                        }
                    }
                }
            }
            else
            {
                transform.GetComponent<RectTransform>().anchoredPosition = oldPosition;
            }
        }
        else
        {
            //템 떨구기
            PlayerControll player;
            player = FindFirstObjectByType<PlayerControll>();

            TetrisListItems itemInGame; // list of items prefab to could be instantiated when dropping item.
            itemInGame = FindFirstObjectByType<TetrisListItems>();

            for (int t = 0; t < itemInGame.prefabs.Length; t++)
            {
                if (itemInGame.items[t].itemName == item.itemName)
                {
                    Instantiate(itemInGame.prefabs[t].gameObject, new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z + 1.5f), Quaternion.identity); //dropa o item

                    Destroy(this.gameObject);
                    break;
                }

            }
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
