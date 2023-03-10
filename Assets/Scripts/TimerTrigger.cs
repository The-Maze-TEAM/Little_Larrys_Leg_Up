using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
	void OnTriggerExit(Collider other)
	{

		// Find and enable the Timer script once the player starts moving
		other.GetComponent<Timer>().enabled = true;

		// Destroy this object once its job is done
		Destroy(gameObject);

	}
}
