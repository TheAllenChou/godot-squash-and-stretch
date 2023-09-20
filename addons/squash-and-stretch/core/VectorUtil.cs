using Godot;

namespace SquashAndStretchKit
{
  public static class VectorUtil
  {
    public static readonly Vector3 Min = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    public static readonly Vector3 Max = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    
    public static Vector3 Rotate2D(Vector3 v, float angle)
    {
      Vector3 results = v;
      float cos = Mathf.Cos(angle);
      float sin = Mathf.Sin(angle);
      results.X = cos * v.X - sin * v.Y;
      results.Y = sin * v.X + cos * v.Y;
      return results;
    }

    public static Vector2 SafeNormalize(Vector2 v, Vector2 fallback)
    {
      return
        v.LengthSquared() > MathUtil.Epsilon
        ? v.Normalized()
        : fallback;
    }

    public static Vector3 SafeNormalize(Vector3 v, Vector3 fallback)
    {
      return
        v.LengthSquared() > MathUtil.Epsilon 
        ? v.Normalized() 
        : fallback;
    }
    
    public static Vector4 SafeNormalize(Vector4 v, Vector4 fallback)
    {
      return
        v.LengthSquared() > MathUtil.Epsilon 
        ? v.Normalized() 
        : fallback;
    }

    // Returns a vector orthogonal to given vector.
    // If the given vector is a unit vector, the returned vector will also be a unit vector.
    public static Vector3 FindOrthogonal(Vector3 v)
    {
      if (v.X >= MathUtil.Sqrt3Inv)
        return new Vector3(v.Y, -v.X, 0.0f);
      else
        return new Vector3(0.0f, v.Z, -v.Y);
    }

    // Yields two extra vectors that form an orthogonal basis with the given vector.
    // If the given vector is a unit vector, the returned vectors will also be unit vectors.
    public static void FormOrthogonalBasis(Vector3 v, out Vector3 a, out Vector3 b)
    {
      a = FindOrthogonal(v);
      b = a.Cross(v);
    }

    // Both vectors must be unit vectors.
    public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
    {
      float dot = a.Dot(b);

      if (dot > 0.99999f)
      {
        // singularity: two vectors point in the same direction
        return a.Lerp(b, t);
      }
      else if (dot < -0.99999f)
      {
        // singularity: two vectors point in the opposite direction
        Vector3 axis = FindOrthogonal(a);
        return new Quaternion(axis, MathUtil.Pi * t) * a;
      }

      float rad = MathUtil.AcosSafe(dot);
      return (Mathf.Sin((1.0f - t) * rad) * a + Mathf.Sin(t * rad) * b) / Mathf.Sin(rad);
    }

    public static Vector3 GetClosestPointOnSegment(Vector3 p, Vector3 segA, Vector3 segB)
    {
      Vector3 v = segB - segA;
      if (v.LengthSquared() < MathUtil.Epsilon)
        return 0.5f * (segA + segB);

      float d = MathUtil.Saturate((p - segA).Dot(v.Normalized()) / v.Length());
      return segA + d * v;
    }

    public static Vector3 TriLerp
    (
      ref Vector3 v000, ref Vector3 v001, ref Vector3 v010, ref Vector3 v011, 
      ref Vector3 v100, ref Vector3 v101, ref Vector3 v110, ref Vector3 v111, 
      float tx, float ty, float tz
    )
    {
      Vector3 lerpPosY00 = v000.Lerp(v001, tx);
      Vector3 lerpPosY10 = v010.Lerp(v011, tx);
      Vector3 lerpPosY01 = v100.Lerp(v101, tx);
      Vector3 lerpPosY11 = v110.Lerp(v111, tx);
      Vector3 lerpPosZ0 = lerpPosY00.Lerp(lerpPosY10, ty);
      Vector3 lerpPosZ1 = lerpPosY01.Lerp(lerpPosY11, ty);
      return lerpPosZ0.Lerp(lerpPosZ1, tz);
    }

    public static Vector3 TriLerp
    (
      ref Vector3 v000, ref Vector3 v001, ref Vector3 v010, ref Vector3 v011, 
      ref Vector3 v100, ref Vector3 v101, ref Vector3 v110, ref Vector3 v111,
      bool lerpX, bool lerpY, bool lerpZ,
      float tx, float ty, float tz
    )
    {
      Vector3 lerpPosY00 = lerpX ? v000.Lerp(v001, tx) : v000;
      Vector3 lerpPosY10 = lerpX ? v010.Lerp(v011, tx) : v010;
      Vector3 lerpPosY01 = lerpX ? v100.Lerp(v101, tx) : v100;
      Vector3 lerpPosY11 = lerpX ? v110.Lerp(v111, tx) : v110;
      Vector3 lerpPosZ0 = lerpY ? lerpPosY00.Lerp(lerpPosY10, ty) : lerpPosY00;
      Vector3 lerpPosZ1 = lerpY ? lerpPosY01.Lerp(lerpPosY11, ty) : lerpPosY01;
      return lerpZ ? lerpPosZ0.Lerp(lerpPosZ1, tz) : lerpPosZ0;
    }

    public static Vector3 TriLerp
    (
      ref Vector3 min, ref Vector3 max, 
      bool lerpX, bool lerpY, bool lerpZ, 
      float tx, float ty, float tz
    )
    {
      Vector3 lerpPosY00 =
        lerpX
        ? (new Vector3(min.X, min.Y, min.Z)).Lerp(new Vector3(max.X, min.Y, min.Z), tx)
        : new Vector3(min.X, min.Y, min.Z);

      Vector3 lerpPosY10 =
        lerpX
        ? (new Vector3(min.X, max.Y, min.Z)).Lerp(new Vector3(max.X, max.Y, min.Z), tx)
        : new Vector3(min.X, max.Y, min.Z);

      Vector3 lerpPosY01 =
        lerpX
        ? (new Vector3(min.X, min.Y, max.Z)).Lerp(new Vector3(max.X, min.Y, max.Z), tx)
        : new Vector3(min.X, min.Y, max.Z);

      Vector3 lerpPosY11 =
        lerpX
        ? (new Vector3(min.X, max.Y, max.Z)).Lerp(new Vector3(max.X, max.Y, max.Z), tx)
        : new Vector3(min.X, max.Y, max.Z);

      Vector3 lerpPosZ0 =
        lerpY
        ? lerpPosY00.Lerp(lerpPosY10, ty)
        : lerpPosY00;

      Vector3 lerpPosZ1 =
        lerpY
        ? lerpPosY01.Lerp(lerpPosY11, ty)
        : lerpPosY01;

      return lerpZ ? lerpPosZ0.Lerp(lerpPosZ1, tz) : lerpPosZ0;
    }

    public static Vector4 TriLerp
    (
      ref Vector4 v000, ref Vector4 v001, ref Vector4 v010, ref Vector4 v011, 
      ref Vector4 v100, ref Vector4 v101, ref Vector4 v110, ref Vector4 v111,
      bool lerpX, bool lerpY, bool lerpZ,
      float tx, float ty, float tz
    )
    {
      Vector4 lerpPosY00 = lerpX ? v000.Lerp(v001, tx) : v000;
      Vector4 lerpPosY10 = lerpX ? v010.Lerp(v011, tx) : v010;
      Vector4 lerpPosY01 = lerpX ? v100.Lerp(v101, tx) : v100;
      Vector4 lerpPosY11 = lerpX ? v110.Lerp(v111, tx) : v110;
      Vector4 lerpPosZ0 = lerpY ? lerpPosY00.Lerp(lerpPosY10, ty) : lerpPosY00;
      Vector4 lerpPosZ1 = lerpY ? lerpPosY01.Lerp(lerpPosY11, ty) : lerpPosY01;
      return lerpZ ? lerpPosZ0.Lerp(lerpPosZ1, tz) : lerpPosZ0;
    }

    public static Vector4 TriLerp
    (
      ref Vector4 min, ref Vector4 max, 
      bool lerpX, bool lerpY, bool lerpZ, 
      float tx, float ty, float tz
    )
    {
      Vector4 lerpPosY00 =
        lerpX
        ? (new Vector4(min.X, min.Y, min.Z, 0.0f)).Lerp(new Vector4(max.X, min.Y, min.Z, 0.0f), tx)
        : new Vector4(min.X, min.Y, min.Z, 0.0f);

      Vector4 lerpPosY10 =
        lerpX
        ? (new Vector4(min.X, max.Y, min.Z, 0.0f)).Lerp(new Vector4(max.X, max.Y, min.Z, 0.0f), tx)
        : new Vector4(min.X, max.Y, min.Z, 0.0f);

      Vector4 lerpPosY01 =
        lerpX
        ? (new Vector4(min.X, min.Y, max.Z, 0.0f)).Lerp(new Vector4(max.X, min.Y, max.Z, 0.0f), tx)
        : new Vector4(min.X, min.Y, max.Z, 0.0f);

      Vector4 lerpPosY11 =
        lerpX
        ? (new Vector4(min.X, max.Y, max.Z, 0.0f)).Lerp(new Vector4(max.X, max.Y, max.Z, 0.0f), tx)
        : new Vector4(min.X, max.Y, max.Z, 0.0f);

      Vector4 lerpPosZ0 =
        lerpY
        ? lerpPosY00.Lerp(lerpPosY10, ty)
        : lerpPosY00;

      Vector4 lerpPosZ1 =
        lerpY
        ? lerpPosY01.Lerp(lerpPosY11, ty)
        : lerpPosY01;

      return lerpZ ? lerpPosZ0.Lerp(lerpPosZ1, tz) : lerpPosZ0;
    }

    public static Vector3 ClampLength(Vector3 v, float minLen, float maxLen)
    {
      float lenSqr = v.LengthSquared();
      if (lenSqr < MathUtil.Epsilon)
        return v;

      float len = Mathf.Sqrt(lenSqr);
      return v * (Mathf.Clamp(len, minLen, maxLen) / len);
    }

    public static float MinComponent(Vector3 v)
    {
      return Mathf.Min(v.X, Mathf.Min(v.Y, v.Z));
    }

    public static float MaxComponent(Vector3 v)
    {
      return Mathf.Max(v.X, Mathf.Max(v.Y, v.Z));
    }

    public static Vector3 ComponentWiseAbs(Vector3 v)
    {
      return new Vector3(Mathf.Abs(v.X), Mathf.Abs(v.Y), Mathf.Abs(v.Z));
    }

    public static Vector3 ComponentWiseMult(Vector3 a, Vector3 b)
    {
      return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }

    public static Vector3 ComponentWiseDiv(Vector3 num, Vector3 den)
    {
      return new Vector3(num.X / den.X, num.Y / den.Y, num.Z / den.Z);
    }

    public static Vector3 ComponentWiseDivSafe(Vector3 num, Vector3 den)
    {
      return 
        new Vector3
        (
          num.X * MathUtil.InvSafe(den.X), 
          num.Y * MathUtil.InvSafe(den.Y), 
          num.Z * MathUtil.InvSafe(den.Z)
        );
    }

    public static Vector3 ClampBend(Vector3 vector, Vector3 reference, float maxBendAngle)
    {
      float vLenSqr = vector.LengthSquared();
      if (vLenSqr < MathUtil.Epsilon)
        return vector;

      float rLenSqr = reference.LengthSquared();
      if (rLenSqr < MathUtil.Epsilon)
        return vector;

      Vector3 vUnit = vector / Mathf.Sqrt(vLenSqr);
      Vector3 rUnit = reference / Mathf.Sqrt(rLenSqr);

      Vector3 cross = rUnit.Cross(vUnit);
      float dot = rUnit.Dot(vUnit);
      Vector3 axis = 
        cross.LengthSquared() > MathUtil.Epsilon 
          ? cross.Normalized() 
          : FindOrthogonal(rUnit);
      float angle = Mathf.Acos(MathUtil.Saturate(dot));

      if (angle <= maxBendAngle)
        return vector;

      Quaternion clampedBendRot = QuaternionUtil.AxisAngle(axis, maxBendAngle);
      Vector3 result = clampedBendRot * reference;
      result *= Mathf.Sqrt(vLenSqr) / Mathf.Sqrt(rLenSqr);

      return result;
    }
  }
}
