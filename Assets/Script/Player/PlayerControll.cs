using Controller;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControll : MonoBehaviour
{
    // 플레이어 이동 속도 
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float sitSpeed = 2f;
    public float slideSpeed = 5f;

    public float jumpForce = 2f;
    public float slideAngleThreshold = 50f;//미끄러지는 경사면각도


    // 카메라
    public Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private Vector3 crouchCameraOffset;

    public float mouseSensitivity = 2f;
    public float crouchCameraDown = 1f;
    private float verticalRotation = 0f;
    private float verticalLookLimit = 80f;

    private Vector3 currentCameraOffset;
    private Vector3 targetCameraOffset;
    [SerializeField] private float cameraLerpSpeed = 5f;

    // 라이트
    [SerializeField] private Light playerLight;

    // 기타 컴포넌트
    public CharacterController characterController { get; private set; }
    public NavMeshObstacle navMeshObstacle;
    private PlayerStatus playerStatus;
    public Animator anim;

    // CharacterController 관련
    public Vector3 velocity;
    public float gravity { get; private set; } = -9.81f;
    public float maxGravity = -60;

    // 라이플 관련
    public float rifleRange = 300f;
    public LayerMask hitLayers;

    // 기타 제어변수
    public bool isCrouch;
    public bool onRifle;

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

        crouchCameraOffset = new Vector3(cameraOffset.x, cameraOffset.y - crouchCameraDown, cameraOffset.z);
        currentCameraOffset = cameraOffset;
        targetCameraOffset = cameraOffset;
        // playerLight.enabled = false;
    }

    private void Update()
    {
        if (playerStatus.playerFreeze) return;

        stateMachine.Update();
        HandleMouseLook();

        if (Input.GetKeyDown(KeyCode.V))
        {
            FireRifleBullet();
        }
    }

    // LateUpdate에서 카메라 위치 추종 → 움직임 후 딜레이 없이 부드럽게
    private void LateUpdate()
    {
        // 현재 카메라 오프셋에서 타겟 카메라 오프셋으로 자연스럽게 이동
        currentCameraOffset = Vector3.Lerp(currentCameraOffset, targetCameraOffset, Time.deltaTime * cameraLerpSpeed);

        FollowCamera();
    }

    private void InitComponent()
    {
        characterController = GetComponent<CharacterController>();
        playerStatus = GetComponent<PlayerStatus>();

        // NavMeshObstacle 컴포넌트 추가 또는 가져오기
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        if (navMeshObstacle == null)
        {
            navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        }
        SetupNavMeshObstacle();
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

        loggingState = new PlayerLoggingState(this, stateMachine, "IsLogging");

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
        cameraTransform.position = cameraPoint.position + currentCameraOffset;
    }

    private void SetupNavMeshObstacle()
    {
        if (navMeshObstacle != null && characterController != null)
        {
            // CharacterController의 크기에 맞춰 NavMeshObstacle 설정
            navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
            navMeshObstacle.radius = characterController.radius;
            navMeshObstacle.height = characterController.height;
            navMeshObstacle.center = characterController.center;
            navMeshObstacle.carving = true; // 동적으로 NavMesh를 조각내기
        }
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
        targetCameraOffset = crouchCameraOffset;
    }

    public void ChangeCameraStand()
    {
        targetCameraOffset = cameraOffset;
    }

    public void FireRifleBullet()
    {
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

        Debug.Log(hitObject.name);

        // 여기에 오브젝트가 맞았을 때의 처리 필요
        // 예: 몬스터가 맞았다면 데미지를 주는 Hit 함수 호출
        // (예시)
        // hitObject.GetComponent<Enemy>()?.Hit(damage);

        Debug.DrawLine(origin, closestHit.point, Color.red, 1f); // 디버그용
    }

    private void PlayerLoggingTree()
    {

    }

    private void OnDrawGizmos()
    {
        if (cameraTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(cameraTransform.transform.position, cameraTransform.transform.forward * rifleRange);
    }
}
