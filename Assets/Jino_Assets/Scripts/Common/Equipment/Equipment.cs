using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public static Equipment instance;
    public List<EquipmentSlot> equipmentSlot = new List<EquipmentSlot>();

    [SerializeField] GameObject ParentObject;
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

    public void Equip(ItemBehaviour item)
    {
        EquipmentSlot myItem = Instantiate(prefabSlot);
        myItem.itemBehaviour = item;
        myItem.icon.sprite = item.data.itemIcon;
        myItem.transform.SetParent(ParentObject.GetComponent<RectTransform>(), false);
        equipmentSlot.Add(myItem);
    }
}
