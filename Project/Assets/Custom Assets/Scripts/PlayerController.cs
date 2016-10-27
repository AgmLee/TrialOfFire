using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour 
{
    public bool useDefaultGravity = false;
    public bool useRigidbodyRotation = false;

    public Transform cameraPivotTransform = null;

    private float groundedRayDistance = -1.49f;
    public float movementSpeed = 10;
    public float jumpHeight = 5;
    public float gravity = 1;
    public float maxClimbAngle = 70;

    private Rigidbody rb;
    private RaycastHit raycastHit;
    private Vector3 targetVel;

    private bool pHorizontalInput;
    private bool pVerticalInput;
    private bool nHorizontalInput;
    private bool nVerticalInput;
    private bool jumpInput;
    private bool isJumping = false;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = useDefaultGravity;
        rb.freezeRotation = !useRigidbodyRotation;
        rb = GetComponent<Rigidbody>();

        groundedRayDistance = this.transform.GetComponent<Collider>().bounds.extents.y * -1.49f;
        groundedRayDistance *= groundedRayDistance;

        if (cameraPivotTransform == null)
        {
            Debug.Log("No camera transform has been assigned to character script!");
        }
    }

    void Update ()
    {
        Debug.Log("grounded");
        pVerticalInput = Input.GetKey (PlayerInput.positiveVerticalInput);
        nVerticalInput = Input.GetKey (PlayerInput.negativeVerticalInput);
        pHorizontalInput = Input.GetKey (PlayerInput.positiveHorizontalInput);
        nHorizontalInput = Input.GetKey (PlayerInput.negativeHorizontalInput);
        jumpInput = Input.GetKey(PlayerInput.jumpInput);
    }

    void FixedUpdate ()
    {
        targetVel = GetHeading();
        RotatePlayer(targetVel);

        targetVel *= movementSpeed;
        Vector3 curVel = rb.velocity;
        Vector3 velocityDiff = targetVel - curVel;
        Mathf.Clamp(velocityDiff.x, -movementSpeed, movementSpeed);
        Mathf.Clamp(velocityDiff.z, -movementSpeed, movementSpeed);
        velocityDiff.y = 0;

        float walkAngle = Vector3.Angle(velocityDiff, raycastHit.normal) - 90;
        if (walkAngle < maxClimbAngle)
        {
            Vector3 relativeRight = Vector3.Cross(velocityDiff, Vector3.up);
            velocityDiff = Quaternion.AngleAxis(walkAngle, relativeRight) * velocityDiff;

            rb.AddForce(velocityDiff, ForceMode.VelocityChange);
        }

        if (IsGrounded ())
        {
            if (jumpInput && !isJumping)
            {
                isJumping = true;
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }
            else
            {
                isJumping = false;
            }
        }

        rb.AddForce(-Vector3.up * gravity, ForceMode.Impulse);
    }

    bool IsGrounded ()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out raycastHit))
        {
            if ((raycastHit.point - transform.position).sqrMagnitude < groundedRayDistance)
            {
                return true;
            }
        }
        return false;
    }

    void RotatePlayer (Vector3 lookVector)
    {
        if (lookVector.magnitude > 0) 
        {
            transform.rotation = Quaternion.LookRotation (lookVector);
        }
    }

    Vector3 GetHeading ()
    {
        Vector3 dir = Vector3.zero;

        if (pVerticalInput)
        {
            dir += cameraPivotTransform.forward;
        }
        else if (nVerticalInput)
        {
            dir -= cameraPivotTransform.forward;
        }

        if (pHorizontalInput)
        {
            dir += cameraPivotTransform.right;
        }
        else if (nHorizontalInput)
        {
            dir -= cameraPivotTransform.right;
        }

        dir.Normalize();
        transform.TransformDirection (dir);
        return dir;

//        Vector3 dir = ((cameraPivotTransform.forward * (pVerticalInput ? 1 : (nVerticalInput ? -1 : 0))) + (cameraPivotTransform.right * (pHorizontalInput ? 1 : (nHorizontalInput ? -1 : 0))));
//        transform.TransformDirection (dir);
//        return dir;
    }
}

public static class PlayerInput 
{
    public static KeyCode positiveVerticalInput = KeyCode.W;
    public static KeyCode negativeVerticalInput = KeyCode.S;
    public static KeyCode positiveHorizontalInput = KeyCode.D;
    public static KeyCode negativeHorizontalInput = KeyCode.A;
    public static KeyCode jumpInput = KeyCode.Space;
}
