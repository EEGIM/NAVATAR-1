// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


using mplt = Mediapipe.LocationData.Types;

namespace Mediapipe.Unity
{
  public class PointListAnnotation : ListAnnotation<PointAnnotation>
  {
    [SerializeField] private Color _color = Color.green;
    [SerializeField] private float _radius = 15.0f;
    public static float zPoint = 80f;
    public static Vector3[] point =new Vector3[33];
    public GameObject canvas;
    public float foot;
    public static float legscale;
    public GameObject cube;
    public GameObject cube2;
    public GameObject cube3;
    private void OnValidate()
    {
      ApplyColor(_color);
      ApplyRadius(_radius);
    }

    public void SetColor(Color color)
    {
      _color = color;
      ApplyColor(_color);
    }

    public void SetRadius(float radius)
    {
      _radius = radius;
      ApplyRadius(_radius);
    }

    public void Draw(IList<Vector3> targets)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target); }
        });
      }
    }

    public void Draw(IList<Landmark> targets, Vector3 scale, bool visualizeZ = true)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, scale, visualizeZ); }
        });
      }
    }

    public void Draw(LandmarkList targets, Vector3 scale, bool visualizeZ = true)
    {
      Draw(targets.Landmark, scale, visualizeZ);
    }

    public float GetAngle(Vector3 vec1, Vector3 vec2)
    {
      float theta = Vector3.Dot(vec1, vec2) / (vec1.magnitude * vec2.magnitude);
      Vector3 dirAngle = Vector3.Cross(vec1, vec2);
      float angle = Mathf.Acos(theta) * Mathf.Rad2Deg;
      if (dirAngle.z < 0.0f) angle = 360 - angle;
      //Debug.Log("사잇각 : " + angle);
      return angle;
    }

    void targetPosition(IList<NormalizedLandmark> a)
    {
      point [23] = new Vector3(a[23].X, a[23].Y, a[23].Z);
      point[24] = new Vector3(a[24].X, a[24].Y, a[24].Z);
      point[27] = new Vector3(a[27].X, a[27].Y, a[27].Z);
      float scale = Vector3.Magnitude(point[23] - point[24]);
      float lscale = Vector3.Magnitude(point[23] - point[27]);
      legscale = lscale / scale;
      for (int i = 0; i < 33; i++)
      {
        point[i] = new Vector3((0.5f - a[i].X) * 2440 * 0.04f, (0.5f - a[i].Y) * 1275 * 0.04f, zPoint); //좌표위치와 똑같이 수정, 캔버스 크기가 바뀌더라도 고치면 안됨.
                                                                                              //좌표의 위치를 결정짓는 실제 스크린 크기는 변하지 않으므로 
      }
      //if (a[17].Z * 40.0f <= -35.0f)
      //{
      //  point[17].z = zPoint + a[17].Z * 40.0f - a[11].Z * 40.0f;
      //  point[13].z = zPoint + a[13].Z * 40.0f - a[11].Z * 40.0f;
      //}
      //if (a[18].Z * 40.0f <= -40.0f)
      //{
      //  point[18].z = zPoint + a[18].Z * 40.0f - a[12].Z * 40.0f;
      //  point[14].z = zPoint + a[14].Z * 40.0f - a[12].Z * 40.0f;
      //}
      //point[29].z = foot;
      //point[30].z = foot;
      //point[13].z = foot;
      //point[14].z = foot;
      //point[15].z = 85 + (a[15].Z + 0.3f) * 40.0f;
      //point[16].z = 85 + (a[16].Z + 0.5f) * 30.0f;
      point[11].z = foot;
      point[12].z = foot;
      point[23].z = foot;
      point[24].z = foot;//팔다리 위치를 결정시켜줌으로 중요
      point[29].z = 85.0f;
      point[30].z = 85.0f;
      //cube.transform.localPosition = point[14];
      //cube2.transform.localPosition = point[18];
      //cube3.transform.localPosition = point[12];
      ////point[11].z = zPoint + a[11].Z * 40.0f;
      ////point[12].z = zPoint + a[12].Z * 40.0f;
      //point[24].z = a[24].Z;
      //point[26].z = a[26].Z;
      //point[28].z = a[28].Z;
    }

    public void Draw(IList<NormalizedLandmark> targets, bool visualizeZ = true)
    {
      targetPosition(targets);
      GameObject.FindWithTag("Player").SendMessage("targetPosition", point);

      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ); }
        });
      }
    }

    public void Draw(NormalizedLandmarkList targets, bool visualizeZ = true)
    {
      Draw(targets.Landmark, visualizeZ);
    }

    public void Draw(IList<AnnotatedKeyPoint> targets, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, focalLength, principalPoint, zScale, visualizeZ); }
        });
      }
    }

    public void Draw(IList<mplt.RelativeKeypoint> targets, float threshold = 0.0f)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, threshold); }
        });
      }
    }

    protected override PointAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetColor(_color);
      annotation.SetRadius(_radius);
      return annotation;
    }

    private void ApplyColor(Color color)
    {
      foreach (var point in children)
      {
        if (point != null) { point.SetColor(color); }
      }
    }

    private void ApplyRadius(float radius)
    {
      foreach (var point in children)
      {
        if (point != null) { point.SetRadius(radius); }
      }
    }
  }
}
