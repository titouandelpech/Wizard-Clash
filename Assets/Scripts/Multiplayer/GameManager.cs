using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

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
    MovementRecognition wand;
    PlayerGame myPlayer;
    PlayerGame otherPlayer;
    SpellSpawner spellSpawner;
    bool isMapLoaded = false;

    void Start()
    {
        //PhotonNetwork.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
        XRSetup = GameObject.Find("Complete XR Origin Set Up");
        int playerNb = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        
        wand = null;
        CopyHandData copyHands = PhotonNetwork.Instantiate("Multiplayer/CopyHandsData", Vector3.zero, Quaternion.identity, 0).GetComponent<CopyHandData>();
        Quaternion playerRotation = Quaternion.identity;

        if (playerNb == 0)
        {
            gameDataKeep.setGameMap(gameDataKeep.mapToLoad);
            XRSetup.transform.position = new Vector3(0, 0, 0);
            wand = PhotonNetwork.Instantiate("Spell Shoot/wand", new Vector3(0.25f, 1.3f, 0.6f), Quaternion.identity, 0).GetComponent<MovementRecognition>();

            GameObject canvasEndGame = GameObject.Find("CanvasEndGame");
            if (canvasEndGame != null)
            {
                Transform canvasTransform = canvasEndGame.transform;
                canvasTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y, 2.5f);
                canvasTransform.rotation = Quaternion.Euler(canvasTransform.rotation.x, 0f, canvasTransform.rotation.z);
            }
        }
        else
        {
            XRSetup.transform.position = new Vector3(0, 0, 16);
            playerRotation = Quaternion.Euler(0, 180, 0);
            XRSetup.transform.rotation = playerRotation;
            wand = PhotonNetwork.Instantiate("Spell Shoot/wand", new Vector3(-0.25f, 1.3f, 15.4f), Quaternion.Euler(0, 180, 0), 0).GetComponent<MovementRecognition>();

            GameObject canvasEndGame = GameObject.Find("CanvasEndGame");
            if (canvasEndGame != null)
            {
                Transform canvasTransform = canvasEndGame.transform;
                canvasTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y, 13.5f);
                canvasTransform.rotation = Quaternion.Euler(canvasTransform.rotation.x, 180, canvasTransform.rotation.z);
            }
        }

        VRArmIKController playerModelIKController = PhotonNetwork.Instantiate("Wizard0" + Random.Range(1, 5), XRSetup.transform.position, playerRotation, 0).GetComponent<VRArmIKController>();
        playerModelIKController.SetTransform(0, copyHands.LeftHandCopy);
        playerModelIKController.SetTransform(1, copyHands.RightHandCopy);
        playerModelIKController.transform.Find(playerModelIKController.gameObject.name.Replace("(Clone)", "")).GetComponent<Renderer>().material = transparentCharacterMaterial; //set own's character material to transparent

        spellSpawner = PhotonNetwork.Instantiate("Spell Shoot/Spell Cast", Vector3.zero, Quaternion.identity, 0).GetComponent<SpellSpawner>();
        spellSpawner.Target = wand.transform.Find("SpellSpawnPoint").gameObject;
        wand.OnRecognized.AddListener(spellSpawner.Spawn);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Button - Restart").GetComponent<Button>().onClick.AddListener(WaitForStartAgain);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Button - Return to Lobby").GetComponent<Button>().onClick.AddListener(LeaveRoom);
    }

    void Update()
    {
        if (!checkSetter()) return;
        else if ((myPlayer.Health <= 0 || otherPlayer.Health <= 0) && !wand.blockSpells)
        {
            DisplayEndGameMenu();
        }
        else if (myPlayer.Health == 100 && otherPlayer.Health == 100 && wand.blockSpells)
        {
            StartAgain();
        }
    }

    bool checkSetter()
    {
        if (!otherPlayer || !myPlayer)
        {
            foreach (PlayerGame player in FindObjectsByType<PlayerGame>(FindObjectsSortMode.None))
            {
                otherPlayer = !otherPlayer && !player.photonView.IsMine ? player : otherPlayer;
                myPlayer = !myPlayer && player.photonView.IsMine ? player : myPlayer;
            }
        }
        if (!spellSpawner.ShieldManager && myPlayer)
        {
            spellSpawner.ShieldManager = PhotonNetwork.Instantiate("Shield/Shield", myPlayer.transform.position, myPlayer.transform.rotation).GetComponent<ShieldManager>();
        }
        if (!isMapLoaded && gameDataKeep.loadedMap != "")
        {
            Instantiate(Resources.Load("Maps/" + gameDataKeep.loadedMap));
            isMapLoaded = true;
        }
        if (!otherPlayer || !myPlayer || !spellSpawner.ShieldManager) return false;
        return true;
    }

    void DisplayEndGameMenu()
    {
        Transform PanelEndGame = GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform;
        if (PanelEndGame.gameObject.activeSelf) return;
        PanelEndGame.gameObject.SetActive(true);
        PanelEndGame.Find("Text - Victory").gameObject.SetActive(otherPlayer.Health <= 0);
        PanelEndGame.Find("Text - Defeat").gameObject.SetActive(myPlayer.Health <= 0);
        wand.blockSpells = true;
        //StartCoroutine(Restart());
    }


    IEnumerator Restart()
    {
        yield return new WaitForSeconds(4);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").gameObject.SetActive(false);
        StartAgain();
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
        Destroy(FindObjectOfType<GameDataKeep>().gameObject);
        Destroy(GameObject.Find("Complete XR Origin Set Up"));
        Destroy(GameObject.Find("XR Device Simulator"));
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").gameObject.SetActive(false);
        FindObjectsOfType<CopyHandData>().Where(copyHandData => copyHandData.photonView.IsMine).ToList().ForEach(Destroy);
        PhotonNetwork.LeaveRoom();
    }

    public void WaitForStartAgain()
    {
        foreach (PlayerGame player in FindObjectsByType<PlayerGame>(FindObjectsSortMode.None))
        {
            if (player.photonView.IsMine)
            {
                player.EditPlayerData(100, PlayerData.Health, ValueEditMode.Set);
                player.EditPlayerData(100, PlayerData.Mana, ValueEditMode.Set);
            }
        }
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Text - Victory").gameObject.SetActive(false);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Text - Defeat").gameObject.SetActive(false);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Button - Restart").gameObject.SetActive(false);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Button - Return to Lobby").gameObject.SetActive(false);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Text - Waiting").gameObject.SetActive(true);

    }

    public void StartAgain()
    {
        wand.blockSpells = false;
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Button - Restart").gameObject.SetActive(true);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Button - Return to Lobby").gameObject.SetActive(true);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").transform.Find("Text - Waiting").gameObject.SetActive(false);
        GameObject.Find("CanvasEndGame").transform.Find("MainPanel").Find("PanelEndGame").gameObject.SetActive(false);
    }
}
