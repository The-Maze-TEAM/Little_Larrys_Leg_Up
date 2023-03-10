using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
	public Transform _player;
	private CharacterController control;
	// Start is called before the first frame update
	void Start()
	{
		control = _player.GetComponent<CharacterController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player")
		{
			control.enabled = false;
			other.transform.position = new Vector3(0, 30, 0);
			control.enabled = true;
		}
		else
			Destroy(other.gameObject);
	}
}
