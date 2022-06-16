using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{

  protected Animator animator;
  public bool ikActive = false;
  public GameObject cube;
    public GameObject cube2;
    public GameObject cube3;
  float z;


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
          //sanimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
          animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
          animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
          //animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
          //animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);
          animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
          animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

          Vector2 v2 = new Vector2(point[15].x, point[15].y) - new Vector2(point[13].x, point[13].y);
          Vector2 v22 = new Vector2(point[16].x, point[16].y) - new Vector2(point[14].x, point[14].y);
          float result = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;//손과 팔꿈치 사이 각도..
          float result2 = Mathf.Atan2(v22.y, v22.x) * Mathf.Rad2Deg;
          animator.SetIKPosition(AvatarIKGoal.RightHand, new Vector3(point[13].x + 10.0f * Mathf.Cos(result * Mathf.PI / 180),
            (point[13].y - 10.0f) + 10.0f * Mathf.Sin(result * Mathf.PI / 180), 100.0f));
          animator.SetIKPosition(AvatarIKGoal.LeftHand, new Vector3(point[14].x + 10.0f * Mathf.Cos(result2 * Mathf.PI / 180),
            (point[14].y - 10.0f) + 10.0f * Mathf.Sin(result2 * Mathf.PI / 180), 104.0f));
          //animator.SetIKPosition(AvatarIKGoal.LeftHand, cube.transform.position);
          //animator.SetIKRotation(AvatarIKGoal.LeftHand, point[16]);
          animator.SetIKHintPosition(AvatarIKHint.RightElbow, new Vector3(point[13].x, point[13].y - 10.0f, 105.0f));
          //animator.SetIKHintPosition(AvatarIKHint.RightElbow, cube2.transform.position);
          //animator.SetIKHintPosition(AvatarIKHint.LeftElbow, cube2.transform.position);
          animator.SetIKHintPosition(AvatarIKHint.LeftElbow, new Vector3(point[14].x, point[14].y - 10.0f, 105.0f));
          //animator.SetIKHintPosition(AvatarIKHint.RightKnee, point[25]);
          //animator.SetIKHintPosition(AvatarIKHint.LeftKnee, point[26]);
          animator.SetIKPosition(AvatarIKGoal.RightFoot, point[29]);
          animator.SetIKPosition(AvatarIKGoal.LeftFoot, point[30]);

          cube.transform.position = new Vector3(point[14].x + 10.0f * Mathf.Cos(result2 * Mathf.PI / 180),
            (point[14].y - 10.0f) + 10.0f * Mathf.Sin(result2 * Mathf.PI / 180), 104.0f);
          cube2.transform.position = new Vector3(point[14].x, point[14].y - 10.0f, 105.0f);

          Vector3 relativePos = (point[12] - point[11]);//어깨선 기준으로 vector값 추출
          Quaternion rotation = Quaternion.Euler(new Vector3(0, 180 + relativePos.z * (-25.0f), 0));
          animator.bodyRotation = rotation;//몸통 돌리기
        }

      }

      //if the IK is not active, set the position and rotation of the hand and head back to the original position
      //else
      //{
      //  animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
      //  animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
      //  animator.SetLookAtWeight(0);
      //}
    }
  }
}
