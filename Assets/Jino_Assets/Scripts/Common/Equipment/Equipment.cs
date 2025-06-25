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

    public void Equip(ItemBehaviour item, Vector3 position)
    {
        EquipmentSlot myItem = Instantiate(prefabSlot, position, Quaternion.identity);

    }
}
