using Photon.Pun;
using UnityEngine;

public class VRArmIKController : MonoBehaviourPunCallbacks
{
    public Animator animator;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    private void OnAnimatorIK(int layerIndex)
    {
        // Set the IK goals for both hands
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
    }

    [PunRPC]
    void RPCSetTransform(int transformNb, int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);

        if (targetView != null)
        {
            Transform targetTransform = targetView.transform;

            if (transformNb == 0)
            {
                leftHandTarget = targetTransform;
            }
            else
            {
                rightHandTarget = targetTransform;
            }
        }
        else
        {
            Debug.LogError("No PhotonView found with the given viewID: " + viewID);
        }
    }

    public void SetTransform(int transformNb, Transform transform)
    {
        PhotonView targetView = transform.GetComponent<PhotonView>();

        if (targetView != null)
        {
            photonView.RPC("RPCSetTransform", RpcTarget.All, transformNb, targetView.ViewID);
        }
        else
        {
            Debug.LogError("No PhotonView component found on the target transform.");
        }
    }

}
