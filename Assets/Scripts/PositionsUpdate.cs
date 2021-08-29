using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PositionsUpdate : MonoBehaviourPun
{
    [SerializeField] private LapsManager lapsManager;
    private TextMeshProUGUI positionsText;
    private PhotonView pV;

    void Start()
    {
        positionsText = GetComponent<TextMeshProUGUI>();
        pV = GetComponent<PhotonView>();
    }

    public void ChangePositionsList()
    {
        pV.RPC("ChangePositionsList_RPC", RpcTarget.All);
        Debug.Log("ChangePositionsList");
    }

    [PunRPC]
    void ChangePositionsList_RPC()
    {
        Debug.Log("ChangePositionsList_RPC");
        positionsText.text = "";
        int[] positionsArray = GetPositions();
        
        for(int i = 0; i < positionsArray.Length; i++)
        {
            positionsText.text += positionsArray[i].ToString() + ". " + PhotonNetwork.PlayerList[i].NickName;
        }
    }


    int[] GetPositions()
    {
        Stack<int> positionsStack = new Stack<int>();
        int max = 0;

        foreach (int i in lapsManager.lapsCompleted)
        {
            if (max > lapsManager.lapsCompleted[i])
            {
                max = lapsManager.lapsCompleted[i];
                positionsStack.Push(i);
            }
        }

        return positionsStack.ToArray();
    }
}
