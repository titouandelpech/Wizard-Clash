using Photon.Pun;
using UnityEngine;

public class OfflineMode : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.OfflineMode = true;
    }
}
