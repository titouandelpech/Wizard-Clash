using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_ProjectileMover : MonoBehaviour
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

    void Start()
    {
        target =  GameObject.FindWithTag("Target");
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
    if (target != null)
    {
        // Calculate the direction to the target
        Vector3 directionToTarget = target.transform.position - transform.position;

        // Normalize the direction to get a unit vector
        Vector3 normalizedDirection = directionToTarget.normalized;

        // Calculate the distance to the target
        distance = Vector3.Distance(transform.position, target.transform.position);

        // Calculate the amount of movement in the x and z directions
        float moveAmountX = normalizedDirection.x * speed * Time.deltaTime;
        float moveAmountZ = normalizedDirection.z * speed * Time.deltaTime;

        // Calculate the amount of movement in the y direction based on a curve
        float yCurve = Mathf.Clamp01(1 - (distance / 10f)); // adjust the "10f" value as desired to control the curve
        float moveAmountY = yCurve * speed * Time.deltaTime;

        // Combine the movement amounts into a single Vector3
        Vector3 moveAmount = new Vector3(moveAmountX, moveAmountY, moveAmountZ);

        // Move the projectile in a curve using Vector3.MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveAmount.magnitude);

        // Rotate the projectile to face the target
        transform.LookAt(target.transform);

    }
}



    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
    void OnCollisionEnter(Collision collision)
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
}
