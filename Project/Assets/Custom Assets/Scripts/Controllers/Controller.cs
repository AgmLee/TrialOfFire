using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

    public float floatingGroundHeight = 0.1f;
    public float bottomRayDist = 2.66f;
    public float forwardRayDis = 1f;
    public float headRayDist = 2.81f;
    public float maxWalkAngle = 65f;
    private List<RaycastHit> forwardRayHitPoints;
    private int forwardRayColIndex = -1;

    public Transform cam;
    public float groundMovementSpeed = 20;
    public float inAirMovementSpeed = 20;
    public float gravity = -80;
    public int forwardRayCount = 5;

    public AnimationCurve jumpForceOverTime;
    public float jumpForce = 200;
    public float jumpDuration = 0.3f;
    private float curJumpTime = 0;
    private bool jumpButtonReleased = true;

    PlayerInput input = new PlayerInput();

    public bool onGround = false;
    public bool onPlatform = false;
    private Vector3 velocity = Vector3.zero;

    public Transform rayPos;
    private RaycastHit groundHit;
    int groundRayCount = 4;
    float highestPoint = float.MinValue;

    public Animator animController;

    public AnimationCurve bouncebackZForceCurve;
    public bool bouncingback = false;
    public float bouncebackYForce = 30;
    public float bouncebackZForce = -25;
    private float bouncebackDuration = 0.2f;
    private float curBouncebackTime = 0;
    private Quaternion bounceBackRot = Quaternion.identity;
    private bool touchedEnemy = false;

    void Start ()
    {
        cam = Camera.main.transform;
        if (!animController)
            animController = GetComponent<Animator> ();
    }

    bool wasOnGround = false;
    float timeWhenLanded = -100;
    void Update ()
    {
        onGround = GroundCollision ();
        if (!wasOnGround && onGround)
        {
            wasOnGround = true;
            timeWhenLanded = Time.time;
        }
        else if (wasOnGround && !onGround)
        {
            wasOnGround = false;
            timeWhenLanded = -100;
        }
        input.SetInputValues ();
        if (!bouncingback)
            Movement();
        Rotation ();
        //Jumping ();
        SetAnimationParameters ();
        Bounceback ();
    }
        
    void Bounceback ()
    {
        curBouncebackTime -= Time.deltaTime;

        if (touchedEnemy)
        {
            bouncingback = true;
            curBouncebackTime = bouncebackDuration;
            bounceBackRot = transform.rotation;
            GetComponent<InventoryManager> ().Hurt ();
            touchedEnemy = false;
        }

        if (curBouncebackTime >= 0)
        {
            float desiredYForce = jumpForceOverTime.Evaluate (bouncebackDuration - curBouncebackTime);
            float desiredZForce = bouncebackZForceCurve.Evaluate (bouncebackDuration - curBouncebackTime);
            transform.position += bounceBackRot * new Vector3 (0, bouncebackYForce * desiredYForce, bouncebackZForce * desiredZForce) * Time.deltaTime;
        }
        else
        {
            bouncingback = false;
        }
    }

    void LateUpdate ()
    {
        if (curJumpTime < 0 && onGround)
        {
            //transform.position = Vector3.MoveTowards(transform.position, groundHit.point + (Vector3.up * floatingGroundHeight), 0.2f);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3 (transform.position.x, groundHit.point.y + floatingGroundHeight, transform.position.z), 0.2f);
            //transform.position = Vector3.Slerp (transform.position, new Vector3 (transform.position.x, groundHit.point.y + floatingGroundHeight, transform.position.z), 1 * Time.deltaTime);
        }

        if (onPlatform && groundHit.transform.GetComponent<MovingPlatform> ())
        {
            Vector3 platformDir = groundHit.transform.GetComponent<MovingPlatform>().GetVelocity();
            transform.position += platformDir * Time.deltaTime;
        }
            
    }

    void Movement ()
    {
        bool forwardCol = ForwardCollision();
        velocity = cam.rotation * new Vector3 (input.horizontal, 0, input.vertical);
        velocity.y = 0;
        velocity.Normalize ();

        if (onGround)
        {
            velocity *= groundMovementSpeed;
        }
        else
        {
            velocity *= inAirMovementSpeed;
        }

//        if (ApplyGravity())
//        {
//            velocity.y += gravity;
//        }
            
        //if (!forwardCol && onGround)
        {
            float angle = Vector3.Angle(velocity, groundHit.normal) - 90;
            Vector3 relativeRight = Vector3.Cross(velocity, Vector3.up);
            Vector3 rotatedVel = Quaternion.AngleAxis(angle, relativeRight) * velocity;

//            if (groundHit.transform.tag == "Platform")
//            {
//                platformVel = groundHit.transform.GetComponent<MovingPlatform>().GetVelocity();    
//            }

//            transform.position += (rotatedVel * Time.deltaTime);
            if (!onGround)
            {
                rotatedVel = Quaternion.AngleAxis(0, relativeRight) * velocity;
            }
            
            velocity = rotatedVel;
        }

        if (forwardCol)
        {
            velocity.x = 0;
            velocity.z = 0;
        }
            
        velocity.y = CalculateYForce();
        if (velocity.y == 0)
        {
            velocity += (Vector3.up * bottomRayDist);
        }
            
        transform.position += velocity * Time.deltaTime;

    }

    float curYForce = 0;
    bool jumpButtonDown = false;
    float CalculateYForce ()
    {
        float force = curYForce;
        curJumpTime -= Time.deltaTime;

        if (jumpButtonDown && input.jump <= 0)
        {
            jumpButtonDown = false;
        }

        if (onGround && input.jump > 0 && Time.time > timeWhenLanded + 0.1f && !jumpButtonDown && !HeadCollision ())
        {
            curJumpTime = jumpDuration;
            jumpButtonDown = true;
        }
            
        if (curJumpTime > (jumpDuration * 0.5f) && input.jump > 0 && !HeadCollision())
        {
            float jumpForcePercentage = jumpForceOverTime.Evaluate((jumpDuration - curJumpTime) / jumpDuration);
            force += (jumpForce * jumpForcePercentage) * Time.deltaTime;
            velocity.z *= 0.5f;
            velocity.x *= 0.5f;
        }
        else if (!onGround)
        {
            if (HeadCollision())
            {
                force += gravity * 20 * Time.deltaTime;
            }
            else
            {
                force += gravity * Time.deltaTime;
            }
        }
        else
        {
            force = 0;
        }

        curYForce = force;
        return force;
    }

    void Rotation ()
    {
        Vector3 lookDir = cam.rotation * new Vector3(input.horizontal, 0, input.vertical);
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }

//    void Jumping ()
//    {
//        curJumpTime -= Time.deltaTime;
//
//        if (input.jump <= 0)
//        {
//            jumpButtonReleased = true;
//        }
//
//        if ((curJumpTime < 0 && input.jump > 0) && onGround && jumpButtonReleased)
//        {
//            curJumpTime = jumpDuration;
//            jumpButtonReleased = false;
//        }
//
//        //if (curJumpTime >= 0 && !HeadCollision ())
//        //{
//        //    transform.position += (Vector3.up * jumpSpeed) * Time.deltaTime;
//        //}
//
//        if (curJumpTime >= 0 && !HeadCollision ())
//        {
//            float desiredJumpForce = jumpCurve.Evaluate (jumpDuration - curJumpTime) * jumpForce;
//            transform.position += (Vector3.up * desiredJumpForce) * Time.deltaTime;
//        }
//
//        if (HeadCollision ())
//        {
//            curJumpTime = -1;
//        }
//
//    }
    float delay = 0;
    float delayAmount = 0.1f;
    void SetAnimationParameters ()
    {
        delay -= Time.deltaTime;
        if (onGround && input.vertical == 0 && input.horizontal == 0 && delay < 0)
        {
            animController.SetBool ("IDLE", true);
            animController.SetBool ("MOVEMENT", false);
            animController.SetBool ("JUMP", false);
            animController.SetBool ("LAND", false);
            delay = delayAmount;
        }

        if (onGround && (input.vertical != 0 || input.horizontal != 0))
        {
            animController.SetBool ("MOVEMENT", true);
            animController.SetBool ("IDLE", false);
            animController.SetBool ("JUMP", false);
            animController.SetBool ("LAND", false);
            delay = delayAmount;
        }

        if ((input.jump > 0) && onGround && curJumpTime >= jumpDuration)
        {
            animController.SetBool ("JUMP", true);
            animController.SetBool ("IDLE", false);
            animController.SetBool ("MOVEMENT", false);
            animController.SetBool ("LAND", false);
            delay = delayAmount;
        }

        if (!onGround && curJumpTime < 0)
        {
            animController.SetBool ("LAND", true);
            animController.SetBool ("JUMP", false);
            animController.SetBool ("IDLE", false);
            animController.SetBool ("MOVEMENT", false);
            delay = delayAmount;
        }
    }


    bool GroundCollision ()
    {
        float separation = 360 / groundRayCount;
        bool collision = false;
    
        for (int i = 0; i < groundRayCount; i++)
        {
            Vector3 rayPoint = Quaternion.Euler(0, separation * i + transform.localEulerAngles.y, 0) * (new Vector3 (0, rayPos.position.y, -0.5f));
            rayPoint += new Vector3 (transform.position.x, 0, transform.position.z);
    
            if (Physics.Raycast (rayPoint, -Vector3.up, out groundHit))
            {
                if ((groundHit.point - rayPoint).sqrMagnitude < (bottomRayDist * bottomRayDist))
                {
                    if (groundHit.transform.tag == "Platform")
                    {
                        onPlatform = true;
                    }
                    else
                    {
                        onPlatform = false;
                    }

                    collision = true;

                    if (groundHit.point.y > highestPoint)
                    {
                        highestPoint = groundHit.point.y;
                    }
                }
            }
    
            Debug.DrawLine (rayPoint, rayPoint + -Vector3.up, Color.red);
        }
    
        return collision;
    }

    //bool GroundCollision ()
    //{
    //    if (Physics.Raycast (rayPos.position, -Vector3.up, out groundHit))
    //    {
    //        if ((groundHit.point - (rayPos.position)).sqrMagnitude < (bottomRayDist * bottomRayDist))
    //        {
    //            if (groundHit.transform.tag == "Platform")
    //            {
    //                onPlatform = true;
    //            }
    //            else
    //            {
    //                onPlatform = false;
    //            }
    //
    //            if (groundHit.transform.GetComponent<Enemy>())
    //            {
    //                touchedEnemy = true;
    //            }
    //
    //            return true;
    //        }
    //    }
    //
    //    onPlatform = false;
    //    return false;
    //}
        
    bool ForwardCollision ()
    {
        float separation = 0;
        RaycastHit hit;
        Vector3 rayPoint = Vector3.zero;
        Ray ray;
        forwardRayHitPoints = new List<RaycastHit> ();
    
        for (int i = 0; i < forwardRayCount; i++)
        {
            separation = ((transform.localScale.y * 2.5f) / (forwardRayCount-1));
            rayPoint = new Vector3 (transform.position.x, (transform.position.y + (transform.localScale.y * 2.5f)) - (separation * i), transform.position.z);
            ray = new Ray (rayPoint, transform.forward);

            if (Physics.Raycast(ray, out hit))
            {
                forwardRayHitPoints.Add (hit);
            }
    
            Debug.DrawLine(rayPoint, rayPoint + transform.forward, Color.red);
        }
        // Check our rays - if any of them is perpecdicular to Vector.up then its a wall
        for (int i = 0; i < forwardRayHitPoints.Count; i++)
        {
            float diff = (forwardRayHitPoints[i].point - new Vector3 (transform.position.x, forwardRayHitPoints[i].point.y, transform.position.z)).sqrMagnitude;
            if (diff <= (forwardRayDis * forwardRayDis))
            {
                if (forwardRayHitPoints[i].transform.GetComponent<Enemy>())
                {
                    touchedEnemy = true;
                    break;
                }

                float angle = Vector3.Angle (Vector3.up, forwardRayHitPoints[i].normal);
                if (angle > maxWalkAngle)
                {
                    forwardRayColIndex = i;
                    return true;
                }

            }
        }

        forwardRayColIndex = -1;
        return false;
    }
    //bool ForwardCollision ()
    //{
    //    bool collisionDetected = false;
    //    float separation = 0;
    //    RaycastHit hit;
    //    Vector3 rayPoint = Vector3.zero;
    //    Ray ray;
    //
    //    for (int i = 0; i < forwardRayCount; i++)
    //    {
    //        separation = ((transform.localScale.y * 2.5f) / (forwardRayCount-1));
    //        rayPoint = new Vector3 (transform.position.x, (transform.position.y + (transform.localScale.y * 2.5f)) - (separation * i), transform.position.z);
    //        ray = new Ray (rayPoint, transform.forward);
    //
    //        if (Physics.Raycast (ray, out hit))
    //        {
    //            if (hit.transform.tag == "Ground")
    //            {
    //                break;
    //            }
    //
    //            float diff = (hit.point - rayPoint).sqrMagnitude;
    //            if (diff <= (forwardRayDis * forwardRayDis))
    //            {
    //                if (hit.transform.GetComponent<Enemy>())
    //                {
    //                    touchedEnemy = true;
    //                    collisionDetected = true;
    //                    break;
    //                }
    //
    //                float walkAngle = Vector3.Angle (transform.forward, hit.normal) - 90;
    //                if (walkAngle <= maxWalkAngle && i != 0)
    //                {
    //                    collisionDetected = false;
    //                    Debug.Log (walkAngle);
    //                    break;
    //                }
    //                else
    //                {
    //                    collisionDetected = true;
    //                }
    //
    //            }
    //        }
    //
    //        if (collisionDetected)
    //        {
    //            break;
    //        }
    //
    //        Debug.DrawLine(rayPoint, rayPoint + transform.forward, Color.red);
    //    }
    //
    //    return collisionDetected;
    //}

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