using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour 
{
	public float movementSpeed = 5;
	public float rotateSpeed = 5;
	public float jumpHeight = 6;
	public float gravity = 20;
	CharacterController controller = null;
	public Transform cameraTransform = null;
	Transform thisTransform = null;
	public int maxSpriteCount;
	int spriteCount;
	Vector3 targetVel = Vector3.zero;

	void Start ()
	{
		controller = GetComponent<CharacterController> ();
		//cameraTransform = Camera.main.transform;
		thisTransform = this.transform;
	}

	void Update ()
	{
		if (controller.isGrounded) 
		{
			//targetVel = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
	
			if (Input.GetAxis ("Vertical") > 0) 
			{
				float camYRot = cameraTransform.localEulerAngles.y;
				Vector3 targetRot = thisTransform.localEulerAngles;
				targetRot.y = camYRot;
				thisTransform.localEulerAngles = targetRot;
			}
	
			targetVel = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			targetVel = this.transform.TransformDirection (targetVel);
			targetVel *= movementSpeed;
		
			if (Input.GetAxis ("Jump") > 0) 
			{
				targetVel.y = jumpHeight;
			}
		}
		
		targetVel.y -= gravity * Time.deltaTime;
		controller.Move (targetVel * Time.deltaTime);
	}
		
}
