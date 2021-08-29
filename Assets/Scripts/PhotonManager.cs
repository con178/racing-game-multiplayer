using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
using System;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] SpawnPoints;
    [SerializeField] private LapsManager lapsManager;
    public Text RPC_InfoText;
    public bool isGameStarted = false;
    public bool isRaceStarted = false;
    private PhotonView pV;

    //private ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] private GameObject countdownCanvas;

    void Start()
    {
        pV = GetComponent<PhotonView>();

        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    
    void SpawnPlayer()
    {
        if(!isGameStarted)
        {
            int playerCount = PhotonNetwork.PlayerList.Length - 1;
            Debug.Log("PlayerCount: " + playerCount);


            if(playerCount > 0)
            {
                pV.RPC("RPCInfoText", RpcTarget.MasterClient, "");
                InitGame();
            }
            if(playerCount == 0)
            {
                pV.RPC("RPCInfoText", RpcTarget.MasterClient, "Waiting for more players...");
            }

            GameObject Player = PhotonNetwork.Instantiate((playerCount).ToString(), SpawnPoints[playerCount].transform.position, new Quaternion(0f, 0f, 0f, 0f));
            Debug.Log("Player: " + Player.name);
        }
    }

    [PunRPC]
    void RPCInfoText(string text)
    {
        RPC_InfoText.text = text;
    }


    public void InitGame()
    {
        
        CountdownTimer.OnCountdownTimerHasExpired -= StartRace;
        CountdownTimer.SetStartTime();
        CountdownTimer.OnCountdownTimerHasExpired += CountDownToGameStart;
    }

    void CountDownToGameStart()
    {
        RPC_InfoText.text = "";
        lapsManager.ClearList(lapsManager.lapsCompleted);
        lapsManager.ClearList(lapsManager.checkPointsCompleted);

        CountdownTimer.OnCountdownTimerHasExpired -= CountDownToGameStart;

        StartGame();
        
        CountdownTimer.OnCountdownTimerHasExpired += StartRace;

        CountdownTimer.SetStartTime();

        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }




    void StartGame()
    {
        isGameStarted = true;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            CarMovement car = player.GetComponent<CarMovement>();
            car.photonView.RPC("MoveToStartPos_RPC", RpcTarget.All, SpawnPoints[(int.Parse(player.name))].transform.position, Vector3.zero, true, false, 0f);
        }
    }


    void StartRace()
    {
        isRaceStarted = true;

        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            CarMovement car = player.GetComponent<CarMovement>();
            car.photonView.RPC("PlayerStartRace_RPC", RpcTarget.All, false, true);
        }
    }

}
