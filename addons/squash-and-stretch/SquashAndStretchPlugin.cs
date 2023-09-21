#if TOOLS
using Godot;

[Tool]
public partial class SquashAndStretchPlugin : EditorPlugin
{
  public override void _EnterTree()
  {
    AddCustomType("Squash & Stretch 2D", "Node", GD.Load<Script>("res://addons/squash-and-stretch/node/SquashAndStretch2D.cs"), null);
    AddCustomType("Squash & Stretch 3D", "Node", GD.Load<Script>("res://addons/squash-and-stretch/node/SquashAndStretch3D.cs"), null);
  }

  public override void _ExitTree()
  {

  }
}
#endif

