using System.Collections.Generic;
using UnityEngine;

public class TetrisListItems : MonoBehaviour
{
    public static TetrisListItems instance;

    public GameObject[] prefabs;
    public List<PickupItemData> items = new List<PickupItemData>();

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        for (int i = 0; i < prefabs.Length; i++)
        {
            items.Add(prefabs[i].GetComponent<PickupItem>().data);
        }
    }
}
