using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerData
{
    Health,
    Mana
}

public enum ValueEditMode
{
    Add,
    Set
}

public class PlayerGame : MonoBehaviourPunCallbacks
{
    [SerializeField] SyncText playerNameText;
    
    private string Name;
    private const int MaxHealth = 100;
    private const int MaxMana = 100;

    public bool isZoneLeft;
    public bool isZoneRight;
    public bool isZoneTop;

    [SerializeField] private int health;
    [SerializeField] private int mana;

    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, MaxHealth);
    }

    public int Mana
    {
        get => mana;
        set => mana = Mathf.Clamp(value, 0, MaxMana);
    }

    void Start()
    {
        Health = MaxHealth;
        Mana = MaxMana;
        if (photonView.IsMine)
        {
            Name = PhotonNetwork.LocalPlayer.NickName;
            UpdateName(Name);
            Instantiate(Resources.Load("PrefabEsquive/Curve"), transform.parent);
        }
        playerNameText.UpdateText("<color=red>" + Health + "/" + MaxHealth + "</color>");
        GameObject zone = Instantiate(Resources.Load("PrefabEsquive/ZoneDodge"), transform.parent) as GameObject;
        if (!photonView.IsMine)
        {
            zone.tag = "EnemyZone";
            gameObject.tag = "Target";
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            CheckKeyDown();
        }
    }

    void CheckKeyDown()
    {
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                //Debug.Log("Touche enfoncée: " + keyCode);
                //playerNameText.UpdateText(keyCode.ToString());

                if (keyCode == KeyCode.Return)
                {
                    foreach (PlayerGame player in FindObjectsByType<PlayerGame>(FindObjectsSortMode.None))
                    {
                        if (!player.photonView.IsMine)
                            player.EditPlayerData(-10, PlayerData.Health, ValueEditMode.Add);
                    }
                }
                break;
            }
        }
    }

    [PunRPC]
    void EditPlayerDataRPC(int valueChange, PlayerData changeValue)
    {
        Debug.Log("Removed " + valueChange + " HP");
        switch (changeValue)
        {
            case PlayerData.Health:
                Health += valueChange;
                playerNameText.UpdateText("<color=red>" + Health + "/" + MaxHealth + "</color>");
                break;
            case PlayerData.Mana:
                Mana += valueChange;
                break;
        }
    }
    public void EditPlayerData(int valueChange, PlayerData changeValue, ValueEditMode valueEditMode)
    {
        switch (valueEditMode)
        {
            case ValueEditMode.Add:
                photonView.RPC("EditPlayerDataRPC", RpcTarget.All, valueChange, changeValue);
                break;
            case ValueEditMode.Set:
                switch (changeValue)
                {
                    case PlayerData.Health:
                        photonView.RPC("EditPlayerDataRPC", RpcTarget.All, valueChange - Health, changeValue);
                        break;
                    case PlayerData.Mana:
                        photonView.RPC("EditPlayerDataRPC", RpcTarget.All, valueChange - Mana, changeValue);
                        break;
                }
                break;
        }
    }

    [PunRPC]
    void SetName(string newName)
    {
        Name = newName;
        playerNameText.setNickname(newName);
    }
    public void UpdateName(string newName)
    {
        photonView.RPC("SetName", RpcTarget.All, newName);
    }
}