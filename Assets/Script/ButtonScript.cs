using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void BackBtnOnClick()
    {
        UIManager.Instance.CloseMenu();
    }
}
