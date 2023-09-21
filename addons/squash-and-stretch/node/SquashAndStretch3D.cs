/******************************************************************************/
/*
  Project   - Squash & Stretch plugin for Godot
              https://github.com/TheAllenChou/godot-squash-and-stretch
  Author    - Ming-Lun "Allen" Chou
              http://AllenChou.net
*/
/******************************************************************************/

using Godot;

namespace SquashAndStretchKit
{
  public partial class SquashAndStretch3D : Node
  {
    [Export] public float MaxStretch = 1.0f;
    [Export] public float MinSpeedThreshold = 1.0f;
    [Export] public float MaxSpeedThreshold = 20.0f;

    private FloatSpring m_speedSpring;
    private Vector3 m_moveDir;
    private Vector3Spring m_dirSpring;

    private Node3D m_node; // target node to manipulate
    private PostRender3D m_postRender;
    private Vector3 m_prevPos;

    public override void _Ready()
    {
      m_node = GetParent<Node3D>();
      m_prevPos = m_node.GlobalPosition;

      m_speedSpring.Reset(0.0f);
      m_dirSpring.Reset(m_node.GlobalRotation * Vector3.Up);
    }

    public override void _Process(double delta)
    {
      if (m_postRender == null)
      {
        m_postRender = new PostRender3D(m_node);
        m_postRender.Name = $"{Name} (post-render)";
        m_node.AddChild(m_postRender);
      }

      float dt = (float) delta;
      Vector3 posDelta = m_node.GlobalPosition - m_prevPos;
      float speed = posDelta.Length() / Mathf.Max(dt, MathUtil.Epsilon);
      m_prevPos = m_node.GlobalPosition;

      m_speedSpring.TrackExponential(speed, 0.01f, dt);
      m_speedSpring.Value = Mathf.Sign(m_speedSpring.Value) * Mathf.Min(MaxSpeedThreshold, Mathf.Abs(m_speedSpring.Value));

      float trackedSpeed = m_speedSpring.Value;

      // update direction spring
      if (trackedSpeed > MathUtil.Epsilon
          && posDelta.LengthSquared() > MathUtil.EpsilonSqr)
      {
        Vector3 targetDir = posDelta.Normalized();
        m_dirSpring.TrackExponential(targetDir, 0.01f, dt);
      }

      float s = MaxStretch * Mathf.Min(1.0f, Mathf.Max(0.0f, trackedSpeed - MinSpeedThreshold) / Mathf.Max(MaxSpeedThreshold - MinSpeedThreshold, MathUtil.Epsilon));
      float scale = 1.0f + s;
      float scaleInv = 1.0f / Mathf.Sqrt(scale);

      Quaternion rot = m_node.GlobalTransform.Basis.GetRotationQuaternion();
      Vector3 stretchAxisOs = rot.Inverse() * VectorUtil.SafeNormalize(m_dirSpring.Value, Vector3.Up);

      Quaternion a = (new Quaternion(Vector3.Up, stretchAxisOs)).Normalized();

      m_postRender.CachePreRenderTransform(m_node.GlobalTransform);
      m_node.GlobalTransform = 
          new Transform3D(new Basis(rot * a), m_node.GlobalPosition) 
        * new Transform3D(new Vector3(scaleInv, 0.0f, 0.0f), new Vector3(0.0f, scale, 0.0f), new Vector3(0.0f, 0.0f, scaleInv), Vector3.Zero) 
        * new Transform3D(new Basis(a.Inverse()), Vector3.Zero);
    }
  }
}

