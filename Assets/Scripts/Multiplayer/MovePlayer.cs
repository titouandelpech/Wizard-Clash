using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class MovePlayer : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5.0f;
    private Rigidbody rb;
    private Vector3 moveDirection;
    [SerializeField] SyncText playerNameText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInput();
            CheckKeyDown();
        }
    }

    void CheckKeyDown()
    {
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                Debug.Log("Touche enfoncée: " + keyCode);
                playerNameText.UpdateText(keyCode.ToString());
                break;
            }
        }
    }


    void ProcessInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (moveDirection != Vector3.zero)
        {
            // Prendre en compte la gravité
            Vector3 gravity = Physics.gravity;
            Vector3 newVelocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y + gravity.y * Time.fixedDeltaTime, moveDirection.z * moveSpeed);

            rb.velocity = newVelocity;
        }
        else
        {
            // Arrêt instantané
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
}
