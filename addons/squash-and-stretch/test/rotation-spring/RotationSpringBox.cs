using Godot;
using SquashAndStretchKit;

public partial class RotationSpringBox : MeshInstance3D
{
  private QuaternionSpring m_spring;
  private Quaternion m_target;

  public override void _Ready()
  {
    m_target = Quaternion.Identity;
    m_spring.Reset(m_target);
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    float dt = (float) delta;
    Vector2 mousePos = GetViewport().GetMousePosition();
    m_target = Quaternion.FromEuler(new Vector3(-mousePos.Y, mousePos.X, 0.0f) * 0.005f);
    Transform = new Transform3D(new Basis(m_spring.TrackHalfLife(m_target, 3.0f, 0.02f, dt)), Transform.Origin);
  }
}

