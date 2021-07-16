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
    //private int i = 0;
    [SerializeField] private PhotonManager photonManager;

    void Start()
    {
        photonManager = (PhotonManager)FindObjectOfType(typeof(PhotonManager));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string playerNumber;
        if (photonManager.isRaceStarted)
        {
            if (collision.CompareTag("Player"))
            {

                playerNumber = collision.gameObject.name;

                
                if (lapsManager.checkPointsCompleted[int.Parse(playerNumber)] == pointNumber - 1)
                {
                    collision.gameObject.GetComponent<Player_UI>().UpdateInfoText("");
                    lapsManager.checkPointsCompleted[int.Parse(playerNumber)]++;
                }
                else
                {
                    collision.gameObject.GetComponent<Player_UI>().UpdateInfoText("Turn Around");
                }
            }
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
