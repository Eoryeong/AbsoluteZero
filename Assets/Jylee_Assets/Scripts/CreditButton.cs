using TMPro;
using UnityEngine;

public class CreditButton : MonoBehaviour
{
    [SerializeField] private GameObject creditButton;
    [SerializeField] private GameObject creditText;
    [SerializeField] private GameObject backButton;

    [SerializeField] private GameObject title1;
    [SerializeField] private GameObject title2;
    [SerializeField] private GameObject title3;
    [SerializeField] private GameObject btn1;
    [SerializeField] private GameObject btn2;
    [SerializeField] private GameObject btn3;

    public void CreditBtnClick()
    {
        backButton.SetActive(true);
        creditText.SetActive(true);
        creditButton.SetActive(false);

        title1.SetActive(false);
        title2.SetActive(false);
        title3.SetActive(false);
        btn1.SetActive(false);
        btn2.SetActive(false);
        btn3.SetActive(false);
    }

    public void BackBtnClick()
    {
        backButton.SetActive(false);
        creditText.SetActive(false);
        creditButton.SetActive(true);

        title1.SetActive(true);
        title2.SetActive(true);
        title3.SetActive(true);
        btn1.SetActive(true);
        btn2.SetActive(true);
        btn3.SetActive(true);
    }
}
