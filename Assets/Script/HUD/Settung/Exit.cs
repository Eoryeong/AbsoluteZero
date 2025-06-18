using UnityEngine;

public class Exit : MonoBehaviour
{
    // 게임 종료
    public void ExitGame()
    {
        // 게임 종료
        Application.Quit();

        // 에디터에서 실행 중인 경우
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
