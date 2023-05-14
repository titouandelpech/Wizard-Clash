using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    GameDataKeep gameDataKeep;
    private void Awake()
    {
        gameDataKeep = FindAnyObjectByType<GameDataKeep>();
    }

    public GameObject playerPrefab;
    [SerializeField] string launcherScene;
    GameObject XRSetup;
    [SerializeField] List<GameObject> listPlayerModels;
    [SerializeField] Material transparentCharacterMaterial;

    void Start()
    {
        //PhotonNetwork.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
        XRSetup = GameObject.Find("Complete XR Origin Set Up");
        int playerNb = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        
        MovementRecognition wand = null;
        CopyHandData copyHands = PhotonNetwork.Instantiate("Multiplayer/CopyHandsData", Vector3.zero, Quaternion.identity, 0).GetComponent<CopyHandData>();
        Quaternion playerRotation = Quaternion.identity;

        if (playerNb == 0)
        {
            gameDataKeep.setGameMap(gameDataKeep.mapToLoad);
            XRSetup.transform.position = new Vector3(0, 0, 0);
            wand = PhotonNetwork.Instantiate("Spell Shoot/wand", new Vector3(0.4f, 1.3f, 1), Quaternion.identity, 0).GetComponent<MovementRecognition>();
        } 
        else
        {
            XRSetup.transform.position = new Vector3(0, 0, 8);
            playerRotation = Quaternion.Euler(0, 180, 0);
            XRSetup.transform.rotation = playerRotation;
            wand = PhotonNetwork.Instantiate("Spell Shoot/wand", new Vector3(-0.4f, 1.3f, 7), Quaternion.identity, 0).GetComponent<MovementRecognition>();
        }

        VRArmIKController playerModelIKController = PhotonNetwork.Instantiate("Wizard0" + Random.Range(1, 5), XRSetup.transform.position, playerRotation, 0).GetComponent<VRArmIKController>();
        playerModelIKController.SetTransform(0, copyHands.LeftHandCopy);
        playerModelIKController.SetTransform(1, copyHands.RightHandCopy);
        playerModelIKController.transform.Find(playerModelIKController.gameObject.name.Replace("(Clone)", "")).GetComponent<Renderer>().material = transparentCharacterMaterial; //set own's character material to transparent
        
        SpellSpawner spellSpawner = PhotonNetwork.Instantiate("Spell Shoot/Spell Cast", Vector3.zero, Quaternion.identity, 0).GetComponent<SpellSpawner>();
        spellSpawner.Target = wand.transform.Find("SpellSpawnPoint").gameObject;
        wand.OnRecognized.AddListener(spellSpawner.Spawn);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log(other.NickName + " just joined the room!");
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log(other.NickName + " just left the room!");
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
