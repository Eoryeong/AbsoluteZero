using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public Animal animal;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            animal.TakeDamage(50f);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadingManager.LoadScene("LoadSceneTest");
        }
    }
}
