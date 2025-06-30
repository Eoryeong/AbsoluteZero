using UnityEngine;

public class PlayerStatusManager : SingletonBehaviour<PlayerStatusManager>
{
    private PlayerControll player => PlayerManager.Instance.PlayerController;

	#region 스테이터스 정보
	[Header("스테이터스 최대치")]
	[SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxThirst = 100f;
	[SerializeField] private float maxMentality = 100f;
    [SerializeField] private float maxCold = 100f;

	private float currentHp;
    private float currentHunger;
    private float currentThirst;
	private float currentMentality;
    private float currentCold;

    [Header("스테이터스 감소율")]
	[SerializeField] private float hungerDecreaseRate = 1f;
    [SerializeField] private float thirstDecreaseRate = 1f;
	[SerializeField] private float mentalityDecreaseRate = 1f;
	[SerializeField] private float coldDecreascRate = 1f;

	[Header("스테이터스별 데미지")]
	[SerializeField] private float hungerDamage = 15f;
	[SerializeField] private float thirstDamage = 10f;
	[SerializeField] private float mentalityDamage = 10f;
	[SerializeField] private float coldDamage = 20f;
	#endregion

	#region 쿨타임
	[Header("내부 쿨타임")]
	[SerializeField] private float statusCoolDown = 1.0f;
	[SerializeField] private float hungerDamageCoolDown = 1.0f;	
	[SerializeField] private float thirstDamageCoolDown = 1.0f;
	[SerializeField] private float mentalityDamageCoolDown = 1.0f;
	[SerializeField] private float coldDamageCoolDown = 1.0f;

	private float statusTimer;
	private float hungerDamageTimer;
	private float thirstDamageTimer;
	private float mentalityTimer;
	private float coldrDamageTimer;

	private float runDebugSpeed = 20f;
    private float runDefaultSpeed = 8f;
    private float runCurrSpeed = 8f;
    #endregion

    #region getter
    public float CurrentHp { get { return currentHp; } }
	public float CurrentHpPercent { get { return currentHp / maxHp; } }
	public float CurrentHunger { get { return currentHunger; } }  
	public float CurrentHungerPercent { get { return currentHunger / maxHunger; } }  
    public float CurrentThirst { get { return currentThirst; } }
    public float CurrentThirstPercent { get { return currentThirst / maxThirst; } }
	public float CurrentMentality { get { return currentMentality; } }
	public float CurrentMentalityPercent { get { return currentMentality / maxMentality; } }
    public float CurrentCold { get { return currentCold; } }
    public float CurrentColdPercent { get { return currentCold / maxCold; } }
	#endregion

	[HideInInspector] public bool isDead = false;
	[HideInInspector] public bool isHunger = false;
	[HideInInspector] public bool isThirst = false;
	[HideInInspector] public bool isTired = false;
	[HideInInspector] public bool isCold = false;

	private bool deadTrigger = false;

	private void Start()
	{
        InitStatus();
	}

	private void Update()
	{
        UpdateStatus();

        if (currentHp <= 0 && !deadTrigger)
		{
            deadTrigger = true;
			player.PlayerDying();
		}

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (runCurrSpeed == 8f)
            {
                runCurrSpeed = runDebugSpeed;
            }
            else
            {
                runCurrSpeed = runDefaultSpeed;
            }
        }
    }

	private void InitStatus()
    {
		currentHp = maxHp;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
		currentMentality = maxMentality;
        currentCold = maxCold;
    }

    private void UpdateStatus()
	{
		if (TimeManager.Instance.isPause)
			return;

		statusTimer += Time.deltaTime;

		if(statusTimer > statusCoolDown)
		{
			currentHunger -= hungerDecreaseRate;
			currentThirst -= thirstDecreaseRate;
			currentMentality -= mentalityDecreaseRate;
			currentCold -= coldDecreascRate;

			statusTimer = 0;
		}

		HungerEvent();
		ThirstEvent();
		MentalityEvent();
		ColdEvent();
	}

	private void HungerEvent()
	{
		if(currentHunger <= 0)
		{
            player.runSpeed = 5;
            player.walkSpeed = 3;
            player.sitSpeed = 1;
            isHunger = true;
            hungerDamageTimer += Time.deltaTime;

			if(hungerDamageTimer > hungerDamageCoolDown)
			{
				TakeDamage(hungerDamage);
				hungerDamageTimer = 0;
			}

			currentHunger = 0;
		}
		else
		{
            player.runSpeed = runCurrSpeed;
            player.walkSpeed = 5;
            player.sitSpeed = 2;
            isHunger = false;
        }
	}

	private void ThirstEvent()
	{
		if(currentThirst <= 0)
		{
            isThirst = true;
            thirstDamageTimer += Time.deltaTime;

			if (thirstDamageTimer > thirstDamageCoolDown)
			{
				TakeDamage(thirstDamage);
				thirstDamageTimer = 0;
			}

			currentThirst = 0;
		}
		else
		{
            isThirst = false;
        }
	}

	private void MentalityEvent()
	{
		if (currentMentality <= 0)
		{
            isTired = true;
            mentalityTimer += Time.deltaTime;

			if (mentalityTimer > mentalityDamageCoolDown)
			{
				TakeDamage(mentalityDamage);
				mentalityTimer = 0;
			}

			currentMentality = 0;
		}
		else
		{
            isTired = false;
        }
	}

	private void ColdEvent()
	{
		if(currentCold <= 0)
		{
            isCold = true;
            coldrDamageTimer += Time.deltaTime;

			if (coldrDamageTimer > coldDamageCoolDown)
			{
				TakeDamage(coldDamage);
				coldrDamageTimer = 0;
			}

			currentCold = 0;
		}
		else
		{
            isCold = false;
        }
	}

	public float GetHungerDecreaseRate()
	{
		return hungerDecreaseRate;
	}

	public void SetHungerDecreaseRate(float decreaseRate)
    {
        hungerDecreaseRate = decreaseRate;
    }

	public float GetThirstDecreaseRate()
	{
		return thirstDecreaseRate;
	}

	public void SetThirstDecreaseRate(float decreaseRate)
	{
		thirstDecreaseRate = decreaseRate;
	}

	public float GetMentalityDecreaseRate()
	{
		return mentalityDecreaseRate;
	}

	public void SetMentalittDecreaseRate(float decreaseRate)
	{
		mentalityDecreaseRate = decreaseRate;
	}

	public float GetColdDecreaseRate()
	{
		return coldDecreascRate;
	}

	public void SetColdDecreaseRate(float decreaseRate)
	{
		coldDecreascRate = decreaseRate;
	}

    public void AddCurrentHunger(float addHunger)
    {
        currentHunger += addHunger;
        if (currentHunger > maxHunger)
            currentHunger = maxHunger;

	}

    public void AddCurrentThirst(float addThiirst)
    {
        currentThirst += addThiirst;
        if (currentThirst > maxThirst)
            currentThirst = maxThirst;
    }

	public void AddCurrentMentality(float addMentality)
	{
		currentMentality += addMentality;
		if(currentMentality > maxMentality)
			currentMentality = maxMentality;
	}

    public void AddCurrentCold(float addCold)
    {
        currentCold += addCold;
        if (currentCold > maxCold)
            currentCold = maxCold;
    }

	public void TakeDamage(float damage)
	{
		currentHp -= damage;
		if(currentHp <= 0)
			isDead = transform;
	}

	public void Heal(float amout)
	{
		currentHp += amout;
		if(currentHp > maxHp)
			currentHp = maxHp;
	}
}
