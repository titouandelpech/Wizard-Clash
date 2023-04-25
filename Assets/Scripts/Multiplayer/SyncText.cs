using UnityEngine;
using Photon.Pun;
using TMPro;

public class SyncText : MonoBehaviourPun
{
    TMP_Text myText;

    void Start()
    {
        myText = GetComponent<TMP_Text>();
    }

    [PunRPC]
    void SetText(string newText)
    {
        myText.text = newText;
    }

    public void UpdateText(string newText)
    {
        photonView.RPC("SetText", RpcTarget.All, newText);
    }
}