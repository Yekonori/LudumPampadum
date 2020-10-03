using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Parameter")]

    [Header("Timer")]
    [SerializeField]
    TextMeshProUGUI textTimer;
    [SerializeField]
    Image imageTimer;

    [Header("Entity")]
    [SerializeField]
    TextMeshProUGUI textEntityNumber;

    public void DrawTimer(float time, float maxTime)
    {
        string timerToDraw = "";
        float t = Mathf.RoundToInt(time * 10);
        if (t < 10f) 
            timerToDraw += "0" + t;
        else
            timerToDraw += t.ToString();

        textTimer.text = timerToDraw;
        imageTimer.fillAmount = time / maxTime;
    }


    public void DrawEntity(int entityNumber)
    {
        textEntityNumber.text = entityNumber.ToString();
    }
}
