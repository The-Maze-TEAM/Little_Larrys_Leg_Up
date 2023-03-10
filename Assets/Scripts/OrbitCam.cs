using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCam : MonoBehaviour
{

	[SerializeField]
	private Transform target;
	[SerializeField]
	private float _distanceFromTarget;
	[SerializeField]
	private float MouseSensitivity;
	[SerializeField]
	private float _smoothTime = .75f;

	private Vector2 _rotation;
	private Vector2 _mouseInput;
	private Vector2 _angleDistanceRange = new Vector2(-20, 75);

	private Vector3 _currentRotation;
	private Vector3 _smoothVelocity = Vector3.zero;

	private bool _camLocked = false;

	private bool _isPanning = false;

	void OnLook(InputValue mouseInput)
	{
		// Capture mouse input values and multiply by sensitivity
		// This function has a minor bug due to it being invoked when the mouse wheel is used to zoom out. :shrug:
		_mouseInput = mouseInput.Get<Vector2>() * MouseSensitivity;
	}

	void OnZoom(InputValue wheelInput)
	{
		float _wheelButtonUsed = wheelInput.Get<float>();
		// In order, wheel down, click, wheel up.
		if (_wheelButtonUsed == -120 && _distanceFromTarget < 15.0f)
			_distanceFromTarget += 0.5f;
		else if (_wheelButtonUsed == 1)
			_distanceFromTarget = 3.0f;
		else if (_wheelButtonUsed == 120 && _distanceFromTarget > 1.5f)
			_distanceFromTarget -= 0.5f;
	}

	void OnLock()
	{
		// Cursor.lockState = CursorLockMode.Locked;
		_camLocked = !_camLocked;
	}

	void OnPanning(InputValue clickIsDown)
	{
		_isPanning = clickIsDown.Get<float>() == 1
			? true
			: false;
		// Debug.Log(_localpanning);
	}

	void LateUpdate()
	{
		if (!_camLocked || _isPanning)
		{
			// Invert mouse input so up is up, and down is down.
			// Otherwise they're inverted.
			_rotation += new Vector2(_mouseInput.x, -_mouseInput.y);

			// Only allow orienting within a certain range.
			// This applies to the Y axis (up/down) of the mouse input, which confusingly
			// Corresponds to the X axis of the camera (also up/down)
			_rotation.y = Mathf.Clamp(_rotation.y, _angleDistanceRange.x, _angleDistanceRange.y);
			// _rotation.x = Mathf.Clamp(_rotation.x, _angleDistanceRange.x, _angleDistanceRange.y);

			// The axes here are inverted to correspond to those of the camera
			Vector3 nextRotation = new Vector3(_rotation.y, _rotation.x);

			// Smooth camera between points over smoothTime seconds at a rate of smoothVelocity
			_currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);

			// Apply the smoothed camera angle
			transform.localEulerAngles = _currentRotation;
		}
		// Debug.Log($"New position will be: {target.position} - {transform.forward} * {_distanceFromTarget} = {target.position - transform.forward * _distanceFromTarget}");

		// Move to the target's position and face them, with some added distance
		transform.position = target.position - transform.forward * _distanceFromTarget;
	}
}
