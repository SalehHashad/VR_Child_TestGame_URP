using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private static string MyRoom = "MyRoom";
    [SerializeField] private TextMeshProUGUI Status;
    [SerializeField] private TextMeshProUGUI clients;
    // Start is called before the first frame update
    void Start()
    {
        
        DontDestroyOnLoad(this);
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            UpdateClients(PhotonNetwork.CurrentRoom.PlayerCount.ToString());
        }


    }
    public void ConnectToServer()
    {
        UpdateStatus("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        UpdateStatus("Connected");
        Debug.Log("Joining To Room...");
        PhotonNetwork.JoinOrCreateRoom(MyRoom, new RoomOptions {MaxPlayers= 4}, TypedLobby.Default);
        UpdateStatus($"Has been joined to:{MyRoom}");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause.ToString());
        
    }
    void UpdateStatus(string text)
    {
        if(Status!= null)
        {
            Status.text = text;
        }
    }
    void UpdateClients(string text)
    {
        if (clients != null)
        {
            clients.text = text;
        }
    }
}
