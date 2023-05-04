using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform SpellSpawnPoint;
    public GameObject SpellPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            var Spell = Instantiate(SpellPrefab, SpellSpawnPoint.position, SpellPrefab.transform.rotation);
        }
    }
}
