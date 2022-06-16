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
    public GameObject rupperleg; public GameObject rdownleg; public GameObject lupperleg; public GameObject ldownleg;
    public GameObject upperbody; public GameObject hips; public GameObject spine2;
    public GameObject avatar;
    private static float legscale;
    private static float armscale;
    private Vector3[] Point = new Vector3[33];
    private float zPoint = 90f;

    // Start is called before the first frame update
    void Start()
    {
    }

    void targetPosition(Vector3[] a)
    {
      Point = a;
    }

    // Update is called once per frame
    void Update()
    {
      targetPosition(PointListAnnotation.point);
      armscale = PointListAnnotation.armscale / 3.19f;

      Vector3 newPosition = new Vector3((Point[23].x + Point[24].x)/2, (Point[23].y + Point[24].y) / 2, zPoint);
      Vector3 newPosition2 = new Vector3((Point[11].x + Point[12].x) / 2, (Point[11].y + Point[12].y) / 2, zPoint);
      avatar.transform.position = newPosition;

      float scale = Vector3.Magnitude(Point[11] - Point[12]) / 25; //몸 크기를 어깨 벡터 크기 비례로
      float hipscale = Vector3.Magnitude(Point[23] - Point[24]) / scale; //몸 크기를 어깨 벡터 크기 비례로
      //Debug.Log("scale/hipscale:" + Vector3.Magnitude(Point[11] - Point[12]) / Vector3.Magnitude(Point[23] - Point[24]));
      float urscale = Vector3.Magnitude(Point[11] - Point[13]) / 25;
      float drscale = Vector3.Magnitude(Point[15] - Point[13]) / 25;
      float upbodyscale = Vector3.Magnitude(newPosition - newPosition2);

      if (-0.2 <= PointListAnnotation.shoulder && PointListAnnotation.shoulder <= 0.2)//특정 각도까지만 몸 크기를 바꾸도록... 
      {
        avatar.transform.localScale = new Vector3(scale,scale, scale);
        if(0.0f < scale && scale <= 3.0f) //이 조건 없으면 에러뜸
        {
          upperbody.transform.localScale = new Vector3(1.1f, 1.10f * (upbodyscale / scale / (26.0f + (scale - 0.5f) * 4.0f)), 1.0f); //가까이있을땐 30나누고 멀땐 25
          //hips.transform.localScale = new Vector3(16.4f + hipscale / 3.73f, hips.transform.localScale.y, hips.transform.localScale.y);
          lupperleg.transform.localScale = new Vector3(1.0f, PointListAnnotation.legscale * 0.25f, 1.0f);
          rupperleg.transform.localScale = new Vector3(1.0f, PointListAnnotation.legscale * 0.25f, 1.0f);
          if(PointListAnnotation.dlegscale * 0.27f >= 1.2f)
          {
            ldownleg.transform.localScale = new Vector3(1.0f, PointListAnnotation.dlegscale * 0.27f, 1.0f);
            rdownleg.transform.localScale = new Vector3(1.0f, PointListAnnotation.dlegscale * 0.27f, 1.0f);
          }
          lupperarm.transform.localScale = new Vector3(1.0f, armscale, 1.0f);
          rupperarm.transform.localScale = new Vector3(1.0f, armscale, 1.0f);
          //ldownarm.transform.localScale = new Vector3(1.0f, 1.54f * (drscale / scale), 1.0f);
          //rdownarm.transform.localScale = new Vector3(1.0f, 1.54f * (drscale / scale), 1.0f);
        }
      }
    }
  }

}
