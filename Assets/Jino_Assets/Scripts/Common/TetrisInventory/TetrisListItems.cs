using System.Collections.Generic;
using UnityEngine;

public class TetrisListItems : MonoBehaviour
{
    private Vector2 finalPos;
    private Vector2 startPos;

    float timeUntilClose = 0.5f;
    float startTime = 0;
    float currentTime;

    [SerializeField] GameObject Inventory;
    public GameObject[] prefabs;
    public List<PickupItemData> items = new List<PickupItemData>();

    private bool isInventoryActivated = false;

    //아이템 떨어뜨리는 거 구현할 때 필요
    void Start()
    {
        startPos = new Vector2(3370f, 23f);
        finalPos = new Vector2(337f, 23f);

        for (int i = 0; i < prefabs.Length; i++)
        {
            items.Add(prefabs[i].GetComponent<PickupItem>().data);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I) && currentTime >= timeUntilClose)
        {
            currentTime = startTime;
            if (isInventoryActivated)
            {
                isInventoryActivated = !isInventoryActivated;
                Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x, startPos.y);
            }
            else
            {
                isInventoryActivated = !isInventoryActivated;
                Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(finalPos.x, finalPos.y);
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

}
