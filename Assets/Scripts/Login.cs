using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private TextMeshProUGUI connectingText;
    [SerializeField] private GameObject[] gameObjectsToHide;


    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            connectingText.text = "Connecting to server...";
            PhotonNetwork.ConnectUsingSettings();
            foreach (GameObject go in gameObjectsToHide)
            {
                go.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject go in gameObjectsToHide)
            {
                go.SetActive(true);
            }
        }
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        connectingText.text = "";
        Debug.Log("Connected to " + PhotonNetwork.CloudRegion);

        foreach (GameObject go in gameObjectsToHide)
        {
            go.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            CallLogin();
    }



    public void CallLogin()
    {
        if (username.text.Length > 2 && username.text.Length <= 12)
        {
            PhotonNetwork.NickName = username.text;
            SceneManager.LoadScene("Menu");
        }
        else if(username.text.Length <= 2)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "Type username with more than 2 letters";
        }
        else if(username.text.Length > 12)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "Type username with no more than 12 letters";
        }
    }
}
