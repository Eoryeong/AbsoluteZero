using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    public bool inMenu;

    [Header("PlayerUI")]
    [SerializeField] private GameObject playerUICanvas;

    [SerializeField] private TextMeshProUGUI uiSelectItem;

    [SerializeField] private GameObject uiProgress;
    [SerializeField] private Image uiProgressBar;

    [Header("ItemPreviewMenuUI")]
    [SerializeField] private GameObject menuUICanvas;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextMeshProUGUI menuTitle;
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewLore;
    [SerializeField] private TextMeshProUGUI menuItemMouseLeft;
    [SerializeField] private TextMeshProUGUI menuItemMouseRight;
    [SerializeField] private GameObject recordPanel;
    [SerializeField] private TextMeshProUGUI totalSurvivedTime;
    [SerializeField] private TextMeshProUGUI totalTraveledDistance;
    [SerializeField] private TextMeshProUGUI totalSleepTime;
    [SerializeField] private TextMeshProUGUI totalEatFood;
    [SerializeField] private TextMeshProUGUI totalDrinkWater;
    [SerializeField] private TextMeshProUGUI totalGunFire;
    [SerializeField] private TextMeshProUGUI totalHitCount;
    [SerializeField] private TextMeshProUGUI totalSuccHunt;
    [SerializeField] private Button menuBackBtn;
    [SerializeField] private Button menuAcceptBtn;
    [SerializeField] private Text menuSleepTime;
    [SerializeField] private Image fadeImage;
    public Transform menuItemPreviewPos;

    [Header("ItemMenuUI")]
    [SerializeField] private TextMeshProUGUI menuItemName;
    [SerializeField] private TextMeshProUGUI menuItemLore;

    [Header("Fade In/Out Setting")]
    [SerializeField] float fadeDuration = 1f;

	private Coroutine currentFade;
	private enum FadeDirection { In, Out }

	void Start()
    {
        inMenu = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            RecordMenuOpen();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseMenu();
        }
    }

	protected override void Init()
	{
		base.Init();
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

    public void BedMenuOpen(ObjectBed bed)
    {
        SetPlayerUICanvas(false);
        CursorVisible(true);
        PlayerManager.Instance.SetPlayerFreeze(true);
        MenuElementAllDisable();

        menuTitle.gameObject.SetActive(true);
        menuAcceptBtn.gameObject.SetActive(true);
        menuAcceptBtn.onClick.AddListener(() => StartCoroutine(bed.Sleep(int.Parse(menuSleepTime.text))));
        menuBackBtn.gameObject.SetActive(true);
        menuSleepTime.gameObject.SetActive(true);
        menuTitle.text = "Go to bed";
        SetMenuUICanvas(true);
    }

    public void ItemPickupMenuOpen()
    {
        MenuElementAllDisable();

        SetPlayerUICanvas(false);
        PlayerManager.Instance.SetPlayerFreeze(true);
        SetMenuUICanvas(true);
        itemPreviewName.gameObject.SetActive(true);
        itemPreviewLore.gameObject.SetActive(true);
        menuItemMouseLeft.gameObject.SetActive(true);
        menuItemMouseRight.gameObject.SetActive(true);
    }

    public void ItemPickupMenuLoreUpdate(PickupItemData pItem)
    {
        itemPreviewName.text = pItem.itemName;
        itemPreviewLore.text = pItem.itemLore;
    }

    public void MenuItemLoreUpdate(PickupItemData pItem)
    {
        menuItemName.text = pItem.itemName;
        menuItemLore.text = pItem.itemLore;
    }

    public void CloseMenu()
    {
        menuAcceptBtn.onClick.RemoveAllListeners();
        SetMenuUICanvas(false);
        SetPlayerUICanvas(true);
        CursorVisible(false);
        PlayerManager.Instance.SetPlayerFreeze(false);
    }

    public void MenuElementAllDisable()
    {
        menuTitle.gameObject.SetActive(false);
        itemPreviewName.gameObject.SetActive(false);
        itemPreviewLore.gameObject.SetActive(false);
        menuItemMouseLeft.gameObject.SetActive(false);
        menuItemMouseRight.gameObject.SetActive(false);
        menuBackBtn.gameObject.SetActive(false);
        menuAcceptBtn.gameObject.SetActive(false);
        recordPanel.gameObject.SetActive(false);
        menuSleepTime.gameObject.SetActive(false);
    }

    public void RecordMenuOpen()
    {
        int day = 0;
        int hour = 0;
        int min = 0;
        float sec = GameRecode.instance.totalSurvivedTime;

        if (GameRecode.instance.totalSurvivedTime > 60)
        {
            min += (int)(sec / 60);
            sec = sec % 60;

            if (min > 60)
            {
                hour += min / 60;
                min= min % 60;

                if (hour > 24)
                {
                    day += hour / 24;
                    hour = hour % 24;
                }
            }
        }

        totalSurvivedTime.text = "생존한 시간 : " + day + "D " + hour + "H " + min + "M " + Mathf.Floor(sec) + "S";
        totalTraveledDistance.text = "이동한 거리 : " + Mathf.RoundToInt(GameRecode.instance.totalTraveledDistance * 100f) / 100f + "M";
        totalSleepTime.text = "잠을 잔 시간 : " + GameRecode.instance.totalSleepTime;
        totalEatFood.text = "회복한 배고픔 : " + GameRecode.instance.totalEatFood;
        totalDrinkWater.text = "회복한 갈증 : " + GameRecode.instance.totalDrinkWater;
        totalGunFire.text = "사격을 실행 한 횟수 : " + GameRecode.instance.totalGunFire;
        totalHitCount.text = "명중한 횟수 : " + GameRecode.instance.totalShootHit;
        totalSuccHunt.text = "사냥에 성공한 횟수 : " + GameRecode.instance.totalSuccessHunt;
    }

	public void FadeIn()
	{
		StartFade(FadeDirection.In);
	}

	public void FadeOut()
	{
		StartFade(FadeDirection.Out);
	}

	private void StartFade(FadeDirection direction)
	{
		if (currentFade != null)
		{
			StopCoroutine(currentFade);
		}

		currentFade = StartCoroutine(FadeRoutine(direction));
	}

	private IEnumerator FadeRoutine(FadeDirection direction)
	{
		float time = 0f;
		Color color = fadeImage.color;

		float startAlpha = (direction == FadeDirection.In) ? 1f : 0f;
		float endAlpha = (direction == FadeDirection.In) ? 0f : 1f;

		color.a = startAlpha;
		fadeImage.color = color;

		while (time < fadeDuration)
		{
			time += Time.deltaTime;
			float t = Mathf.Clamp01(time / fadeDuration);
			color.a = Mathf.Lerp(startAlpha, endAlpha, t);
			fadeImage.color = color;
			yield return null;
		}

		color.a = endAlpha;
		fadeImage.color = color;
		currentFade = null;
	}
}
