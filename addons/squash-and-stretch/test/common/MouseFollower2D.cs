using Godot;

using SquashAndStretch;

public partial class MouseFollower2D : Node
{
  private Node2D m_node;

  private Vector2Spring m_spring;

  public override void _Ready()
  {
    m_node = GetParent<Node2D>();
    m_spring.Reset(m_node.GlobalPosition);
  }

  public override void _Process(double delta)
  {
    float dt = (float) delta;
    m_node.GlobalPosition = m_spring.TrackExponential(GetViewport().GetMousePosition(), 0.01f, dt);
  }
}

