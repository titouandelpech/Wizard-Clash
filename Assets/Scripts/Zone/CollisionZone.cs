using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionZone : MonoBehaviour
{
    public int zone;
    public int count;
    public bool isActive;
    public bool isHandActive;

    public PlayerGame player;

    void OnTriggerEnter(Collider target) {
        Debug.Log("touched " + target.tag);
        if ((target.tag == "Spell" && transform.parent.tag == "EnemyZone" && zone != 4) || (target.tag == "SpellUp" && transform.parent.tag == "EnemyZone" && zone == 3))
        {
            ProjectileSet pSet = target.gameObject.GetComponent<ProjectileSet>();
            if (pSet.photonView.IsMine && !pSet.triggered)
            {
                pSet.OnCollisionProjectile(target.transform.position, target.transform.forward);
                foreach (PlayerGame player in FindObjectsByType<PlayerGame>(FindObjectsSortMode.None))
                {
                    if (!player.photonView.IsMine && ((player.isZoneLeft && zone == 1) || (player.isZoneRight && zone == 2) || (player.isZoneTop && zone == 3)))
                        player.EditPlayerData(-pSet.damage, PlayerData.Health, ValueEditMode.Add);
                }
            }
        }
        if (target.tag == "PlayerDetection" && player.photonView.IsMine)
        {
            isActive = true;
            player.SetZoneBool(tag, isActive);
        } else if (target.tag == "RightHand" && zone != 3) {
                isHandActive = true;
                switch (zone) {
                case 1:
                    Debug.Log("EnterRight");
                    break;
                case 2:
                    Debug.Log("EnterLeft");
                    break;
                case 3:
                    Debug.Log("EnterTop");
                    break;
                default:
                    Debug.Log("Wtf");
                    break;
            }
        }
    }
    void OnTriggerExit(Collider target) {
        if(target.tag == "PlayerDetection")
        {
            isActive = false;
            player.SetZoneBool(tag, isActive);
        } else if (target.tag == "RightHand" && zone != 3) {
            isHandActive = false;
            switch (zone) {
            case 1:
                Debug.Log("LeaveRight");
                break;
            case 2:
                Debug.Log("LeaveLeft");
                break;
            case 3:
                Debug.Log("LeaveTop");
                break;
            default:
                Debug.Log("Wtf");
                break;
            }
        }
    }
}
