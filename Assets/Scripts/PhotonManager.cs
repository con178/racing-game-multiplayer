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

    [SerializeField] private List<PhotonView> photonViews;

    private ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] private GameObject countdownCanvas;


    void Start()
    {
        Debug.Log("Player list length: " + PhotonNetwork.PlayerList.Length);
        
        

        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    [Obsolete]
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
            photonViews.Add(Player.transform.FindChild((playerCount).ToString()).GetComponent<PhotonView>());
            
        }
    }


    void CountDownToGameStart()
    {
        Debug.Log("CountDownToGameStart");

        CountdownTimer.OnCountdownTimerHasExpired -= CountDownToGameStart;

        StartGame();

        CountdownTimer.OnCountdownTimerHasExpired += StartRace;
        CountdownTimer.SetStartTime();

        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        
    }




    void StartGame()
    {
        Debug.Log("Start game");
        isGameStarted = true;
        foreach (var photonView in photonViews)
        {
            if(photonView.CompareTag("Player"))
            {
                var player = photonView.gameObject;

                CarMovement car = player.GetComponent<CarMovement>();

                car.MoveToStartPos(SpawnPoints[(int.Parse(player.name))].transform.position, Vector3.zero, true, false);
            }
        }
    }


    void StartRace()
    {
        Debug.Log("Start Race");
        isRaceStarted = true;
        foreach (var photonView in photonViews)
        {
            if (photonView.CompareTag("Player"))
            {
                var player = photonView.gameObject;

                CarMovement car = player.GetComponent<CarMovement>();

                car.rb.freezeRotation = false;
                car.canMove = true;
            }
        }
    }
}
