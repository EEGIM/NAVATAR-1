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
    public GameObject rupperarm;
    public GameObject rdownarm;
    public GameObject lupperarm;
    public GameObject ldownarm;
    public GameObject upperbody;
    public GameObject avatar;
    private Vector3[] Point = new Vector3[33];

    private float zPoint = 85.0f;
    float uplegscale;

    // Start is called before the first frame update
    void Start()
    {
      uplegscale = rupperarm.transform.localScale.y;
    }

    void targetPosition(Vector3[] a)
    {
      Point = a;
    }

    // Update is called once per frame
    void Update()
    {
      targetPosition(PointListAnnotation.point);

      Vector3 newPosition = new Vector3((Point[23].x + Point[24].x)/2, (Point[23].y + Point[24].y) / 2, zPoint);
      Vector3 newPosition2 = new Vector3((Point[11].x + Point[12].x) / 2, (Point[11].y + Point[12].y) / 2, zPoint);
      avatar.transform.position = newPosition;

      float scale = Vector3.Magnitude(Point[11] - Point[12]) / 55; //몸 크기를 어깨 벡터 크기 비례로
      float upbodyscale = Vector3.Magnitude(newPosition - newPosition2) /70;
      float spinescale;

      Vector3 relativePos = (Point[12] - Point[11]);
      if (-10 <= relativePos.z && relativePos.z <= 10)//특정 각도까지만 몸 크기를 바꾸도록... 
      {
        avatar.transform.localScale = new Vector3(1.5f * (scale), 1.0f * (scale), 1.5f * (scale));
        if (1.0f <= avatar.transform.localScale.y && avatar.transform.localScale.y <= 2.0f)
        {
          spinescale = (upbodyscale + 0.3f) / avatar.transform.localScale.y; // 아바타 크기 * 허리크기 >= 좌표상의상체길이로 허리길이를 맞춤
          upperbody.transform.localScale = new Vector3(1.0f, 1.0f * spinescale, 1.0f);
        }
      }


      float scale2 = (Vector3.Magnitude(Point[12] - Point[14]) + Vector3.Magnitude(Point[14] - Point[18])) / 120;
      //rupperarm.transform.localScale = new Vector3(1.0f, uplegscale * scale2, 1.0f);
      lupperarm.transform.localScale = new Vector3(1.0f, uplegscale * scale2, 1.0f);
    }
  }

}
