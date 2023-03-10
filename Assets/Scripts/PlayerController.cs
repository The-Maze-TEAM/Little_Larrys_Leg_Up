using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public CharacterController control;
	// Exploding Face
	private Transform[] facePieces;
	private bool faceExploded = false;
	public Transform newFaceParent;

	public Transform cam;
	public float speed;
	// Speed and Duration of player rotation
	private float turnSmoothTime = 10.0f;
	// Jump-related vars
	private float _maxJumpHeight;
	private float heightJumped;
	private bool jumped = false;
	private bool disallownewjump = false;
	public static Vector3 movement = Vector3.zero;
	[SerializeField] private float zScaleFactor, yScaleFactor;

	// Player input as X/Y axes
	private Vector2 _movementInput;

	void Start()
	{
		setRandomColors();
		facePieces = this.transform.Find("Face").gameObject.GetComponentsInChildren<Transform>();
	}

	void OnMove(InputValue moveInput)
	{
		_movementInput = moveInput.Get<Vector2>();
	}

	void OnReset()
	{
		// SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
		setRandomColors();
	}

	void OnJump()
	{
		if (!disallownewjump || control.isGrounded)
		{

			if (!control.isGrounded)
			{
				disallownewjump = true;
				_maxJumpHeight = 20f;
			}
			else
			{
				disallownewjump = false;
				_maxJumpHeight = 30f;
			}
			jumped = true;
			heightJumped = 0;
		}

	}

	void OnExplode()
	{
		ExplodeFace();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		captureAndSync();
		handleJump();
		control.Move(movement * Time.deltaTime);
 	}

	void captureAndSync()
	{
		movement = (Quaternion.Euler(0, cam.eulerAngles.y, 0) * new Vector3(_movementInput.x, 0.0f, _movementInput.y).normalized) * speed;

		if (movement.magnitude != 0)
			syncAvatarWithDirection();

	}

	void syncAvatarWithDirection()
	{
		Quaternion expectedRotation = Quaternion.LookRotation(movement, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, expectedRotation, turnSmoothTime * Time.deltaTime);
	}

	void handleJump()
	{
		if (jumped && heightJumped < _maxJumpHeight)
		{
			movement.y += ((heightJumped += (_maxJumpHeight / 20)) * 1.5f);
			// Quite literally squashes and stretches the character.
			transform.localScale += new Vector3(0, yScaleFactor, zScaleFactor);
		}
		else
		{
			jumped = false;
			if (transform.localScale != Vector3.one)
				transform.localScale -= new Vector3(0, yScaleFactor, zScaleFactor);
		}

		movement.y += Physics.gravity.y;
	}

	void setRandomColors()
	{
		// Get all renderer components from the object
		Renderer[] bodyMats = GetComponentsInChildren<Renderer>();
		// Make a new list to store objects which have matching colors
		List<GameObject> matchParentsList = new List<GameObject>();

		foreach (var item in bodyMats)
		{
			if (item.transform.parent == null || // Don't add the main body to the list
			    item.transform.parent.tag != "MatchingColors")// Check for the right tag
			{
				// Generate a random color and alpha, then update the Renderer
				Color32 randomColor = new Color32((byte)Random.Range(0, 255),
								  (byte)Random.Range(0, 255),
								  (byte)Random.Range(0, 255),
								  (byte)Random.Range(0, 255));
				item.material.SetColor("_Color", randomColor);
			}
			else if (!matchParentsList.Contains(item.transform.parent.gameObject))
				matchParentsList.Add(item.transform.parent.gameObject);
		}

		// This runs on parents whose children should have matching colors
		foreach (GameObject matchParent in matchParentsList)
		{
			Color32 randomColor = new Color32((byte)Random.Range(0, 255),
							  (byte)Random.Range(0, 255),
							  (byte)Random.Range(0, 255),
							  (byte)Random.Range(0, 255));

			// Get all renderer objects, which are children of the current GO
			Renderer[] matchChildren = matchParent.GetComponentsInChildren<Renderer>();

			foreach (var item in matchChildren)
				item.material.SetColor("_Color", randomColor);

		}
	}

	public void ExplodeFace()
	{
		if (!faceExploded)
		{
			foreach (var item in facePieces)
			{
				Rigidbody faceGrav = item.gameObject.AddComponent<Rigidbody>();
				faceGrav.useGravity = true;
				faceGrav.AddExplosionForce(15f, item.position, 7.5f, 3.0F);
				item.transform.parent = newFaceParent;
			}
			faceExploded = true;
		}
		else
			foreach (var item in facePieces)
				item.gameObject.GetComponent<Rigidbody>().AddExplosionForce(150f, item.position, 75f, 30F);

	}
}
