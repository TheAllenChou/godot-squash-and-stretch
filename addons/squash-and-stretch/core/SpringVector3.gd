class_name SpringVector3

@export var value:Vector3 = Vector3.ZERO
@export var velocity:Vector3 = Vector3.ZERO

func reset(init_value:Vector3, init_velocity:Vector3 = Vector3.ZERO):
  value = init_value
  velocity = init_velocity
  pass

func track_damping_ratio(target:Vector3, angular_frequency:float, damping_ratio:float, delta_time:float):
  var delta:Vector3 = target - value

  var f:float = 1.0 + 2.0 * delta_time * damping_ratio * angular_frequency
  var oo:float = angular_frequency * angular_frequency
  var hoo:float = delta_time * oo
  var hhoo:float = delta_time * hoo
  var det_inv:float = 1.0 / (f + hhoo)
  var det_x:Vector3 = f * value + delta_time * velocity + hhoo * target
  var det_v:Vector3 = velocity + hoo * delta

  velocity = det_v * det_inv
  value = det_x * det_inv

  if velocity.length_squared() < SquashAndStretchUtil.EPSILON_SQR and delta.length_squared() < SquashAndStretchUtil.EPSILON_SQR:
    velocity = Vector3.ZERO
    value = target
  pass

  return value

func track_half_life(target:Vector3, frequency_hz:float, half_life:float, delta_time:float):
  if half_life < SquashAndStretchUtil.EPSILON:
    velocity = Vector3.ZERO
    value = target
    return value

  var angular_frequency:float = frequency_hz * SquashAndStretchUtil.TWO_PI
  var damping_ratio:float = SquashAndStretchUtil.LN_2 / (angular_frequency * half_life)
  return track_damping_ratio(target, angular_frequency, damping_ratio, delta_time)

func track_exponential(target:Vector3, half_life:float, delta_time:float):
  if half_life < SquashAndStretchUtil.EPSILON:
    velocity = Vector3.ZERO
    value = target
    return value

  var angular_frequency:float = SquashAndStretchUtil.LN_2 / half_life
  var damping_ratio:float = 1.0
  return track_damping_ratio(target, angular_frequency, damping_ratio, delta_time)
