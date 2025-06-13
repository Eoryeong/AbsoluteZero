using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float itemCreateChance;
    public List<int> itemCodeList;

    private void Start()
    {
        float roll = Random.value * 100f;

        if (roll < itemCreateChance)
        {
            int selectedItemIndex = Random.Range(1, itemCodeList.Count + 1) - 1;
            GameObject selectedItem = ItemDatabase.instance.GetItemPrefabByCode(itemCodeList[selectedItemIndex]);
            Instantiate(selectedItem, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("생성X");
        }
    }
}
