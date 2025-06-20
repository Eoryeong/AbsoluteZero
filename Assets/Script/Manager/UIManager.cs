using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    public bool inMenu;

    [Header("PlayerUI")]
    [SerializeField] private GameObject playerUICanvas;
    [SerializeField] private Image uiPlayerHpBar;
    [SerializeField] private Image uiPlayerHungerBar;
    [SerializeField] private Image uiPlayerTirstBar;
    [SerializeField] private Image uiPlayerMentalityBar;
    [SerializeField] private Image uiPlayerColdBar;

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
        if (PlayerStatusManager.Instance == null) return;

        uiPlayerHpBar.fillAmount = PlayerStatusManager.Instance.CurrentHpPercent;
		uiPlayerHungerBar.fillAmount = PlayerStatusManager.Instance.CurrentHungerPercent;
		uiPlayerTirstBar.fillAmount = PlayerStatusManager.Instance.CurrentThirstPercent;
		uiPlayerMentalityBar.fillAmount = PlayerStatusManager.Instance.CurrentMentalityPercent;
		uiPlayerColdBar.fillAmount = PlayerStatusManager.Instance.CurrentColdPercent;
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
		PlayerManager.Instance.SetPlayerFreeze(true);
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
		PlayerManager.Instance.SetPlayerFreeze(true);
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
		PlayerManager.Instance.SetPlayerFreeze(true);
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
		PlayerManager.Instance.SetPlayerFreeze(true);
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
