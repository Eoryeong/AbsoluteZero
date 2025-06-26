using System.Collections.Generic;
using UnityEngine;

public class CampfireArea : MonoBehaviour
{
    public float detectionRadius = 3f;
    public LayerMask detectionLayer;

    private HashSet<PickupItem> grillTargets = new(); // 현재 영역 내 고기

    private void Update()
    {
        foreach (var item in grillTargets)
        {
            if (item == null) continue;

            // itemCode 6 = 날고기
            if (item.data.itemCode == 6)
            {
                item.grillTime -= Time.deltaTime;

                if (item.grillTime <= 0)
                {
                    // 익은 고기 생성 itemCode 7 = 익힌 고기
                    Instantiate(ItemDatabase.instance.GetItemPrefabByCode(7), item.transform.position, Quaternion.identity);
                    Destroy(item.gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // 주기적으로 주변 오브젝트 확인 (최소화된 검사)
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        HashSet<PickupItem> currentFrame = new();

        foreach (var col in hits)
        {
            if (col.CompareTag("Item"))
            {
                var item = col.GetComponent<PickupItem>();
                if (item != null && item.data.itemCode == 6)
                {
                    currentFrame.Add(item);
                    grillTargets.Add(item); // 새로 들어온 항목 포함
                }
            }
        }

        // 영역을 벗어난 오브젝트 제거
        grillTargets.RemoveWhere(item => !currentFrame.Contains(item));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}