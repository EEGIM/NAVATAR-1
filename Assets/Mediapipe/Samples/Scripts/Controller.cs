using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Specialized;

namespace Mediapipe.Unity
{
  public class Controller : ListAnnotation<PointAnnotation>
  {
    public LineRenderer line;
    public GameObject rupperarm; public GameObject rdownarm; public GameObject lupperarm; public GameObject ldownarm;
    public GameObject rupperleg; public GameObject rdownleg; public GameObject lupperleg; public GameObject ldownleg; // 여기 정리하기
    public GameObject upperbody; public GameObject hips; public GameObject spine2;
    public GameObject avatar;
    public RectTransform canvas;
    private Vector3[] Point = new Vector3[33];
    private float zPoint = 90f; //차후 수정 필요

    // Start is called before the first frame update
    void Start()
    {
    }

    void targetPosition(Vector3[] point)
    {
      Point = point;
    }

    // Update is called once per frame
    void Update()
    {
      targetPosition(PointListAnnotation.point);//pointAnnotation에서 캔버스에 맞게 바꿔준 좌표를 참조한다.
      Vector3 newPosition = new Vector3((Point[23].x + Point[24].x) / 2, (Point[23].y + Point[24].y) / 2 + 10.0f, zPoint); //캐릭터의 위치를 적절한 곳으로 지정 (골반)
      Vector3 newPosition2 = new Vector3((Point[11].x + Point[12].x) / 2, (Point[11].y + Point[12].y) / 2, zPoint); //(어깨 위치)
      avatar.transform.position = newPosition;

      float scale = Vector3.Magnitude(Point[11] - Point[12]) / 25; //몸 크기를 어깨 벡터 크기 비례로 (고쳐야 할 필요 있음... 어깨와 골반 둘다 비교를 한다던가...)
      float upbodyscale = Vector3.Magnitude(newPosition - newPosition2);

      if (-0.2 <= PointListAnnotation.shoulder && PointListAnnotation.shoulder <= 0.2)//특정 각도까지만 몸 크기를 바꾸도록... (없으면 어깨가 기울어질때 전체 사이즈가 같이 줄어듦)
      {
        avatar.transform.localScale = new Vector3(scale,scale, scale);
        if(0.0f < scale && scale <= 3.0f) //이 조건 없으면 에러뜸
        {
          upperbody.transform.localScale = new Vector3(1.1f, 1.10f * (upbodyscale / scale / (24.0f + (scale - 0.3f) * 4.2f)), 1.0f); //가까이있을땐 30나누고 멀땐 25(더 깔끔한 수정 필요)
          if(PointListAnnotation.dlegscale * 0.27f >= 1.2f || PointListAnnotation.legscale * 0.25f >= 0.80f) //다리 사이즈 조절
          {
            lupperleg.transform.localScale = new Vector3(lupperleg.transform.localScale.x, PointListAnnotation.legscale * 0.26f, 1.0f);
            rupperleg.transform.localScale = new Vector3(lupperleg.transform.localScale.x, PointListAnnotation.legscale * 0.26f, 1.0f);
            ldownleg.transform.localScale = new Vector3(1.0f, PointListAnnotation.dlegscale * 0.27f, 1.0f);
            rdownleg.transform.localScale = new Vector3(1.0f, PointListAnnotation.dlegscale * 0.27f, 1.0f);
          }
        }
      }
    }
  }

}
