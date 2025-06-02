using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void BackBtnOnClick()
    {
        UIManager.instance.CloseMenu();
    }
}
