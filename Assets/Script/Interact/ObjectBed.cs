using System.Collections;
using UnityEngine;

public class ObjectBed : MonoBehaviour
{
    private float fadeTime = 2f;

    public void TryUseBed()
    {
        UIManager.Instance.BedMenuOpen(this);
    }

    public IEnumerator Sleep(int sleepTime)
    {
        UIManager.Instance.FadeOut();
            
        float addHunger = PlayerStatusManager.Instance.GetHungerDecreaseRate() * (sleepTime * (3600 / TimeManager.Instance.TimeScale));
        float addThirst = PlayerStatusManager.Instance.GetThirstDecreaseRate() * (sleepTime * (3600 / TimeManager.Instance.TimeScale));
        float addMentality = PlayerStatusManager.Instance.GetMentalityDecreaseRate() * (sleepTime * (3600 / TimeManager.Instance.TimeScale));
        float addcoold = PlayerStatusManager.Instance.GetColdDecreaseRate() * (sleepTime * (3600 / TimeManager.Instance.TimeScale));
        PlayerStatusManager.Instance.AddCurrentHunger(-addHunger);
        PlayerStatusManager.Instance.AddCurrentThirst(-addThirst);
        PlayerStatusManager.Instance.AddCurrentMentality(addMentality);
        PlayerStatusManager.Instance.AddCurrentCold(-addcoold);

        yield return new WaitForSeconds(fadeTime);

        TimeManager.Instance.AddHour(sleepTime);

        UIManager.Instance.FadeIn();       
    }
}
