using UnityEngine;

public class AnimalAttCol : MonoBehaviour
{
    Collider attcol;
    Animal animal;
    private void Awake()
    {
        attcol = GetComponent<Collider>();
        animal = GetComponentInParent<Animal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().TakeDamage(animal.attackDamage);

            Debug.Log($"Player took {animal.attackDamage} damage from {animal.name}.");
        }
    }
}
