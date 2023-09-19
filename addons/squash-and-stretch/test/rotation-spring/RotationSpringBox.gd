extends MeshInstance3D

var spring:SpringQuaternion
var target:Quaternion

# Called when the node enters the scene tree for the first time.
func _ready():
  target = Quaternion.IDENTITY
  spring = SpringQuaternion.new()
  spring.reset(target)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
  var m = get_viewport().get_mouse_position()
  target = Quaternion.from_euler(0.005 * Vector3(-m.y, m.x, 0.0))
  spring.track_half_life(target, 4.0, 0.1, delta)
  transform.basis = Basis(spring.value)
