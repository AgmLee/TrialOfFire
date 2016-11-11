using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public float floatingGroundHeight = 1.25f;
    public float bottomRayDist = 3;
    public float forwardRayDis = 0.5f;
    public float headRayDist = 2;

    public Transform cam;
    public float groundMovementSpeed = 10;
    public float inAirMovementSpeed = 6;
    public float gravity = -20;
    //float jumpHeight = 10;
    //float jumpSpeed = 2;
    public int forwardRayCount = 5;
    //float rayOffset = 0.01f;

    public float jumpSpeed = 10;
    public float jumpDuration = 0.4f;
    private float curJumpTime = 0;

    PlayerInput input = new PlayerInput();
    Bounds bounds;

    public bool onGround = false;
    public bool onPlatform = false;
    private Vector3 velocity = Vector3.zero;
    private Quaternion walkAngle = Quaternion.identity;

    public Transform rayPos;
    private Vector3 groundpoint;
    private RaycastHit groundHit;

    void Start ()
    {
        bounds = GetComponent<Collider> ().bounds;
        cam = Camera.main.transform;
    }

    void Update ()
    {
        onGround = GroundCollision ();
        input.SetInputValues ();
        Movement();
        Rotation ();
        Jumping ();
    }

    void LateUpdate ()
    {
        if (curJumpTime < 0 && onGround)
        {
            transform.position = Vector3.MoveTowards(transform.position, groundHit.point + (Vector3.up * floatingGroundHeight), 0.1f);
        }
        if (onPlatform)
        {
            Vector3 platformDir = groundHit.transform.GetComponent<MovingPlatform>().GetVelocity();
            transform.position += platformDir * Time.deltaTime;
        }
    }

    void Movement ()
    {
        bool forwardCol = ForwardCollision();
        Vector3 platformVel = Vector3.zero;
        velocity = cam.rotation * new Vector3 (input.horizontal, 0, input.vertical);
        velocity.Normalize ();
        //if (curJumpTime < 0)
        //{
        //    velocity = walkAngle * velocity;
        //}

        if (onGround)
        {
            velocity *= groundMovementSpeed;
        }
        else
        {
            velocity *= inAirMovementSpeed;
        }

        if (ApplyGravity())
        {
            velocity.y += gravity;
        }

        if (onPlatform && false)
        {
            Vector3 platformDir = groundHit.transform.GetComponent<MovingPlatform>().GetVelocity();
            transform.position += (platformDir) * Time.deltaTime;
        }
        else if (!forwardCol && onGround)
        {
            float angle = Vector3.Angle(velocity, groundHit.normal) - 90;
            Vector3 relativeRight = Vector3.Cross(velocity, Vector3.up);
            Vector3 rotatedVel = Quaternion.AngleAxis(angle, relativeRight) * velocity;
            if (groundHit.transform.tag == "Platform")
            {
                platformVel = groundHit.transform.GetComponent<MovingPlatform>().GetVelocity();    
            }

            transform.position += (rotatedVel * Time.deltaTime);
        }
        else if (!forwardCol)
        {
            transform.position += (velocity * Time.deltaTime);
        }

        if (forwardCol && ApplyGravity())
        {
            transform.position += (Vector3.up * gravity) * Time.deltaTime;
        }

    }


    void Rotation ()
    {
        Vector3 lookDir = cam.rotation * new Vector3(input.horizontal, 0, input.vertical);
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }
        
    void Jumping ()
    {
        curJumpTime -= Time.deltaTime;

        if ((curJumpTime < 0 && input.jump > 0) && onGround)
        {
            curJumpTime = jumpDuration;
        }

        if (curJumpTime >= 0 && !HeadCollision ())
        {
            transform.position += (Vector3.up * jumpSpeed) * Time.deltaTime;
        }
    }

    bool GroundCollision ()
    {
        if (Physics.Raycast (rayPos.position, -Vector3.up, out groundHit))
        {
            if ((groundHit.point - (rayPos.position)).sqrMagnitude < (bottomRayDist * bottomRayDist))
            {
                groundpoint = groundHit.point;
                if (groundHit.transform.tag == "Platform")
                {
                    onPlatform = true;
                }
                else
                {
                    onPlatform = false;
                }

                return true;
            }
        }

        onPlatform = false;
        return false;
    }

    bool ForwardCollision ()
    {
        bool collisionDetected = false;
        float separation = 0;
        RaycastHit hit;
        Vector3 rayPoint = Vector3.zero;
        Ray ray;

        for (int i = 0; i < forwardRayCount; i++)
        {
            separation = ((transform.localScale.y * 2.5f) / (forwardRayCount-1));
            rayPoint = new Vector3 (transform.position.x, (transform.position.y + (transform.localScale.y * 2.5f)) - (separation * i), transform.position.z);
            ray = new Ray (rayPoint, transform.forward);

            if (Physics.Raycast (ray, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    break;
                }
                float diff = (hit.point - rayPoint).sqrMagnitude;
                if (diff <= (forwardRayDis * forwardRayDis))
                {
                    collisionDetected = true;
                }
            }
            if (collisionDetected)
            {
                break;
            }
            Debug.DrawLine(rayPoint, rayPoint + transform.forward, Color.red);
        }

        return collisionDetected;
    }

    bool HeadCollision ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * transform.localScale.y), Vector3.up, out hit))
        {
            if ((hit.point - transform.position).sqrMagnitude < (headRayDist * headRayDist))
            {
                return true;
            }
        }
        return false;
    }

    bool ApplyGravity ()
    {
        if (onGround && curJumpTime < 0)
        {
            return false;
        }

        if (!onGround && curJumpTime >= 0)
        {
            return false;
        }

        if (!onGround && curJumpTime < 0)
        {
            return true;
        }

        return false;
    }

    class PlayerInput
    {
        public float horizontal = 0;
        public float vertical = 0;
        public float jump = 0;

        public void SetInputValues ()
        {
            horizontal = Input.GetAxis ("Horizontal");
            vertical = Input.GetAxis ("Vertical");
            jump = Input.GetAxis ("Jump");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(rayPos.position,
            ( rayPos.position )+ (-Vector3.up * bottomRayDist));
    }
}