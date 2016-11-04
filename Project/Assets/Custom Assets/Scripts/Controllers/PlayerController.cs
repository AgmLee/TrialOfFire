using UnityEngine;
using System.Collections;


public static class PlayerInput 
{
    public static KeyCode positiveVerticalInput = KeyCode.W;
    public static KeyCode negativeVerticalInput = KeyCode.S;
    public static KeyCode positiveHorizontalInput = KeyCode.D;
    public static KeyCode negativeHorizontalInput = KeyCode.A;
    public static KeyCode jumpInput = KeyCode.Space;
}


[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour 
{
    public bool useDefaultGravity = false;
    public bool useRigidbodyRotation = false;

    public Transform cameraPivotTransform = null;
    [Range(0,1)]
    public float groundCheckLength = 0.75f;
    public float movementSpeed = 10;
    public float jumpHeight = 20;
    public float gravity = 1;
    public float maxClimbAngle = 70;

    private int platformNo = -1;
    private float lastFps = 0;
    private float fps = 0;
    private float avgFps = 0;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    private Rigidbody rb;
    private RaycastHit raycastHit;
    private Vector3 targetVel;
    private MovingPlatform[] platforms = null;
    private Vector3 collisionPoint = Vector3.zero;

    private bool jumpInput;
    private bool isJumping = false;
    private bool collidingInAir = false;
    private bool stillOnPlatform = false;
    private bool checkForPlatform = false;
    private bool grounded = false;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = useDefaultGravity;
        rb.freezeRotation = !useRigidbodyRotation;
        rb = GetComponent<Rigidbody>();

        GameObject[] plats = GameObject.FindGameObjectsWithTag("Platform");
        platforms = new MovingPlatform[plats.GetLength(0)];

        for (int i = 0; i < platforms.GetLength(0); i++)
        {
            platforms[i] = plats[i].GetComponent<MovingPlatform>();
        }
    }

    void OnApplicationQuit ()
    {
        Debug.Log("Average FPS: " +avgFps);
    }

    void Update ()
    {
        fps = Time.frameCount / Time.time;
        avgFps = (fps + lastFps) / 2;
        lastFps = fps;

        jumpInput = Input.GetButton ("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        grounded = IsGrounded();

        if (grounded && checkForPlatform)
        {
            platformNo = GetPlatform();
            checkForPlatform = false;
        }

        if (grounded)
        {
            if (jumpInput && !isJumping)
            {
                isJumping = true;
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
                platformNo = -1;
                stillOnPlatform = false;
            }
            else if (isJumping && !jumpInput)
            {
                isJumping = false;
            }

            collidingInAir = false;
            collisionPoint = Vector3.zero;
        }
        else
        {
            checkForPlatform = true;
        }

        if (transform.position.y < -1000)
        {
            LevelManager.instance.SpawnPlayer();
        }
    }

    void FixedUpdate ()
    {

        if (platformNo > -1 && grounded)
        {
            transform.position += (platforms[platformNo].GetVelocity() * Time.fixedDeltaTime);
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
        //else if (walkAngle < maxClimbAngle && collidingInAir)
        //{
        //    Vector3 relativeRight = Vector3.Cross(velocityDiff, Vector3.up);
        //    velocityDiff = Quaternion.AngleAxis(walkAngle, relativeRight) * velocityDiff;
        //    float dot = Vector3.Dot((new Vector3(collisionPoint.x, transform.position.y, collisionPoint.z) - transform.position).normalized, velocityDiff.normalized);
        //    if (dot < 0.0f)
        //    {
        //        rb.AddForce(velocityDiff, ForceMode.VelocityChange);
        //    }
        //}
   
        rb.AddForce(-Vector3.up * gravity, ForceMode.Impulse);      
    }

    bool IsGrounded ()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out raycastHit))
        {
            if ((raycastHit.point - transform.position).sqrMagnitude < groundCheckLength)
            {
                return true;
            }
        }
        else if (Physics.Raycast(transform.position + (transform.forward * groundCheckLength), -Vector3.up, out raycastHit))
       {
            if ((raycastHit.point - transform.position).sqrMagnitude < groundCheckLength)
           {
               return true;
           }
       }
        else if (Physics.Raycast(transform.position - (transform.forward * groundCheckLength), -Vector3.up, out raycastHit))
       {
            if ((raycastHit.point - transform.position).sqrMagnitude < groundCheckLength)
           {
               return true;
           }
       }

        return false;
    }

    int GetPlatform ()
    {
        //Debug.Log("Looking for platform");
        for (int i = 0; i < platforms.GetLength(0); i++)
        {
            float platformMaxX = platforms[i].transform.position.x + (platforms[i].transform.localScale.x / 2);
            float platformMinX = platforms[i].transform.position.x - (platforms[i].transform.localScale.x / 2);

            float platformMaxZ = platforms[i].transform.position.z + (platforms[i].transform.localScale.z / 2);
            float platformMinZ = platforms[i].transform.position.z - (platforms[i].transform.localScale.z / 2);

            float platformMaxY = platforms[i].transform.position.y + (transform.localScale.y);
            float platformMinY = platforms[i].transform.position.y + (platforms[i].transform.localScale.y / 2);

            if (IsGrounded())
            {
                if (transform.position.x >= platformMinX && transform.position.x <= platformMaxX &&
                transform.position.z >= platformMinZ && transform.position.z <= platformMaxZ &&
                transform.position.y >= platformMinY && transform.position.y <= platformMaxY)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    void OnCollisionEnter(Collision info)
    {
        if (!grounded)
        {
            collidingInAir = true;
            collisionPoint = info.contacts[0].point;
        }
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

        dir += cameraPivotTransform.forward * verticalInput;
        dir += cameraPivotTransform.right * horizontalInput;

        dir.Normalize();
        transform.TransformDirection (dir);
        return dir;
    }
}
