using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RBCharacterController : MonoBehaviour {

	Rigidbody rb = null;
	public Transform cameraTransform = null;
	public float movementSpeed = 10;
	public float rotateSpeed = 2;

	bool horizontalInput;
	bool verticalInput;

	Vector3 forward;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		horizontalInput = Input.GetAxis ("Horizontal") != 0 ? true : false;
		verticalInput = Input.GetAxis ("Vertical") != 0 ? true : false;

		if (verticalInput) 
		{
			Vector3 rot = transform.localEulerAngles;
			rot.y = cameraTransform.localEulerAngles.y;
			transform.localEulerAngles = rot;
			forward = rot;
		}

		if (horizontalInput) 
		{
			Vector3 rot = transform.localEulerAngles;
			rot.y = forward.y + (90 * Input.GetAxis ("Horizontal"));
			transform.localEulerAngles = rot;
		}

		rb.velocity = transform.forward * (movementSpeed * Input.GetAxis ("Vertical"));
	}
}
