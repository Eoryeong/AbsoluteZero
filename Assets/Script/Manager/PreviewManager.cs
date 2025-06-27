using UnityEngine;

public class PreviewManager : SingletonBehaviour<PreviewManager>
{
    private void Awake()
    {
        // 씬 전환 시 파괴되지 않도록 설정
        m_IsDestroyOnLoad = false;
    }
}
