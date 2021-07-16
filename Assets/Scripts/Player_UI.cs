using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_UI : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI lapText;

    public void UpdateLapText(int lap, int maxLap)
    {
        lapText.text = "Laps: " + lap + " / " + maxLap;
    }

    public void UpdateInfoText(string _text)
    {
        infoText.text = _text;
    }
}
