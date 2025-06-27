using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public static Equipment instance;
    public List<EquipmentSlot> equipmentSlot = new List<EquipmentSlot>();

    [SerializeField] GameObject ParentUpper;
    [SerializeField] GameObject ParentLower;
    [SerializeField] EquipmentSlot prefabSlot;

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
    }

    public void EquipUpper(ItemBehaviour item)
    {
        EquipmentSlot myItem = Instantiate(prefabSlot);
        myItem.itemBehaviour = item;
        myItem.icon.sprite = item.data.itemIcon;
        myItem.transform.SetParent(ParentUpper.GetComponent<RectTransform>(), false);
        equipmentSlot.Add(myItem);
    }

    public void EquipLower(ItemBehaviour item)
    {
        EquipmentSlot myItem = Instantiate(prefabSlot);
        myItem.itemBehaviour = item;
        myItem.icon.sprite = item.data.itemIcon;
        myItem.transform.SetParent(ParentLower.GetComponent<RectTransform>(), false);
        equipmentSlot.Add(myItem);
    }
}
