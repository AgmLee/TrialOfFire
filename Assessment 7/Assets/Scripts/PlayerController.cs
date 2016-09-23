using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed = 10;
	public float rotateSpeed = 1;
	public bool rigidbodyRotation = false;
	Rigidbody rigidbody = null;
	float horizontalMovement;
	float verticalMovement;

	void Start ()
	{
		rigidbody = this.GetComponent<Rigidbody> ();
		if (rigidbodyRotation)
			rigidbody.freezeRotation = true;
		else
			rigidbody.freezeRotation = false;
	}

	void FixedUpdate ()
	{
		horizontalMovement += Input.GetAxis ("Horizontal");
		verticalMovement = Input.GetAxis ("Vertical");

		Vector3 movement = transform.TransformDirection(new Vector3 (0, 0, verticalMovement));
		rigidbody.velocity = movement * moveSpeed;
		rigidbody.MoveRotation (Quaternion.Euler (Vector3.up * horizontalMovement * rotateSpeed));
	}
}
