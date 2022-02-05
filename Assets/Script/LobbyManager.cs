using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;
    public RoomItem roomItemsPrefab;
    public float timeBTNUpdates = 1.5f;
    float nextUpdateTime;
    List<RoomItem> roomItemsList = new List<RoomItem>();

    public Transform contentObject;


    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
      //  lobbyPanel.SetActive(true);
       // roomPanel.SetActive(false);

    }

    public void OnClickCreate()
    {
        if (roomName.text.Length >= 2)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() {MaxPlayers = 10, BroadcastPropsChangeToAll = true });
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       if(Time.time >= nextUpdateTime)
        {
            base.OnRoomListUpdate(roomList);
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBTNUpdates;
        }

    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach(RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemsPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnclickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }
    void UpdatePlayerList()
    {
        foreach(PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        if (PhotonNetwork.CurrentRoom == null) return;

        foreach (KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
        {
          PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
           
            newPlayerItem.SetPlayerInfo(player.Value);

            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }
            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerList();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient /*&& PhotonNetwork.CurrentRoom.PlayerCount >= 2*/)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false); 
        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("NFTworld");
       
    }
}
