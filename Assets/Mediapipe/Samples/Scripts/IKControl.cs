using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{

  protected Animator animator;
  public bool ikActive = false;

  private Vector3[] point = new Vector3[33];
  void Start()
  {
    animator = GetComponent<Animator>();
  }

  void targetPosition(Vector3[] a)
  {
    point = a;
    // 0: lefthand, 1: righthand, 2: leftelbow, 3:rightElbow
  }

  //a callback for calculating IK
  void OnAnimatorIK()
  {
    if (animator)
    {

      //if the IK is active, set the position and rotation directly to the goal. 
      if (ikActive)
      {
        if (point[15] != null && point[16] != null)
        {
          animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
          //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
          animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
          //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
          //animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
          //animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
          animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
          animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);
          animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
          animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

          animator.SetIKPosition(AvatarIKGoal.RightHand, point[15]);
          //animator.SetIKRotation(AvatarIKGoal.RightHand, point[0].rotation);
          animator.SetIKPosition(AvatarIKGoal.LeftHand, point[16]);
          //animator.SetIKRotation(AvatarIKGoal.LeftHand, point[1].rotation);
          //animator.SetIKHintPosition(AvatarIKHint.RightElbow, point[13]);
          //animator.SetIKHintPosition(AvatarIKHint.LeftElbow, point[14]);
          animator.SetIKHintPosition(AvatarIKHint.RightKnee, point[25]);
          animator.SetIKHintPosition(AvatarIKHint.LeftKnee, point[26]);
          animator.SetIKPosition(AvatarIKGoal.RightFoot, point[29]);
          animator.SetIKPosition(AvatarIKGoal.LeftFoot, point[30]);

          Vector3 relativePos = (point[12]- point[11]);//어깨선 기준으로 vector값 추출
          //Debug.Log("relativePos.z:" + relativePos.z * (-1.0f));
          Quaternion rotation = Quaternion.Euler(new Vector3(0, 180 + relativePos.z * (-1.0f), 0));
          animator.bodyRotation = rotation;//몸통 돌리기
        }

      }

      //if the IK is not active, set the position and rotation of the hand and head back to the original position
      else
      {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        animator.SetLookAtWeight(0);
      }
    }
  }
}
