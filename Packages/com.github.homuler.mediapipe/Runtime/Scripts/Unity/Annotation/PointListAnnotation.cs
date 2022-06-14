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
    public static float zPoint = 90f;
    public static Vector3[] point =new Vector3[33];
    public GameObject canvas;
    public float foot;
    public static float legscale;
    public static float armscale;
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
      return angle;
    }

    void targetPosition(IList<NormalizedLandmark> a)
    {
      point[23] = new Vector3(a[23].X, a[23].Y, a[23].Z); point[24] = new Vector3(a[24].X, a[24].Y, a[24].Z); point[27] = new Vector3(a[27].X, a[27].Y, a[27].Z);
      point[11] = new Vector3(a[11].X, a[11].Y, a[11].Z); point[13] = new Vector3(a[13].X, a[13].Y, a[13].Z); point[15] = new Vector3(a[15].X, a[15].Y, a[15].Z);
      point[12] = new Vector3(a[12].X, a[12].Y, a[12].Z);
      legscale = Vector3.Magnitude(point[23] - point[27]) / Vector3.Magnitude(point[23] - point[24]);
      armscale = (Vector3.Magnitude(point[15] - point[13]) + Vector3.Magnitude(point[11] - point[13])) / Vector3.Magnitude(point[11] - point[12]);
      for (int i = 0; i < 33; i++)
      {
        point[i] = new Vector3((0.5f - a[i].X) * 2440 * 0.04f, (0.5f - a[i].Y) * 1275 * 0.04f, zPoint); //좌표위치와 똑같이 수정, 캔버스 크기가 바뀌더라도 고치면 안됨.
                                                                                              //좌표의 위치를 결정짓는 실제 스크린 크기는 변하지 않으므로 
      }
      Debug.Log("point[15]:" + point[15]);
      for (int i = 15; i < 23; i++)
      {
        point[i].z = zPoint + (25.0f * (a[i].Z + 0.3f));//z값 받아서 그것을 화면에 반영했을때 이상적인 비율 곱함
      }
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
