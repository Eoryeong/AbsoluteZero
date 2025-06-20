using UnityEngine;

public class ObjectWood : MonoBehaviour
{
    public bool canBreak;
    private bool isBreaking;

    [SerializeField] private float breakDuration;
    [SerializeField] private GameObject fireWoodPrefab;
    [SerializeField] private int fireWoodGenQty;
    [SerializeField] private float fireWoodGenPosX;
    [SerializeField] private float fireWoodGenPosZ;

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
                TreeBreaking();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isBreaking = false;
            UIManager.Instance.HideProgress();
        }
    }

    public void TryChopTree()
    {
        if (!canBreak) return;

        isBreaking = true;
        breakTimer = 0f;
        UIManager.Instance.ShowProgress(0f);
    }

    private void TreeBreaking()
    {
        Vector3 origin = transform.position;

        for (int i = 0; i < fireWoodGenQty; i++)
        {
            float offsetX = Random.Range(-fireWoodGenPosX, fireWoodGenPosX);
            float offsetZ = Random.Range(-fireWoodGenPosZ, fireWoodGenPosZ);
            Vector3 spawnPos = new Vector3(origin.x + offsetX, origin.y, origin.z + offsetZ);

            Instantiate(fireWoodPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
