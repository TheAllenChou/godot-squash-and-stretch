class_name SpringFloat

@export var value:float = 0.0
@export var velocity:float = 0.0

func reset(init_value:float, init_velocity:float = 0.0):
  value = init_value
  velocity = init_velocity

func track_damping_ratio(target:float, angular_frequency:float, damping_ratio:float, delta_time:float) -> float:
  var delta:float = target - value

  var f:float = 1.0 + 2.0 * delta_time * damping_ratio * angular_frequency
  var oo:float = angular_frequency * angular_frequency
  var hoo:float = delta_time * oo
  var hhoo:float = delta_time * hoo
  var det_inv:float = 1.0 / (f + hhoo)
  var det_x:float = f * value + delta_time * velocity + hhoo * target
  var det_v:float = velocity + hoo * delta

  velocity = det_v * det_inv
  value = det_x * det_inv

  if velocity < SquashAndStretchUtil.EPSILON and delta < SquashAndStretchUtil.EPSILON:
    velocity = 0.0
    value = target

  return value

func track_half_life(target:float, frequency_hz:float, half_life:float, delta_time:float) -> float:
  if half_life < SquashAndStretchUtil.EPSILON:
    velocity = 0.0
    value = target
    return value

  var angular_frequency:float = frequency_hz * SquashAndStretchUtil.TWO_PI
  var damping_ratio:float = SquashAndStretchUtil.LN_2 / (angular_frequency * half_life)
  return track_damping_ratio(target, angular_frequency, damping_ratio, delta_time)

func track_exponential(target:float, half_life:float, delta_time:float) -> float:
  if half_life < SquashAndStretchUtil.EPSILON:
    velocity = 0.0
    value = target
    return value

  var angular_frequency:float = SquashAndStretchUtil.LN_2 / half_life
  var damping_ratio:float = 1.0
  return track_damping_ratio(target, angular_frequency, damping_ratio, delta_time)
