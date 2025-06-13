using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    [SerializeField] private List<GameObject> itemPrefabList;
    private List<PickupItemData> ItemDataList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        ItemDataList = new List<PickupItemData>();
    }

    private void Start()
    {
        foreach (GameObject obj in itemPrefabList)
        {
            if (obj == null) continue;

            PickupItem pickup = obj.GetComponent<PickupItem>();
            if (pickup != null && pickup.data != null)
            {
                ItemDataList.Add(pickup.data);
            }
            else
            {
                Debug.LogWarning($"프리팹 {obj.name}에 PickupItem 또는 data가 없습니다.");
            }
        }
    }

    public GameObject GetItemPrefabByCode(int code)
    {
        foreach (GameObject obj in itemPrefabList)
        {
            PickupItem pickup = obj.GetComponent<PickupItem>();
            if (pickup != null && pickup.data != null && pickup.data.itemCode == code)
            {
                return obj;
            }
        }

        Debug.LogWarning($"itemCode {code}에 해당하는 프리팹을 찾지 못했습니다.");
        return null;
    }

    public PickupItemData GetItemDataByCode(int code)
    {
        foreach (PickupItemData data in ItemDataList)
        {
            if (data.itemCode == code)
            {
                return data;
            }
        }

        Debug.LogWarning($"itemCode {code}에 해당하는 데이터가 없습니다.");
        return null;
    }
}
