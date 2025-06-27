using UnityEngine;

public class PlayerDeathManager : SingletonBehaviour<PlayerDeathManager>
{
    [Header("사망 조건 설정")]
    [SerializeField] private bool enableHpDeath = true;          // 체온으로 인한 사망
    [SerializeField] private bool enableHungerDeath = true;      // 굶주림으로 인한 사망
    [SerializeField] private bool enableThirstDeath = true;      // 탈수로 인한 사망
    [SerializeField] private bool enableMentalDeath = true;      // 정신력으로 인한 사망

    [Header("사망 임계값")]
    [SerializeField] private float deathThreshold = 0.01f;       // 사망 판정 임계값 (1%)
    [SerializeField] private float deathCheckInterval = 1f;      // 사망 체크 간격 (초)


    // 사망 이벤트
    public static event System.Action<string> OnPlayerDied;

    // 사망 상태
    private bool isDead = false;
    private string lastDeathReason = "";

    #region Unity Lifecycle


    protected override void Init()
    {
        base.Init();

        m_IsDestroyOnLoad = false;
    }


    private void Start()
    {
        // 주기적으로 사망 조건 체크
        InvokeRepeating(nameof(CheckDeathConditions), 1f, deathCheckInterval);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();


        OnPlayerDied = null;
    }

    #endregion

    #region Death Check


    // 사망 조건 체크
    private void CheckDeathConditions()
    {
        if (isDead) return;
        if (PlayerStatusManager.Instance == null) return;

        var status = PlayerStatusManager.Instance;
        string deathReason = GetDeathReason(status);

        if (!string.IsNullOrEmpty(deathReason))
        {
            TriggerDeath(deathReason);
        }
    }


    // 사망 원인 판정
    private string GetDeathReason(PlayerStatusManager status)
    {
        // 우선순위 순으로 체크

        // 1. 체온 (저체온증)
        if (enableHpDeath && status.isCold && status.CurrentHpPercent <= deathThreshold)
        {
            return "저체온증";
        }

        // 2. 탈수
        if (enableHpDeath && status.isThirst && status.CurrentHpPercent <= deathThreshold)
        {
            return "탈수";
        }

        // 3. 굶주림
        if (enableHpDeath && status.isHunger && status.CurrentHpPercent <= deathThreshold)
        {
            return "굶주림";
        }

        // 4. 정신력 (정신 붕괴)
        if (enableHpDeath && status.isTired && status.CurrentHpPercent <= deathThreshold)
        {
            return "정신력";
        }

        // 5. 기본 사망 원인 (체력 관련)
        if (status.CurrentHpPercent <= deathThreshold)
        {
            return "체력 고갈";
        }

        // 모든 조건에 해당하지 않으면 일반적인 체력 저하로 간주
        return "";
    }

    #endregion

    #region Public Methods


    // 사망 처리 (외부에서 호출 가능)
    public void TriggerDeath(string reason)
    {
        if (isDead) return;

        isDead = true;
        lastDeathReason = reason;

        Debug.Log($"[PlayerDeathManager] 플레이어 사망: {reason}");

        // 게임 일시정지
        Time.timeScale = 0f;

        // 생존 기록 표시
        if (SurvivalRecordUI.Instance != null)
        {
            SurvivalRecordUI.Instance.ShowRecord(reason);
        }

        // 사망 이벤트 발생 (다른 시스템에서 필요하다면)
        OnPlayerDied?.Invoke(reason);
    }


    // 즉시 사망 (치트 또는 특수 상황용)
    // public void KillPlayer(string reason = "알 수 없음")
    // {
    //     TriggerDeath(reason);
    // }


    // 사망 상태 리셋 (재시작 시 호출)
    public void ResetDeathState()
    {
        isDead = false;
        lastDeathReason = "";
        Time.timeScale = 1f;
    }

    #endregion




    #region Properties


    // 플레이어 사망 여부 (읽기 전용) 
    public bool IsDead => isDead;


    // 마지막 사망 원인 (읽기 전용)  
    public string LastDeathReason => lastDeathReason;

    #endregion
}
