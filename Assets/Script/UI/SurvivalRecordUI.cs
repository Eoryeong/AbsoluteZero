using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;



public class SurvivalRecordUI : SingletonBehaviour<SurvivalRecordUI>
{

    [Header("생존 정보 UI")]
    [SerializeField] private TextMeshProUGUI survivalTimeText;      // 생존 일수
    [SerializeField] private TextMeshProUGUI deathReasonText;       // 사망 원인

    [Header("최종 상태 UI")]
    [SerializeField] private TextMeshProUGUI lastMentalText;        // 마지막 정신력
    [SerializeField] private TextMeshProUGUI lasttemperatureText;   // 마지막 체온
    [SerializeField] private TextMeshProUGUI lastThirstText;        // 마지막 수분
    [SerializeField] private TextMeshProUGUI lastHungerText;        // 마지막 허기

    [Header("게임 통계 UI")]
    [SerializeField] private TextMeshProUGUI killCountText;         // 사냥한 동물 수
    [SerializeField] private TextMeshProUGUI craftCountText;        // 제작한 아이템 수
    [SerializeField] private TextMeshProUGUI regionCountText;       // 발견한 지역 수
    [SerializeField] private TextMeshProUGUI distanceText;          // 총 이동 거리

    [Header("UI 패널")]
    [SerializeField] private GameObject recordPanel;               // 생존 기록 패널

    [Header("버튼")]
    [SerializeField] private Button restartButton;                 // 재시작 버튼
    [SerializeField] private Button menuButton;                    // 메뉴 버튼

    [Header("설정")]
    [SerializeField] private bool pauseGameWhenShown = true;       // 기록 표시 시 게임 일시정지
    [SerializeField] private bool showCursorWhenShown = true;      // 기록 표시 시 커서 표시

    // 이벤트
    public static event Action OnRecordShown;                      // 기록 표시 이벤트
    public static event Action OnRestartRequested;                 // 재시작 요청 이벤트
    public static event Action OnMenuRequested;                    // 메뉴 요청 이벤트

    // 생존 기록 데이터
    private SurvivalData currentRecord;


    // 생존 기록 데이터 구조체
    [Serializable]
    public struct SurvivalData
    {
        public int survivalDays;           // 생존 일수
        public int survivalHours;          // 생존 시간
        public int survivalMinutes;        // 생존 분
        public string deathReason;         // 사망 원인

        // 최종 상태
        public float finalMental;          // 최종 정신력 (0-1)
        public float finaltemperature;     // 최종 체온 (0-1)
        public float finalThirst;          // 최종 수분 (0-1)
        public float finalHunger;          // 최종 허기 (0-1)

        // 게임 통계
        public int killCount;              // 사냥한 동물 수
        public int craftCount;             // 제작한 아이템 수
        public int regionCount;            // 발견한 지역 수
        public float totalDistance;        // 총 이동 거리 (km)
    }

    #region Unity Lifecycle



    private void Start()
    {
        SetupButtonEvents();
    }



    protected override void OnDestroy()
    {
        base.OnDestroy();

        // 정적 이벤트 정리
        OnRecordShown = null;
        OnRestartRequested = null;
        OnMenuRequested = null;
    }

    protected override void Init()
    {
        base.Init();

        // UI가 씬 전환 시 파괴되도록 설정
        m_IsDestroyOnLoad = true;

        InitializeUI();
    }

    #endregion

    #region Initialization


    // UI 초기화
    private void InitializeUI()
    {
        if (recordPanel != null)
        {
            recordPanel.SetActive(false);
        }
    }


    // 버튼 이벤트 설정

    private void SetupButtonEvents()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnClickRestart);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(OnClickMenu);
        }
    }

    #endregion

    #region Public Methods


    // 생존 기록 표시 (현재 게임 상태 기반)
    public void ShowRecord()
    {
        ShowRecord(GetCurrentSurvivalData());
    }


    // 생존 기록 표시 (사망 원인 지정)
    public void ShowRecord(string deathReason)
    {
        var data = GetCurrentSurvivalData();
        data.deathReason = deathReason;
        ShowRecord(data);
    }


    // 생존 기록 표시 (데이터 직접 제공)
    public void ShowRecord(SurvivalData data)
    {
        currentRecord = data;

        UpdateUI(data);
        HandleGameState(true);

        // UI 활성화
        if (recordPanel != null)
        {
            recordPanel.SetActive(true);
        }

        // 이벤트 발생
        OnRecordShown?.Invoke();
    }


    // 생존 기록 숨기기
    public void HideRecord()
    {
        if (recordPanel != null)
        {
            recordPanel.SetActive(false);
        }

        HandleGameState(false);
    }

    #endregion

    #region Private Methods


    // 현재 게임 상태로부터 생존 데이터 수집
    private SurvivalData GetCurrentSurvivalData()
    {
        var data = new SurvivalData();

        // 시간 정보
        if (PlayerStatusManager.Instance != null)
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
                    min = min % 60;

                    if (hour > 24)
                    {
                        day += hour / 24;
                        hour = hour % 24;
                    }
                }
            }

            data.survivalDays = day;
            data.survivalHours = hour;
            data.survivalMinutes = min;
        }

        // 플레이어 상태
        if (PlayerStatusManager.Instance != null)
        {
            var p = PlayerStatusManager.Instance;
            data.finalMental = p.CurrentMentalityPercent;
            data.finaltemperature = p.CurrentColdPercent;
            data.finalThirst = p.CurrentThirstPercent;
            data.finalHunger = p.CurrentHungerPercent;
        }

        // 게임 통계 (GameStatsManager 사용)
        if (GameStatsManager.Instance != null)
        {
            data.killCount = GameStatsManager.Instance.KillCount;
            data.craftCount = GameStatsManager.Instance.CraftCount;
            data.regionCount = GameStatsManager.Instance.RegionCount;
            data.totalDistance = GameStatsManager.Instance.TotalDistance;
        }

        data.deathReason = "알 수 없음"; // 기본값

        return data;
    }


    // UI 업데이트
    private void UpdateUI(SurvivalData data)
    {
        // 생존 시간
        if (survivalTimeText != null)
        {
            survivalTimeText.text = $"{data.survivalDays}일, {data.survivalHours}시간 {data.survivalMinutes}분";
        }

        // 사망 원인
        if (deathReasonText != null)
        {
            deathReasonText.text = $"사망 원인 : {data.deathReason}";
        }

        // 최종 상태 (백분율로 표시)
        if (lastMentalText != null)
            lastMentalText.text = $"정신력 : {(int)(data.finalMental * 100)}%";

        if (lasttemperatureText != null)
            lasttemperatureText.text = $"체온 : {(int)(data.finaltemperature * 100)}%";

        if (lastThirstText != null)
            lastThirstText.text = $"수분 : {(int)(data.finalThirst * 100)}%";

        if (lastHungerText != null)
            lastHungerText.text = $"허기 : {(int)(data.finalHunger * 100)}%";

        // 게임 통계
        if (killCountText != null)
            killCountText.text = $"사냥한 동물 : {data.killCount}마리";

        if (craftCountText != null)
            craftCountText.text = $"제작한 아이템 : {data.craftCount}개";

        if (regionCountText != null)
            regionCountText.text = $"발견한 지역 : {data.regionCount}개";

        if (distanceText != null)
            distanceText.text = $"이동 거리 : {data.totalDistance:F1}km";
    }


    // 게임 상태 처리 (일시정지, 커서 등)
    private void HandleGameState(bool showingRecord)
    {
        if (showingRecord)
        {
            // 게임 일시정지
            if (pauseGameWhenShown)
            {
                Time.timeScale = 0f;
            }

            // 커서 표시
            if (showCursorWhenShown)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            // 게임 재개
            if (pauseGameWhenShown)
            {
                Time.timeScale = 1f;
            }

            // 커서 숨김 (게임 설정에 따라)
            if (showCursorWhenShown)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    #endregion

    #region Button Events


    // 재시작 버튼 클릭 이벤트
    public void OnClickRestart()
    {
        HideRecord();

        // 플레이어 사망 상태 리셋
        if (PlayerDeathManager.Instance != null)
        {
            PlayerDeathManager.Instance.ResetDeathState();
        }

        // 게임 통계 리셋 (선택사항)
        if (GameStatsManager.Instance != null)
        {
            GameStatsManager.Instance.ResetAllStats();
        }

        OnRestartRequested?.Invoke();

        // 기본 동작: 현재 씬 리로드
        if (OnRestartRequested == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }


    // 메뉴 버튼 클릭 이벤트
    public void OnClickMenu()
    {
        HideRecord();
        OnMenuRequested?.Invoke();

        // 기본 동작: 메인 메뉴로 이동 (씬 이름은 프로젝트에 맞게 수정)
        if (OnMenuRequested == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
            Debug.Log("메인 메뉴로 이동 (씬 이름을 설정해주세요)");
        }
    }

    #endregion

    #region Properties


    // 현재 표시 중인 생존 기록 (읽기 전용)
    public SurvivalData CurrentRecord => currentRecord;


    // 기록 패널 활성화 여부 (읽기 전용)
    public bool IsShowing => recordPanel != null && recordPanel.activeInHierarchy;

    #endregion
}
