using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField] private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField] private Material green;
    [SerializeField] private Material red;

    void Update()
    {
        ChangeColor();
    }

    // private void Build()
    // {
    //     if (isBuildable())
    //     {
    //         TetrisSlot.instanceSlot.itemCountDict[selectedItemCode] -= selectedItemNum;
    //         for (int i = 0; i < selectedItemNum; i++)
    //         {
    //             foreach (TetrisItemSlot slot in TetrisSlot.instanceSlot.itemsInBag)
    //             {
    //                 if (slot.item.itemCode == selectedItemCode)
    //                 {
    //                     TetrisSlot.instanceSlot.itemsInBag.Remove(slot);
    //                     Destroy(slot.gameObject);
    //                     break;
    //                 }
    //             }
    //         }

    //         Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
    //         Destroy(go_Preview);
    //         isActivated = false;
    //         isPreviewActivated = false;
    //         go_Preview = null;
    //         go_Prefab = null;
    //         selectedItemCode = 0;
    //         selectedItemNum = 0;
    //     }
    // }

    private void ChangeColor()
    {
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);//초록
    }

    private void SetColor(Material mat)
    {
        foreach (Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);
    }

}
