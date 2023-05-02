using UnityEngine;
using Photon.Pun;
using TMPro;

public class SyncText : MonoBehaviourPun
{
    TMP_Text myText;
    string nickname;

    void Awake()
    {
        myText = GetComponent<TMP_Text>();
    }

    [PunRPC]
    void SetText(string newText)
    {
        string setNewText = "";
        if (nickname != "") setNewText += nickname + '\n';
        setNewText += newText;
        myText.text = setNewText;
    }
    public void UpdateText(string newText)
    {
        photonView.RPC("SetText", RpcTarget.All, newText);
    }

    public void setNickname(string newNickname)
    {
        nickname = newNickname;
    }
}