class_name SpringQuaternion

@export var value:Quaternion = Quaternion.IDENTITY:
  set(v):
    value = v
  get:
    return value.normalized()

@export var velocity:Quaternion = Quaternion(0.0, 0.0, 0.0, 0.0):
  set(v):
    velocity = v
  get:
    return velocity

func reset(init_value:Quaternion, init_velocity:Quaternion = Quaternion(0.0, 0.0, 0.0, 0.0)):
  value = init_value
  velocity = init_velocity
  pass

func track_damping_ratio(target:Quaternion, angular_frequency:float, damping_ratio:float, delta_time:float):
  if target.dot(value) < 0.0:
    target = -target
    
  var delta:Quaternion = target - value

  var f:float = 1.0 + 2.0 * delta_time * damping_ratio * angular_frequency
  var oo:float = angular_frequency * angular_frequency
  var hoo:float = delta_time * oo
  var hhoo:float = delta_time * hoo
  var det_inv:float = 1.0 / (f + hhoo)
  var det_x:Quaternion = f * value + delta_time * velocity + hhoo * target
  var det_v:Quaternion = velocity + hoo * delta

  velocity = det_v * det_inv
  value = det_x * det_inv

  if velocity.length_squared() < SquashAndStretchUtil.EPSILON_SQR and delta.length_squared() < SquashAndStretchUtil.EPSILON_SQR:
    velocity = Quaternion(0.0, 0.0, 0.0, 0.0)
    value = target

  return value

func track_half_life(target:Quaternion, frequency_hz:float, half_life:float, delta_time:float):
  if target.dot(value) < 0.0:
    target = -target

  if half_life < SquashAndStretchUtil.EPSILON:
    velocity = Quaternion(0.0, 0.0, 0.0, 0.0)
    value = target
    return value

  var angular_frequency:float = frequency_hz * SquashAndStretchUtil.TWO_PI
  var damping_ratio:float = SquashAndStretchUtil.LN_2 / (angular_frequency * half_life)
  return track_damping_ratio(target, angular_frequency, damping_ratio, delta_time)

func track_exponential(target:Quaternion, half_life:float, delta_time:float):
  if target.dot(value) < 0.0:
    target = -target

  if half_life < SquashAndStretchUtil.EPSILON:
    velocity = Quaternion(0.0, 0.0, 0.0, 0.0)
    value = target
    return value

  var angular_frequency:float = SquashAndStretchUtil.LN_2 / half_life
  var damping_ratio:float = 1.0
  return track_damping_ratio(target, angular_frequency, damping_ratio, delta_time)
