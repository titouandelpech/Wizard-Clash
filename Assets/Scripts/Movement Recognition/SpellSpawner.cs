using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpellSpawner : MonoBehaviourPunCallbacks
{
    public List<GameObject> objects;
    public GameObject Target;
    public CollisionZone WallLeft;
    public CollisionZone WallRight;
    public CollisionZone WallUp;
    public ShieldManager ShieldManager;

    void Start()
    {
        WallLeft = GameObject.FindWithTag("WallLeft").GetComponent<CollisionZone>();
        WallRight = GameObject.FindWithTag("WallRight").GetComponent<CollisionZone>();
        WallUp = GameObject.FindWithTag("WallUpHand").GetComponent<CollisionZone>();
    }

    void Update()
    {
        
    }

    public void Spawn(string objectName)
    {
        PlayerGame myPlayer = FindObjectsOfType<PlayerGame>().FirstOrDefault(player => player.photonView.IsMine);

        if (objectName == "O") {
            int shieldManaCost = 20;
            if (myPlayer.Mana >= shieldManaCost)
                ShieldManager.ActivateShield(true);
                myPlayer.EditPlayerData(-shieldManaCost, PlayerData.Mana, ValueEditMode.Add);
        } else {
            foreach (var item in objects)
            {
                if (objectName == item.name)
                {
                    if (PhotonNetwork.OfflineMode) {
                        GameObject newitem =  Instantiate(item, Target.transform.position,  Target.transform.rotation);
                        if (objectName != "O") {
                            if (WallUp.isHandActive == true) {
                                Debug.Log("tata");
                                newitem.GetComponent<ProjectileSet>().curveTarget = 3;
                                newitem.gameObject.tag = "SpellUp";
                            } else if (WallRight.isHandActive == true) {
                                Debug.Log("tato");
                                newitem.GetComponent<ProjectileSet>().curveTarget = 1;
                            } else {
                                Debug.Log("toto");
                                newitem.GetComponent<ProjectileSet>().curveTarget = 2;
                            }
                        }
                    } if (!PhotonNetwork.OfflineMode) {
                        GameObject newitem = PhotonNetwork.Instantiate(item.name, Target.transform.position, Target.transform.rotation);
                        ProjectileSet pSet = newitem.GetComponent<ProjectileSet>();
                        if (photonView.IsMine)
                        {
                            if (myPlayer.Mana < pSet.manaCost)
                                PhotonNetwork.Destroy(newitem);
                            else
                                myPlayer.EditPlayerData(-pSet.manaCost, PlayerData.Mana, ValueEditMode.Add);
                        }
                        if (objectName != "O") {
                            if (WallUp.isHandActive == true) {
                                Debug.Log("tata");
                                pSet.curveTarget = 3;
                                newitem.gameObject.tag = "SpellUp";
                            } else if (WallRight.isHandActive == true) {
                                Debug.Log("tato");
                                pSet.curveTarget = 1;
                            } else {
                                Debug.Log("toto");
                                pSet.curveTarget = 2;
                            }
                        }
                    }
                }
            }
        }
    }
}
