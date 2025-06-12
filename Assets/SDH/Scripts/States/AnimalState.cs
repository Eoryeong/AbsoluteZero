using UnityEngine;

public class AnimalState
{
    public Animal animal;
    public AnimalState(Animal animal)   
    {
        this.animal = animal;
    }
    public virtual void EnterState()
    {
    }
    public virtual void UpdateState()
    {
    }
    public virtual void ExitState()
    {
    }
}
