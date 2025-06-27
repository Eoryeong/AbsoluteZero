using UnityEngine;

public class HUD : SingletonBehaviour<HUD>
{
    protected override void Init()
    {
        base.Init();

        // 씬 전환 시 파괴되지 않도록 설정
        m_IsDestroyOnLoad = false;
    }
}
