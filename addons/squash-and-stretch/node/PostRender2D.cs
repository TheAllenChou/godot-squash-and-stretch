using Godot;

namespace SquashAndStretchKit
{
  public partial class PostRender2D : Node
  {
    private Node2D m_node;
    private bool m_cachedTransformValid = false;
    private Transform2D m_cachedTransform;

    public PostRender2D(Node2D node)
    {
      m_node = node;
    }

    public override void _Ready()
    {
      ProcessPriority = -100;
    }

    public override void _Process(double delta)
    {
      if (!m_cachedTransformValid)
        return;

      m_node.GlobalTransform = m_cachedTransform;
    }

    public void CachePreRenderTransform(in Transform2D transform)
    {
      m_cachedTransformValid = true;
      m_cachedTransform = transform;
    }
  }
}

