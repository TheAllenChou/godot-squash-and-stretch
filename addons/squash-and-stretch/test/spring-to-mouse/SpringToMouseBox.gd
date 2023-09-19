extends Polygon2D

var spring:SpringVector2
var target:Vector2

# Called when the node enters the scene tree for the first time.
func _ready():
  target = position
  spring = SpringVector2.new()
  spring.reset(target)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
  target = get_global_mouse_position()
  spring.track_half_life(target, 3.0, 0.2, delta)
  position = spring.value
  
