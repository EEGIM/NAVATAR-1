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
    public static Vector3 midPoint;
    public static Vector3 wristPoint;
    public static Vector3 bodyPoint;
    public static float zPoint = 85f;
    public static Vector3[] point =new Vector3[12];

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

    public void Draw(IList<NormalizedLandmark> targets, bool visualizeZ = true)
    {
      midPoint = new Vector3(0.5f - targets[11].X , 0.5f - targets[11].Y, 0);
      wristPoint = new Vector3(0.5f - targets[12].X, 0.5f - targets[12].Y, 0);
      bodyPoint = new Vector3(0.5f - ((targets[11].X + targets[12].X) / 2) , 0.5f - ((targets[11].Y + targets[12].Y) / 2), zPoint);

      point = new Vector3[12];
      Debug.Log("leftHand:" + targets[17].Z + "rightHand:" +targets[18].Z);
      point[0] = new Vector3((0.5f - targets[17].X) * 2440 * 0.08f, (0.5f - targets[17].Y) * 1373 * 0.08f, zPoint);
      point[1] = new Vector3((0.5f - targets[18].X) * 2440 * 0.08f, (0.5f - targets[18].Y) * 1373 * 0.08f, zPoint);
      point[2] = new Vector3((0.5f - targets[13].X) * 2440 * 0.08f, (0.5f - targets[13].Y) * 1373 * 0.08f, zPoint);
      point[3] = new Vector3((0.5f - targets[14].X) * 2440 * 0.08f, (0.5f - targets[14].Y) * 1373 * 0.08f, zPoint);
      point[4] = new Vector3((0.5f - targets[25].X) * 2440 * 0.08f, (0.5f - targets[25].Y) * 1373 * 0.08f, zPoint);
      point[5] = new Vector3((0.5f - targets[26].X) * 2440 * 0.08f, (0.5f - targets[26].Y) * 1373 * 0.08f, zPoint);
      point[6] = new Vector3((0.5f - targets[27].X) * 2440 * 0.08f, (0.5f - targets[27].Y) * 1373 * 0.08f, zPoint);
      point[7] = new Vector3((0.5f - targets[28].X) * 2440 * 0.08f, (0.5f - targets[28].Y) * 1373 * 0.08f, zPoint);
      point[8] = new Vector3((0.5f - targets[11].X) * 2440 * 0.08f, (0.5f - targets[11].Y) * 2440 * 0.08f, 0.5f - targets[11].Z);
      point[9] = new Vector3((0.5f - targets[12].X) * 2440 * 0.08f, (0.5f - targets[12].Y) * 2440 * 0.08f, 0.5f - targets[12].Z);
      point[10] = new Vector3((0.5f - targets[23].X) * 2440 * 0.08f, (0.5f - targets[23].Y) * 1373 * 0.08f, zPoint);
      point[11] = new Vector3((0.5f - targets[24].X) * 2440 * 0.08f, (0.5f - targets[24].Y) * 1373 * 0.08f, zPoint);

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
