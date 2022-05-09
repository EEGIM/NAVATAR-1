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
    public GameObject avatar;
    [SerializeField, Range(0.5f, 1.5f)] private float ymul = 1.0f;
    [SerializeField, Range(0.5f, 1.5f)] private float ymul2 = 1.0f;
    private Vector3[] linePosition = new Vector3[4];
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

      Vector3 newPosition = new Vector3((Point[11].x + Point[12].x)/2, (Point[11].y + Point[12].y) / 2, zPoint);
      avatar.transform.position = newPosition;

      float scale = Vector3.Magnitude(Point[11] - Point[12]) / 55; //몸 크기를 어깨 벡터 크기 비례로

      Vector3 relativePos = (Point[11] - Point[12]);
      if (-0.2 <= relativePos.z && relativePos.z <= 0.2)//특정 각도까지만 몸 크기를 바꾸도록... 
         avatar.transform.localScale = new Vector3(1.5f * (scale) , 1.0f * (scale), 1.5f * (scale));

      float scale2 = (Vector3.Magnitude(Point[12] - Point[14]) + Vector3.Magnitude(Point[14] - Point[18])) / 120;
      rupperarm.transform.localScale = new Vector3(1.0f, uplegscale * scale2, 1.0f);
      lupperarm.transform.localScale = new Vector3(1.0f, uplegscale * scale2, 1.0f);
    }
  }

}
