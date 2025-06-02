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
        if (Input.GetMouseButtonDown(0) && isFocus)
        {
            hitObject.GetComponent<InteractObject>().TryInteractObject();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (!isFocus)
            {
                isFocus = true;

                hitObject = hit.collider.gameObject;
                string objName = hitObject.GetComponent<InteractObject>().InteractNameUpdate();

                UIManager.instance.FocusInItem(objName);
            }
        }
        else
        {
            if (isFocus)
            {
                isFocus = false;
                UIManager.instance.FocusOutItem();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactRange);
    }
}
