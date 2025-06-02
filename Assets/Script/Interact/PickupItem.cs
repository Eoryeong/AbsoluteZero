using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PickupItemData data;
    private ItemBehaviour behaviour;
    private GameObject previewObj;

    private bool tryPickup;
    private float clickDelay;

    private void Awake()
    {
        switch (data.itemType)
        {
            case ItemTypes.Food:
                behaviour = new FoodItem(data);
                break;
            case ItemTypes.Weapon:
                behaviour = new WeaponItem(data);
                break;
            case ItemTypes.Tool:
                behaviour = new ToolItem(data);
                break;
        }

        tryPickup = false;
    }

    public void TryPickupItem()
    {
        UIManager.instance.ItemPickupMenuOpen();
        UIManager.instance.ItemPickupMenuLoreUpdate(data);
        ShowPreview();
        tryPickup = true;
        clickDelay = 0.1f;
    }

    public void UseItem()
    {
        behaviour?.UseItem();
    }

    private void Update()
    {
        clickDelay -= Time.deltaTime;

        if (!tryPickup || clickDelay > 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CloseUIAndDestroyDelay());
            DestroyPreview();
            tryPickup = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            UIManager.instance.CloseMenu();
            DestroyPreview();
            tryPickup = false;
        }
    }

    IEnumerator CloseUIAndDestroyDelay()
    {
        yield return null; // 한 프레임 대기
        UIManager.instance.CloseMenu();
        Destroy(gameObject);
    }

    public void ShowPreview()
    {
        if (previewObj != null)
        {
            Destroy(previewObj);
        }

        previewObj = Instantiate(data.previewPrefab, UIManager.instance.menuItemPreviewPos.position, Quaternion.identity);
        previewObj.layer = LayerMask.NameToLayer("ItemPreview");

        foreach (Transform child in previewObj.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("ItemPreview");
        }

        previewObj.AddComponent<ItemPreviewRotator>();
    }

    private void DestroyPreview()
    {
        Destroy(previewObj);
    }
}
