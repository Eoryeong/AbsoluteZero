using UnityEngine;
using UnityEngine.AI;

public class PlayerControll : MonoBehaviour
{
    // 플레이어 이동 속도 
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float sitSpeed = 2f;
    public float applySpeed;

    public float jumpForce = 2f;

    private bool isRun = false;
    private bool isSit = false;

    // 카메라
    public Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private Vector3 sitCameraOffset;

    public float mouseSensitivity = 2f;
    public float sitCameraDown = 1f;
    private float verticalRotation = 0f;
    private float verticalLookLimit = 80f;

    private Vector3 currentCameraOffset;
    private Vector3 targetCameraOffset;
    [SerializeField] private float cameraLerpSpeed = 5f;


    // 바닥 충돌 감지
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;
    private bool isGround;

    // 라이트
    [SerializeField] private Light playerLight;

    // 기타 컴포넌트
    public CharacterController characterController { get; private set; }
    private PlayerStatus playerStatus;
    private NavMeshObstacle navMeshObstacle;

    // CharacterController 관련
    private Vector3 velocity;
    [SerializeField] private float gravity = -9.81f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        playerStatus = GetComponent<PlayerStatus>();

        // NavMeshObstacle 컴포넌트 추가 또는 가져오기
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        if (navMeshObstacle == null)
        {
            navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        }
        SetupNavMeshObstacle();

        applySpeed = walkSpeed;
        sitCameraOffset = new Vector3(cameraOffset.x, cameraOffset.y - sitCameraDown, cameraOffset.z);
        currentCameraOffset = cameraOffset;
        targetCameraOffset = cameraOffset;
        playerLight.enabled = false;
    }

    private void Update()
    {
        if (playerStatus.playerFreeze) return;
        InputCheck();
        GroundCheck();
        HandleMovement();
        HandleMouseLook();
    }

    // LateUpdate에서 카메라 위치 추종 → 움직임 후 딜레이 없이 부드럽게
    private void LateUpdate()
    {
        // 현재 카메라 오프셋에서 타겟 카메라 오프셋으로 자연스럽게 이동
        currentCameraOffset = Vector3.Lerp(currentCameraOffset, targetCameraOffset, Time.deltaTime * cameraLerpSpeed);

        FollowCamera();
    }

    private void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerRunStart();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && !isSit)
        {
            PlayerRunCancel();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isSit)
            {
                PlayerSitStart();
            }
            else
            {
                PlayerSitCancel();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            PlayerJump();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            playerLight.enabled = !playerLight.enabled;
        }
    }

    private void PlayerRunStart()
    {
        if (isSit)
        {
            PlayerSitCancel();
        }
        isRun = true;
        applySpeed = runSpeed;
    }

    private void PlayerRunCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void PlayerSitStart()
    {
        isSit = true;
        applySpeed = sitSpeed;
        // 타겟 카메라 오프셋을 앉은상태 오프셋으로 변경
        targetCameraOffset = sitCameraOffset;
        UpdateNavMeshObstacleHeight();
    }

    private void PlayerSitCancel()
    {
        isSit = false;
        applySpeed = walkSpeed;
        // 타겟 카메라 오프셋을 평상태 오프셋으로 변경
        targetCameraOffset = cameraOffset;
        UpdateNavMeshObstacleHeight();
    }

    private void PlayerJump()
    {
        // 앉았을때 점프 시도 앉은 상태 해제
        if (isSit)
        {
            PlayerSitCancel();
        }

        if (isGround)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move = move.normalized * applySpeed;

        // 중력 적용
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 바닥에 붙어있도록
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // 최종 이동 벡터 (수평 이동 + 수직 이동)
        Vector3 finalMove = move * Time.deltaTime + Vector3.up * velocity.y * Time.deltaTime;
        characterController.Move(finalMove);
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
        cameraTransform.position = transform.position + currentCameraOffset;
    }

    private void GroundCheck()
    {
        // CharacterController의 isGrounded를 기본으로 사용
        // 추가적인 체크가 필요한 경우에만 Physics.CheckSphere 사용
        isGround = characterController.isGrounded;

        // 더 정확한 바닥 감지가 필요한 경우 추가 체크
        if (!isGround && groundCheckOffset != null)
        {
            isGround = Physics.CheckSphere(groundCheckOffset.position, groundCheckRadius, groundLayer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckOffset.position, groundCheckRadius);
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

    // 앉기 상태에 따라 NavMeshObstacle 크기 조정
    private void UpdateNavMeshObstacleHeight()
    {
        if (navMeshObstacle != null)
        {
            if (isSit)
            {
                navMeshObstacle.height = characterController.height * 0.5f; // 앉은 상태
            }
            else
            {
                navMeshObstacle.height = characterController.height; // 서있는 상태
            }
        }
    }
}
