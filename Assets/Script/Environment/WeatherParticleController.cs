using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class WeatherParticleController : MonoBehaviour
{
    private PlayerControll player;

    [SerializeField] private GameObject weatherParticle;
	private GameObject[] particles = new GameObject[3];

	private int index = 0;

	private void Start()
	{
        player = PlayerManager.Instance.PlayerController;		
		InitWeatherParticle();		
	}

	private void OnEnable()
	{
		InitWeatherParticle();		
	}

	private void OnDisable()
	{
		for (int i = 0; i < particles.Length; i++)
		{
			if(particles[i].gameObject.GetComponent<VisualEffect>() != null)
				particles[i].gameObject.GetComponent<VisualEffect>().Stop();
			else if(particles[i].gameObject.GetComponent<ParticleSystem>() != null)
				particles[i].gameObject.GetComponent<ParticleSystem>().Stop();
		}
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			SetWeatherParticle(index);
		}
	}


	private void InitWeatherParticle()
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 9, player.transform.position.z);

		for (int i = 0; i < particles.Length; i++)
		{
			if (particles[i] == null)
				particles[i] = Instantiate(weatherParticle, transform.position, Quaternion.identity);
			else
			{
				if (particles[i].gameObject.GetComponent<VisualEffect>() != null)
					particles[i].gameObject.GetComponent<VisualEffect>().Play();
				else if (particles[i].gameObject.GetComponent<ParticleSystem>() != null)
					particles[i].gameObject.GetComponent<ParticleSystem>().Play();
			}

			particles[i].transform.position = transform.position;
		}
	}

	private void SetWeatherParticle(int index)
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 9, player.transform.position.z);

		particles[index].transform.position = transform.position;

		this.index++;
		if (this.index == 3) 
			this.index = 0;
	}
}
