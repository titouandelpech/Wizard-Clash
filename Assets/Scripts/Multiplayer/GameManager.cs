using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    [SerializeField] string launcherScene;

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log(other.NickName + " just joined the room!");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log(other.NickName + " just joined the room!");
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(launcherScene);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
