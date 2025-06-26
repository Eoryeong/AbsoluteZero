using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public static Equipment instance;
    public List<EquipmentSlot> equipmentSlot = new List<EquipmentSlot>();

    [SerializeField] EquipmentSlot prefabSlot;

    void Start()
    {
        instance = this;
    }

    public void Equip(ItemBehaviour item, GameObject hovered)
    {
        EquipmentSlot myItem = Instantiate(prefabSlot);
        myItem.itemBehaviour = item;
        myItem.icon.sprite = item.data.itemIcon;
        myItem.transform.SetParent(hovered.GetComponent<RectTransform>(), false);
        equipmentSlot.Add(myItem);
    }
}
