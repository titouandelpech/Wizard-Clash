using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyHandData : MonoBehaviourPunCallbacks
{
    Transform LeftHand;
    Transform RightHand;
    public Transform LeftHandCopy;
    public Transform RightHandCopy;

    private void Awake()
    {
        Transform XRSetup = GameObject.Find("Complete XR Origin Set Up").transform;
        LeftHand = XRSetup.Find("XR Origin").Find("CameraOffset").Find("LeftHand (Smooth locomotion)");
        RightHand = XRSetup.Find("XR Origin").Find("CameraOffset").Find("RightHand (Teleport Locomotion)");
    }

    private void Update()
    {
        if (photonView.IsMine)
            Debug.Log(LeftHandCopy.transform.position);
        LeftHandCopy.transform.position = LeftHand.transform.position;
        RightHandCopy.transform.position = RightHand.transform.position;
    }
}
