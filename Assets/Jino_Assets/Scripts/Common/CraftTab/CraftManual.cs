using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;
    public int needItemCode;
    public int needItemNum;
    public GameObject go_Prefab;
    public GameObject go_PreviewPrefab;
}

public class CraftManual : BaseUI
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private Craft[] craft_fire;

    private GameObject go_Preview;
    private GameObject go_Prefab;

    private int selectedItemCode;
    private int selectedItemNum;

    [SerializeField] private Transform tf_Player;

    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;


    void Update()
    {
        if (isPreviewActivated)
            PreviewPositionUpdate();

        if (Input.GetKeyDown(KeyCode.Escape) && isPreviewActivated)
            Cancel();
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            TetrisSlot.instanceSlot.itemCountDict[selectedItemCode] -= selectedItemNum;
            for (int i = 0; i < selectedItemNum; i++)
            {
                foreach (TetrisItemSlot slot in TetrisSlot.instanceSlot.itemsInBag)
                {
                    if (slot.item.itemCode == selectedItemCode)
                    {
                        TetrisSlot.instanceSlot.itemsInBag.Remove(slot);
                        Destroy(slot.gameObject);
                        break;
                    }
                }
            }

            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
            selectedItemCode = 0;
            selectedItemNum = 0;
        }
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        selectedItemCode = 0;
        selectedItemNum = 0;

        go_BaseUI.GetComponent<GameMenuController>().HandlePanelToggle(PanelType.Crafting);
    }

    public void SlotClick(int _slotNumber)
    {
        if (CheckIngredient(_slotNumber))
        {
            go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
            go_Prefab = craft_fire[_slotNumber].go_Prefab;
            selectedItemCode = craft_fire[_slotNumber].needItemCode;
            selectedItemNum = craft_fire[_slotNumber].needItemNum;
            isPreviewActivated = true;

            UIManager.Instance.CursorVisible(false);
            PlayerManager.Instance.SetPlayerFreeze(false);
            go_BaseUI.GetComponent<GameMenuController>().HandlePanelToggle(PanelType.Crafting);
        }
    }

    public bool CheckIngredient(int _slotNumber)
    {
        if (TetrisSlot.instanceSlot.itemCountDict.ContainsKey(craft_fire[_slotNumber].needItemCode))
        {
            if (TetrisSlot.instanceSlot.itemCountDict[craft_fire[_slotNumber].needItemCode] < craft_fire[_slotNumber].needItemNum)
            {
                Debug.Log("아이템 재료의 개수가 부족합니다.");
                return false;
            }
        }
        else
        {
            Debug.Log("아이템의 재료 자체가 없습니다.");
            return false;
        }

        return true;
    }
}
