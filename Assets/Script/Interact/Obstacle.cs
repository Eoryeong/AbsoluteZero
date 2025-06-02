using UnityEngine;

public class Obstacle : MonoBehaviour
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
            UIManager.instance.ShowProgress(pct);

            if(breakTimer >= breakDuration)
            {
                UIManager.instance.HideProgress();
                isBreaking = false;
                ObstacleBreaking();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isBreaking = false;
            UIManager.instance.HideProgress();
        }
    }

    public void TryObstacleBreak()
    {
        if (!canBreak) return;

        isBreaking = true;
        breakTimer = 0f;
        UIManager.instance.ShowProgress(0f);
    }

    private void ObstacleBreaking()
    {
        Destroy(gameObject);
    }
}
