/******************************************************************************/
/*
  Project   - Squash & Stretch plugin for Godot
              https://github.com/TheAllenChou/godot-squash-and-stretch
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using Godot;

using System.Runtime.InteropServices;

namespace SquashAndStretch
{
  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct FloatSpring
  {
    public static readonly int Stride = 2 * sizeof(float);

    public float Value;
    public float Velocity;

    public void Reset()
    {
      Value = 0.0f;
      Velocity = 0.0f;
    }

    public void Reset(float initValue)
    {
      Value = initValue;
      Velocity = 0.0f;
    }

    public void Reset(float initValue, float initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public float TrackDampingRatio(float targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        return Value;
      }

      float delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      float detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      float detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity < MathUtil.Epsilon && Mathf.Abs(delta) < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        Value = targetValue;
      }

      return Value;
    }

    public float TrackHalfLife(float targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = MathUtil.Ln2 / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public float TrackExponential(float targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = 0.0f;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = MathUtil.Ln2 / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct Vector2Spring
  {
    public static readonly int Stride = 4 * sizeof(float);

    public Vector2 Value;
    public Vector2 Velocity;

    public void Reset()
    {
      Value = Vector2.Zero;
      Velocity = Vector2.Zero;
    }

    public void Reset(Vector2 initValue)
    {
      Value = initValue;
      Velocity = Vector2.Zero;
    }

    public void Reset(Vector2 initValue, Vector2 initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public Vector2 TrackDampingRatio(Vector2 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = Vector2.Zero;
        return Value;
      }

      Vector2 delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector2 detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      Vector2 detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity.Length() < MathUtil.Epsilon && delta.Length() < MathUtil.Epsilon)
      {
        Velocity = Vector2.Zero;
        Value = targetValue;
      }

      return Value;
    }

    public Vector2 TrackHalfLife(Vector2 targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector2.Zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = MathUtil.Ln2 / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Vector2 TrackExponential(Vector2 targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector2.Zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = MathUtil.Ln2 / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct Vector3Spring
  {
    public static readonly int Stride = 8 * sizeof(float); // 32 bytes

    public Vector3 Value;
    private float m_padding0;
    public Vector3 Velocity;
    private float m_padding1;

    public void Reset()
    {
      Value = Vector3.Zero;
      Velocity = Vector3.Zero;
    }

    public void Reset(Vector3 initValue)
    {
      Value = initValue;
      Velocity = Vector3.Zero;
    }

    public void Reset(Vector3 initValue, Vector3 initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public Vector3 TrackDampingRatio(Vector3 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = Vector3.Zero;
        return Value;
      }

      Vector3 delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector3 detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      Vector3 detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity.Length() < MathUtil.Epsilon && delta.Length() < MathUtil.Epsilon)
      {
        Velocity = Vector3.Zero;
        Value = targetValue;
      }

      return Value;
    }

    public Vector3 TrackHalfLife(Vector3 targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector3.Zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = MathUtil.Ln2 / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Vector3 TrackExponential(Vector3 targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector3.Zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = MathUtil.Ln2 / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct Vector4Spring
  {
    public static readonly int Stride = 8 * sizeof(float);

    public Vector4 Value;
    public Vector4 Velocity;

    public void Reset()
    {
      Value = Vector4.Zero;
      Velocity = Vector4.Zero;
    }

    public void Reset(Vector4 initValue)
    {
      Value = initValue;
      Velocity = Vector4.Zero;
    }

    public void Reset(Vector4 initValue, Vector4 initVelocity)
    {
      Value = initValue;
      Velocity = initVelocity;
    }

    public Vector4 TrackDampingRatio(Vector4 targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        Velocity = Vector4.Zero;
        return Value;
      }

      Vector4 delta = targetValue - Value;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector4 detX = f * Value + deltaTime * Velocity + hhoo * targetValue;
      Vector4 detV = Velocity + hoo * delta;

      Velocity = detV * detInv;
      Value = detX * detInv;

      if (Velocity.Length() < MathUtil.Epsilon && delta.Length() < MathUtil.Epsilon)
      {
        Velocity = Vector4.Zero;
        Value = targetValue;
      }

      return Value;
    }

    public Vector4 TrackHalfLife(Vector4 targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector4.Zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = MathUtil.Ln2 / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Vector4 TrackExponential(Vector4 targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        Velocity = Vector4.Zero;
        Value = targetValue;
        return Value;
      }

      float angularFrequency = MathUtil.Ln2 / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }


  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct QuaternionSpring
  {
    public static readonly int Stride = 8 * sizeof(float);

    public Vector4 ValueVec;
    public Vector4 VelocityVec;

    public Quaternion ValueQuat
    {
      get { return QuaternionUtil.FromVector4(ValueVec); }
      set { ValueVec = QuaternionUtil.ToVector4(value); }
    }

    public void Reset()
    {
      ValueVec = QuaternionUtil.ToVector4(Quaternion.Identity);
      VelocityVec = Vector4.Zero;
    }

    public void Reset(Vector4 initValue)
    {
      ValueVec = initValue;
      VelocityVec = Vector4.Zero;
    }

    public void Reset(Vector4 initValue, Vector4 initVelocity)
    {
      ValueVec = initValue;
      VelocityVec = initVelocity;
    }

    public void Reset(Quaternion initValue)
    {
      ValueVec = QuaternionUtil.ToVector4(initValue);
      VelocityVec = Vector4.Zero;
    }

    public void Reset(Quaternion initValue, Quaternion initVelocity)
    {
      ValueVec = QuaternionUtil.ToVector4(initValue);
      VelocityVec = QuaternionUtil.ToVector4(initVelocity);
    }

    public Quaternion TrackDampingRatio(Vector4 targetValueVec, float angularFrequency, float dampingRatio, float deltaTime)
    {
      if (angularFrequency < MathUtil.Epsilon)
      {
        VelocityVec = QuaternionUtil.ToVector4(Quaternion.Identity);
        return QuaternionUtil.FromVector4(ValueVec);
      }

      // keep in same hemisphere for shorter track delta
      if (ValueVec.Dot(targetValueVec) < 0.0f)
      {
        targetValueVec = -targetValueVec;
      }

      Vector4 delta = targetValueVec - ValueVec;

      float f = 1.0f + 2.0f * deltaTime * dampingRatio * angularFrequency;
      float oo = angularFrequency * angularFrequency;
      float hoo = deltaTime * oo;
      float hhoo = deltaTime * hoo;
      float detInv = 1.0f / (f + hhoo);
      Vector4 detX = f * ValueVec + deltaTime * VelocityVec + hhoo * targetValueVec;
      Vector4 detV = VelocityVec + hoo * delta;

      VelocityVec = detV * detInv;
      ValueVec = detX * detInv;

      if (VelocityVec.Length() < MathUtil.Epsilon 
          && delta.Length() < MathUtil.Epsilon)
      {
        VelocityVec = Vector4.Zero;
        ValueVec = targetValueVec;
      }

      return QuaternionUtil.FromVector4(ValueVec);
    }

    public Quaternion TrackDampingRatio(Quaternion targetValue, float angularFrequency, float dampingRatio, float deltaTime)
    {
      return TrackDampingRatio(QuaternionUtil.ToVector4(targetValue), angularFrequency, dampingRatio, deltaTime);
    }

    public Quaternion TrackHalfLife(Vector4 targetValueVec, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        VelocityVec = Vector4.Zero;
        ValueVec = targetValueVec;
        return QuaternionUtil.FromVector4(targetValueVec);
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = MathUtil.Ln2 / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValueVec, angularFrequency, dampingRatio, deltaTime);
    }

    public Quaternion TrackHalfLife(Quaternion targetValue, float frequencyHz, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        VelocityVec = QuaternionUtil.ToVector4(Quaternion.Identity);
        ValueVec = QuaternionUtil.ToVector4(targetValue);
        return targetValue;
      }

      float angularFrequency = frequencyHz * MathUtil.TwoPi;
      float dampingRatio = MathUtil.Ln2 / (angularFrequency * halfLife);
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }

    public Quaternion TrackExponential(Vector4 targetValueVec, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        VelocityVec = Vector4.Zero;
        ValueVec = targetValueVec;
        return QuaternionUtil.FromVector4(targetValueVec);
      }

      float angularFrequency = MathUtil.Ln2 / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValueVec, angularFrequency, dampingRatio, deltaTime);
    }

    public Quaternion TrackExponential(Quaternion targetValue, float halfLife, float deltaTime)
    {
      if (halfLife < MathUtil.Epsilon)
      {
        VelocityVec = QuaternionUtil.ToVector4(Quaternion.Identity);
        ValueVec = QuaternionUtil.ToVector4(targetValue);
        return targetValue;
      }

      float angularFrequency = MathUtil.Ln2 / halfLife;
      float dampingRatio = 1.0f;
      return TrackDampingRatio(targetValue, angularFrequency, dampingRatio, deltaTime);
    }
  }
}
