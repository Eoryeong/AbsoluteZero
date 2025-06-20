using UnityEngine;

public class ObjectWood : MonoBehaviour
{
    public bool canBreak;
    private bool isBreaking;

    [SerializeField] private float breakDuration;
    private float breakTimer;

    private void Update()
    {
        if (!isBreaking) return;

        if (Input.GetMouseButton(0))
        {
            breakTimer += Time.deltaTime;
            float pct = breakTimer / breakDuration;
            UIManager.Instance.ShowProgress(pct);

            if (breakTimer >= breakDuration)
            {
                UIManager.Instance.HideProgress();
                isBreaking = false;
                ObstacleBreaking();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isBreaking = false;
            UIManager.Instance.HideProgress();
        }
    }

    public void TryObstacleBreak()
    {
        if (!canBreak) return;

        isBreaking = true;
        breakTimer = 0f;
        UIManager.Instance.ShowProgress(0f);
    }

    private void ObstacleBreaking()
    {
        Destroy(gameObject);
    }
}
