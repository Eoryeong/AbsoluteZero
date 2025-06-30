using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField] private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField] private Material green;
    [SerializeField] private Material red;

    public Craft craft_fire;

    private Transform tf_Player;

    private RaycastHit hitInfo;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    void Start()
    {
        tf_Player = Camera.main.transform;
    }

    void Update()
    {
        PreviewPositionUpdate();

        ChangeColor();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Build();
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                transform.position = _location;
            }
        }
    }

    private void Build()
    {
        if (isBuildable())
        {
            TetrisSlot.instanceSlot.itemCountDict[craft_fire.needItemCode] -= craft_fire.needItemNum;
            for (int i = 0; i < craft_fire.needItemNum; i++)
            {
                foreach (TetrisItemSlot slot in TetrisSlot.instanceSlot.itemsInBag)
                {
                    if (slot.item.itemCode == craft_fire.needItemCode)
                    {
                        TetrisSlot.instanceSlot.itemsInBag.Remove(slot);
                        Destroy(slot.gameObject);
                        break;
                    }
                }
            }

            Instantiate(craft_fire.go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(gameObject);
        }
    }

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
