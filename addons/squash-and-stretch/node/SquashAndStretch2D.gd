extends Node
@export var max_stretch:float = 1.0
@export var min_speed_threshld:float = 100.0
@export var max_speed_threshold:float = 500.0

var speed_spring:SpringFloat = SpringFloat.new()
var dir_spring:SpringVector2 = SpringVector2.new()

@onready var obj:Node2D = get_parent()
var prev_pos:Vector2

func _ready():
  prev_pos = obj.position
  speed_spring.reset(0.0)
  dir_spring.reset(Vector2(cos(obj.rotation), sin(obj.rotation)))

func _process(delta):
  var pos_delta:Vector2 = obj.position - prev_pos
  var speed:float = (pos_delta).length() / max(delta, SquashAndStretchUtil.EPSILON)
  prev_pos = obj.position

  speed_spring.track_exponential(speed, 0.01, delta)
  speed_spring.value = sign(speed_spring.value) * min(max_speed_threshold, abs(speed_spring.value))
  
  var tracked_speed:float = speed_spring.value

  # update direction spring
  if tracked_speed > SquashAndStretchUtil.EPSILON and pos_delta.length_squared() > SquashAndStretchUtil.EPSILON_SQR:
    var target_dir:Vector2 = pos_delta.normalized()
    dir_spring.track_exponential(target_dir, 0.001, delta)
  
  var s:float = max_stretch * min(1.0, max(0.0, tracked_speed - min_speed_threshld) / max(max_speed_threshold - min_speed_threshld, SquashAndStretchUtil.EPSILON))
  var a:float = atan2(dir_spring.value.y, dir_spring.value.x)
  
  obj.scale = Vector2(1.0 + s, 1.0 / (1.0 + s))
  obj.rotation = a






