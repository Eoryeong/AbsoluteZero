using UnityEngine;

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
    private Rigidbody rb;
    private PlayerStatus playerStatus;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        playerStatus = GetComponent<PlayerStatus>();

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
    }

    private void PlayerSitCancel()
    {
        isSit = false;
        applySpeed = walkSpeed;
        // 타겟 카메라 오프셋을 평상태 오프셋으로 변경
        targetCameraOffset = cameraOffset;
    }

    private void PlayerJump()
    {
        // 앉았을때 점프 시도 앉은 상태 해제
        if (isSit)
        {
            PlayerSitCancel();
        }
        rb.linearVelocity = transform.up * jumpForce;
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move * applySpeed;

        Vector3 newPosition = rb.position + velocity * Time.deltaTime;
        rb.MovePosition(newPosition);
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
        isGround = Physics.CheckSphere(groundCheckOffset.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckOffset.position, groundCheckRadius);
    }
}
