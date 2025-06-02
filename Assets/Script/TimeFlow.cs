using UnityEngine;

public class TimeFlow : MonoBehaviour
{
    public Light directionalLight;
    public Material daySkybox;
    public Material nightSkybox;

    // �� ������
    private readonly Vector3 dayRotation = new Vector3(50f, 30f, 0f);
    private const float dayIntensity = 1.0f;
    private readonly Color dayAmbient = new Color(0.5f, 0.5f, 0.5f);

    // �� ������
    private readonly Vector3 nightRotation = new Vector3(-50f, 30f, 0f);
    private const float nightIntensity = 0.05f;
    private readonly Color nightAmbient = Color.black;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetDay();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetNight();
        }
    }

    private void SetDay()
    {
        directionalLight.transform.rotation = Quaternion.Euler(dayRotation);
        directionalLight.intensity = dayIntensity;
        RenderSettings.ambientLight = dayAmbient;
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment(); // ȯ�汤 ��� �ݿ�
    }

    private void SetNight()
    {
        directionalLight.transform.rotation = Quaternion.Euler(nightRotation);
        directionalLight.intensity = nightIntensity;
        RenderSettings.ambientLight = nightAmbient;
        RenderSettings.skybox = nightSkybox;
        DynamicGI.UpdateEnvironment(); // ȯ�汤 ��� �ݿ�
    }
}
