using UnityEngine;

public class TetrisInventory : BaseUI
{
    public static TetrisInventory instanceTetris;

    private void Awake()
    {
        if (null == instanceTetris)
        {
            instanceTetris = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public int numberSlots;
}
