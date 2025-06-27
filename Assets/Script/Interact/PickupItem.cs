using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PickupItemData data;
    private ItemBehaviour behaviour;
    private GameObject previewObj;

    private bool tryPickup;
    private float clickDelay;

    // 고기 굽기용 변수
    public float grillTime;

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
            case ItemTypes.Material:
                behaviour = new ToolItem(data);
                break;
            case ItemTypes.Clothing:
                behaviour = new ToolItem(data);
                break;
        }

        tryPickup = false;
        grillTime = data.grillTime;
    }

    public void TryPickupItem()
    {
        UIManager.Instance.ItemPickupMenuOpen();
        UIManager.Instance.ItemPickupMenuLoreUpdate(data);
        SoundManager.Instance.PlayItemPickup();
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
            UIManager.Instance.CloseMenu();
            DestroyPreview();
            tryPickup = false;
        }
    }

    IEnumerator CloseUIAndDestroyDelay()
    {
        yield return null; // 한 프레임 대기
        TetrisSlot.instanceSlot.addInFirstSpace(behaviour);
        UIManager.Instance.CloseMenu();
        Destroy(gameObject);
    }

    public void ShowPreview()
    {
        if (previewObj != null)
        {
            Destroy(previewObj);
        }

        previewObj = Instantiate(data.previewPrefab, UIManager.Instance.menuItemPreviewPos.position, data.previewPrefab.gameObject.transform.rotation);
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
