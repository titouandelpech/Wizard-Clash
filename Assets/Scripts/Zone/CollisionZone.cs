using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionZone : MonoBehaviour
{
    public int zone;
    public int count;
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider target) {
        if(target.tag == "Player")
        {
            isActive = true;
         
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
            isActive = false;
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
