using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxPlayerHp;
    public float currentPlayerHp;

    public float maxPlayerHunger;
    public float currentPlayerHunger;

    public float maxPlayerSanity;
    public float currentPlayerSanity;

    public bool playerFreeze;

    private void Start()
    {
        currentPlayerHp = maxPlayerHp;
        currentPlayerHunger = maxPlayerHunger;
        currentPlayerSanity = maxPlayerSanity;
    }

    public void SetPlayerFreeze(bool freeze)
    {
        playerFreeze = freeze;
    }
}
