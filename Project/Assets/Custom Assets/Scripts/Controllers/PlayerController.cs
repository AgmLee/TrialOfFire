using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour 
{
    public bool useDefaultGravity = false;
    public bool useRigidbodyRotation = false;

    public Transform cameraPivotTransform = null;

    //private float groundedRayDistance = 0;
    //public float groundedRayOffsetDist = -1.49f;
    public float movementSpeed = 10;
    public float jumpHeight = 20;
    public float gravity = 1;
    public float maxClimbAngle = 70;

    private Rigidbody rb;
    private RaycastHit raycastHit;
    private Vector3 targetVel;
    private MovingPlatform platform;

    private Vector3 extraVel;

    private bool pHorizontalInput;
    private bool pVerticalInput;
    private bool nHorizontalInput;
    private bool nVerticalInput;
    private bool jumpInput;
    private bool isJumping = false;
    private bool collidingInAir = false;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = useDefaultGravity;
        rb.freezeRotation = !useRigidbodyRotation;
        rb = GetComponent<Rigidbody>();

        //collider = this.transform.GetComponent<Collider> ();
        //groundedRayDistance = collider.bounds.extents.y * groundedRayOffsetDist; 
        //groundedRayDistance *= groundedRayDistance;

        if (cameraPivotTransform == null)
        {
            Debug.Log("No camera transform has been assigned to character script!");
        }

        platform = GameObject.FindGameObjectWithTag("Platform").GetComponent<MovingPlatform>();
    }

    void Update ()
    {
        pVerticalInput = Input.GetKey (PlayerInput.positiveVerticalInput);
        nVerticalInput = Input.GetKey (PlayerInput.negativeVerticalInput);
        pHorizontalInput = Input.GetKey (PlayerInput.positiveHorizontalInput);
        nHorizontalInput = Input.GetKey (PlayerInput.negativeHorizontalInput);
        jumpInput = Input.GetKey(PlayerInput.jumpInput);

        if (IsGrounded ())
        {
            if (jumpInput && !isJumping)
            {
                isJumping = true;
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }
            else if (isJumping && !jumpInput)
            {
                isJumping = false;
            }

            collidingInAir = false;
        }

        //if (platform != null)
        //{
        //    transform.position += (platform.GetVelocity () * Time.deltaTime);
        //}

        //transform.position += extraVel * Time.deltaTime;


        Debug.Log (OnPlatform ());
    }

    void FixedUpdate ()
    {

        if (OnPlatform())
        {
            transform.position += (platform.GetVelocity() * Time.fixedDeltaTime);
        }
        targetVel = GetHeading();
        RotatePlayer(targetVel);

        targetVel *= movementSpeed;
        Vector3 curVel = rb.velocity;
        Vector3 velocityDiff = targetVel - curVel;
        Mathf.Clamp(velocityDiff.x, -movementSpeed, movementSpeed);
        Mathf.Clamp(velocityDiff.z, -movementSpeed, movementSpeed);
        velocityDiff.y = 0;

        float walkAngle = Vector3.Angle(velocityDiff, raycastHit.normal) - 90;

        if (walkAngle < maxClimbAngle && !collidingInAir)
        {
            Vector3 relativeRight = Vector3.Cross(velocityDiff, Vector3.up);
            velocityDiff = Quaternion.AngleAxis(walkAngle, relativeRight) * velocityDiff;
            rb.AddForce(velocityDiff, ForceMode.VelocityChange);
        }
            
        rb.AddForce(-Vector3.up * gravity, ForceMode.Impulse);      
    }

    bool IsGrounded ()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out raycastHit))
        {
            Debug.DrawLine(transform.position, raycastHit.point, Color.green);
            if ((raycastHit.point - transform.position).sqrMagnitude < 0.25f)
            {
                return true;
            }
            //if (Vector3.Distance(transform.position, raycastHit.point) < 0.5f)
            //{
            //    return true;
            //}
        }
       //else if (Physics.Raycast(transform.position + (transform.forward * 0.5f), -Vector3.up, out raycastHit))
       //{
       //    if ((raycastHit.point - transform.position).sqrMagnitude < groundedRayDistance)
       //    {
       //        return true;
       //    }
       //}
       //else if (Physics.Raycast(transform.position - (transform.forward * 0.5f), -Vector3.up, out raycastHit))
       //{
       //    if ((raycastHit.point - transform.position).sqrMagnitude < groundedRayDistance)
       //    {
       //        return true;
       //    }
       //}

        return false;
    }

    bool OnPlatform ()
    {
        float platformMaxX = platform.transform.position.x + (platform.transform.localScale.x/2);
        float platformMinX = platform.transform.position.x - (platform.transform.localScale.x/2);

        float platformMaxZ = platform.transform.position.z + (platform.transform.localScale.z/2);
        float platformMinZ = platform.transform.position.z - (platform.transform.localScale.z/2);

        float platformMaxY = platform.transform.position.y + (transform.localScale.y);
        float platformMinY = platform.transform.position.y + (platform.transform.localScale.y/2);

        if (IsGrounded())
        {
            if (transform.position.x >= platformMinX && transform.position.x <= platformMaxX &&
                transform.position.z >= platformMinZ && transform.position.z <= platformMaxZ &&
                transform.position.y >= platformMinY && transform.position.y <= platformMaxY)
            {
                return true;
            }
        }

        return false;
    }

    void OnCollisionEnter(Collision info)
    {
        if (!IsGrounded())
            collidingInAir = true;
        
        //if (info.transform.tag == "Platform")
        //{
        //    platform = info.transform.GetComponent<MovingPlatform>();
        //}
    }
    //
    //void OnCollisionExit (Collision info)
    //{
    //    if (info.transform.tag == "Platform")
    //    {
    //        platform = null;
    //    }
    //}

    //public void SetExtra(Vector3 value)
    //{
    //    extraVel = value;
    //}

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
