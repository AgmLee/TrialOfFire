using UnityEngine;

public class CameraController : MonoBehaviour {
    //Reference to the players transform    
    public Transform playerTransform;
    //Position offset
    public Vector3 offset;
    //Angeled offset (LookAt position)
    public float angelOffset;

    //Reference to own transform
    private Transform myTransform;
    //The position to move the camera to
    private Vector3 newPos;

    void Start()
    {
        //Sets players transform reference
        if (playerTransform == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
            {
                playerTransform = obj.GetComponent<Transform>();
            }
        }

        //Sets self transform reference
        if (myTransform == null)
        {
            myTransform = gameObject.transform;
        }

        //Error Checking
        if (myTransform == null || playerTransform == null)
        {
            //Set Message
            string messages = "";
            if (playerTransform == null)
            {
                messages += "Player Transform not found";
            }
            if (myTransform == null)
            {
                if (messages != "")
                {
                    messages += " and ";
                }
                messages += "Transform not found";
            }

            //Send Error
            GameManager.inst.ErrorSystem(messages, this, true, 0);
            Destroy(this);
        }

        //Sets the position
        myTransform.position = playerTransform.position + offset;
        //Sets the rotation
        myTransform.LookAt(new Vector3(playerTransform.position.x, playerTransform.position.y + angelOffset, playerTransform.position.z));
        //Sets the newPos to the current offset
        newPos = offset;
    }

    void Update()
    {   
        //Sets the offset  
        if (!TolComp(newPos, offset, 0.1f))
        {
            Vector3 newDif = Vector3.Lerp(offset, newPos, 5 * Time.deltaTime);
            offset = newDif;
        }           
        else 
        {
            offset = newPos;
        }                
        
        //Sets the position         
        myTransform.position = playerTransform.position + offset;
        //Sets the rotation
        myTransform.LookAt(new Vector3(playerTransform.position.x, playerTransform.position.y + angelOffset, playerTransform.position.z));
    }

    //Set the newPos function
    public void SetCameraDir(Vector3 newDirection)
    {
        newPos = newDirection;        
    }

    //Compare using a Tolerence between two vectors
    bool TolComp(Vector3 l, Vector3 r, float TOLERANCE = 0.00001f)
    {
        if (Mathf.Abs(l.x - r.x) > TOLERANCE ||
            Mathf.Abs(l.y - r.y) > TOLERANCE ||
            Mathf.Abs(l.z - r.z) > TOLERANCE)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
