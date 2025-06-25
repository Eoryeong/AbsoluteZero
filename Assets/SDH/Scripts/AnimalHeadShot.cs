using UnityEngine;

public class AnimalHeadShot : MonoBehaviour
{
    Animal animal;
    private void Start()
    {
        animal = GetComponentInParent<Animal>();
    }

    public void HeadShot(float dmg)
    {
        animal.TakeDamage(dmg*2f);
    }
}
