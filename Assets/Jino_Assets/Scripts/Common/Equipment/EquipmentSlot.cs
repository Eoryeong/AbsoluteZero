using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public ItemBehaviour itemBehaviour;
    public Image icon;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.position = eventData.position;
        transform.SetParent(GameObject.FindWithTag("Category").transform, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 밑에 ui 컴포넌트 게임 오브젝트 있을 시
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GameObject hoveredObj = eventData.pointerEnter;
            if (hoveredObj != null && hoveredObj.CompareTag("Inventory"))
            {
                Equipment.instance.equipmentSlot.Remove(this);
                TetrisSlot.instanceSlot.addInFirstSpace(itemBehaviour);
                Destroy(gameObject);
            }
            else
            {
                transform.position = initialPosition;
            }
        }
        else    //ui 컴포넌트 외에 놓을 시
        {
            transform.position = initialPosition;
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Equipment.instance.equipmentSlot.Remove(this);
            TetrisSlot.instanceSlot.addInFirstSpace(itemBehaviour);
            Destroy(gameObject);
        }
    }
}
