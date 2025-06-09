using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    // 플레이어 이동 속도 
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float sitSpeed = 2f;

    public float jumpForce = 2f;

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
    public Rigidbody rb;
    private PlayerStatus playerStatus;

    #region State
    private PlayerStateMachine stateMachine;
    public PlayerIdleStete idleStete;
    public PlayerWalkState walkState;
    public PlayerRunState runState;
    public PlayerSitState sitState;
    public PlayerSitWalkState sitWalkState;
    public PlayerJumpState jumpState;
    public PlayerAirState airState;
	#endregion

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		InitState();
		InitComponent();

		sitCameraOffset = new Vector3(cameraOffset.x, cameraOffset.y - sitCameraDown, cameraOffset.z);
		currentCameraOffset = cameraOffset;
		targetCameraOffset = cameraOffset;
		playerLight.enabled = false;
	}

	private void Update()
    {
        if (playerStatus.playerFreeze) return;

        stateMachine.Update();
        GroundCheck();
        HandleMouseLook();
    }

    // LateUpdate에서 카메라 위치 추종 → 움직임 후 딜레이 없이 부드럽게
    private void LateUpdate()
    {
        // 현재 카메라 오프셋에서 타겟 카메라 오프셋으로 자연스럽게 이동
        currentCameraOffset = Vector3.Lerp(currentCameraOffset, targetCameraOffset, Time.deltaTime * cameraLerpSpeed);

        FollowCamera();
    }

    private void InitState()
    {
        stateMachine = new PlayerStateMachine();

        idleStete = new PlayerIdleStete(this, stateMachine, "Idle");
        walkState = new PlayerWalkState(this, stateMachine, "Walk");
        runState = new PlayerRunState(this, stateMachine, "Run");
        sitState = new PlayerSitState(this, stateMachine, "Sit");
        sitWalkState = new PlayerSitWalkState(this, stateMachine, "SitWalk");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Fall");

        stateMachine.InitState(idleStete);
    }

	private void InitComponent()
	{
		rb = GetComponent<Rigidbody>();
		playerStatus = GetComponent<PlayerStatus>();
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

    public bool GroundCheck()
    {
        isGround = Physics.CheckSphere(groundCheckOffset.position, groundCheckRadius, groundLayer);
        return isGround;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckOffset.position, groundCheckRadius);
    }
}
