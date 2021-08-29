using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using Photon.Pun;
public class CheckPoint : MonoBehaviourPun
{
    [SerializeField] private LapsManager lapsManager;
    [SerializeField] private int pointNumber;
    [SerializeField] private PhotonManager photonManager;
    [SerializeField] private PositionsUpdate positionsUpdate;
    private PhotonView pV;

    void Start()
    {
        photonManager = (PhotonManager)FindObjectOfType(typeof(PhotonManager));
        pV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string playerNumber;
        if (photonManager.isRaceStarted)
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
    private void UpdateLapsManagerList(int playerIndex, int player)
    {
        if (lapsManager.checkPointsCompleted[playerIndex] == pointNumber - 1)
        {
            PhotonView.Find(player).gameObject.GetComponent<Player_UI>().UpdateInfoText("");
            lapsManager.checkPointsCompleted[playerIndex]++;
            positionsUpdate.ChangePositionsList();
        }
        else
        {
            if(photonManager.isRaceStarted)
                PhotonView.Find(player).gameObject.GetComponent<Player_UI>().UpdateInfoText("Turn Around");
        }
    }
    /*
    IEnumerator SignalText(GameObject go)
    {
        TextMeshProUGUI text = go.transform.Find("Canvas/InfoText").GetComponent<TextMeshProUGUI>();

        text.text = "Turn around";
        i++;

        yield return new WaitForSeconds(1);

        if (i < 2)
            text.text = "";
        else
            StopCoroutine(SignalText(null));
    }
    */
}
