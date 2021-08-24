using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Player_UI : MonoBehaviourPun
{
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI photonNickName;

    void Start()
    {
        if (photonView.IsMine)
        {
            SetNickName(PhotonNetwork.NickName);
        }
        else
        {
            SetNickName(photonView.Owner.NickName);
        }
    }

    void Update()
    {
        if(photonNickName != null)
            photonNickName.gameObject.transform.LookAt(Vector3.up);
    }

    public void UpdateLapText(int lap, int maxLap)
    {
        lapText.text = "Laps: " + lap + " / " + maxLap;
    }

    public void UpdateInfoText(string _text)
    {
        infoText.text = _text;
    }

    void SetNickName(string nickName)
    {
        photonNickName.text = nickName;
    }
}
