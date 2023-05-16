using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviourPunCallbacks
{
    public float timeRemaining = 3;
    public bool isActivate = false;

    void Update()
    {
        if (isActivate) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            }
            else {
                timeRemaining = 0;
                isActivate = false;
                ActivateShield(false);
            }
        }
    }

    [PunRPC]
    void RPCActivateShield(bool activate)
    {
        isActivate = activate;
        GetComponent<MeshRenderer>().enabled = isActivate;
    }
    public void ActivateShield(bool activate)
    {
        photonView.RPC("RPCActivateShield", RpcTarget.All, activate);
    }
}
