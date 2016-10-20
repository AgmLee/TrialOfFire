using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RBCharacterController : MonoBehaviour {

	public Transform cameraTransform = null;
	public float movementSpeed = 10;
	public float jumpForce = 30;
	public float gravity = 1;

	public bool grounded;

	private Rigidbody rb = null;
	private KeyCode positiveVerticalInput = KeyCode.W;
	private KeyCode negativeVerticalInput = KeyCode.S;
	private KeyCode positiveHorizontalInput = KeyCode.D;
	private KeyCode negativeHorizontalInput = KeyCode.A;

	private bool pHorizontalInput;
	private bool pVerticalInput;
	private bool nHorizontalInput;
	private bool nVerticalInput;
	private Vector3 dir = Vector3.zero;
	private Vector3 XYDir = Vector3.zero;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;
	}

	void FixedUpdate ()
	{
		pVerticalInput = Input.GetKey (positiveVerticalInput);
		nVerticalInput = Input.GetKey (negativeVerticalInput);
		pHorizontalInput = Input.GetKey (positiveHorizontalInput);
		nHorizontalInput = Input.GetKey (negativeHorizontalInput);

		XYDir = new Vector3 (rb.velocity.x, 0, rb.velocity.z);

		if (XYDir.magnitude > movementSpeed) 
		{
			XYDir.Normalize ();
			XYDir *= movementSpeed;
			XYDir.y = rb.velocity.y;
			rb.velocity = XYDir;
		}

		if (IsGrounded()) 
		{
			if (Input.GetAxis ("Jump") > 0) 
			{
				rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}
		} 

		dir = ((cameraTransform.forward * (pVerticalInput ? 1 : (nVerticalInput ? -1 : 0))) + (cameraTransform.right * (pHorizontalInput ? 1 : (nHorizontalInput ? -1 : 0))));
		transform.TransformDirection (dir);
		if (dir.magnitude > 0)
			transform.rotation = Quaternion.LookRotation (dir);
		dir.Normalize ();
		if (!TouchingSides())
			rb.AddForce (dir, ForceMode.Impulse);
		rb.AddForce (-Vector3.up * gravity, ForceMode.Impulse);
		grounded = false;
	}

	bool TouchingSides ()
	{
		return (Physics.Raycast(transform.position, dir, transform.GetComponent<Collider>().bounds.extents.z + 0.1f));
	}

	bool IsGrounded ()
	{
		return (Physics.Raycast (transform.position, -Vector3.up, transform.GetComponent<Collider>().bounds.extents.y + 0.1f)); 
	}
}
