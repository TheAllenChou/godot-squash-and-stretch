using Godot;
using SquashAndStretchKit;

namespace SquashAndStretchKit
{
  public partial class MouseFollower : Node
  {
    private Node2D m_parent;
    private Vector2Spring m_spring;

    public override void _Ready()
    {
      m_parent = GetParent<Node2D>();
      m_spring.Reset(m_parent.GlobalPosition);
    }

    public override void _Process(double delta)
    {
      float dt = (float) delta;
      m_parent.GlobalPosition = m_spring.TrackExponential(GetViewport().GetMousePosition(), 0.01f, dt);
    }
  }
}

