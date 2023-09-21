using Godot;
using SquashAndStretchKit;

public partial class MouseManipulator3D : Node
{
  private Node3D m_node;

  private Vector3 m_targetPos;
  private Vector3Spring m_posSpring;

  private QuaternionSpring m_rotSpring;
  private Quaternion m_targetRot;

  private Vector2 m_prevMousePos;

  public override void _Ready()
  {
    m_node = GetParent<Node3D>();

    m_targetPos = m_node.GlobalPosition;
    m_posSpring.Reset(m_targetPos);

    m_targetRot = m_node.Transform.Basis.GetRotationQuaternion();
    m_rotSpring.Reset(m_targetRot);

    m_prevMousePos = GetViewport().GetMousePosition();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    float dt = (float) delta;
    Vector2 mousePos = GetViewport().GetMousePosition();
    Vector2 mouseDelta = 0.01f * (mousePos - m_prevMousePos);

    if (Input.IsMouseButtonPressed(MouseButton.Left))
    {
      Quaternion rotDelta = Quaternion.FromEuler(new Vector3(-mouseDelta.Y, mouseDelta.X, 0.0f) * 5.0f);
      m_targetRot = (rotDelta * m_targetRot).Normalized();
      m_node.GlobalTransform = new Transform3D(new Basis(m_rotSpring.TrackExponential(m_targetRot, 0.0025f, dt)), m_node.GlobalTransform.Origin);
    }

    if (Input.IsMouseButtonPressed(MouseButton.Right))
    {
      m_targetPos += new Vector3(mouseDelta.X, -mouseDelta.Y, 0.0f);
      m_node.GlobalPosition = m_posSpring.TrackExponential(m_targetPos, 0.00256f, dt);
    }

    m_prevMousePos = mousePos;
  }
}

