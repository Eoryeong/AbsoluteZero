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
    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private Craft[] craft_fire;

    private GameObject go_Preview;
    private GameObject go_Prefab;

    private int selectedItemCode;
    private int selectedItemNum;

    [SerializeField] private Transform tf_Player;


    public void SlotClick(int _slotNumber)
    {
        if (CheckIngredient(_slotNumber))
        {
            go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
            go_Preview.GetComponent<PreviewObject>().craft_fire = craft_fire[_slotNumber];
            go_Prefab = craft_fire[_slotNumber].go_Prefab;
            selectedItemCode = craft_fire[_slotNumber].needItemCode;
            selectedItemNum = craft_fire[_slotNumber].needItemNum;

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
