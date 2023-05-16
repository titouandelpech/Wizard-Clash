using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionZone : MonoBehaviour
{
    public int zone;
    public int count;
    public bool isActive;
    public bool isHandActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider target) {
        Debug.Log("touché " + target.tag);
        if(target.tag == "Player")
        {
            isActive = true;
        } else if (target.tag == "RightHand") {
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
            /*switch (zone) {
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
            }*/
        
    }
    void OnTriggerExit(Collider target) {
        if(target.tag == "PlayerDetection")
        {
            isActive = false;
        } else if (target.tag == "RightHand") {
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
            /*switch (zone) {
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
            }*/
        }
}
