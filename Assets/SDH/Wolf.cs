using UnityEngine;

public class Wolf : Animal
{
    protected override void Awake()
    {
        base.Awake();
        detectionRange = 15f;
        speed = 6f;
    }
}
