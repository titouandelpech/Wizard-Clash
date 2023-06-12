using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerPos : MonoBehaviourPunCallbacks
{
    Transform playerCam;
    void Start()
    {
        if (!photonView.IsMine) 
            this.enabled = false;
        else 
            playerCam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (transform)
            transform.position = new Vector3(playerCam.position.x, transform.position.y, playerCam.position.z);
    }
}
