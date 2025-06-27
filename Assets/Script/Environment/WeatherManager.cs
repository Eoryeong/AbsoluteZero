using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeatherType
{
	Sunny,
	Cloudy,
	Foggy,
	Snowy
}

public class WeatherManager : SingletonBehaviour<WeatherManager>
{
    [SerializeField] private WeatherType currentWeatherType;

    [SerializeField] private int weatherDuration;
    private int weatherStartHour;
    private int weatherStartMinute;
    private int weatherCurrentHour;
    private int weatherCurrentMinute;

    private Light lightCompo;
    private PlayerControll player;

    private float currentIntensity;

    [SerializeField] private GameObject fogPrefab;
    [SerializeField] private GameObject snowPrefab;
    private GameObject fog;
    private GameObject snow;

    [SerializeField] private Material sunnyAfternoonSkyBox;
    [SerializeField] private Material sunnyNightSkyBox;
    [SerializeField] private Material CloduyAfternoonSkyBox;
    [SerializeField] private Material CloduyNightSkyBox;

    [SerializeField] private float sunnyIntensity;
    [SerializeField] private float cloudyIntensity;

    void Start()
	{
		InitWeather();
		SetWeather();
	}

	void Update()
    {
        UpdateWeather();
        DebugWeather();
    }

	private void InitWeather()
	{
		player = PlayerManager.Instance.PlayerController;
        lightCompo = RenderSettings.sun;

		fog = Instantiate(fogPrefab);
        snow = Instantiate(snowPrefab);

		fog.gameObject.SetActive(false);
		snow.gameObject.SetActive(false);
	}

    private void SetWeather()
    {
        int weather = Random.Range(0, 10);
        int time = Random.Range(60, 721);

        weatherCurrentHour = 0;
        weatherCurrentMinute = 0;

        if (weather < 5)
        {
            currentWeatherType = WeatherType.Sunny;
            currentIntensity = sunnyIntensity;
        }
        else if (weather < 8)
        {
            currentWeatherType = WeatherType.Cloudy;
            currentIntensity = cloudyIntensity;
        }
        else if (weather < 9)
        {
            currentWeatherType = WeatherType.Foggy;
            currentIntensity = cloudyIntensity;
            SpawnParticle(currentWeatherType);
        }
        else if(weather < 10)
        {
            currentWeatherType = WeatherType.Snowy;
            currentIntensity = cloudyIntensity;
            SpawnParticle(currentWeatherType);
        }

        SetSkyBox();

        weatherDuration = time;

        weatherStartHour = TimeManager.Instance.gameHour;
        weatherStartMinute = TimeManager.Instance.gameMinute;
    }

    private void UpdateWeather()
    {
        weatherCurrentHour = TimeManager.Instance.gameHour - weatherStartHour;
        weatherCurrentMinute = TimeManager.Instance.gameMinute - weatherStartMinute;

        if(weatherDuration < weatherCurrentHour * 60 + weatherCurrentMinute)
        {
            DestroyParticle(currentWeatherType);           
            SetWeather();
            return;
        }

        if(SceneManager.GetActiveScene().name == "In_Game_Scene")
        {
            lightCompo.intensity = Mathf.Lerp(lightCompo.intensity, currentIntensity, Time.deltaTime * 0.5f);
        }
    }

    private void SetSkyBox()
    {
        if(TimeManager.Instance.gameHour > 9)
        {
            if(currentWeatherType == WeatherType.Sunny)
            {
                RenderSettings.skybox = sunnyAfternoonSkyBox;
            }
            else
            {
                RenderSettings.skybox = CloduyAfternoonSkyBox;
            }
        }
        else
        {
			if (currentWeatherType == WeatherType.Sunny)
			{
				RenderSettings.skybox = sunnyNightSkyBox;
			}
			else
			{
				RenderSettings.skybox = CloduyNightSkyBox;
			}
		}


        DynamicGI.UpdateEnvironment();

	}

    private void SpawnParticle(WeatherType weatherType)
    {
        switch(weatherType)
        {
            case WeatherType.Foggy:
                fog.gameObject.SetActive(true); 
                break;
            case WeatherType.Snowy:
                snow.gameObject.SetActive(true);
                break;
            default:
                break;
		}

    }

    private void DestroyParticle(WeatherType weatherType)
    {
        switch(weatherType)
        {
            case WeatherType.Foggy:
                fog.gameObject.SetActive(false);
                break;
            case WeatherType.Snowy:
                snow.gameObject.SetActive(false);
                break;
        }
    }

    private void DebugWeather()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            DestroyParticle(WeatherType.Foggy);
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Sunny;
			currentIntensity = sunnyIntensity;
		}
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            DestroyParticle(WeatherType.Foggy);
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Cloudy;
			currentIntensity = cloudyIntensity;
		}
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Foggy;
			currentIntensity = cloudyIntensity;
			SpawnParticle(currentWeatherType);
		}
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            DestroyParticle(WeatherType.Foggy);
			currentWeatherType = WeatherType.Snowy;
            currentIntensity = cloudyIntensity;
			SpawnParticle(currentWeatherType);
		}

        SetSkyBox();
    }
}
