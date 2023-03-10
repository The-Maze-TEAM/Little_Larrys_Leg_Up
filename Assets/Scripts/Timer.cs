using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Text TimerText;
	private float runTime ;
	private string minutes, seconds, milliseconds;

	void Update()
	{
		runTime += Time.deltaTime;

		// minutes = Mathf.FloorToInt(runTime / 60F).ToString("0");
		// seconds = ":" + Mathf.FloorToInt(runTime % 60F).ToString("00");
		// milliseconds = "." + Mathf.FloorToInt((runTime * 100f) % 100F).ToString("00");

		// TimerText.text = minutes + seconds + milliseconds;

		TimerText.text = Mathf.FloorToInt(runTime / 60F).ToString("0") + ":" + // Minutes
			Mathf.FloorToInt(runTime % 60F).ToString("00") + "." + // Seconds
			Mathf.FloorToInt((runTime * 100F) % 100F).ToString("00"); // Milliseconds
	}
}
