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
    public RectTransform annotation;
    public float foot;
    public static float legscale;
    public static float dlegscale;
    public static float armscale;
    public static float shoulder;
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
      point[23] = new Vector3(a[23].X, a[23].Y, a[23].Z); point[24] = new Vector3(a[24].X, a[24].Y, a[24].Z); point[25] = new Vector3(a[25].X, a[25].Y, a[25].Z); point[27] = new Vector3(a[27].X, a[27].Y, a[27].Z);
      point[11] = new Vector3(a[11].X, a[11].Y, a[11].Z); point[13] = new Vector3(a[13].X, a[13].Y, a[13].Z); point[15] = new Vector3(a[15].X, a[15].Y, a[15].Z);
      point[12] = new Vector3(a[12].X, a[12].Y, a[12].Z);
      Vector3 relativePos = (point[12] - point[11]);
      shoulder = relativePos.z;

      //Debug.Log("어깨너비: " + Vector3.Magnitude(point[11] - point[12]) + "골반너비: " + Vector3.Magnitude(point[23] - point[24]));

      legscale = Vector3.Magnitude(point[23] - point[25]) / Vector3.Magnitude(point[23] - point[24]);
      dlegscale = Vector3.Magnitude(point[25] - point[27]) / Vector3.Magnitude(point[23] - point[24]);
      armscale = (Vector3.Magnitude(point[15] - point[13]) + Vector3.Magnitude(point[11] - point[13])) / Vector3.Magnitude(point[11] - point[12]);
      for (int i = 0; i < 33; i++)
      {
        point[i] = new Vector3((0.5f - a[i].X) * annotation.rect.width * 0.04f, (0.5f - a[i].Y) * annotation.rect.height * 0.04f, zPoint); //좌표위치와 똑같이 수정, 캔버스 크기가 바뀌더라도 고치면 안됨.
                                                                                              //좌표의 위치를 결정짓는 실제 스크린 크기는 변하지 않으므로 
      }
      point[11].z -= shoulder;
      point[12].z += shoulder;
      for (int i = 13; i < 23; i++)
      {
        if(a[i].Z + 0.3f >= -5.0f)
        {
          point[i].z = 95.0f + (50.0f * (a[i].Z + 0.3f));//z값 받아서 그것을 화면에 반영했을때 이상적인 비율 곱함 (scale별로 다르게 설정해야...)
        }
      }
      for (int i = 23; i < 33; i++)
      {
        point[i].z = foot;
        if (i >= 27 && i % 2 == 1)
        {
          point[i].x -= 0.5f;
          point[i + 1].x += 0.5f;
        }
      }
      //Vector3 newPosition = new Vector3((point[23].x + point[24].x) / 2, (point[23].y + point[24].y) / 2, zPoint);
      //Vector3 newPosition2 = new Vector3((point[11].x + point[12].x) / 2, (point[11].y + point[12].y) / 2, zPoint);
      //Debug.Log("골반너비 * 2 / 어깨너비:" + Vector3.Magnitude(point[23] - point[24]) * 2 / Vector3.Magnitude(point[11] - point[12]));
      //Debug.Log("세로너비/ 가로너비:" + Vector3.Magnitude(newPosition - newPosition2) / Vector3.Magnitude(point[11] - point[12]));
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
