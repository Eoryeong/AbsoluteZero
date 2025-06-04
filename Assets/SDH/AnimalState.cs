using UnityEngine;

public class AnimalState
{
    public Animal anim;
    public AnimalState(Animal animal)   
    {
        anim = animal;
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
