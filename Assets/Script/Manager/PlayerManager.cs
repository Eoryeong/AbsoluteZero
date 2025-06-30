using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    private GameObject player;
    private PlayerControll playerController;

    public PlayerControll PlayerController { get  { return playerController; } }

    public Transform startPos;
    public Transform housePos;

    public string currScene = "";
    public string prevScene = "";

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

    public void SceneManageUpdate()
    {
        prevScene = currScene;
        currScene = SceneManager.GetActiveScene().name;
        InitPlayer();
    }

    public void PlayerSetPos()
    {
        Debug.Log(currScene);
        if (currScene.Contains("In_Game_Scene"))
        {
            if (prevScene.Contains("SungZun_Scene02"))
            {
                playerController.gameObject.transform.position = housePos.position;
            }
            else
            {
                playerController.gameObject.transform.position = startPos.position;
            }
        }
    }
}
