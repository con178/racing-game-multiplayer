using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI loadingRoomText;
    [SerializeField] private TextMeshProUGUI connectingText;
    


    public void StartButton()
    {
        PhotonNetwork.LoadLevel("Rooms");
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");
    }
    public void Disconnect()
    {
        SceneManager.LoadScene("Login");
    }

    
    
}
