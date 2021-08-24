using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
using Photon.Pun;
public class FinalPoint : MonoBehaviourPun
{
    [SerializeField] private LapsManager lapsManager;
    private bool needToTurn;
    [SerializeField] private PhotonManager photonManager;
    [SerializeField] private Text mainText;
    private PhotonView pV;


    void Start()
    {
        photonManager = (PhotonManager)FindObjectOfType(typeof(PhotonManager));
        pV = GetComponent<PhotonView>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        string playerNumber;
        if(photonManager.isRaceStarted)
        {
            if (collision.CompareTag("Player"))
            {
                playerNumber = collision.gameObject.name;

                if (lapsManager.checkPointsCompleted[int.Parse(playerNumber)] == lapsManager.checkpoints.Length || lapsManager.lapsCompleted[int.Parse(playerNumber)] == 0)
                {
                    collision.gameObject.GetComponent<Player_UI>().UpdateInfoText("");
                    lapsManager.lapsCompleted[int.Parse(playerNumber)]++;
                    lapsManager.checkPointsCompleted[int.Parse(playerNumber)] = 0;

                    collision.gameObject.GetComponent<Player_UI>().UpdateLapText(lapsManager.lapsCompleted[int.Parse(playerNumber)], lapsManager.lapsToFinish);

                    pV.RPC("CheckIfFinish", RpcTarget.All, int.Parse(playerNumber));
                }
                else
                {
                    collision.gameObject.GetComponent<Player_UI>().UpdateInfoText("Turn Around");
                }
            }
        }
    }

    [PunRPC]
    void CheckIfFinish(int playerNumber)
    {
        if (lapsManager.lapsCompleted[playerNumber] == lapsManager.lapsToFinish)
        {
            mainText.text = String.Format("{0} wins!", PhotonNetwork.PlayerList.ElementAt(playerNumber).NickName);
        }
    }

    /*
    IEnumerator SignalText(Player_UI playerUI)
    {

        playerUI.UpdateInfoText("Turn around");
        i++;

        yield return new WaitForSeconds(1);

        if (i < 2)
            playerUI.UpdateInfoText("");
        else
            StopCoroutine(SignalText(null));
    }
    */
}
