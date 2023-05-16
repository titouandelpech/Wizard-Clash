using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDataKeep : MonoBehaviourPunCallbacks
{
    public string mapToLoad = "";
    public string loadedMap = "";
    LobbyMainPanel mainPanel;
    void Start()
    {
        mainPanel = FindAnyObjectByType<LobbyMainPanel>();
    }

    void Update()
    {
        if (mainPanel && mainPanel.mapsList[mainPanel.mapIndex] != mapToLoad)
            mapToLoad = mainPanel.mapsList[mainPanel.mapIndex];
    }

    [PunRPC]
    void setGameMapRPC(string newGameMap)
    {
        loadedMap = newGameMap;
    }

    public void setGameMap(string newGameMap)
    {
        photonView.RPC("setGameMapRPC", RpcTarget.All, newGameMap);
    }
}
