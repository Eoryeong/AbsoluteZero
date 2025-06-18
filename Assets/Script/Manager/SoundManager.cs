using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


//실행할 매서드
//걸음 종류시
// SoundManager.Instance.PlayFootstep(SoundManager.FootstepType.Snow);

// 무기 종류
// SoundManager.Instance.PlayWeaponSound(SoundManager.WeaponType.Gun);

// 날씨 효과 종류
// SoundManager.Instance.PlayWeatherSound(SoundWeatherType.Snow);

// 환경음 종류
// SoundManager.Instance.PlayAmbientSound(AmbientType.Nature);

// UI/시스템 사운드
// SoundManager.Instance.PlayItemPickup();

// 음식 먹는 종류
// SoundManager.Instance.PlayFoodSound(SoundManager.FoodType.Eating);

// 생존에 필요한 종류
// SoundManager.Instance.PlaySurvivalSound(SoundManager.SurvivalSoundType.Lighter);

// 물 빠졌을때 
// SoundManager.Instance.PlayWaterSound(SoundManager.WaterSoundType.Splash);



[System.Serializable]
public class SoundClipGroup
{
    //랜덤 재생을 위한 클래스
    public string groupName;
    public AudioClip[] clips;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }
}

[System.Serializable]
public class SceneBGMSetting
{
    [Tooltip("씬 이름 (대소문자 구분 안함)")]
    public string sceneName; [Tooltip("재생할 BGM AudioClip")]
    public AudioClip bgmClip;

    [Tooltip("환경음 타입")]
    public AmbientType ambientType = AmbientType.None;

    [Tooltip("날씨 효과 (None은 날씨 효과 없음)")]
    public SoundWeatherType weatherType = SoundWeatherType.None;

    [Tooltip("페이드 인 사용 여부")]
    public bool useFadeIn = true;
}

public enum SoundWeatherType
{
    None, Wind, Snow, Thunder
}

public enum AmbientType
{
    None, Nature, Cave, Forest, Snow, City, Underground
}

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource campfireSource;
    [SerializeField] private AudioSource torchSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource weaponSource;
    [SerializeField] private AudioSource weatherSource;

    [Header("BGM Settings")]
    [SerializeField] private float bgmFadeTime = 2f;


    [Header("Scene BGM Settings")]
    [Tooltip("씬별 BGM 및 환경음 설정")]
    [SerializeField] private SceneBGMSetting[] sceneBGMSettings;
    private Dictionary<string, SceneBGMSetting> sceneBGMDict = new Dictionary<string, SceneBGMSetting>();

    [Header("Footstep Sounds")]
    [SerializeField] private SoundClipGroup snowFootsteps;
    [SerializeField] private SoundClipGroup iceFootsteps;
    [SerializeField] private SoundClipGroup stoneFootsteps;



    [Header("Weapon Sounds")]
    [SerializeField] private SoundClipGroup gunShoot;
    [SerializeField] private SoundClipGroup gunReload;
    [SerializeField] private SoundClipGroup swordSwing;
    [SerializeField] private SoundClipGroup axeSwing;
    [SerializeField] private SoundClipGroup bowShoot;
    [SerializeField] private SoundClipGroup bowDraw;
    [SerializeField] private SoundClipGroup hitSounds;

    [Header("Weather Sounds")]
    [SerializeField] private AudioClip windSound;
    [SerializeField] private AudioClip snowSound;
    [SerializeField] private AudioClip thunderSound;

    [Header("Environment Sounds")]
    [SerializeField] private SoundClipGroup natureAmbient;
    [SerializeField] private SoundClipGroup caveAmbient;
    [SerializeField] private SoundClipGroup snowAmbient;

    [Header("UI/System Sounds")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip itemPickupSound;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip abilityUpSound;


    [Header("Food/Drink Sounds")]
    [SerializeField] private SoundClipGroup eatingSounds;      // 과자/음식 먹는 소리
    [SerializeField] private SoundClipGroup drinkingSounds;    // 음료 마시는 소리
    [SerializeField] private SoundClipGroup crunchySounds;     // 바삭한 과자 소리    
    [Header("Survival/Life Sounds")]
    [SerializeField] private SoundClipGroup lighterSounds;     // 라이터 소리
    [SerializeField] private SoundClipGroup campfireSounds;    // 캠프파이어 소리

    [SerializeField] private SoundClipGroup clothingChangeSounds; // 의상 갈아입기 소리

    [Header("Water Sounds")]
    [SerializeField] private SoundClipGroup waterSplashSounds;  // 물에 빠지는 소리
    [SerializeField] private SoundClipGroup waterDrownSounds;   // 물속에서 죽는 소리 (익사)    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 0.5f;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;
    [Range(0f, 1f)] public float ambientVolume = 0.5f;

    [Header("Mute Settings")]
    public bool isMasterMuted = false;
    public bool isBGMMuted = false;
    public bool isSFXMuted = false;
    public bool isAmbientMuted = false;

    // 현재 재생될 사운드
    private string currentBGM = "";
    private Coroutine bgmFadeCoroutine;

    protected override void Init()
    {
        base.Init();
        InitializeAudioSources();
        InitializeSceneBGMDictionary();

        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoadedCallback;
    }

    protected override void Dispose()
    {
        // 씬 로드 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoadedCallback;
        base.Dispose();
    }

    // Unity 씬 로드 이벤트 콜백
    private void OnSceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        OnSceneChanged(scene.name);
    }

    void Start()
    {
        ApplyVolumeSettings();
        LoadVolumeSettings();
    }

    void Update()
    {
        // 볼륨 설정 실시간 적용 (Inspector에서 조정 시)
        ApplyVolumeSettings();
    }

    private void InitializeAudioSources()
    {
        // AudioSource가 없으면 자동 생성
        if (bgmSource == null) bgmSource = CreateAudioSource("BGM", true);
        if (ambientSource == null) ambientSource = CreateAudioSource("Ambient", true);
        if (campfireSource == null) campfireSource = CreateAudioSource("Ambient02", true);
        if (torchSource == null) torchSource = CreateAudioSource("Torch", true);
        if (sfxSource == null) sfxSource = CreateAudioSource("SFX", false);
        if (footstepSource == null) footstepSource = CreateAudioSource("Footstep", false);
        if (weaponSource == null) weaponSource = CreateAudioSource("Weapon", false);
        if (weatherSource == null) weatherSource = CreateAudioSource("Weather", true);

    }

    private AudioSource CreateAudioSource(string name, bool loop)
    {
        GameObject audioObj = new GameObject($"AudioSource_{name}");
        audioObj.transform.SetParent(transform);
        AudioSource source = audioObj.AddComponent<AudioSource>();
        source.loop = loop;
        source.playOnAwake = false;
        return source;
    }



    private void InitializeSceneBGMDictionary()
    {
        //씬 이름 대소문자 구분 없이 딕셔너리에 저장, 인스펙터에 있는 씬네임
        sceneBGMDict.Clear();
        if (sceneBGMSettings != null)
        {
            foreach (var setting in sceneBGMSettings)
            {
                if (!string.IsNullOrEmpty(setting.sceneName))
                {
                    sceneBGMDict[setting.sceneName.ToLower()] = setting;
                }
            }
        }
    }
    private void ApplyVolumeSettings()
    {
        // 볼륨 설정을 AudioSource에 적용 (뮤트 상태 고려)
        float effectiveMasterVolume = isMasterMuted ? 0f : masterVolume;
        float effectiveBGMVolume = (isBGMMuted || isMasterMuted) ? 0f : bgmVolume;
        float effectiveSFXVolume = (isSFXMuted || isMasterMuted) ? 0f : sfxVolume;
        float effectiveAmbientVolume = (isAmbientMuted || isMasterMuted) ? 0f : ambientVolume;

        if (bgmSource != null) bgmSource.volume = effectiveBGMVolume * effectiveMasterVolume;
        if (ambientSource != null) ambientSource.volume = effectiveAmbientVolume * effectiveMasterVolume;
        if (campfireSource != null) campfireSource.volume = effectiveAmbientVolume * effectiveMasterVolume;
        if (torchSource != null) torchSource.volume = effectiveAmbientVolume * effectiveMasterVolume;
        if (sfxSource != null) sfxSource.volume = effectiveSFXVolume * effectiveMasterVolume;
        if (footstepSource != null) footstepSource.volume = effectiveSFXVolume * effectiveMasterVolume;
        if (weaponSource != null) weaponSource.volume = effectiveSFXVolume * effectiveMasterVolume;
        if (weatherSource != null) weatherSource.volume = effectiveAmbientVolume * effectiveMasterVolume;
    }

    #region BGM Management

    // 다른 클래스에서 사용할 때:
    // 페이드 인으로 BGM 재생
    // SoundManager.Instance.PlayBGM(newBGMClip);

    // 즉시 BGM 재생 (페이드 없음)
    // SoundManager.Instance.PlayBGM(newBGMClip, false);
    public void PlayBGM(AudioClip bgmClip, bool fadeIn = true)
    {
        // BGM 재생 메소드

        if (bgmClip == null) return;

        if (currentBGM == bgmClip.name) return; // 이미 재생 중

        currentBGM = bgmClip.name; // 현재 BGM 업데이트

        if (bgmFadeCoroutine != null)
        {
            // 이전 BGM 페이드 아웃 중지
            StopCoroutine(bgmFadeCoroutine);
        }

        if (fadeIn)
        {
            bgmFadeCoroutine = StartCoroutine(FadeBGM(bgmClip));
        }
        else
        {
            // 즉시 BGM 변경
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }

    public void StopBGM(bool fadeOut = true)
    {
        if (fadeOut)
        {
            // BGM 페이드 아웃
            if (bgmFadeCoroutine != null) StopCoroutine(bgmFadeCoroutine);
            bgmFadeCoroutine = StartCoroutine(FadeOutBGM());
        }
        else
        {
            // 즉시 BGM 중지
            bgmSource.Stop();
        }
        currentBGM = ""; // 현재 BGM 초기화
    }


    // BGM을 다른 BGM으로 교체할 때 사용하는 페이드 효과
    // 기존 BGM을 페이드 아웃 → 새 BGM으로 교체 → 새 BGM을 페이드 인

    private IEnumerator FadeBGM(AudioClip newClip)
    {
        float originalVolume = bgmSource.volume;

        // 1단계: 현재 재생 중인 BGM을 페이드 아웃
        if (bgmSource.isPlaying)
        {
            while (bgmSource.volume > 0)
            {
                bgmSource.volume -= originalVolume * Time.deltaTime / bgmFadeTime;
                yield return null;
            }
        }

        // 2단계: 새로운 BGM으로 교체하고 재생 시작
        bgmSource.clip = newClip;
        bgmSource.Play();

        // 3단계: 새로운 BGM을 페이드 인
        while (bgmSource.volume < originalVolume)
        {
            bgmSource.volume += originalVolume * Time.deltaTime / bgmFadeTime;
            yield return null;
        }

        // 페이드 완료 후 볼륨을 정확한 값으로 설정
        bgmSource.volume = originalVolume;
        bgmFadeCoroutine = null;
    }


    // BGM을 완전히 정지할 때 사용하는 페이드 아웃 효과
    // 현재 BGM을 페이드 아웃한 후 완전히 정지 (새 BGM으로 교체하지 않음)

    private IEnumerator FadeOutBGM()
    {
        float startVolume = bgmSource.volume;

        // 현재 BGM을 페이드 아웃
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / bgmFadeTime;
            yield return null;
        }

        // BGM 완전히 정지하고 볼륨 복원
        bgmSource.Stop();
        bgmSource.volume = startVolume;  // 다음에 BGM을 재생할 때를 위해 볼륨 복원
        bgmFadeCoroutine = null;
    }
    #endregion

    #region Footstep Sounds    
    public enum FootstepType
    {
        Snow, Ice, Stone
    }

    // 다른 클래스에서 사용할 때:
    // 눈 위를 걷는 소리
    // SoundManager.Instance.PlayFootstep(FootstepType.Snow);

    public void PlayFootstep(FootstepType type)
    {
        SoundClipGroup clipGroup = null;

        switch (type)
        {

            case FootstepType.Snow: clipGroup = snowFootsteps; break;
            case FootstepType.Ice: clipGroup = iceFootsteps; break;
            case FootstepType.Stone: clipGroup = stoneFootsteps; break;

        }

        if (clipGroup != null)
        {
            AudioClip clip = clipGroup.GetRandomClip(); // 랜덤으로 발소리 클립 선택
            if (clip != null)
            {
                footstepSource.pitch = Random.Range(0.9f, 1.1f); // 약간의 피치 변화
                footstepSource.PlayOneShot(clip);
            }
        }
    }
    #endregion

    #region Weapon Sounds
    public enum WeaponType
    {
        Gun, Sword, Axe, Bow, BowDraw, Hit, Reload
    }

    // 다른 클래스에서 사용할 때:
    // 총 발사 소리
    // SoundManager.Instance.PlayWeaponSound(WeaponType.Gun);

    public void PlayWeaponSound(WeaponType type)
    {
        SoundClipGroup clipGroup = null; switch (type)
        {
            case WeaponType.Gun: clipGroup = gunShoot; break;
            case WeaponType.Sword: clipGroup = swordSwing; break;
            case WeaponType.Axe: clipGroup = axeSwing; break;
            case WeaponType.Bow: clipGroup = bowShoot; break;
            case WeaponType.BowDraw: clipGroup = bowDraw; break;
            case WeaponType.Hit: clipGroup = hitSounds; break;
            case WeaponType.Reload: clipGroup = gunReload; break;
        }

        if (clipGroup != null)
        {
            // 랜덤으로 무기 소리 클립 선택
            AudioClip clip = clipGroup.GetRandomClip();
            if (clip != null)
            {
                weaponSource.pitch = Random.Range(0.95f, 1.05f);
                weaponSource.PlayOneShot(clip);
            }
        }
    }
    #endregion    #region Weather Sounds


    //다른 클래스에서 사용할때.
    // 눈 내리는 소리를 루프로 재생
    // SoundManager.Instance.PlayWeatherSound(SoundWeatherType.Snow);

    // 눈 내리는 소리를 한 번만 재생
    // SoundManager.Instance.PlayWeatherSound(SoundWeatherType.Snow, false);
    public void PlayWeatherSound(SoundWeatherType type, bool loop = true)
    {
        // 날씨 효과 소리 재생 메소드
        AudioClip clip = null;

        switch (type)
        {
            case SoundWeatherType.Wind: clip = windSound; break;
            case SoundWeatherType.Snow: clip = snowSound; break;
            case SoundWeatherType.Thunder: clip = thunderSound; break;
        }

        if (clip != null)
        {
            if (type == SoundWeatherType.Thunder)
            {
                // 천둥소리는 한 번만 재생
                sfxSource.PlayOneShot(clip);
            }
            else
            {
                // 바람과 눈 소리는 루프 재생
                weatherSource.clip = clip;
                weatherSource.loop = loop;
                weatherSource.Play();
            }
        }
    }

    public void StopWeatherSound()
    {
        weatherSource.Stop();
    }

    #region Environment Sounds

    // 다른 클래스에서 사용할 때:
    // 자연 환경음 (새소리, 바람소리)
    // SoundManager.Instance.PlayAmbientSound(AmbientType.Nature);

    public void PlayAmbientSound(AmbientType ambientType, bool loop = true)
    {
        // 환경음 재생 메소드
        if (ambientType == AmbientType.None) return;

        SoundClipGroup clipGroup = null;

        switch (ambientType)
        {
            case AmbientType.Nature: clipGroup = natureAmbient; break;
            case AmbientType.Cave: clipGroup = caveAmbient; break;
            case AmbientType.Forest: clipGroup = snowAmbient; break;
            case AmbientType.Snow: clipGroup = snowAmbient; break;
            // 추가 환경음이 필요하면 여기에 추가

            case AmbientType.City:
                // cityAmbient가 있다면 할당
                break;
            case AmbientType.Underground:
                clipGroup = caveAmbient; // 동굴음을 지하 환경음으로 사용
                break;
        }

        if (clipGroup != null)
        {
            AudioClip clip = clipGroup.GetRandomClip();
            if (clip != null)
            {
                ambientSource.clip = clip;
                ambientSource.loop = loop; // 루프 여부 설정
                ambientSource.Play();
            }
        }
    }



    public void StopAmbientSound()
    {
        ambientSource.Stop();
    }
    #endregion




    #region SFX Sounds


    // SFX를 재생하는 메소드    

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }


    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }


    public void PlayItemPickup()
    {
        PlaySFX(itemPickupSound);
    }


    public void PlayDoorOpen()
    {
        PlaySFX(doorOpenSound);
    }

    // 다른 클래스에서 사용할 때:
    // 레벨업/능력 향상 소리
    // SoundManager.Instance.PlayLevelUp();
    public void PlayLevelUp()
    {
        PlaySFX(abilityUpSound);
    }


    #endregion



    #region Scene Management
    public void OnSceneChanged(string sceneName)
    {
        string sceneKey = sceneName.ToLower();

        if (sceneBGMDict.ContainsKey(sceneKey)) // 씬 이름을 소문자로 변환하여 딕셔너리에서 찾기
        {
            SceneBGMSetting setting = sceneBGMDict[sceneKey];

            // BGM 재생
            if (setting.bgmClip != null)
            {
                PlayBGM(setting.bgmClip, setting.useFadeIn);
            }            // 환경음 재생
            if (setting.ambientType != AmbientType.None)
            {
                PlayAmbientSound(setting.ambientType);
            }

            // 날씨 효과 재생
            if (setting.weatherType != SoundWeatherType.None)
            {
                PlayWeatherSound(setting.weatherType);
            }
        }
        else
        {
            Debug.Log($"씬 '{sceneName}'에 대한 BGM 설정을 찾을 수 없습니다. Inspector에서 Scene BGM Settings를 확인해주세요.");
        }
    }
    #endregion
    #region Food/Drink Sounds
    public enum FoodType
    {
        Eating,      // 일반 음식 먹기
        Drinking,    // 음료 마시기  
        Crunchy      // 바삭한 과자류
    }

    // 다른 클래스에서 사용할 때:
    // 일반 음식 먹는 소리
    // SoundManager.Instance.PlayFoodSound(FoodType.Eating);
    // 특정 볼륨으로 재생
    // SoundManager.Instance.PlayFoodSound(FoodType.Eating, 0.7f);

    public void PlayFoodSound(FoodType type)
    {
        SoundClipGroup clipGroup = null;

        switch (type)
        {
            case FoodType.Eating: clipGroup = eatingSounds; break;      // 음식 먹는 소리
            case FoodType.Drinking: clipGroup = drinkingSounds; break;  // 음료 마시는 소리
            case FoodType.Crunchy: clipGroup = crunchySounds; break;    // 바삭한 과자 소리
        }

        if (clipGroup != null)
        {
            // 랜덤으로 음식/음료 소리 클립 선택
            AudioClip clip = clipGroup.GetRandomClip();
            if (clip != null)
            {
                sfxSource.pitch = Random.Range(0.95f, 1.05f);  // 약간의 피치 변화
                sfxSource.PlayOneShot(clip);
            }
        }
        else
        {
            Debug.LogWarning($"음식/음료 소리 타입 '{type}'에 대한 클립이 설정되지 않았습니다.");
        }
    }



    #endregion
    #region Survival/Life Sounds
    public enum SurvivalSoundType
    {
        Lighter,         // 라이터 켜기/끄기
        Campfire,        // 캠프파이어 타는 소리
        Torch,           // 횃불 켜기/끄기 (캠프파이어와 동일한 소리 사용)
        ClothingChange   // 의상 갈아입기
    }

    // 다른 클래스에서 사용할 때:
    // 라이터 켜기 소리
    // SoundManager.Instance.PlaySurvivalSound(SurvivalSoundType.Lighter);
    // 캠프파이어 소리 (루프 재생)    
    // SoundManager.Instance.PlaySurvivalSound(SurvivalSoundType.Campfire);


    // 생존/생활 관련 소리를 재생합니다.

    public void PlaySurvivalSound(SurvivalSoundType type)
    {
        SoundClipGroup clipGroup = null; switch (type)
        {
            case SurvivalSoundType.Lighter: clipGroup = lighterSounds; break;           // 라이터 소리
            case SurvivalSoundType.Campfire: clipGroup = campfireSounds; break;        // 캠프파이어 소리
            case SurvivalSoundType.Torch: clipGroup = campfireSounds; break;           // 횃불 소리 (캠프파이어와 동일)
            case SurvivalSoundType.ClothingChange: clipGroup = clothingChangeSounds; break; // 의상 갈아입기
        }

        if (clipGroup != null)
        {
            // 랜덤으로 생존/생활 소리 클립 선택
            AudioClip clip = clipGroup.GetRandomClip(); if (clip != null)
            {
                // 캠프파이어와 횃불은 지속적인 소리이므로 ambientSource 사용, 나머지는 sfxSource
                if (type == SurvivalSoundType.Campfire)
                {
                    campfireSource.clip = clip;
                    campfireSource.loop = true;  // 캠프파이어 루프 재생
                    campfireSource.Play();
                }
                else if (type == SurvivalSoundType.Torch)
                {
                    torchSource.clip = clip;
                    torchSource.loop = true;  // 횃불 루프 재생
                    torchSource.Play();
                }
                else
                {
                    sfxSource.pitch = Random.Range(0.95f, 1.05f);  // 약간의 피치 변화
                    sfxSource.PlayOneShot(clip);
                }
            }
        }
        else
        {
            Debug.LogWarning($"생존/생활 소리 타입 '{type}'에 대한 클립이 설정되지 않았습니다.");
        }
    }





    /// 현재 재생 중인 불 소리를 정지합니다.
    /// (캠프파이어, 횃불 등 모든 불 관련 소리가 ambientSource를 공유하므로 하나의 메서드로 통일)

    public void StopFireSound()
    {
        if (ambientSource.isPlaying && ambientSource.clip != null)
        {
            campfireSource.Stop();
            Debug.Log("불 소리가 정지되었습니다.");
        }
    }


    /// StopFireSound()와 동일 (명확한 네이밍을 위한 별칭)
    public void StopTorchSound()
    {
        if (torchSource.isPlaying && torchSource.clip != null)
        {
            torchSource.Stop();
            Debug.Log("횃불 소리가 정지되었습니다.");
        }
    }
    #endregion
    #region Water Sounds
    public enum WaterSoundType
    {
        Splash,    // 물에 빠지는 소리
        Drown      // 물속에서 죽는 소리 (익사)
    }

    // 다른 클래스에서 사용할 때:
    // 물에 빠지는 소리
    // SoundManager.Instance.PlayWaterSound(WaterSoundType.Splash);

    // 익사 소리
    // SoundManager.Instance.PlayWaterSound(WaterSoundType.Drown);

    // 특정 볼륨으로 재생
    // SoundManager.Instance.PlayWaterSound(WaterSoundType.Splash, 0.8f);

    // 익사 시퀀스 재생 (물에 빠짐 → 익사)
    // SoundManager.Instance.PlayDrowningSequence();


    public void PlayWaterSound(WaterSoundType type)
    {
        SoundClipGroup clipGroup = null;

        switch (type)
        {
            case WaterSoundType.Splash: clipGroup = waterSplashSounds; break;  // 물에 빠지는 소리
            case WaterSoundType.Drown: clipGroup = waterDrownSounds; break;    // 익사 소리
        }

        if (clipGroup != null)
        {
            // 랜덤으로 물 소리 클립 선택
            AudioClip clip = clipGroup.GetRandomClip();
            if (clip != null)
            {
                // 물 소리는 중요한 게임 이벤트이므로 피치 변화 없이 원본 그대로 재생
                sfxSource.pitch = 1.0f;
                sfxSource.PlayOneShot(clip);

                // 물에 빠지는 경우 횃불 자동 정지 (물에 젖어서 꺼짐)
                if (type == WaterSoundType.Splash)
                {
                    StopTorchSound();
                    Debug.Log("물에 빠져서 횃불이 꺼졌습니다.");
                }

                // 익사 소리의 경우 추가 로직 (예: 화면 효과, 게임오버 등)을 위한 로그
                if (type == WaterSoundType.Drown)
                {
                    Debug.Log("익사 사운드 재생됨 - 게임오버 로직 실행 가능");
                }
            }
        }
        else
        {
            Debug.LogWarning($"물 소리 타입 '{type}'에 대한 클립이 설정되지 않았습니다.");
        }
    }

    /// 물에 빠지는 시퀀스 소리를 재생합니다 (물방울 -> 첨벙 -> 익사).
    /// 생존 게임에서 플레이어가 물에 빠져 죽을 때 사용합니다.

    public void PlayDrowningSequence()
    {
        StartCoroutine(DrowningSequenceCoroutine());
    }
    private IEnumerator DrowningSequenceCoroutine()
    {
        // 물에 빠지자마자 횃불 소리 정지 (물에 젖어서 꺼짐)
        StopTorchSound();

        // 1단계: 물에 빠지는 소리
        PlayWaterSound(WaterSoundType.Splash);

        // 잠시 대기 (물에 빠지는 소리가 끝날 때까지)
        yield return new WaitForSeconds(1.0f);

        // 2단계: 익사 소리
        PlayWaterSound(WaterSoundType.Drown);

        Debug.Log("익사 시퀀스 완료 - 게임오버 처리 필요 / 횃불 자동 정지됨");
    }
    #endregion




    #region Volume Control
    // 볼륨 설정을 저장하고 불러오는 메소드
    public void SetMasterVolume(float volume)
    {
        // 마스터 볼륨 설정
        masterVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }

    public void SetBGMVolume(float volume)
    {
        // BGM 볼륨 설정
        bgmVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        // SFX 볼륨 설정
        sfxVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }

    public void SetAmbientVolume(float volume)
    {
        // 환경음 볼륨 설정
        ambientVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }
    private void SaveVolumeSettings()
    {
        // 볼륨 설정을 PlayerPrefs에 저장
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);

        // 뮤트 설정 저장 (bool을 int로 변환: false = 0, true = 1)
        PlayerPrefs.SetInt("MasterMuted", isMasterMuted ? 1 : 0);
        PlayerPrefs.SetInt("BGMMuted", isBGMMuted ? 1 : 0);
        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);
        PlayerPrefs.SetInt("AmbientMuted", isAmbientMuted ? 1 : 0);

        PlayerPrefs.Save(); // 변경 사항 저장
    }
    private void LoadVolumeSettings()
    {
        // PlayerPrefs에서 볼륨 설정을 불러오기,씬이동시 사용하기 위해
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 0.5f);

        // 뮤트 설정 불러오기 (0 = false, 1 = true)
        isMasterMuted = PlayerPrefs.GetInt("MasterMuted", 0) == 1;
        isBGMMuted = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        isAmbientMuted = PlayerPrefs.GetInt("AmbientMuted", 0) == 1; ApplyVolumeSettings();
    }


    public void SetMasterMute(bool muted)
    {
        isMasterMuted = muted;
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }


    public void SetBGMMute(bool muted)
    {
        isBGMMuted = muted;
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }

    public void SetSFXMute(bool muted)
    {
        isSFXMuted = muted;
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }


    public void SetAmbientMute(bool muted)
    {
        isAmbientMuted = muted;
        ApplyVolumeSettings();
        SaveVolumeSettings();
    }


    public bool IsMasterMuted() => isMasterMuted;


    public bool IsBGMMuted() => isBGMMuted;


    public bool IsSFXMuted() => isSFXMuted;


    public bool IsAmbientMuted() => isAmbientMuted;





    #endregion

}
