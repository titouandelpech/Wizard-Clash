using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviourPunCallbacks
{
    public float timePerShield = 3;
    private float timeRemaining;
    [SerializeField] private bool isActivate = false;

    void Start()
    {
        timeRemaining = timePerShield;
    }

    void Update()
    {
        if (isActivate) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            }
            else {
                timeRemaining = timePerShield;
                ActivateShield(false);
            }
        }
    }

    public bool GetIsActivate()
    {
        return isActivate;
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
