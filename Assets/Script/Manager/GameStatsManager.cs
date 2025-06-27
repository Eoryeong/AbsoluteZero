using UnityEngine;
using System;


public class GameStatsManager : SingletonBehaviour<GameStatsManager>
{
    [Header("게임 통계")]
    [SerializeField] private int killCount = 0;           // 사냥한 동물 수
    [SerializeField] private int craftCount = 0;          // 제작한 아이템 수
    [SerializeField] private int regionCount = 0;         // 발견한 지역 수
    [SerializeField] private float totalDistance = 0f;    // 총 이동 거리 (km)

    [Header("이동 거리 추적")]
    [SerializeField] private bool trackMovement = true;    // 이동 거리 추적 여부
    [SerializeField] private float trackInterval = 1f;    // 추적 간격 (초)

    // 이동 거리 추적용 변수
    private Vector3 lastPosition;
    private bool hasInitialPosition = false;


    #region Events
    // 사냥 수 변경 이벤트
    public static event Action<int> OnKillCountChanged;


    // 제작 수 변경 이벤트  
    public static event Action<int> OnCraftCountChanged;


    // 지역 발견 수 변경 이벤트
    public static event Action<int> OnRegionCountChanged;


    // 이동 거리 변경 이벤트  
    public static event Action<float> OnDistanceChanged;

    #endregion

    #region Unity Lifecycle




    protected override void Init()
    {
        base.Init();


        m_IsDestroyOnLoad = false;
    }


    private void Start()
    {
        InitializeTracking();
    }


    //이동 거리 추적
    private void Update()
    {
        if (trackMovement)
        {
            TrackPlayerMovement();
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy(); // 부모 클래스의 OnDestroy 호출

        // 정적 이벤트 정리
        OnKillCountChanged = null;
        OnCraftCountChanged = null;
        OnRegionCountChanged = null;
        OnDistanceChanged = null;
    }

    #endregion

    #region Initialization

    // 추적 시스템 초기화
    private void InitializeTracking()
    {
        // 플레이어 매니저에서 플레이어 위치 가져오기
        if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerController != null)
        {
            UpdateTrackingPosition(PlayerManager.Instance.PlayerController.transform.position);
        }
    }

    #endregion

    #region Movement Tracking


    // 플레이어 이동 거리 추적
    private void TrackPlayerMovement()
    {
        if (!hasInitialPosition) return;
        if (PlayerManager.Instance == null || PlayerManager.Instance.PlayerController == null) return;

        Vector3 currentPosition = PlayerManager.Instance.PlayerController.transform.position;
        float distance = Vector3.Distance(lastPosition, currentPosition);

        // 최소 이동 거리 체크 (노이즈 제거)
        if (distance > 0.1f)
        {
            totalDistance += distance / 1000f; // 미터를 킬로미터로 변환
            lastPosition = currentPosition;
        }
    }

    #endregion

    #region Public Methods - Kill Stats


    // 동물 사냥 기록 추가
    public void AddKill(string animalName = "")
    {
        killCount++;
        Debug.Log($"[GameStatsManager] 동물 사냥: {animalName} (총 {killCount}마리)");
        OnKillCountChanged?.Invoke(killCount);
    }


    // 다수 동물 사냥 기록 추가
    public void AddKills(int count)
    {
        if (count <= 0) return;

        killCount += count;
        Debug.Log($"[GameStatsManager] 동물 사냥: {count}마리 (총 {killCount}마리)");
        OnKillCountChanged?.Invoke(killCount);
    }

    #endregion

    #region Public Methods - Craft Stats


    // 아이템 제작 기록 추가
    public void AddCraft(string itemName = "")
    {
        craftCount++;
        Debug.Log($"[GameStatsManager] 아이템 제작: {itemName} (총 {craftCount}개)");
        OnCraftCountChanged?.Invoke(craftCount);
    }


    // 다수 아이템 제작 기록 추가
    public void AddCrafts(int count)
    {
        if (count <= 0) return;

        craftCount += count;
        Debug.Log($"[GameStatsManager] 아이템 제작: {count}개 (총 {craftCount}개)");
        OnCraftCountChanged?.Invoke(craftCount);
    }

    #endregion

    #region Public Methods - Region Stats


    // 새로운 지역 발견 기록 추가, 새로운 지역 관련 코드 없음
    public void AddRegionDiscovered(string regionName = "")
    {
        regionCount++;
        Debug.Log($"[GameStatsManager] 지역 발견: {regionName} (총 {regionCount}개)");
        OnRegionCountChanged?.Invoke(regionCount);
    }

    #endregion

    #region Public Methods - Distance Stats


    // 이동 거리 수동 추가 (특수 상황용) 당장은 쓰이지 않음
    public void AddDistance(float distanceKm)
    {
        if (distanceKm <= 0) return;

        totalDistance += distanceKm;
        OnDistanceChanged?.Invoke(totalDistance);
    }


    // 이동 거리 추적 위치 업데이트 (플레이어 위치 변경 시 호출)
    public void UpdateTrackingPosition(Vector3 newPosition)
    {
        lastPosition = newPosition;
        hasInitialPosition = true;
    }

    #endregion

    #region Public Methods - Reset



    // 모든 통계 리셋 (새 게임 시작 시 호출), 게임 시작 버튼 호출할때 같이 발동
    public void ResetAllStats()
    {
        killCount = 0;
        craftCount = 0;
        regionCount = 0;
        totalDistance = 0f;

        // 플레이어 위치 다시 초기화
        if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerController != null)
        {
            UpdateTrackingPosition(PlayerManager.Instance.PlayerController.transform.position);
        }

        Debug.Log("[GameStatsManager] 모든 통계가 리셋되었습니다.");

        // 모든 이벤트 발생
        OnKillCountChanged?.Invoke(killCount);
        OnCraftCountChanged?.Invoke(craftCount);
        OnRegionCountChanged?.Invoke(regionCount);
        OnDistanceChanged?.Invoke(totalDistance);
    }


    /// 특정 통계만 리셋
    public void ResetStats(bool resetKills = false, bool resetCrafts = false,
                          bool resetRegions = false, bool resetDistance = false)
    {
        if (resetKills)
        {
            killCount = 0;
            OnKillCountChanged?.Invoke(killCount);
        }

        if (resetCrafts)
        {
            craftCount = 0;
            OnCraftCountChanged?.Invoke(craftCount);
        }

        if (resetRegions)
        {
            regionCount = 0;
            OnRegionCountChanged?.Invoke(regionCount);
        }

        if (resetDistance)
        {
            totalDistance = 0f;
            OnDistanceChanged?.Invoke(totalDistance);
        }
    }

    #endregion



    #region Properties


    // 사냥한 동물 수 (읽기 전용)  
    public int KillCount => killCount;


    // 제작한 아이템 수 (읽기 전용) 
    public int CraftCount => craftCount;


    // 발견한 지역 수 (읽기 전용)   
    public int RegionCount => regionCount;


    // 총 이동 거리 km (읽기 전용)
    public float TotalDistance => totalDistance;

    #endregion
}
