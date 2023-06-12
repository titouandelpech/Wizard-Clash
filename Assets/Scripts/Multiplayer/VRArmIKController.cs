using Photon.Pun;
using UnityEngine;

public class VRArmIKController : MonoBehaviourPunCallbacks
{
    public Animator animator;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [SerializeField] PlayerGame playerGame;

    Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        animator.SetBool("crouched", !playerGame.isZoneTop);
        Debug.Log((transform.position - lastPos).magnitude);
        if ((transform.position - lastPos).magnitude > 0.01)
        {
            if (Mathf.Abs(lastPos.x - transform.position.x) > Mathf.Abs(lastPos.z - transform.position.z))
            {
                if (lastPos.x < transform.position.x)
                {
                    animator.SetFloat("Move left", 1f);
                }
                else if (lastPos.x > transform.position.x)
                {
                    animator.SetFloat("Move left", -1f);
                }
                animator.SetFloat("Move front", 0);
            }
            else
            {
                if (lastPos.z < transform.position.z)
                {
                    animator.SetFloat("Move front", 1f);
                }
                else if (lastPos.z > transform.position.z)
                {
                    animator.SetFloat("Move front", -1f);
                }
                animator.SetFloat("Move left", 0);
            }
        }
        else
        {
            animator.SetFloat("Move front", 0);
            animator.SetFloat("Move left", 0);
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Idle01") && !stateInfo.IsName("To Idle02") && !stateInfo.IsName("Idle02") && !stateInfo.IsName("To Idle01") && animator.GetFloat("Move front") == 0 && animator.GetFloat("Move front") == 0 && !animator.GetBool("crouched"))
            animator.SetTrigger("idle");
        lastPos = transform.position;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!leftHandTarget || !rightHandTarget) return;

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
