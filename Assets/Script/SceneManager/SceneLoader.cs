using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingProgressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image fadeImage;

    [Header("Loading Settings")]
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float minLoadingTime = 1f; // 최소 로딩 시간 (너무 빨리 지나가지 않도록)


    private bool isLoading = false;

    // 비동기 씬 로드 (페이드 효과 포함)
    public void LoadSceneAsync(string sceneName)
    {
        if (isLoading) return;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneLoader: sceneName이 비어있습니다!");
            return;
        }

        // 씬이 존재하는지 확인
        if (!IsSceneValid(sceneName))
        {
            Debug.LogError($"SceneLoader: 씬 '{sceneName}'을 찾을 수 없습니다!");
            return;
        }

        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        isLoading = true;
        float startTime = Time.time;

        // 플레이어 동작 정지
        FreezePlayer(true);

        // 페이드 인 (화면을 어둡게)
        yield return StartCoroutine(FadeIn());

        // 로딩 UI 활성화
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        // 로딩 사운드 재생
        PlayLoadingSound();

        // 비동기 씬 로드 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // 90%까지만 로드하고 대기

        // 로딩 진행률 업데이트
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            UpdateLoadingUI(progress, sceneName);

            // 90% 로드 완료 및 최소 로딩 시간 경과 시 씬 활성화
            if (asyncLoad.progress >= 0.9f && Time.time - startTime >= minLoadingTime)
            {
                UpdateLoadingUI(1f, sceneName);
                yield return new WaitForSeconds(0.5f); // 잠시 대기
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoading = false;
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;

        Color color = fadeImage.color;

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }

    private void UpdateLoadingUI(float progress, string sceneName)
    {
        if (loadingProgressBar != null)
        {
            loadingProgressBar.value = progress;
        }

        if (loadingText != null)
        {
            loadingText.text = $"Loading ... {Mathf.RoundToInt(progress * 100)}%";
        }
    }

    private void FreezePlayer(bool freeze)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            
            if (PlayerManager.Instance != null)
            {
				PlayerManager.Instance.SetPlayerFreeze(freeze);
            }
        }
    }

    private void PlayLoadingSound()
    {
        // 로딩 사운드 재생 (예: AudioManager를 통해)
    }

    private bool IsSceneValid(string sceneName)
    {
        // Build Settings에 씬이 포함되어 있는지 확인
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameFromPath == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    // 즉시 로드 (기존 방식)
    public void LoadSceneImmediate(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneLoader: sceneName이 비어있습니다!");
            return;
        }

        if (!IsSceneValid(sceneName))
        {
            Debug.LogError($"SceneLoader: 씬 '{sceneName}'을 찾을 수 없습니다!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    // UI 버튼용 비동기 로드
    public void OnClickLoadSceneAsync(string sceneName)
    {
        LoadSceneAsync(sceneName);
    }

    // UI 버튼용 즉시 로드  
    public void OnClickLoadSceneImmediate(string sceneName)
    {
        LoadSceneImmediate(sceneName);
    }

    // 씬 전환 완료 후 호출 (새 씬에서 사용)
    public void OnSceneLoaded()
    {
        StartCoroutine(OnSceneLoadedCoroutine());
    }

    private IEnumerator OnSceneLoadedCoroutine()
    {
        // 로딩 UI 비활성화
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        // 페이드 아웃 (화면을 밝게)
        yield return StartCoroutine(FadeOut());

        // 플레이어 동작 재개
        FreezePlayer(false);
    }
}
