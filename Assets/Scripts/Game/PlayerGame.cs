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

    public bool isZoneLeft = true;
    public bool isZoneRight = true;
    public bool isZoneTop = true;

    [SerializeField] private int health;
    [SerializeField] private int mana;

    private float manaTimer;
    private const float manaAddCooldown = 1f;
    private const int manaAddValue = 2;

    BookFlipper book;

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
        manaTimer = Time.time;
        if (photonView.IsMine)
        {
            Name = PhotonNetwork.LocalPlayer.NickName;
            UpdateName(Name);
            Instantiate(Resources.Load("PrefabEsquive/Curve"), transform.parent);
        }
        playerNameText.UpdateText("<color=red>" + Health + "/" + MaxHealth + "</color>");
        GameObject zones = Instantiate(Resources.Load("PrefabEsquive/ZoneDodge"), transform.parent) as GameObject;
        foreach(CollisionZone zone in zones.transform.GetComponentsInChildren<CollisionZone>())
        {
            zone.player = this;
        }
        if (!photonView.IsMine)
        {
            zones.tag = "EnemyZone";
            tag = "Target";
        }
        book = FindObjectOfType<BookFlipper>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            CheckKeyDown();
        }
        if (Time.time - manaTimer > manaAddCooldown)
        {
            EditPlayerData(manaAddValue, PlayerData.Mana, ValueEditMode.Add);
            manaTimer = Time.time;
        }
        book.UpdateHealthBar(MaxHealth, Health);
        book.UpdateManaBar(MaxMana, Mana);
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

    [PunRPC]
    void SetZoneBoolRPC(string zone, bool isActive)
    {
        if (zone == "WallLeft")
            isZoneLeft = isActive;
        if (zone == "WallRight")
            isZoneRight = isActive;
        if (zone == "WallUp")
            isZoneTop = isActive;
    }
    public void SetZoneBool(string zone, bool isActive)
    {
        photonView.RPC("SetZoneBoolRPC", RpcTarget.All, zone, isActive);
    }
}