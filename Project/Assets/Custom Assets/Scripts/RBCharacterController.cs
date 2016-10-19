using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RBCharacterController : MonoBehaviour {

	public Transform cameraTransform = null;
	public float movementSpeed = 10;
	public float rotateSpeed = 2;
	public float jumpForce = 2;

	public int spriteCount;
	public int maxSpriteCount;

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

	void Update ()
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

		if (IsGrounded ()) 
		{
			dir = ((cameraTransform.forward * (pVerticalInput ? 1 : (nVerticalInput ? -1 : 0))) + (cameraTransform.right * (pHorizontalInput ? 1 : (nHorizontalInput ? -1 : 0))));
			transform.TransformDirection (dir);
			transform.rotation = XYDir.magnitude == 0 ? transform.rotation : Quaternion.LookRotation (dir);
			dir.Normalize ();
			if (Input.GetAxis ("Jump") > 0) 
			{
				rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}
			rb.AddForce (dir, ForceMode.VelocityChange);
		}

	}

	bool IsGrounded ()
	{
		return (Physics.Raycast (transform.position, -Vector3.up, 1.5f)); 
	}

}
