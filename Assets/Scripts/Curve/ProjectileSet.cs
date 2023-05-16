using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSet : MonoBehaviourPunCallbacks
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;
    public int damage;
    private GameObject target;
    private float distance;
    public QuadraticCurve curve;
    public int curveTarget;

    private float sampleTime;

    void Start()
    {
        target =  GameObject.FindWithTag("Target");

        switch (curveTarget) {
                case 1:
                    curve = GameObject.FindWithTag("CurveRight").GetComponent<QuadraticCurve>();
                    break;
                case 2:
                    curve = GameObject.FindWithTag("CurveLeft").GetComponent<QuadraticCurve>();
                    break;
                case 3:
                    curve = GameObject.FindWithTag("CurveUp").GetComponent<QuadraticCurve>();
                    break;
                default:
                    curve = GameObject.FindWithTag("CurveRight").GetComponent<QuadraticCurve>();
                    break;
            }
        //curve = GameObject.FindWithTag("CurveRight").GetComponent<QuadraticCurve>();
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            
            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject,5);
	}

   void FixedUpdate()
   {
        if ((!photonView || photonView.IsMine))
        {
            sampleTime += Time.deltaTime * speed;
            transform.position = curve.evaluateA(sampleTime);
            transform.forward = curve.evaluateA(sampleTime + 0.001f) - transform.position;

            if (sampleTime >= 1f) {
               //Debug.Log("pif pas pouf");
                Destroy(gameObject);
            }

        }
   }

    void OnCollisionEnterLegacy(Collision collision)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }
        //Destroy projectile on collision
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView)
            OnCollisionEnterLegacy(collision);
        else if (photonView.IsMine)
        {
            photonView.RPC("RPC_OnCollisionEnter", RpcTarget.All, collision.contacts[0].point, collision.contacts[0].normal);
            foreach (PlayerGame player in FindObjectsByType<PlayerGame>(FindObjectsSortMode.None))
            {
                if (!player.photonView.IsMine)
                    player.EditPlayerData(-damage, PlayerData.Health, ValueEditMode.Add);
            }
        }
    }

    [PunRPC]
    private void RPC_OnCollisionEnter(Vector3 contactPoint, Vector3 contactNormal)
    {
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactNormal);
        Vector3 pos = contactPoint + contactNormal * hitOffset;

        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contactPoint + contactNormal); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }
        //Destroy projectile on collision
        Destroy(gameObject);
    }
}
