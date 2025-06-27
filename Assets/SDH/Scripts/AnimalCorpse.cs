using UnityEngine;

public class AnimalCorpse : MonoBehaviour
{
    public bool canBreak;
    private bool isBreaking;

    [SerializeField] private float breakDuration;
    [SerializeField] private GameObject DropItem;

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
                CompleteLooting();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isBreaking = false;
            UIManager.Instance.HideProgress();
        }
    }

    public void TryLootCorpse()
    {
        if (!canBreak) return;

        isBreaking = true;
        breakTimer = 0f;
        UIManager.Instance.ShowProgress(0f);
    }

    private void CompleteLooting()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        Instantiate(DropItem, origin, Quaternion.identity);
        Debug.Log("Looting completed, item dropped!");

        Destroy(gameObject);
    }
}
