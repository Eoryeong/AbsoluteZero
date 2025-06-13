using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public bool inMenu;

    [SerializeField] private PlayerStatus playerData;
    [SerializeField] private PlayerControll playerControll;

    [Header("PlayerUI")]
    [SerializeField] private GameObject playerUICanvas;
    [SerializeField] private TextMeshProUGUI uiPlayerHp;
    [SerializeField] private TextMeshProUGUI uiPlayerHunger;
    [SerializeField] private TextMeshProUGUI uiPlayerSanity;

    [SerializeField] private TextMeshProUGUI uiSelectItem;

    [SerializeField] private GameObject uiProgress;
    [SerializeField] private Image uiProgressBar;

    [Header("MenuUI")]
    [SerializeField] private GameObject menuUICanvas;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextMeshProUGUI menuTitle;
    [SerializeField] private TextMeshProUGUI menuItemName;
    [SerializeField] private TextMeshProUGUI menuItemLore;
    [SerializeField] private TextMeshProUGUI menuItemMouseLeft;
    [SerializeField] private TextMeshProUGUI menuItemMouseRight;
    [SerializeField] private GameObject recordPanel;
    [SerializeField] private TextMeshProUGUI totalSurvivedTime;
    [SerializeField] private TextMeshProUGUI totalTraveledDistance;
    [SerializeField] private TextMeshProUGUI totalSleepTime;
    [SerializeField] private TextMeshProUGUI totalEatFood;
    [SerializeField] private TextMeshProUGUI totalDrinkWater;
    [SerializeField] private TextMeshProUGUI test;
    [SerializeField] private Button menuBackBtn;
    [SerializeField] private Button menuAcceptBtn;
    public Transform menuItemPreviewPos;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inMenu = false;
    }

    void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.O))
        {
            RecordMenuOpen();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseMenu();
        }
    }

    private void UpdateUI()
    {
        if (playerData == null) return;

        uiPlayerHp.text = "Hp : " + playerData.currentPlayerHp;
        uiPlayerHunger.text = "Hunger : " + playerData.currentPlayerHunger;
        uiPlayerSanity.text = "Sanity : " + playerData.currentPlayerSanity;
    }

    public void CursorVisible(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void SetPlayerUICanvas(bool value)
    {
        playerUICanvas.SetActive(value);
    }

    private void SetMenuUICanvas(bool value)
    {
        menuUICanvas.SetActive(value);
        inMenu = value;
    }

    public void FocusInItem(string itemName)
    {
        uiSelectItem.text = itemName;
        uiSelectItem.gameObject.SetActive(true);
    }

    public void FocusOutItem()
    {
        uiSelectItem.text = "";
        uiSelectItem.gameObject.SetActive(false);
    }

    public void ShowProgress(float pct)
    {
        uiProgress.SetActive(true);
        uiProgressBar.fillAmount = pct;
    }

    public void HideProgress()
    {
        uiProgress.SetActive(false);
    }

    public void BedMenuOpen()
    {
        SetPlayerUICanvas(false);
        CursorVisible(true);
        playerData.SetPlayerFreeze(true);
        MenuElementAllDisable();

        menuTitle.gameObject.SetActive(true);
        menuAcceptBtn.gameObject.SetActive(true);
        menuBackBtn.gameObject.SetActive(true);
        menuTitle.text = "Go to bed";
        SetMenuUICanvas(true);
    }

    public void ItemPickupMenuOpen()
    {
        MenuElementAllDisable();

        SetPlayerUICanvas(false);
        playerData.SetPlayerFreeze(true);
        SetMenuUICanvas(true);
        menuItemName.gameObject.SetActive(true);
        menuItemLore.gameObject.SetActive(true);
        menuItemMouseLeft.gameObject.SetActive(true);
        menuItemMouseRight.gameObject.SetActive(true);
    }

    public void ItemPickupMenuLoreUpdate(PickupItemData pItem)
    {
        menuItemName.text = pItem.itemName;
        menuItemLore.text = pItem.itemLore;
    }

    public void CloseMenu()
    {
        SetMenuUICanvas(false);
        SetPlayerUICanvas(true);
        CursorVisible(false);
        playerData.SetPlayerFreeze(false);
    }

    public void MenuElementAllDisable()
    {
        menuTitle.gameObject.SetActive(false);
        menuItemName.gameObject.SetActive(false);
        menuItemLore.gameObject.SetActive(false);
        menuItemMouseLeft.gameObject.SetActive(false);
        menuItemMouseRight.gameObject.SetActive(false);
        menuBackBtn.gameObject.SetActive(false);
        menuAcceptBtn.gameObject.SetActive(false);
        recordPanel.gameObject.SetActive(false);
    }

    public void RecordMenuOpen()
    {
        MenuElementAllDisable();

        SetPlayerUICanvas(false);
        playerData.SetPlayerFreeze(true);
        SetMenuUICanvas(true);

        GameRecode.instance.AddRecord(GameRecordEvent.Test);

        menuTitle.gameObject.SetActive(true);
        recordPanel.gameObject.SetActive(true);
        totalSurvivedTime.text = "생존한 시간 : " + GameRecode.instance.totalSurvivedTime;
        totalTraveledDistance.text = "이동한 거리 : " + GameRecode.instance.totalTraveledDistance;
        totalSleepTime.text = "잠을 잔 시간 : " + GameRecode.instance.totalSleepTime;
        totalEatFood.text = "먹은 음식의 수 : " + GameRecode.instance.totalEatFood;
        totalDrinkWater.text = "마신 물의 양 : " + GameRecode.instance.totalDrinkWater;
        test.text = "해당 메뉴를 열은 횟수 : " + GameRecode.instance.test;
    }
}
