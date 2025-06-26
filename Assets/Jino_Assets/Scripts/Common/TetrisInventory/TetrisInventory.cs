using UnityEngine;

public class TetrisInventory : BaseUI
{
    public static TetrisInventory instanceTetris;

    private void Awake()
    {
        if (null == instanceTetris)
        {
            instanceTetris = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public int numberSlots;
}
