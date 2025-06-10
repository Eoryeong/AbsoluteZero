using UnityEngine;

public class SunControiller : MonoBehaviour
{
	[Header("Sun Settings")]
	[SerializeField] private Light sunLight; 
	private float sunBaseAngle = -90f; // 자정 기준 각도 (동쪽 지평선 아래)

	void Update()
    {
        SunRotate();
	}

    private void SunRotate()
    {
		float totalMinute = TimeManager.Instance.gameHour * 60 + TimeManager.Instance.gameMinute;

		float dayProgress = totalMinute / 1440f; // 0~1

		// 0~360도 범위에서 회전, 기준 각도에서 시작
		float sunAngle = sunBaseAngle + (dayProgress * 360f);

		// 태양을 X축을 중심으로 회전시킴 (동쪽에서 떠서 남쪽을 지나 서쪽으로 지는 구조)
		sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
	}
}
