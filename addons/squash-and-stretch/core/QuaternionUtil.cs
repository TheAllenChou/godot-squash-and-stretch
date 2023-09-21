/******************************************************************************/
/*
  Project   - Squash & Stretch plugin for Godot
              https://github.com/TheAllenChou/godot-squash-and-stretch
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using Godot;

namespace SquashAndStretch
{
  public class QuaternionUtil
  {
    // basic stuff
    // ------------------------------------------------------------------------

    public static float Magnitude(Quaternion q)
    {
      return Mathf.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
    }

    public static float MagnitudeSqr(Quaternion q)
    {
      return q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
    }

    public static Quaternion Normalize(Quaternion q)
    {
      float magInv = 1.0f / Magnitude(q);
      return new Quaternion(magInv * q.X, magInv * q.Y, magInv * q.Z, magInv * q.W);
    }

    // axis must be normalized
    public static Quaternion AxisAngle(Vector3 axis, float angle)
    {
      float h = 0.5f * angle;
      float s = Mathf.Sin(h);
      float c = Mathf.Cos(h);

      return new Quaternion(s * axis.X, s * axis.Y, s * axis.Z, c);
    }

    public static Vector3 GetAxis(Quaternion q)
    {
      Vector3 v = new Vector3(q.X, q.Y, q.Z);
      float len = v.Length();
      if (len < MathUtil.Epsilon)
        return Vector3.Left;

      return v / len;
    }

    public static float GetAngle(Quaternion q)
    {
      return 2.0f * Mathf.Acos(Mathf.Clamp(q.W, -1.0f, 1.0f));
    }

    public static Quaternion FromAngularVector(Vector3 v)
    {
      float len = v.Length();
      if (len < MathUtil.Epsilon)
        return Quaternion.Identity;

      v /= len;

      float h = 0.5f * len;
      float s = Mathf.Sin(h);
      float c = Mathf.Cos(h);

      return new Quaternion(s * v.X, s * v.Y, s * v.Z, c);
    }

    public static Vector3 ToAngularVector(Quaternion q)
    {
      Vector3 axis = GetAxis(q);
      float angle = GetAngle(q);

      return angle * axis;
    }

    public static Quaternion Pow(Quaternion q, float exp)
    {
      Vector3 axis = GetAxis(q);
      float angle = GetAngle(q) * exp;
      return AxisAngle(axis, angle);
    }

    // v: derivative of q
    public static Quaternion Integrate(Quaternion q, Quaternion v, float dt)
    {
      return Pow(v, dt) * q;
    }

    // omega: angular velocity (direction is axis, magnitude is angle)
    // https://www.ashwinnarayan.com/post/how-to-integrate-quaternions/
    // https://gafferongames.com/post/physics_in_3d/
    public static Quaternion Integrate(Quaternion q, Vector3 omega, float dt)
    {
      omega *= 0.5f;
      Quaternion p = (new Quaternion(omega.X, omega.Y, omega.Z, 0.0f)) * q;
      return Normalize(new Quaternion(q.X + p.X * dt, q.Y + p.Y * dt, q.Z + p.Z * dt, q.W + p.W * dt));
    }

    public static Vector4 ToVector4(Quaternion q)
    {
      return new Vector4(q.X, q.Y, q.Z, q.W);
    }

    public static Quaternion FromVector4(Vector4 v, bool normalize = true)
    {
      if (normalize)
      {
        float magSqr = v.LengthSquared();
        if (magSqr < MathUtil.Epsilon)
          return Quaternion.Identity;

        v /= Mathf.Sqrt(magSqr);
      }

      return new Quaternion(v.X, v.Y, v.Z, v.W);
    }

    // ------------------------------------------------------------------------
    // end: basic stuff


    // swing-twist decomposition & interpolation
    // ------------------------------------------------------------------------

    public static void DecomposeSwingTwist
    (
      Quaternion q, 
      Vector3 twistAxis, 
      out Quaternion swing, 
      out Quaternion twist
    )
    {
      Vector3 r = new Vector3(q.X, q.Y, q.Z); // (rotaiton axis) * cos(angle / 2)

      // singularity: rotation by 180 degree
      if (r.LengthSquared() < MathUtil.Epsilon)
      {
        Vector3 rotatedTwistAxis = q * twistAxis;
        Vector3 swingAxis = twistAxis.Cross(rotatedTwistAxis);

        if (swingAxis.LengthSquared() > MathUtil.Epsilon)
        {
          float swingAngle = twistAxis.AngleTo(rotatedTwistAxis);
          swing = new Quaternion(swingAxis, swingAngle);
        }
        else
        {
          // more singularity: rotation axis parallel to twist axis
          swing = Quaternion.Identity; // no swing
        }

        // always twist 180 degree on singularity
        twist = new Quaternion(twistAxis, Mathf.Pi);
        return;
      }

      // formula & proof: 
      // http://www.euclideanspace.com/maths/geometry/rotations/for/decomposition/
      Vector3 p = r.Project(twistAxis);
      twist = new Quaternion(p.X, p.Y, p.Z, q.W);
      twist = Normalize(twist);
      swing = q * twist.Inverse();
    }

    // same swing & twist parameters
    public static Quaternion Sterp
    (
      Quaternion a, 
      Quaternion b, 
      Vector3 twistAxis, 
      float t
    )
    {
      Quaternion swing;
      Quaternion twist;
      return Sterp(a, b, twistAxis, t, out swing, out twist);
    }

    // same swing & twist parameters with individual interpolated swing & twist outputs
    public static Quaternion Sterp
    (
      Quaternion a, 
      Quaternion b, 
      Vector3 twistAxis, 
      float t, 
      out Quaternion swing, 
      out Quaternion twist
    )
    {
      return Sterp(a, b, twistAxis, t, t, out swing, out twist);
    }

    // different swing & twist parameters
    public static Quaternion Sterp
    (
      Quaternion a, 
      Quaternion b, 
      Vector3 twistAxis, 
      float tSwing, 
      float tTwist
    )
    {
      Quaternion swing;
      Quaternion twist;
      return Sterp(a, b, twistAxis, tSwing, tTwist, out swing, out twist);
    }

    // master sterp function
    public static Quaternion Sterp
    (
      Quaternion a, 
      Quaternion b, 
      Vector3 twistAxis, 
      float tSwing, 
      float tTwist, 
      out Quaternion swing, 
      out Quaternion twist
    )
    {
      Quaternion q = b * a.Inverse();
      Quaternion swingFull;
      Quaternion twistFull;
      QuaternionUtil.DecomposeSwingTwist(q, twistAxis, out swingFull, out twistFull);

      swing = Quaternion.Identity.Slerp(swingFull, tSwing);
      twist = Quaternion.Identity.Slerp(twistFull, tTwist);

      return twist * swing;
    }

    // ------------------------------------------------------------------------
    // end: swing-twist decomposition & interpolation
  }
}
