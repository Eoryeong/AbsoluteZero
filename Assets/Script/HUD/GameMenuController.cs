using UnityEngine;
using System;
using System.Collections.Generic;


public enum PanelType
{
    Status,
    Equipment,
    Inventory,
    Crafting,
    Map,
    Record,
    Settings
}

[Serializable]
public class MenuPanel
{
    public PanelType type;
    public GameObject panel;
    public KeyCode hotkey;      // 단축키
}

public class GameMenuController : MonoBehaviour
{

    [Header("카테고리 패널")]
    [SerializeField] private MenuPanel[] menuPanels;

    [Header("메뉴 루트")]
    [SerializeField] private GameObject menuRoot; // ESC 누르면 전체 메뉴 꺼짐

    [Header("설정")]
    [SerializeField] private bool pauseGameWhenOpen = true; // 메뉴 열 때 게임 일시정지 여부
    [SerializeField] private bool showCursorWhenOpen = true; // 메뉴 열 때 커서 표시 여부




    public static event Action<bool> OnMenuStateChanged;     // 메뉴 상태 변경 이벤트
    public static event Action<PanelType> OnPanelOpened;     // 패널 열림 이벤트
    public static event Action OnMenuOpened;                 // 메뉴 열림 이벤트
    public static event Action OnMenuClosed;                 // 메뉴 닫힘 이벤트



    private bool isMenuOpen = false;                         // 메뉴 열림 상태
    private PanelType? currentOpenPanel = null;              // 현재 열린 패널
    private Dictionary<PanelType, GameObject> panelLookup;   // 패널 타입별 GameObject 조회 
    private Dictionary<KeyCode, PanelType> hotkeyLookup;     // 키코드별 패널 타입 조회
    private bool wasGamePaused = false;                      // 메뉴 열기 전 게임 일시정지 상태
    private CursorLockMode previousCursorLockMode;           // 이전 커서 잠금 모드
    private bool previousCursorVisible;                      // 이전 커서 표시 상태   
    void Start()
    {
        InitializeLookupTables(); // 조회 테이블 초기화
        InitializeMenuState();    // 메뉴 상태 초기화
    }


    void Update()
    {
        HandleInput();
    }

    #region Initialization - 초기화


    private void InitializeLookupTables()
    {
        panelLookup = new Dictionary<PanelType, GameObject>();
        hotkeyLookup = new Dictionary<KeyCode, PanelType>();

        // 메뉴 패널 배열을 순회하며 조회 테이블 구성
        foreach (var menuPanel in menuPanels)
        {
            if (menuPanel.panel != null)
            {
                panelLookup[menuPanel.type] = menuPanel.panel;     // 패널 타입 -> GameObject
                hotkeyLookup[menuPanel.hotkey] = menuPanel.type;   // 키코드 -> 패널 타입
            }
        }
    }


    // 메뉴 초기 상태 설정

    private void InitializeMenuState()
    {
        isMenuOpen = false;
        currentOpenPanel = null;

        // 메뉴 루트 비활성화
        if (menuRoot != null)
        {
            menuRoot.SetActive(false);
        }

        CloseAllPanels(); // 모든 패널 닫기
    }



    #endregion

    #region Input Handling - 입력 처리


    // 키보드 입력 처리

    private void HandleInput()
    {
        // 등록된 모든 단축키 확인
        foreach (var kvp in hotkeyLookup)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                HandlePanelToggle(kvp.Value); // 해당 패널 토글
                break;
            }
        }
    }


    private void HandlePanelToggle(PanelType panelType)
    {
        if (!isMenuOpen)
        {
            // 메뉴가 닫혀있으면 메뉴 열고 해당 패널 열기
            OpenMenu();
            OpenPanel(panelType);
        }
        else if (currentOpenPanel == panelType)
        {
            // 같은 패널이 열려있으면 메뉴 닫기
            CloseMenu();
        }
        else
        {
            // 다른 패널이 열려있으면 해당 패널로 전환
            OpenPanel(panelType);
        }
    }

    #endregion

    #region Menu Management - 메뉴 관리


    public void OpenMenu()
    {
        if (isMenuOpen) return; // 이미 열려있으면 리턴

        isMenuOpen = true;

        // 메뉴 루트 활성화
        if (menuRoot != null)
        {
            menuRoot.SetActive(true);
        }

        HandleGamePause(true);   // 게임 일시정지 처리
        HandleCursorState(true); // 커서 상태 처리

        // 이벤트 발생
        OnMenuOpened?.Invoke();
        OnMenuStateChanged?.Invoke(true);
    }


    // 메뉴 닫기

    public void CloseMenu()
    {
        if (!isMenuOpen) return; // 이미 닫혀있으면 리턴

        isMenuOpen = false;
        currentOpenPanel = null;

        // 메뉴 루트 비활성화
        if (menuRoot != null)
        {
            menuRoot.SetActive(false);
        }

        CloseAllPanels();         // 모든 패널 닫기
        HandleGamePause(false);   // 게임 일시정지 해제
        HandleCursorState(false); // 커서 상태 복구

        // 이벤트 발생
        OnMenuClosed?.Invoke();
        OnMenuStateChanged?.Invoke(false);
    }


    #endregion

    #region Panel Management - 패널 관리


    // 특정 패널 열기

    public void OpenPanel(PanelType panelType)
    {
        // 패널 조회 테이블에서 해당 패널 찾기
        if (!panelLookup.TryGetValue(panelType, out GameObject targetPanel))
        {
            Debug.LogWarning($"[{nameof(GameMenuController)}] Panel {panelType} not found!");
            return;
        }

        CloseAllPanels(); // 기존 패널들 모두 닫기

        // 대상 패널 활성화
        targetPanel.SetActive(true);
        currentOpenPanel = panelType;

        // 패널 열림 이벤트 발생
        OnPanelOpened?.Invoke(panelType);
    }


    // 모든 패널 닫기

    private void CloseAllPanels()
    {
        foreach (var panel in panelLookup.Values)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        currentOpenPanel = null;
    }

    // UI 버튼용 퍼블릭 메소드들
    public void OnStatusButton() => HandlePanelToggle(PanelType.Status);        // 상태 버튼 클릭
    public void OnEquipmentButton() => HandlePanelToggle(PanelType.Equipment);  // 장비 버튼 클릭
    public void OnInventoryButton() => HandlePanelToggle(PanelType.Inventory);  // 인벤토리 버튼 클릭
    public void OnCraftingButton() => HandlePanelToggle(PanelType.Crafting);    // 제작 버튼 클릭
    public void OnMapButton() => HandlePanelToggle(PanelType.Map);              // 지도 버튼 클릭
    public void OnRecordButton() => HandlePanelToggle(PanelType.Record);        // 기록 버튼 클릭
    public void OnSettingsButton() => HandlePanelToggle(PanelType.Settings);    // 설정 버튼 클릭

    #endregion

    #region Game State Management - 게임 상태 관리


    // 게임 일시정지 처리

    private void HandleGamePause(bool shouldPause)
    {
        if (!pauseGameWhenOpen) return; // 설정에서 비활성화된 경우 리턴

        if (shouldPause)
        {
            // 메뉴 열기 전 게임 상태 저장
            wasGamePaused = Time.timeScale == 0f;
            if (!wasGamePaused)
            {
                Time.timeScale = 0f; // 게임 일시정지
            }
        }
        else
        {
            // 메뉴 열기 전에 일시정지되지 않았던 경우만 재개
            if (!wasGamePaused)
            {
                Time.timeScale = 1f; // 게임 재개
            }
        }
    }


    // 커서 상태 처리
    private void HandleCursorState(bool showCursor)
    {
        if (!showCursorWhenOpen) return; // 설정에서 비활성화된 경우 리턴

        if (showCursor)
        {
            // 현재 커서 상태 저장
            previousCursorLockMode = Cursor.lockState;
            previousCursorVisible = Cursor.visible;

            // 커서 표시 및 잠금 해제
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // 이전 커서 상태로 복구
            Cursor.lockState = previousCursorLockMode;
            Cursor.visible = previousCursorVisible;
        }
    }

    #endregion


    #region Unity Lifecycle - Unity 라이프사이클


    // Unity OnDestroy 메소드 - 정적 이벤트 정리
    void OnDestroy()
    {
        // 정적 이벤트 정리 (메모리 누수 방지)
        OnMenuStateChanged = null;
        OnPanelOpened = null;
        OnMenuOpened = null;
        OnMenuClosed = null;
    }

    #endregion
}
