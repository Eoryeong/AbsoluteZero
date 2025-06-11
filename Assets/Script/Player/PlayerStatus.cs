using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxPlayerHp;
    public float currentPlayerHp;

    public float maxPlayerHunger;
    public float currentPlayerHunger;
    public float playerHungerDecreaseRate = 0.1f;
    public bool isHungerDebuffed;

    public float maxPlayerThirst;
    public float currentPlayerThirst;
    public float playerThirstDecreaseRate = 0.1f;
    public bool isThirstDebuffed;

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

    private void Update()
    {
        DecreaseHunger();
        DecreaseThirst();
    }



    #region HP 관련 
    
    public void TakeDamage(float amount)
    {
        currentPlayerHp -= amount;
        if (currentPlayerHp < 0)
        {
            currentPlayerHp = 0;
            Die();
        }

    }

    public void Heal(float amount)
    {
        currentPlayerHp += amount;
        if (currentPlayerHp > maxPlayerHp) currentPlayerHp = maxPlayerHp;
    }

    public void Die()
    {
        //죽음관련로직
    }

    #endregion


    #region 배고픔 관련
    
    public void DecreaseHunger()
    {
        if (currentPlayerHunger > 0)
        {
            currentPlayerHunger -= playerHungerDecreaseRate * Time.deltaTime;
            if (currentPlayerHunger < 0) currentPlayerHunger = 0;
        }
    }

    public void Eat(float amount)
    {
        currentPlayerHunger += amount;
        if (currentPlayerHunger > maxPlayerHunger) currentPlayerHunger = maxPlayerHunger;
    }

    public void HungerDebuff()
    {
        if (isHungerDebuffed) return;
        isHungerDebuffed = true;
        StartCoroutine(HungerDebuffStart());
    }

    public void CureHungerDebuff()
    {
        if (!isHungerDebuffed) return;
        isHungerDebuffed = false;
        StopCoroutine(HungerDebuffStart());
        playerHungerDecreaseRate /= 1.5f;
    }

    IEnumerator HungerDebuffStart()
    {
        playerHungerDecreaseRate *= 1.5f;
        yield return new WaitForSeconds(10f);
        playerHungerDecreaseRate /= 1.5f;
        isHungerDebuffed = false;
    }

    #endregion


    #region 갈증 관련

    public void DecreaseThirst()
    {
        if (currentPlayerThirst > 0)
        {
            currentPlayerThirst -= playerThirstDecreaseRate * Time.deltaTime;
            if (currentPlayerThirst < 0) currentPlayerThirst = 0;
        }
    }
    public void Drink(float amount)
    {
        currentPlayerThirst += amount;
        if (currentPlayerThirst > maxPlayerThirst) currentPlayerThirst = maxPlayerThirst;
    }


    public void ThirstDebuff()
    {
        if (isThirstDebuffed) return;
        isThirstDebuffed = true;
        StartCoroutine(ThirstDebuffStart());
    }

    public void CureThirstDebuff()
    {
        if (!isThirstDebuffed) return;
        isThirstDebuffed = false;
        StopCoroutine(ThirstDebuffStart());
        playerThirstDecreaseRate /= 1.5f;
    }

    IEnumerator ThirstDebuffStart()
    {
        playerThirstDecreaseRate *= 1.5f;
        yield return new WaitForSeconds(10f);
        playerThirstDecreaseRate /= 1.5f;
        isThirstDebuffed = false;
    }

    #endregion
}
