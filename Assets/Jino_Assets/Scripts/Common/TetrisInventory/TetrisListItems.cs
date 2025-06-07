using System.Collections.Generic;
using UnityEngine;

public class TetrisListItems : MonoBehaviour
{
    public GameObject[] prefabs;
    public List<PickupItemData> items = new List<PickupItemData>();

    //아이템 떨어뜨리는 거 구현할 때 필요
    void Start()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            items.Add(prefabs[i].GetComponent<PickupItem>().data);
        }
    }
}
