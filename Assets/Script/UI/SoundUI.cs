using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 게임 설정 메뉴의 사운드 옵션 UI를 관리하는 클래스
/// 볼륨 슬라이더와 뮤트 체크박스를 통해 사운드 설정을 제어합니다.
/// </summary>
public class SoundUI : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider effectVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider ambientVolumeSlider;

    [Header("Volume Percentage Text")]
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI effectVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI ambientVolumeText;

    [Header("Mute Toggles")]
    [SerializeField] private Toggle masterMuteToggle;
    [SerializeField] private Toggle effectMuteToggle;
    [SerializeField] private Toggle musicMuteToggle;
    [SerializeField] private Toggle ambientMuteToggle;

    [Header("UI Settings")]
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject soundPanel;

    private void Start()
    {
        InitializeUI();
        LoadCurrentSettings(); // 이미 SetupEventListeners()가 포함됨
    }
    private void InitializeUI()
    {
        // 슬라이더 범위 설정 (0~1)
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;
        }
        if (effectVolumeSlider != null)
        {
            effectVolumeSlider.minValue = 0f;
            effectVolumeSlider.maxValue = 1f;
        }
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;
        }
        if (ambientVolumeSlider != null)
        {
            ambientVolumeSlider.minValue = 0f;
            ambientVolumeSlider.maxValue = 1f;
        }
    }


    private void SetupEventListeners()
    {
        // 볼륨 슬라이더 이벤트
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (effectVolumeSlider != null)
            effectVolumeSlider.onValueChanged.AddListener(OnEffectVolumeChanged);
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (ambientVolumeSlider != null)
            ambientVolumeSlider.onValueChanged.AddListener(OnAmbientVolumeChanged);

        // 뮤트 토글 이벤트
        if (masterMuteToggle != null)
            masterMuteToggle.onValueChanged.AddListener(OnMasterMuteToggled);
        if (effectMuteToggle != null)
            effectMuteToggle.onValueChanged.AddListener(OnEffectMuteToggled);
        if (musicMuteToggle != null)
            musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToggled);
        if (ambientMuteToggle != null)
            ambientMuteToggle.onValueChanged.AddListener(OnAmbientMuteToggled);

        // 닫기 버튼 이벤트
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSoundPanel);
    }
    // 현재 사운드 설정을 불러와 UI에 반영합니다.
    private void LoadCurrentSettings()
    {
        if (SoundManager.Instance == null) return;

        // 이벤트 리스너 일시 제거 (무한 루프 방지)
        RemoveAllEventListeners();

        // 볼륨 값 불러오기
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = SoundManager.Instance.masterVolume;
        if (effectVolumeSlider != null)
            effectVolumeSlider.value = SoundManager.Instance.sfxVolume;
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = SoundManager.Instance.bgmVolume;
        if (ambientVolumeSlider != null)
            ambientVolumeSlider.value = SoundManager.Instance.ambientVolume;

        // 뮤트 상태 불러오기
        if (masterMuteToggle != null)
            masterMuteToggle.isOn = SoundManager.Instance.IsMasterMuted();
        if (effectMuteToggle != null)
            effectMuteToggle.isOn = SoundManager.Instance.IsSFXMuted();
        if (musicMuteToggle != null)
            musicMuteToggle.isOn = SoundManager.Instance.IsBGMMuted();
        if (ambientMuteToggle != null)
            ambientMuteToggle.isOn = SoundManager.Instance.IsAmbientMuted();

        // 텍스트 업데이트
        UpdateAllVolumeTexts();

        // 이벤트 리스너 다시 추가
        SetupEventListeners();
    }

    #region Volume Slider Events


    // 마스터 볼륨 슬라이더 값이 변경될 때 호출됩니다.

    private void OnMasterVolumeChanged(float value)
    {
        SoundManager.Instance?.SetMasterVolume(value);
        UpdateVolumeText(masterVolumeText, value);

        // 마스터 볼륨 변경 시 버튼 클릭 소리 재생
        SoundManager.Instance?.PlayButtonClick();
    }


    // 효과음 볼륨 슬라이더 값이 변경될 때 호출됩니다.

    private void OnEffectVolumeChanged(float value)
    {
        SoundManager.Instance?.SetSFXVolume(value);
        UpdateVolumeText(effectVolumeText, value);

        // 효과음 볼륨 변경 시 소리 재생
        SoundManager.Instance?.PlayButtonClick();
    }


    // 음악 볼륨 슬라이더 값이 변경될 때 호출됩니다.

    private void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance?.SetBGMVolume(value);
        UpdateVolumeText(musicVolumeText, value);

        SoundManager.Instance?.PlayButtonClick();
    }


    // 환경음 볼륨 슬라이더 값이 변경될 때 호출됩니다.

    private void OnAmbientVolumeChanged(float value)
    {
        SoundManager.Instance?.SetAmbientVolume(value);
        UpdateVolumeText(ambientVolumeText, value);

        SoundManager.Instance?.PlayButtonClick();
    }

    #endregion

    #region Mute Toggle Events


    // 마스터 뮤트 토글이 변경될 때 호출됩니다.

    private void OnMasterMuteToggled(bool isMuted)
    {
        SoundManager.Instance?.SetMasterMute(isMuted);
        SoundManager.Instance?.PlayButtonClick();
    }
    // 효과음 뮤트 토글이 변경될 때 호출됩니다.

    private void OnEffectMuteToggled(bool isMuted)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSFXMute(isMuted);

            // 뮤트가 해제되었을 때만 테스트 소리 재생
            if (!isMuted)
                SoundManager.Instance.PlayButtonClick();
        }
    }


    // 음악 뮤트 토글이 변경될 때 호출됩니다.

    private void OnMusicMuteToggled(bool isMuted)
    {
        SoundManager.Instance?.SetBGMMute(isMuted);
        SoundManager.Instance?.PlayButtonClick();
    }


    // 환경음 뮤트 토글이 변경될 때 호출됩니다.

    private void OnAmbientMuteToggled(bool isMuted)
    {
        SoundManager.Instance?.SetAmbientMute(isMuted);
        SoundManager.Instance?.PlayButtonClick();
    }

    #endregion

    #region UI Update Methods


    // 볼륨 텍스트를 업데이트합니다.

    private void UpdateVolumeText(TextMeshProUGUI textComponent, float volume)
    {
        if (textComponent != null)
        {
            int percentage = Mathf.RoundToInt(volume * 100);
            textComponent.text = $"{percentage}%";
        }
    }


    // 모든 볼륨 텍스트를 업데이트합니다.

    private void UpdateAllVolumeTexts()
    {
        if (SoundManager.Instance == null) return;

        UpdateVolumeText(masterVolumeText, SoundManager.Instance.masterVolume);
        UpdateVolumeText(effectVolumeText, SoundManager.Instance.sfxVolume);
        UpdateVolumeText(musicVolumeText, SoundManager.Instance.bgmVolume);
        UpdateVolumeText(ambientVolumeText, SoundManager.Instance.ambientVolume);
    }

    #endregion

    #region Public Methods


    public void OpenSoundPanel()
    {
        if (soundPanel != null)
        {
            soundPanel.SetActive(true);
            LoadCurrentSettings(); // 패널 열 때 현재 설정 다시 로드
        }

        SoundManager.Instance?.PlayButtonClick();
    }


    public void CloseSoundPanel()
    {
        if (soundPanel != null)
            soundPanel.SetActive(false);

        SoundManager.Instance?.PlayButtonClick();
    }


    /// 모든 볼륨을 기본값으로 초기화합니다.

    public void ResetToDefaults()
    {
        // 기본값 설정
        SoundManager.Instance?.SetMasterVolume(0.5f);
        SoundManager.Instance?.SetSFXVolume(0.5f);
        SoundManager.Instance?.SetBGMVolume(0.5f);
        SoundManager.Instance?.SetAmbientVolume(0.5f);

        // 모든 뮤트 해제
        SoundManager.Instance?.SetMasterMute(false);
        SoundManager.Instance?.SetSFXMute(false);
        SoundManager.Instance?.SetBGMMute(false);
        SoundManager.Instance?.SetAmbientMute(false);

        // UI 업데이트
        LoadCurrentSettings();

        SoundManager.Instance?.PlayButtonClick();
    }



    #endregion

    #region Unity Lifecycle


    // 모든 이벤트 리스너를 제거합니다.

    private void RemoveAllEventListeners()
    {
        // 볼륨 슬라이더 이벤트 제거
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        if (effectVolumeSlider != null)
            effectVolumeSlider.onValueChanged.RemoveListener(OnEffectVolumeChanged);
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (ambientVolumeSlider != null)
            ambientVolumeSlider.onValueChanged.RemoveListener(OnAmbientVolumeChanged);

        // 뮤트 토글 이벤트 제거
        if (masterMuteToggle != null)
            masterMuteToggle.onValueChanged.RemoveListener(OnMasterMuteToggled);
        if (effectMuteToggle != null)
            effectMuteToggle.onValueChanged.RemoveListener(OnEffectMuteToggled);
        if (musicMuteToggle != null)
            musicMuteToggle.onValueChanged.RemoveListener(OnMusicMuteToggled);
        if (ambientMuteToggle != null)
            ambientMuteToggle.onValueChanged.RemoveListener(OnAmbientMuteToggled);

        // 닫기 버튼 이벤트 제거
        if (closeButton != null) if (closeButton != null)
                closeButton.onClick.RemoveListener(CloseSoundPanel);
    }

    private void OnDestroy()
    {
        // 이벤트 리스너 해제
        RemoveAllEventListeners();
    }

    #endregion
}
