using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviourPunCallbacks
{
    public List<GameObject> objects;
    public GameObject Target;
    public CollisionZone WallLeft;
    public CollisionZone WallRight;
    public CollisionZone WallUp;
    // Start is called before the first frame update
    void Start()
    {
        WallLeft = GameObject.FindWithTag("WallLeft").GetComponent<CollisionZone>();
        WallRight = GameObject.FindWithTag("WallRight").GetComponent<CollisionZone>();
        WallUp = GameObject.FindWithTag("WallUpHand").GetComponent<CollisionZone>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(string objectName)
    {
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
                }
            }
        }
    }
}
