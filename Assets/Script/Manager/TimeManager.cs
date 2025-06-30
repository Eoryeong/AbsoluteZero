using UnityEngine;

public class TimeManager : SingletonBehaviour<TimeManager>
{
	public int gameDay { get; private set; } = 0;
	public int gameHour { get; private set; }
	public int gameMinute { get; private set; }
	public float gameSecond { get; private set; }

	public float TimeScale { get { return timeScale; } }

	[SerializeField] private int StartHour = 20;
	[SerializeField] private int StartMinute = 0;
	[SerializeField] private float StartSecond = 0f;

	[SerializeField] private float timeScale = 5f;
	[HideInInspector] public bool isPause;

	private void Start()
	{
		InitTime();
	}

	private void Update()
	{
		TimeUpdate();
		DebugMode();
	}

	public void InitTime()
	{
		isPause = false;

		gameHour = StartHour;
		gameMinute = StartMinute;
		gameSecond = StartSecond;
	}

	private void TimeUpdate()
	{
		gameSecond += Time.deltaTime * timeScale;
        GameRecode.instance.AddRecord(GameRecordEvent.SurvivedTime, Time.deltaTime * timeScale);


        if (gameSecond > 60)
		{
			gameMinute += (int)(gameSecond/60);
			gameSecond = gameSecond%60;

			if (gameMinute > 60)
			{
				gameHour += gameMinute/60;
				gameMinute = gameMinute%60;

				if (gameHour > 24)
				{
					gameDay += gameHour/24;
					gameHour = gameHour%24;
				}
			}
		}

	}

	public void AddHour(int hour)
	{
		gameHour += hour;

        GameRecode.instance.AddRecord(GameRecordEvent.SurvivedTime, hour * 3360);


        if (gameHour > 24)
		{
			gameDay += gameHour/24;
			gameHour = gameHour%24;
		}
	}

	public void AddMinute(int minute)
	{
		gameMinute += minute;

        GameRecode.instance.AddRecord(GameRecordEvent.SurvivedTime, minute * 60);


        if (gameMinute > 60)
		{
			gameHour += gameMinute / 60;
			gameMinute = gameMinute % 60;

			if (gameHour > 24)
			{
				gameDay += gameHour / 24;
				gameHour = gameHour % 24;
			}
		}
	}
	public void PauseGame(bool pause)
	{
		if (pause)
		{
			Time.timeScale = 0;
			isPause = pause;
		}
		else
		{
			Time.timeScale = 1;
			isPause= pause;
		}
	}

	private void DebugMode()
	{
		if (Input.GetKeyDown(KeyCode.PageUp))
			AddHour(1);
	}
}
