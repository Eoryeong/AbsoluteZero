using UnityEngine;

public enum WeatherType
{
	Sunny,
	Cloudy,
	Foggy,
	Snowy
}

public class WeatherController : MonoBehaviour
{
    [SerializeField] private WeatherType currentWeatherType;

    [SerializeField] private int weatherDuration;
    private int weatherStartHour;
    private int weatherStartMinute;
    private int weatherCurrentHour;
    private int weatherCurrentMinute;

    [SerializeField] private Light lightCompo;
    private PlayerControll player;

    private float currentIntensity;

    [SerializeField] private GameObject mistPrefab;
    [SerializeField] private GameObject snowPrefab;
    private GameObject mist;
    private GameObject snow;

    [SerializeField] private Material sunnyAfternoonSkyBox;
    [SerializeField] private Material sunnyNightSkyBox;
    [SerializeField] private Material CloduyAfternoonSkyBox;
    [SerializeField] private Material CloduyNightSkyBox;

    [SerializeField] private float sunnyIntensity;
    [SerializeField] private float cloudyIntensity;

    void Start()
	{
		Init();
		SetWeather();
	}

	void Update()
    {
        UpdateWeather();
        DebugWeather();
    }

	private void Init()
	{
		player = PlayerManager.Instance.player;

		mist = Instantiate(mistPrefab, player.transform.position, Quaternion.identity, player.transform);
		snow = Instantiate(snowPrefab, player.transform.position, Quaternion.identity, player.transform);

		mist.gameObject.SetActive(false);
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

        lightCompo.intensity = Mathf.Lerp(lightCompo.intensity, currentIntensity, Time.deltaTime * 0.5f);
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
                mist.gameObject.SetActive(true); 
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
                mist.gameObject.SetActive(false);
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
			currentIntensity = 2;
		}
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            DestroyParticle(WeatherType.Foggy);
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Cloudy;
			currentIntensity = 0.5f;
		}
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            DestroyParticle(WeatherType.Snowy);
			currentWeatherType = WeatherType.Foggy;
			currentIntensity = 0.5f;
			SpawnParticle(currentWeatherType);
		}
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            DestroyParticle(WeatherType.Foggy);
			currentWeatherType = WeatherType.Snowy;
			currentIntensity = 0.5f;
			SpawnParticle(currentWeatherType);
		}

        SetSkyBox();
    }
}
