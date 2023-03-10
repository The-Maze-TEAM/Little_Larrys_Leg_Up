using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
	private Text timeUI;
	private Timer timeScript;

	void Start()
	{
		timeUI = GameObject.Find("TimerText").GetComponent<Text>();
		timeScript = GameObject.Find("Player").GetComponent<Timer>();
	}

	void OnTriggerEnter(Collider other)
	{
		// Only run this on the player
		if (other.name != "Player") return;

		// Run the ExplodeFace Method from the PlayerController
		other.GetComponent<PlayerController>().ExplodeFace();

		// Blow up the timer text, then stop the script
		timeUI.color = Color.green;
		timeUI.fontSize = 60;
		Destroy(timeScript);
	}
}
