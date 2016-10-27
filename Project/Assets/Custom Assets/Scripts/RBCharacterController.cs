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

    private RaycastHit raycastHit;
    private bool isJumping = false;
    private float groundedRayDistance = -1.5f;

    void Start ()
    {
        rb = GetComponent<Rigidbody> ();
        rb.freezeRotation = true;
        groundedRayDistance = -this.transform.GetComponent<Collider>().bounds.extents.y - 1;
        groundedRayDistance *= groundedRayDistance;
    }

    void Update ()
    {

        pVerticalInput = Input.GetKey (positiveVerticalInput);
        nVerticalInput = Input.GetKey (negativeVerticalInput);
        pHorizontalInput = Input.GetKey (positiveHorizontalInput);
        nHorizontalInput = Input.GetKey (negativeHorizontalInput);

        if (IsGrounded()) 
        {
            if (Input.GetAxis("Jump") > 0) 
            {
                if (!isJumping)
                {
                    isJumping = true;
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }
            else
            {
                isJumping = false;
            }
        } 
    }

    void FixedUpdate ()
    {
        XYDir = new Vector3 (rb.velocity.x, 0, rb.velocity.z);

        if (XYDir.magnitude > movementSpeed) 
        {
            XYDir.Normalize ();
            XYDir *= movementSpeed;
            XYDir.y = rb.velocity.y;
            rb.velocity = XYDir;
        }

        dir = ((cameraTransform.forward * (pVerticalInput ? 1 : (nVerticalInput ? -1 : 0))) + (cameraTransform.right * (pHorizontalInput ? 1 : (nHorizontalInput ? -1 : 0))));
        transform.TransformDirection (dir);

        if (dir.magnitude > 0) {
            transform.rotation = Quaternion.LookRotation (dir);
        }


        float AA = Vector3.Angle(dir, raycastHit.normal) - 90;

        // if(AA > 5)
        {
            Vector3 RIGHT = Vector3.Cross(dir, Vector3.up);
            dir =  Quaternion.AngleAxis(AA, RIGHT) * dir;
        }


        dir.Normalize ();
        if (!TouchingSides())
        {
            rb.AddForce(dir, ForceMode.Impulse);
        }
        rb.AddForce (-Vector3.up * gravity, ForceMode.Impulse);
        grounded = false;
    }

    bool TouchingSides ()
    {
        return (Physics.Raycast(transform.position, dir, transform.GetComponent<Collider>().bounds.extents.z + 0.1f));
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

}