using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviourPunCallbacks
{
    public List<GameObject> objects;
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        
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
                //var go = Instantiate(item, Target.transform.position,  Target.transform.rotation) as GameObject;
                PhotonNetwork.Instantiate(item.name, Target.transform.position, Target.transform.rotation);
            }
        }
    }
}
