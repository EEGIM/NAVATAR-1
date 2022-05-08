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

    private Vector3 midPoint;
    private Vector3 wristPoint;//이것도 point로 바꾸기..
    private Vector3 bodyPoint;
    private Vector3 Point1;
    private Vector3 Point3;
    private Vector3 Point8;
    private Vector3 Point9;

    private float zPoint = 85.0f;
    float uplegscale;

    // Start is called before the first frame update
    void Start()
    {
      uplegscale = rupperarm.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
      midPoint = PointListAnnotation.midPoint;
      wristPoint = PointListAnnotation.wristPoint;
      bodyPoint = PointListAnnotation.bodyPoint;
      Point1 = PointListAnnotation.point[1];
      Point3 = PointListAnnotation.point[3];
      Point8 = PointListAnnotation.point[8];
      Point9 = PointListAnnotation.point[9];//좌표 한번에 다 가져오는걸로 수정
      

    Vector3 newPosition = new Vector3(bodyPoint.x * 2440 * 0.08f, bodyPoint.y * 1373 * 0.08f, zPoint);
      avatar.transform.position = newPosition;

      float scale = Vector3.Magnitude(wristPoint - midPoint) * 2440 / 700; //몸 크기를 어깨 벡터 크기 비례로

      Vector3 relativePos = (Point9 - Point8);
      if (-0.2 <= relativePos.z && relativePos.z <= 0.2)
         avatar.transform.localScale = new Vector3(1.5f * (scale) , 1.0f * (scale), 1.5f * (scale));

      float scale2 = (Vector3.Magnitude(Point9 - Point3) + Vector3.Magnitude(Point3 - Point1)) / 120;
      rupperarm.transform.localScale = new Vector3(1.0f, uplegscale * scale2, 1.0f);
      lupperarm.transform.localScale = new Vector3(1.0f, uplegscale * scale2, 1.0f);
    }
  }

}
