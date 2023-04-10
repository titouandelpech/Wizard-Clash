using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lancher : MonoBehaviourPunCallbacks
{
    [SerializeField] Button ConnectButton;
    [SerializeField] TMP_Text FeedbackText;

    byte maxPlayerPerRoom = 2;

    bool isConnecting;
    string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        FeedbackText.text = "";

        isConnecting = true;

        ConnectButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            LogFeedback("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    void LogFeedback(string message)
    {
        if (FeedbackText == null)
        {
            return;
        }
        FeedbackText.text += System.Environment.NewLine + message;
    }

    #region Callback Functions

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayerPerRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);
        Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");

        isConnecting = false;
        ConnectButton.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Loading level");
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    #endregion
}