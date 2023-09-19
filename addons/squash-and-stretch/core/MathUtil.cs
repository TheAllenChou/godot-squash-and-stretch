/******************************************************************************/
/*
  Project   - Boing Kit
  Publisher - Long Bunny Labs
              http://LongBunnyLabs.com
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using Godot;

namespace SquashAndStretchKit
{
  public class MathUtil
  {
    public static readonly float Pi        = Mathf.Pi;
    public static readonly float TwoPi     = 2.0f * Mathf.Pi;
    public static readonly float HalfPi    = Mathf.Pi / 2.0f;
    public static readonly float QuaterPi  = Mathf.Pi / 4.0f;
    public static readonly float SixthPi   = Mathf.Pi / 6.0f;

    public static readonly float Sqrt2    = Mathf.Sqrt(2.0f);
    public static readonly float Sqrt2Inv = 1.0f / Mathf.Sqrt(2.0f);
    public static readonly float Sqrt3    = Mathf.Sqrt(3.0f);
    public static readonly float Sqrt3Inv = 1.0f / Mathf.Sqrt(3.0f);

    public static readonly float Epsilon = 1.0e-6f;
    public static readonly float EpsilonSqr = Epsilon * Epsilon;
    public static readonly float Rad2Deg = 180.0f / Mathf.Pi;
    public static readonly float Deg2Rad = Mathf.Pi / 180.0f;

    public static float Saturate(float x)
    {
      return Mathf.Clamp(x, 0.0f, 1.0f);
    }

    public static float AsinSafe(float x)
    {
      return Mathf.Asin(Mathf.Clamp(x, -1.0f, 1.0f));
    }

    public static float AcosSafe(float x)
    {
      return Mathf.Acos(Mathf.Clamp(x, -1.0f, 1.0f));
    }

    public static float Atan2(Vector2 v)
    {
      return Mathf.Atan2(v.Y, v.X);
    }

    public static float InvSafe(float x)
    {
      return 1.0f / Mathf.Max(Epsilon, x);
    }

    public static float Seek(float current, float target, float maxDelta)
    {
      float delta = target - current;
      delta = Mathf.Sign(delta) * Mathf.Min(maxDelta, Mathf.Abs(delta));
      return current + delta;
    }

    public static Vector2 Seek(Vector2 current, Vector2 target, float maxDelta)
    {
      Vector2 delta = target - current;
      float deltaMag = delta.Length();
      if (deltaMag < Epsilon)
        return target;

      delta = Mathf.Min(maxDelta, deltaMag) * delta.Normalized();
      return current + delta;
    }

    public static float Remainder(float a, float b)
    {
      return a - (a / b) * b;
    }

    public static int Remainder(int a, int b)
    {
      return a - (a / b) * b;
    }

    public static float Modulo(float a, float b)
    {
      return Mathf.PosMod(a, b);
    }

    public static int Modulo(int a, int b)
    {
      int r = a % b;
      return r >= 0 ? r : r + b;
    }
  }
}

