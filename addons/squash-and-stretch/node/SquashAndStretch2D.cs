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
  public partial class SquashAndStretch2D : Node
  {
    [Export] public float MaxStretch = 1.0f;
    [Export] public float MinSpeedThreshold = 500.0f;
    [Export] public float MaxSpeedThreshold = 2000.0f;

    private FloatSpring m_speedSpring;
    private Vector2 m_moveDir;
    private Vector2Spring m_dirSpring;

    private Node2D m_node; // target node to manipulate
    private PostRender2D m_postRender;
    private Vector2 m_prevPos;

    public override void _Ready()
    {
      m_node = GetParent<Node2D>();
      m_prevPos = m_node.GlobalPosition;

      m_speedSpring.Reset(0.0f);
      m_dirSpring.Reset(new Vector2(Mathf.Cos(m_node.GlobalRotation), Mathf.Sin(m_node.GlobalRotation)));
    }

    public override void _Process(double delta)
    {
      if (m_postRender == null)
      {
        m_postRender = new PostRender2D(m_node);
        m_postRender.Name = $"{Name} (post-render)";
        m_node.AddChild(m_postRender);
      }

      float dt = (float) delta;
      Vector2 posDelta = m_node.GlobalPosition - m_prevPos;
      float speed = posDelta.Length() / Mathf.Max(dt, MathUtil.Epsilon);
      m_prevPos = m_node.GlobalPosition;

      m_speedSpring.TrackExponential(speed, 0.01f, dt);
      m_speedSpring.Value = Mathf.Sign(m_speedSpring.Value) * Mathf.Min(MaxSpeedThreshold, Mathf.Abs(m_speedSpring.Value));

      float trackedSpeed = m_speedSpring.Value;

      // update direction spring
      if (trackedSpeed > MathUtil.Epsilon 
          && posDelta.LengthSquared() > MathUtil.EpsilonSqr)
      {
        Vector2 targetDir = posDelta.Normalized();
        m_dirSpring.TrackExponential(targetDir, 0.01f, dt);
      }

      float s = MaxStretch * Mathf.Min(1.0f, Mathf.Max(0.0f, trackedSpeed - MinSpeedThreshold) / Mathf.Max(MaxSpeedThreshold - MinSpeedThreshold, MathUtil.Epsilon));
      float scale = 1.0f + s;
      float scaleInv = 1.0f / scale;

      float a = MathUtil.Atan2(VectorUtil.SafeNormalize(m_dirSpring.Value, Vector2.Right));

      m_postRender.CachePreRenderTransform(m_node.GlobalTransform);
      m_node.GlobalTransform = 
          new Transform2D(a, m_node.GlobalPosition) 
        * new Transform2D(new Vector2(scale, 0.0f), new Vector2(0.0f, scaleInv), Vector2.Zero) 
        * new Transform2D(-a, Vector2.Zero);
    }
  }
}

