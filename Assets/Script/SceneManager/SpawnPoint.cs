using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private string spawnID;
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;
    [SerializeField] private bool showGizmo = true;
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private float gizmoSize = 1f;
    
    public string SpawnID => spawnID;
    public Vector3 SpawnPosition => transform.position + spawnOffset;
    
    private void OnValidate()
    {
        // Inspector에서 자동으로 spawnID 설정
        if (string.IsNullOrEmpty(spawnID))
        {
            spawnID = gameObject.name;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!showGizmo) return;
        
        Gizmos.color = gizmoColor;
        Vector3 spawnPos = transform.position + spawnOffset;
        
        // 스폰 위치에 구 그리기
        Gizmos.DrawWireSphere(spawnPos, gizmoSize * 0.5f);
        
        // 플레이어 방향 표시 (앞쪽 화살표)
        Vector3 forward = transform.forward * gizmoSize;
        Gizmos.DrawRay(spawnPos, forward);
        
        // 화살표 머리 부분
        Vector3 arrowHead1 = spawnPos + forward + transform.right * 0.3f * gizmoSize - transform.forward * 0.3f * gizmoSize;
        Vector3 arrowHead2 = spawnPos + forward - transform.right * 0.3f * gizmoSize - transform.forward * 0.3f * gizmoSize;
        
        Gizmos.DrawLine(spawnPos + forward, arrowHead1);
        Gizmos.DrawLine(spawnPos + forward, arrowHead2);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!showGizmo) return;
        
        Gizmos.color = Color.yellow;
        Vector3 spawnPos = transform.position + spawnOffset;
        
        // 선택된 상태에서는 더 큰 원으로 표시
        Gizmos.DrawWireSphere(spawnPos, gizmoSize);
        
        // 스폰 ID 표시 (Scene 뷰에서만 보임)
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(spawnPos + Vector3.up * (gizmoSize + 0.5f), $"Spawn: {spawnID}");
        #endif
    }
}
