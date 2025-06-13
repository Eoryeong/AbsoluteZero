using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AreaSwitcher : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string transitionName;

    [Header("Player Spawn Settings")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;

    [Header("Door Settings")]
    [SerializeField] private bool isLocked = false;
    [SerializeField] private string lockedMessage = "문이 잠겨있습니다.";




    private PlayerStatus playerStatus;

    void Start()
    {
        // 씬 시작 시 플레이어 위치 설정
        if (PlayerPrefs.HasKey("Transition"))
        {
            if (PlayerPrefs.GetString("Transition") == transitionName)
            {
                StartCoroutine(SetPlayerPositionAfterFrame());
            }
        }
    }

    private IEnumerator SetPlayerPositionAfterFrame()
    {
        // 한 프레임 대기하여 모든 오브젝트가 초기화되도록 함
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && startPoint != null)
        {
            SetPlayerPosition(player, startPoint.position + spawnOffset);
        }
    }

    public void TryOpenDoor()
    {
        if (isLocked)
        {
            HandleLockedDoor();
            return;
        }

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning("AreaSwitcher: sceneToLoad가 설정되지 않았습니다!");
            return;
        }

        LoadNewScene();
    }
    private void HandleLockedDoor()
    {
        //잠긴문 사운드

        // 잠긴 문 메시지 표시
        Debug.Log(lockedMessage);

        // TODO: 실제 UI 메시지 시스템이 있다면 여기서 호출
        // 예: UIManager.Instance.ShowMessage(lockedMessage);
    }

    private void LoadNewScene()
    {
        // 플레이어 동작 정지
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {

            playerStatus = player.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.SetPlayerFreeze(true);
            }
        }

        // 문 열기 사운드 재생



        // 전환 마커 저장
        PlayerPrefs.SetString("Transition", transitionName);
        PlayerPrefs.Save();

        // 씬 로드
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SetPlayerPosition(GameObject player, Vector3 newPosition)
    {
        if (player == null) return;

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            // CharacterController는 직접 transform.position을 설정할 수 없으므로
            // 먼저 비활성화하고 위치 설정 후 다시 활성화
            controller.enabled = false;
            player.transform.position = newPosition;
            controller.enabled = true;
        }
        else
        {
            // CharacterController가 없는 경우 직접 설정
            player.transform.position = newPosition;
        }

        Debug.Log($"플레이어 위치 설정: {newPosition}");
    }



    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

    public bool IsLocked()
    {
        return isLocked;
    }


}

