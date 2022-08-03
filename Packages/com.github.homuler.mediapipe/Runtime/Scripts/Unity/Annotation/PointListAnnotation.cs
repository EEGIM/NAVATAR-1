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
    public static float zPoint = 90f; //차후 수정 필요
    public static Vector3[] point =new Vector3[33];
    public GameObject canvas;
    public RectTransform annotation;
    public float foot; //수정필요
    public static float legscale;
    public static float dlegscale;
    public static float armscale;
    public static float shoulder;
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

    public Vector3 FloattoVect(float X, float Y, float Z)
    {
      Vector3 vector;
      vector = new Vector3(X, Y, Z);
      return vector;
    }

    void targetPosition(IList<NormalizedLandmark> a)
    {
      for (int i = 0; i < 33; i++)
      {
        point[i] = FloattoVect(a[i].X, a[i].Y, a[i].Z);//a의 형식을 Vector3 리스트인 point로 고침
      }
      shoulder = (point[12] - point[11]).z;//IK Control 에서 어깨 회전 하기 위해 z축을 살리는 작업.. 

      legscale = Vector3.Magnitude(point[23] - point[25]) / Vector3.Magnitude(point[23] - point[24]); //Controller.cs에서 몸 scale을 조절하기 위해 필요한 float값..
      dlegscale = Vector3.Magnitude(point[25] - point[27]) / Vector3.Magnitude(point[23] - point[24]);
      //armscale = (Vector3.Magnitude(point[15] - point[13]) + Vector3.Magnitude(point[11] - point[13])) / Vector3.Magnitude(point[11] - point[12]);

      for (int i = 0; i < 33; i++)
      {
        point[i] = new Vector3((0.5f - a[i].X) * annotation.rect.width * 0.04f, (0.5f - a[i].Y) * annotation.rect.height * 0.04f, zPoint); //좌표위치와 똑같이 수정, 캔버스 크기가 바뀌더라도 고치면 안됨
      }
      point[11].z -= shoulder;
      point[12].z += shoulder;//어깨회전 z축 좌표 조정

      for (int i = 13; i < 23; i++) //차후 수정 필요 (103~ 116)
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
