using UnityEngine;
using UnityEngine.UI;

public class SleepBtn : MonoBehaviour 
{
    [SerializeField] private int maxSleepTime = 12;
    [SerializeField] private int minSleepTime = 1;
    private Text currentTex;

	private void Start()
	{
		currentTex = GetComponent<Text>();
	}

	public void UpBtn()
    {
		int currentTime = int.Parse(currentTex.text);

		currentTime++;

		if (currentTime > maxSleepTime)
			currentTime = maxSleepTime;

		currentTex.text = currentTime.ToString();
    }

	public void DownBtn()
	{
		int currentTime = int.Parse(currentTex.text);

		currentTime--;

		if (currentTime < minSleepTime)
			currentTime = minSleepTime;

		currentTex.text = currentTime.ToString();
	}
}
