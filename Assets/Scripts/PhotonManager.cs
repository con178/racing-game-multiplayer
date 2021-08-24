using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
using System;

public class PhotonManager : MonoBehaviourPun
{
    [SerializeField] private GameObject[] SpawnPoints;
    public bool isGameStarted = false;
    public bool isRaceStarted = false;


    private ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] private GameObject countdownCanvas;


    void Start()
    {
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

            if(playerCount == 0)
            {
                CountdownTimer.SetStartTime();
                CountdownTimer.OnCountdownTimerHasExpired += CountDownToGameStart;
            }

            GameObject Player = PhotonNetwork.Instantiate((playerCount).ToString(), SpawnPoints[playerCount].transform.position, new Quaternion(0f, 0f, 0f, 0f));
            //Player.GetComponentInChildren<Player_UI>().SetNickName(PhotonNetwork.NickName);
            Debug.Log("Player: " + Player.name);
        }
    }


    void CountDownToGameStart()
    {

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
