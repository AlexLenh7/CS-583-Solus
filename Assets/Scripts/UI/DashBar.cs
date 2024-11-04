using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    public Slider slider;

    public void SetFill(float fillAmount)
    {
        if (slider != null)
        {
            slider.value = fillAmount;
        }
    }

    public void Hide()
    {
        // Disable the entire dash bar UI
        gameObject.SetActive(false);
    }
}
