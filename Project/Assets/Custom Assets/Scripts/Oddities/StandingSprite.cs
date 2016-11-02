using UnityEngine;       
                           
public class StandingSprite : MonoBehaviour
{
    //Settings
    public float resetTime = 1.0f;
    public AnimationCurve bob;
    //Private Variables
    private float offsetY = 0.0f;
    //References
    private Transform ownTrans;
    private Transform refer = null;
    private SphereCollider ownCol;
    public AudioSource audioS;      

    // Use this for initialization
    void Start () {
        ownTrans = transform;
        offsetY = ownTrans.localPosition.y;                 
        ownCol = gameObject.GetComponent<SphereCollider>();           
	}


    private float timer = 0.0f;        
    void Update ()
    {
        transform.localPosition = new Vector3(ownTrans.localPosition.x, offsetY + bob.Evaluate(Time.time), ownTrans.localPosition.z);
        if (ownCol)
        {
            if (refer != null)
            {
                ownTrans.LookAt(refer);
                ownTrans.rotation = Quaternion.LookRotation(-ownTrans.forward);
            }
            else if (timer >= resetTime)
            {
                ownTrans.rotation = new Quaternion(0.0f, ownTrans.rotation.y, 0.0f, ownTrans.rotation.z);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (audioS)
            {
                audioS.Play();
            }
            refer = col.gameObject.transform;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            refer = null;
        }
    }
}
