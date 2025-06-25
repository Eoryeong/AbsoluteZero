using UnityEngine;

public class ObjectBed : MonoBehaviour
{
    public void TryUseBed()
    {
        UIManager.Instance.BedMenuOpen(this);
    }

    public void Sleep(int sleepTime)
    {
        UIManager.Instance.FadeIn();       
        UIManager.Instance.FadeOut();
    }
}
