using UnityEngine;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    private GameObject player;
    private PlayerControll playerController;

    public PlayerControll PlayerController { get  { return playerController; } }

    public bool playerFreeze { get; private set; } = false;

	private void Start()
	{
        InitPlayer();
	}

	private void OnEnable()
	{
        InitPlayer();
	}

	private void InitPlayer()
    {
        player = GameObject.FindWithTag("Player");

        Debug.Log(player.name);

        playerController = player.GetComponent<PlayerControll>();
    }

    public void SetPlayerFreeze(bool freeze)
    {
        Debug.Log(freeze);
        playerFreeze = freeze;
    }
}
