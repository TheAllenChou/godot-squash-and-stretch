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
  public partial class PostRender3D : Node
  {
    private Node3D m_node;
    private bool m_cachedTransformValid = false;
    private Transform3D m_cachedTransform;

    public PostRender3D(Node3D node)
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

    public void CachePreRenderTransform(in Transform3D transform)
    {
      m_cachedTransformValid = true;
      m_cachedTransform = transform;
    }
  }
}

