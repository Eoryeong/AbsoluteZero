using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Camera playerCamera;

    private bool isFocus;
    private GameObject hitObject;

    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (playerStatus.playerFreeze) return;

        TryInteract();
        if (Input.GetMouseButtonDown(0) && isFocus && hitObject != null)
        {
            InteractObject interactable = hitObject.GetComponentInParent<InteractObject>();
            if(interactable != null)
            {
                interactable.TryInteractObject();
            }
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            GameObject target = hit.collider.gameObject;

            // 자식 오브젝트인 경우 부모 오브젝트 검사
            InteractObject interactable = target.GetComponentInParent<InteractObject>();

            if(interactable != null)
            {
                if (!isFocus || interactable.gameObject != hitObject)
                {
                    isFocus = true;
                    hitObject = interactable.gameObject;

                    string objName = interactable.GetComponent<InteractObject>().InteractNameUpdate();
                    UIManager.instance.FocusInItem(objName);
                }
            }
            else
            {
                ClearFocus();
            }
        }
        else
        {
            ClearFocus();
        }
    }

    private void ClearFocus()
    {
        if (isFocus)
        {
            isFocus = false;
            hitObject = null;
            UIManager.instance.FocusOutItem();
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactRange);
    }
}
