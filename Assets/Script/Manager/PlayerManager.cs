using UnityEngine;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    [SerializeField] private GameObject player;
    private PlayerControll playerController;
    private PlayerStatus playerStatus;

    public GameObject Player { get { return player; } }
    public PlayerControll PlayerController { get  { return playerController; } }
    public PlayerStatus PlayerState { get { return playerStatus; } }

    public bool playerFreeze { get; private set; } = false;

	private void Start()
	{
        InitPlayer();
	}

    private void InitPlayer()
    {
        playerController = player.GetComponent<PlayerControll>();
        playerStatus = player.GetComponent<PlayerStatus>();
    }

    public void SetPlayerFreeze(bool freeze)
    {
        playerFreeze = freeze;
    }
}
