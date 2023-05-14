using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticProjectile : MonoBehaviour
{
    public QuadraticCurve curve;
    public float speed;

    private float sampleTime;
    // Start is called before the first frame update
    void Start()
    {
        sampleTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        sampleTime += Time.deltaTime * speed;
        transform.position = curve.evaluateA(sampleTime);
        transform.forward = curve.evaluateA(sampleTime + 0.001f) - transform.position;

        if (sampleTime >= 1f) {
            Debug.Log("pif pas pouf");
            Destroy(gameObject);
        }
    }
}
