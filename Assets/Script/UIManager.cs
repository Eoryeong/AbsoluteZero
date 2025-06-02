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
    }

    private void UpdateUI()
    {
        if (playerData == null) return;

        uiPlayerHp.text = "Hp : " + playerData.currentPlayerHp;
        uiPlayerHunger.text = "Hunger : " + playerData.currentPlayerHunger;
        uiPlayerSanity.text = "Sanity : " + playerData.currentPlayerSanity;
    }

    private void CursorVisible(bool value)
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
    }
}
