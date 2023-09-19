using Godot;

using SquashAndStretchKit;

public partial class SquashAndStretch2D : Node
{
  public float MaxStretch = 1.0f;
  public float MinSpeedThreshold = 500.0f;
  public float MaxSpeedThreshold = 2000.0f;

  private FloatSpring m_speedSpring;
  private Vector2 m_moveDir;
  private Vector2Spring m_dirSpring;

  private Node2D m_node; // target node to manipulate
  private Vector2 m_prevPos;


  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    m_node = GetParent<Node2D>();
    m_prevPos = m_node.GlobalPosition;
    m_speedSpring.Reset(0.0f);
    m_dirSpring.Reset(new Vector2(Mathf.Cos(m_node.GlobalRotation), Mathf.Sin(m_node.GlobalRotation)));
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    float dt = (float) delta;
    Vector2 posDelta = m_node.GlobalPosition - m_prevPos;
    float speed = posDelta.Length() / Mathf.Max(dt, MathUtil.Epsilon);
    m_prevPos = m_node.GlobalPosition;

    m_speedSpring.TrackExponential(speed, 0.01f, dt);
    m_speedSpring.Value = Mathf.Sign(m_speedSpring.Value) * Mathf.Min(MaxSpeedThreshold, Mathf.Abs(m_speedSpring.Value));

    float trackedSpeed = m_speedSpring.Value;

    // update direction spring
    float maxS = 1.0f;
    if (trackedSpeed > MathUtil.Epsilon 
        && posDelta.LengthSquared() > MathUtil.EpsilonSqr)
    {
      Vector2 targetDir = posDelta.Normalized();
      if (m_dirSpring.Value.LengthSquared() > MathUtil.EpsilonSqr)
      {
        m_dirSpring.TrackExponential(targetDir, 0.01f, dt);
      }
      if (m_dirSpring.Value.LengthSquared() > MathUtil.EpsilonSqr)
      {
        maxS = Mathf.Max(0.0f, 1.0f - Mathf.Abs(MathUtil.Atan2(targetDir) - MathUtil.Atan2(m_dirSpring.Value)) / MathUtil.HalfPi);
      }
    }

    float s = Mathf.Min(1.0f, Mathf.Max(0.0f, trackedSpeed - MinSpeedThreshold) / Mathf.Max(MaxSpeedThreshold - MinSpeedThreshold, MathUtil.Epsilon));
    s = Mathf.Min(s, maxS);
    s *= MaxStretch;

    float a = MathUtil.Atan2(m_dirSpring.Value);

    m_node.Transform = 
        new Transform2D(a, m_node.GlobalPosition) 
      * new Transform2D(new Vector2(1.0f + s, 0.0f), new Vector2(0.0f, 1.0f / (1.0f + s)), Vector2.Zero) 
      * new Transform2D(-a, Vector2.Zero);
  }
}
