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
    public GameObject upperarm;
    public GameObject downarm;
    public GameObject avatar;
    [SerializeField, Range(0.5f, 1.5f)] private float ymul = 1.0f;
    [SerializeField, Range(0.5f, 1.5f)] private float ymul2 = 1.0f;
    private Vector3[] linePosition = new Vector3[4];

    private Vector3 midPoint;
    private Vector3 wristPoint;
    private Vector3 bodyPoint;
    private Vector3 Point1;
    private Vector3 Point2;
    private Vector3 Point3;

    private float zPoint = 85.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      midPoint = PointListAnnotation.midPoint;
      wristPoint = PointListAnnotation.wristPoint;
      bodyPoint = PointListAnnotation.bodyPoint;
      Point1 = PointListAnnotation.point[0];
      Point2 = PointListAnnotation.point[1];
      Point3 = PointListAnnotation.point[2];

      Vector3 newPosition = new Vector3(bodyPoint.x * 2440 * 0.08f, bodyPoint.y * 1373 * 0.08f, zPoint);
      avatar.transform.position = newPosition;

      float scale = Vector3.Magnitude(wristPoint - midPoint) * 2440 / 700; //몸 크기를 어깨 벡터 크기 비례로
      //Debug.Log("팔:" + Vector3.Magnitude(Point1 - Point2) + "햄스워스:" + Vector3.Magnitude(Point2 - Point3));
      avatar.transform.localScale = new Vector3(1.5f * (scale) , 1.0f * (scale), 1.5f * (scale));
      upperarm.transform.localScale = new Vector3(1.0f, 1.0f * (ymul), 1.0f);
      downarm.transform.localScale = new Vector3(1.0f, 1.0f * (ymul2), 1.0f);
    }
  }

}
