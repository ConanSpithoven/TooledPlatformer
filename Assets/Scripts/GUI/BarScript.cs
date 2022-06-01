using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image mask;

    [SerializeField]
    private Text HealthText;

    public float Health
    {
        set
        {
            string[] tmp = HealthText.text.Split(':');
            HealthText.text = tmp[0] + ": " + value + "/" + 100;
            fillAmount = Map(value, 0, 100, 0, 1);
        }
    }

    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if (fillAmount != mask.fillAmount)
        {
            mask.fillAmount = Mathf.Lerp(mask.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);

        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}