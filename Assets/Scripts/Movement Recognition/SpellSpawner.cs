using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
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
    public GameObject _wand;
    private float rotationY;
    private float rotationX;

    void Start()
    {
        WallLeft = GameObject.FindWithTag("WallLeft").GetComponent<CollisionZone>();
        WallRight = GameObject.FindWithTag("WallRight").GetComponent<CollisionZone>();
        WallUp = GameObject.FindWithTag("WallUpHand").GetComponent<CollisionZone>();
        _wand = GameObject.FindWithTag("Wand");
    }

    void Update()
    {
        Debug.Log("Angle x : " + _wand.GetComponent<Transform>().eulerAngles.x);
        Debug.Log("Angle y : " + _wand.GetComponent<Transform>().eulerAngles.y);
       
        //Debug.Log("rotationY : " + rotationY);
        //Debug.Log("rotationX : " + rotationY);
    }

    public void Spawn(string objectName)
    {
        PlayerGame myPlayer = FindObjectsOfType<PlayerGame>().FirstOrDefault(player => player.photonView.IsMine);

        rotationY = _wand.GetComponent<Transform>().eulerAngles.y;
        rotationX = _wand.GetComponent<Transform>().eulerAngles.x;
        if (rotationY > 180)
           rotationY -= 360;
        if (rotationX > 180)
           rotationX -= 360;
        Debug.Log("rotationY : " + rotationY);
        Debug.Log("rotationX : " + rotationX);
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
                            //poignet
                            if (rotationX <= -40) {
                                newitem.GetComponent<ProjectileSet>().curveTarget = 3;
                                newitem.gameObject.tag = "SpellUp";
                            } else if (rotationY <= -30) {
                                newitem.GetComponent<ProjectileSet>().curveTarget = 2;
                            } else if (rotationY >= 30){
                                newitem.GetComponent<ProjectileSet>().curveTarget = 1;
                            }
                            //position de la main
                            else if (WallUp.isHandActive == true) {
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
                            //Debug.Log("Angle x : " + _wand.GetComponent<Transform>().EulerAngles.x);
                            //Debug.Log("Angle y : " + _wand.GetComponent<Transform>().EulerAngles.y);
                             if (rotationX <= -40) {
                                newitem.GetComponent<ProjectileSet>().curveTarget = 3;
                                newitem.gameObject.tag = "SpellUp";
                            } else if (rotationY <= -30) {
                                if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == 0) {
                                    newitem.GetComponent<ProjectileSet>().curveTarget = 2;
                                }
                                else { 
                                    newitem.GetComponent<ProjectileSet>().curveTarget = 1;
                                }
                            } else if (rotationY >= 30){
                                if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == 0) {
                                    newitem.GetComponent<ProjectileSet>().curveTarget = 1;
                                }
                                else {
                                    newitem.GetComponent<ProjectileSet>().curveTarget = 2;
                                }
                                //newitem.GetComponent<ProjectileSet>().curveTarget = 1;
                            }
                            else if (WallUp.isHandActive == true) {
                                Debug.Log("tata");
                                pSet.curveTarget = 3;
                                newitem.gameObject.tag = "SpellUp";
                            } else if (WallRight.isHandActive == true) {
                                Debug.Log("tato");
                                pSet.curveTarget = 1;
                            } else {
                                Debug.Log("toto22");
                                pSet.curveTarget = 2;
                            }
                        }
                    }
                }
            }
        }
    }
}
