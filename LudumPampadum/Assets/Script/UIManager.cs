﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Script Parameters

    [Header("Parameter")]

    [Header("Timer")]

    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private Image imageTimer;

    [Header("Entity")]
    [SerializeField] private TextMeshProUGUI textEntityNumber;

    [Header("Buttons")]
    public Button reloadButton;
    public Button startButton;

    #endregion

    public void DrawTimer(float time, float maxTime)
    {
        string timerToDraw = "";
        //float t = Mathf.RoundToInt(time * 10);
        float t = Mathf.RoundToInt(time);
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