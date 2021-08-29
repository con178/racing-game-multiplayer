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
    [SerializeField] private PhotonManager photonManager;
    [SerializeField] private Text timerText;
    [SerializeField] private PositionsUpdate positionsUpdate;

    private bool needToTurn;
    private PhotonView pV;
    private float timer = 0f;

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
                int viewID = collision.gameObject.GetComponent<PhotonView>().ViewID;
                pV.RPC("UpdateLapsManagerList", RpcTarget.All, int.Parse(playerNumber), viewID);
            }
        }
    }

    [PunRPC]
    void UpdateLapsManagerList(int playerIndex, int player)
    {
        if (lapsManager.checkPointsCompleted[playerIndex] == lapsManager.checkpoints.Length || lapsManager.lapsCompleted[playerIndex] == 0)
        {
            PhotonView.Find(player).gameObject.GetComponent<Player_UI>().UpdateInfoText("");
            PhotonView.Find(player).gameObject.GetComponent<Player_UI>().UpdateLapText(lapsManager.lapsCompleted[playerIndex] + 1, lapsManager.lapsToFinish);

            lapsManager.lapsCompleted[playerIndex]++;
            lapsManager.checkPointsCompleted[playerIndex] = 0;

            positionsUpdate.ChangePositionsList();

            if (lapsManager.lapsCompleted[playerIndex] == lapsManager.lapsToFinish)
            {
                PhotonView.Find(player).gameObject.GetComponent<Player_UI>().UpdateInfoText("");
                PhotonView.Find(player).gameObject.GetComponent<Player_UI>().ClearLapText();
                string text = String.Format("{0} wins!", PhotonNetwork.PlayerList.ElementAt(playerIndex).NickName);
                pV.RPC("FinishRaceInfo", RpcTarget.All, playerIndex, text);
            }
        }
        else
        {
            if (photonManager.isRaceStarted)
                PhotonView.Find(player).gameObject.GetComponent<Player_UI>().UpdateInfoText("Turn Around");
        }
    }

    [PunRPC]
    void FinishRaceInfo(int playerNumber, string _mainText)
    {
        photonManager.RPC_InfoText.text = _mainText;
        photonManager.isRaceStarted = false;
        photonManager.InitGame();
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
