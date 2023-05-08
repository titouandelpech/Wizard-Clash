using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.Demo.Asteroids;

public class PlayerMultiData : MonoBehaviour
{
    public int playerNb = 0;
    
    LobbyMainPanel mainPanel;
    private void Start()
    {
        mainPanel = GetComponent<LobbyMainPanel>();
    }


}