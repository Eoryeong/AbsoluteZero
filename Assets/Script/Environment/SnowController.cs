using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SnowController : MonoBehaviour
{
    private PlayerControll player;

    [SerializeField] private GameObject snowEffect;
	private GameObject efffct1;
	private GameObject efffct2;
	private GameObject efffct3;

	private int snowIndex = 0;

	private float delay = 5f;


	private void Start()
	{
        player = PlayerManager.Instance.player;		
		InitSnowEffect();		
	}

	private void OnEnable()
	{
		InitSnowEffect();		
	}

	private void OnDisable()
	{
		efffct1.gameObject?.GetComponent<VisualEffect>()?.Stop();
		efffct2.gameObject?.GetComponent<VisualEffect>()?.Stop();
		efffct3.gameObject?.GetComponent<VisualEffect>()?.Stop();
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			SetSnowEffect(snowIndex);
		}
	}


	private void InitSnowEffect()
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 9, player.transform.position.z);

		if (efffct1 == null)
			efffct1 = Instantiate(snowEffect, transform.position, Quaternion.identity);
		else
			efffct1.gameObject?.GetComponent<VisualEffect>()?.Play();

		if (efffct2 == null)
			efffct2 = Instantiate(snowEffect, transform.position, Quaternion.identity);
		else
			efffct2.gameObject?.GetComponent<VisualEffect>()?.Play();

		if (efffct3 == null)
			efffct3 = Instantiate(snowEffect, transform.position, Quaternion.identity);
		else
			efffct3.gameObject?.GetComponent<VisualEffect>()?.Play();

		efffct1.transform.position = transform.position;
		efffct2.transform.position = transform.position;
		efffct3.transform.position = transform.position;
	}

	private void SetSnowEffect(int index)
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 9, player.transform.position.z);

		switch (snowIndex)
		{ 
			case 0:
				efffct2.transform.position = transform.position;
				break;
			case 1:
				efffct3.transform.position = transform.position;
				break;
			case 2:
				efffct1.transform.position = transform.position;
				break;
		}

		snowIndex++;
		if (snowIndex == 3) 
			snowIndex = 0;
	}
}
