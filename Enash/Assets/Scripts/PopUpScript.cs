using UnityEngine;
using System.Collections;

public class PopUpScript : MonoBehaviour
{
    public void ShowPopUp()
    {
        gameObject.SetActive(true);
    }

    public void HidePopUp()
    {
        gameObject.SetActive(false);
    }
}
