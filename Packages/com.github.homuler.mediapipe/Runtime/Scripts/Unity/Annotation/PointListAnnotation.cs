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
    public static float zPoint = 85f;
    public GameObject cube;
    public GameObject cube2;
    public static Vector3[] point =new Vector3[33];

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
      for (int i = 0; i < 33; i++)
      {
        point[i] = new Vector3((0.5f - a[i].X) * 2440 * 0.08f, (0.5f - a[i].Y) * 1373 * 0.08f, zPoint); //좌표위치와 똑같이 수정
      }
      point[17].z = zPoint + a[17].Z * 2000 * 0.08f;
      point[18].z = zPoint + a[18].Z * 2000 * 0.08f;
      point[13].z = zPoint + a[13].Z * 1000 * 0.08f;
      point[14].z = zPoint + a[14].Z * 1000 * 0.08f;
      point[11].z = zPoint + a[11].Z * 1000 * 0.08f;
      point[12].z = zPoint + a[12].Z * 1000 * 0.08f;
      //cube.transform.localPosition = point[11];
      //cube2.transform.localPosition = point[12];
      point[24].z = a[24].Z;
      point[26].z = a[26].Z;
      point[28].z = a[28].Z;
    }

    public void Draw(IList<NormalizedLandmark> targets, bool visualizeZ = true)
    {
      targetPosition(targets);//targets을 좌표위치에맞게 형식변환
      float angle = GetAngle(point[16] - point[14], point[12] - point[14]);
      if (angle >= 0)
      {
        for(int i = 13; i <= 16; i++)
          point[i].z = zPoint;
      }
      GameObject.FindWithTag("Player").SendMessage("targetPosition", point);//연산량 많아지니까 사용하지말기.

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
