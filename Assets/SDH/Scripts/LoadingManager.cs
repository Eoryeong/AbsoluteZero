using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("UI Components")]
    public Image loadingBar;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI percentText;
    public GameObject keyInputText;

    [Header("Loading Settings")]
    public float minimumLoadingTime = 2f;
    public KeyCode continueKey = KeyCode.Space;
    static string nextSceneName;

    private AsyncOperation loadingOperation;
    private bool isLoadingComplete = false;
    private float loadingTimer = 0f;

    void Start()
    {
        if (keyInputText != null)
            keyInputText.SetActive(false);

        StartCoroutine(LoadSceneAsync());
    }

    void Update()
    {
        if (isLoadingComplete && Input.GetKeyDown(continueKey))
        {
            Debug.Log("Continuing to next scene: " + nextSceneName);
            ContinueToNextScene();
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        loadingOperation = SceneManager.LoadSceneAsync(nextSceneName);
        loadingOperation.allowSceneActivation = false;

        loadingTimer = 0f;

        while (!isLoadingComplete)
        {
            loadingTimer += Time.deltaTime;

            // 실제 로딩 진행도 (0 ~ 0.9)
            float realProgress = loadingOperation.progress;

            // 최소 로딩 시간 기반 진행도 (0 ~ 1)
            float timeProgress = loadingTimer / minimumLoadingTime;

            // 두 진행도 중 작은 값을 사용
            float displayProgress = Mathf.Min(realProgress / 0.9f, timeProgress);

            UpdateLoadingUI(displayProgress);

            if (realProgress >= 0.9f && timeProgress >= 1f)
            {
                OnLoadingComplete();
            }

            yield return null;
        }
    }

    private void UpdateLoadingUI(float progress)
    {
        if (loadingBar != null)
        {
            loadingBar.fillAmount = progress;
        }

        if (percentText != null)
        {
            percentText.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }

        if (loadingText != null)
        {
            int dotCount = Mathf.FloorToInt(Time.time * 2) % 4;
            string dots = new string('.', dotCount);
            loadingText.text = $"Loading{dots}";
        }
    }

    private void OnLoadingComplete()
    {
        isLoadingComplete = true;
        if (loadingBar != null)
        {
            loadingBar.fillAmount = 1f;
        }

        if (percentText != null)
        {
            percentText.text = "100%";
        }

        if (loadingText != null)
        {
            loadingText.text = "Loading Complete!";
        }

        // 키 입력 프롬프트 표시
        if (keyInputText != null)
        {
            keyInputText.SetActive(true);
            StartCoroutine(BlinkText());
        }
    }

    private IEnumerator BlinkText()
    {
        if (keyInputText == null) yield break;

        while (isLoadingComplete)
        {
            keyInputText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            keyInputText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ContinueToNextScene()
    {
        if (loadingOperation != null)
        {
            loadingOperation.allowSceneActivation = true;
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene_Test");
    }
}
