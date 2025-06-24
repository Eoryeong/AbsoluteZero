using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
	[SerializeField] private Image uiPlayerHpBar;
	[SerializeField] private Image uiPlayerHungerBar;
	[SerializeField] private Image uiPlayerTirstBar;
	[SerializeField] private Image uiPlayerMentalityBar;
	[SerializeField] private Image uiPlayerColdBar;


    void Update()
    {
        UpdateUI();
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
}
