using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
	[SerializeField]
	private Transform _player;
	private CharacterController control;
	[SerializeField]
	private Vector3 _teleportDestination = new Vector3(0, 30, 0);


	void Start()
	{
		control = _player.GetComponent<CharacterController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player")
		{
			// This method works, but is slightly more expensive.
			// However, for scenarios where the framerate is significantly
			// higher than the physics step, this method may be more reliable.
			// It seems to call Physics.SyncTransforms under the hood.
			    // control.enabled = false;
			    // other.transform.position = _teleportDestination;
			    // control.enabled = true;
			        // Alternatively, you can do Edit > Project Settings > Physics > AutoSyncTransforms = True
			        // Or set Physics.autoSyncTransforms to True via script.
			// We could also just ditch CharacterControllers altogether over Rigidbody physics,
			// but I'm not in favor of that idea.

			// Benchmarks from a similar scenario seem to indicate this is the easiest
			// and most performant option for this project.

			// https://forum.unity.com/threads/character-controller-ignores-transform-position.617107/

			other.transform.position = _teleportDestination;
			Physics.SyncTransforms();
		}
		else
			Destroy(other.gameObject);
	}
}
