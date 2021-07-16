using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] GOToLoadAfterConnect;
    [SerializeField] private TextMeshProUGUI connectingText;
    [SerializeField] private GameObject contentOfScrollView;

    [SerializeField] private GameObject buttonRoom;
    private List<RoomInfo> roomList;

    void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    private void ClearRoomList()
    { 
        foreach(Transform a in contentOfScrollView.transform)
        {
            Destroy(a.gameObject);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> pun_list)
    {
        roomList = pun_list;
        ClearRoomList();

        foreach(RoomInfo room in roomList)
        {
            GameObject newRoomButton = Instantiate(buttonRoom, contentOfScrollView.transform);

            newRoomButton.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = room.Name;
            newRoomButton.transform.Find("NumberOfPlayersText").GetComponent<TextMeshProUGUI>().text = room.PlayerCount + " / " + room.MaxPlayers;

            newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); });
        }

        base.OnRoomListUpdate(roomList);
    }

    public void CreateRoom()
    {
        Debug.Log("CreateRoom");
        int roomNumber = Random.Range(1000, 9999);
        PhotonNetwork.CreateRoom(roomNumber.ToString(), new RoomOptions
        {
            MaxPlayers = 6,
            IsVisible = true,
            IsOpen = true
        });
    }
    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Gameplay");
    }

    public void JoinRoom(Transform button)
    {
        string roomName = button.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text;
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            foreach (GameObject go in GOToLoadAfterConnect)
            {
                go.SetActive(false);
            }
            Debug.Log("OnJoinedRoom");
            connectingText.text = "Loading room...";
            PhotonNetwork.LoadLevel("Gameplay");
        }
        else
        {
            Debug.Log("MasterClient");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed: " + returnCode + ", " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
        foreach (GameObject go in GOToLoadAfterConnect)
        {
            go.SetActive(true);
        }
    }
}
