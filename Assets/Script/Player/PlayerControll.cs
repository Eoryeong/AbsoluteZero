using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerControll : MonoBehaviour
{
    // 플레이어 이동 속도 
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float sitSpeed = 2f;
    public float slideSpeed = 5f;

    public float jumpForce = 2f;
    public float slideAngleThreshold = 50f;//미끄러지는 경사면각도

    //플레이어 추락정보
    public float fallingHight = 8f;
    public float fallDamageRate = 5f;

    // 카메라
    public Transform cameraTransform;
    [SerializeField] private Transform cameraPoint;

    private Vector3 currentCameraPosition;
    public Transform nextCameraTarget;
    public Transform standCameraTransform;
    public Transform crouchCameraTransform;
    public Transform standRifleCameraTransform;
    public Transform loggingCameraTransform;

    public float mouseSensitivity = 2f;
    public float crouchCameraDown = 1f;
    private float verticalRotation = 0f;
    private float verticalLookLimit = 80f;

    [SerializeField] private float cameraLerpSpeed = 5f;

    // 라이트
    [SerializeField] private Light playerLight;

    // 기타 컴포넌트
    public CharacterController characterController { get; private set; }
    public Animator anim;

    // CharacterController 관련
    public Vector3 velocity;
    public float gravity { get; private set; } = -9.81f;
    public float maxGravity = -60;

    // 라이플 관련
    public GameObject rifleObj;
    public float rifleRange = 300f;
    public LayerMask hitLayers;
    public TwoBoneIKConstraint rifleLeftHandIK;
    [SerializeField] private Transform muzzlePos;
    [SerializeField] private GameObject gunFireEff;

    public GameObject axeObj;

    // 발걸음
    public float soundDelay;
    public float walkSoundDelay;
    public float RunSoundDelay;

    // 기타 제어변수
    public bool isCrouch;
    public bool onRifle;
    private bool isCameraTransitioning;
    private int cameraPosType;

    // 이동 거리 기록
    private Vector3 previousPosition;

    #region State
    public PlayerStateMachine stateMachine;
    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerRunState runState;
    public PlayerSitState sitState;
    public PlayerSitWalkState sitWalkState;
    public PlayerSlideState slideState;
    public PlayerJumpState jumpState;
    public PlayerAirState airState;
    public PlayerRifleIdleState rifleIdleState;
    public PlayerRifleWalkState rifleWalkState;
    public PlayerRifleRunState rifleRunState;
    public PlayerRifleAimState rifleAimState;
    public PlayerRifleSitIdleState rifleSitIdleState;
    public PlayerRifleSitWalkState rifleSitWalkState;
    public PlayerRifleSitAimState rifleSitAimState;
    public PlayerLoggingState loggingState;
    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InitState();

        InitComponent();

        currentCameraPosition = standCameraTransform.position;
        nextCameraTarget = standCameraTransform;

        isCameraTransitioning = false;
        isCrouch = false;
        onRifle = false;
        cameraPosType = 0;

        previousPosition = transform.position;

        PlayerManager.Instance.SetPlayerFreeze(false);

        PlayerManager.Instance.SceneManageUpdate();
        PlayerManager.Instance.PlayerSetPos();
    }

    private void Update()
    {
        if (PlayerManager.Instance.playerFreeze)
        {
            return;
        }

        stateMachine.Update();
        HandleMouseLook();
        TrackDistanceMoved();

        if (Input.GetKeyDown(KeyCode.V))
        {
            // FireRifleBullet();
            onRifle = !onRifle;
            rifleObj.SetActive(onRifle);
            anim.SetBool("OnRifle", onRifle);

            if (onRifle)
            {
                stateMachine.ChangeState(rifleIdleState);
                ChangeCameraStandRifle();
                rifleLeftHandIK.weight = 1;
            }
            else
            {
                stateMachine.ChangeState(idleState);
                ChangeCameraStand();
                rifleLeftHandIK.weight = 0;
            }
        }
    }

    // LateUpdate에서 카메라 위치 추종 → 움직임 후 딜레이 없이 부드럽게
    private void LateUpdate()
    {

        if (isCameraTransitioning)
        {
            currentCameraPosition = Vector3.Lerp(currentCameraPosition, nextCameraTarget.position, Time.deltaTime * cameraLerpSpeed);

            // 위치 거의 도달했으면 즉시 고정하고 종료
            if (Vector3.Distance(currentCameraPosition, nextCameraTarget.position) < 0.05f)
            {
                currentCameraPosition = nextCameraTarget.position;
                isCameraTransitioning = false;
            }
        }
        else
        {
            // 즉시 위치로 설정 (회전 등 상황)
            currentCameraPosition = nextCameraTarget.position;
        }

        FollowCamera();
    }

    private void InitComponent()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void PlayerDying()
    {
        PlayerManager.Instance.SetPlayerFreeze(true);

        anim.SetBool("IsLogging", false);
        anim.SetBool("IsDying", true);

        nextCameraTarget = loggingCameraTransform;

        cameraTransform.localRotation = Quaternion.Euler(45, cameraTransform.rotation.y, cameraTransform.rotation.z);
    }

    private void InitState()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "");
        walkState = new PlayerWalkState(this, stateMachine, "");
        runState = new PlayerRunState(this, stateMachine, "");
        sitState = new PlayerSitState(this, stateMachine, "");
        sitWalkState = new PlayerSitWalkState(this, stateMachine, "");
        slideState = new PlayerSlideState(this, stateMachine, "");
        jumpState = new PlayerJumpState(this, stateMachine, "");
        airState = new PlayerAirState(this, stateMachine, "");

        rifleIdleState = new PlayerRifleIdleState(this, stateMachine, "");
        rifleWalkState = new PlayerRifleWalkState(this, stateMachine, "");
        rifleRunState = new PlayerRifleRunState(this, stateMachine, "");
        rifleAimState = new PlayerRifleAimState(this, stateMachine, "");
        rifleSitIdleState = new PlayerRifleSitIdleState(this, stateMachine, "");
        rifleSitWalkState = new PlayerRifleSitWalkState(this, stateMachine, "");
        rifleSitAimState = new PlayerRifleSitAimState(this, stateMachine, "");

        loggingState = new PlayerLoggingState(this, stateMachine, "");

        stateMachine.InitState(idleState);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        transform.Rotate(Vector3.up * mouseX);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, 0f);
    }

    private void FollowCamera()
    {
        cameraTransform.position = currentCameraPosition;
    }

    public bool IsOnSteepSlope()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            // Debug.Log(slopeAngle);
            return slopeAngle > slideAngleThreshold;
        }
        return false;
    }

    public void ChangeCameraCrouch()
    {
        if (cameraPosType == 1) return;

        cameraPosType = 1;
        nextCameraTarget = crouchCameraTransform;
        isCameraTransitioning = true;
        anim.SetBool("IsCrouch", true);
    }

    public void ChangeCameraStand()
    {
        if (cameraPosType == 0) return;

        cameraPosType = 0;
        nextCameraTarget = standCameraTransform;
        isCameraTransitioning = true;
        anim.SetBool("IsCrouch", false);
    }

    public void ChangeCameraStandRifle()
    {
        if (cameraPosType == 2) return;

        cameraPosType = 2;
        nextCameraTarget = standRifleCameraTransform;
        isCameraTransitioning = true;
        anim.SetBool("IsCrouch", false);
    }

    public void ChangeCameraCrouchRifle()
    {
        if (cameraPosType == 1) return;

        cameraPosType = 1;
        nextCameraTarget = crouchCameraTransform;
        isCameraTransitioning = true;
        anim.SetBool("IsCrouch", true);
    }

    public void FireRifleBullet()
    {
        if (TetrisSlot.instanceSlot.itemCountDict[11] > 0)
        {
            TetrisSlot.instanceSlot.itemCountDict[11] -= 1;
            for (int i = 0; i < 1; i++)
            {
                foreach (TetrisItemSlot slot in TetrisSlot.instanceSlot.itemsInBag)
                {
                    if (slot.item.itemCode == 11)
                    {
                        TetrisSlot.instanceSlot.itemsInBag.Remove(slot);
                        Destroy(slot.gameObject);
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.Log("총알이 없습니다.");
        }

        // 이펙트
        GameObject eff = Instantiate(gunFireEff, muzzlePos.position, Quaternion.identity);
        Destroy(eff, 0.5f);

        // 사운드
        SoundManager.Instance.PlayWeaponSound(SoundManager.WeaponType.Gun);

        // 사격 기록
        GameRecode.instance.AddRecord(GameRecordEvent.GunFire);

        Vector3 origin = cameraTransform.transform.position; // 혹은 총구 위치
        Vector3 direction = cameraTransform.transform.forward;

        // raycast로 모든 충돌체 감지
        RaycastHit[] hits = Physics.RaycastAll(origin, direction, rifleRange, hitLayers);

        if (hits.Length == 0) return;

        // 가장 가까운 오브젝트 찾기
        RaycastHit closestHit = hits[0];
        float minDistance = Vector3.Distance(origin, closestHit.point);

        foreach (RaycastHit hit in hits)
        {
            float distance = Vector3.Distance(origin, hit.point);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestHit = hit;
            }
        }

        // 가장 가까운 오브젝트에 Hit 함수 실행
        GameObject hitObject = closestHit.collider.gameObject;

        Debug.Log(hitObject.tag);

        if (hitObject.tag.Contains("NpcBody"))
        {
            GameRecode.instance.AddRecord(GameRecordEvent.ShootHit);
            hitObject.GetComponent<Animal>().TakeDamage(50);
        }
        else if (hitObject.tag.Contains("NpcHead"))
        {
            GameRecode.instance.AddRecord(GameRecordEvent.ShootHit);
            hitObject.GetComponent<AnimalHeadShot>().HeadShot(50);
        }

        Debug.DrawLine(origin, closestHit.point, Color.red, 1f); // 디버그용
    }

    public void PlayerLoggingTree(bool value)
    {
        cameraPosType = 3;
        anim.SetBool("IsLogging", value);
        axeObj.SetActive(value);
        nextCameraTarget = value ? loggingCameraTransform : standCameraTransform;

        if (value == true)
        {
            StartCoroutine(LoggingSound());
        }
    }

    IEnumerator LoggingSound()
    {
        yield return new WaitForSeconds(0.8f);

        SoundManager.Instance.PlayWeaponSound(SoundManager.WeaponType.Hit);
    }

    private void OnDrawGizmos()
    {
        if (cameraTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(cameraTransform.transform.position, cameraTransform.transform.forward * rifleRange);
    }

    public void ToggleRifle()
    {
        // FireRifleBullet();
        onRifle = !onRifle;
        rifleObj.SetActive(onRifle);
        anim.SetBool("OnRifle", onRifle);

        if (onRifle)
        {
            stateMachine.ChangeState(rifleIdleState);
            ChangeCameraStandRifle();
            rifleLeftHandIK.weight = 1;
        }
        else
        {
            stateMachine.ChangeState(idleState);
            ChangeCameraStand();
            rifleLeftHandIK.weight = 0;
        }
    }

    private void TrackDistanceMoved()
    {
        Vector3 currentPosition = transform.position;

        // 수직 이동 제외하고 수평 거리만 추적하고 싶다면:
        Vector3 flatPrev = new Vector3(previousPosition.x, 0, previousPosition.z);
        Vector3 flatCurrent = new Vector3(currentPosition.x, 0, currentPosition.z);

        float distance = Vector3.Distance(flatPrev, flatCurrent);

        // 너무 미세한 움직임은 무시
        if (distance > 0.001f)
        {
            GameRecode.instance.AddRecord(GameRecordEvent.TraveledDistance, distance);
            previousPosition = currentPosition;
        }
    }
}
