using UnityEngine;

public class Deer : Animal
{
   protected override void Awake()
    {
        base.Awake();
        detectionRange = 10f; 
        speed = 3f; 
    }
}
