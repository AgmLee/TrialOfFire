using UnityEngine;
public class InventoryManager : MonoBehaviour {
    //Sprites
    public int maxSpriteCount = 3;
    public int spriteCount = 0;
    //Transforms
    public Transform rotationTransform;
    public Transform player;
    public Transform[] spritePoints;
    //References
    public GameObject standingSprite;
    public GameObject movingSprite;
    //Settings
    public float rotationSpeed = 5.0f;
    public float xAngle = 15.0f;
    public float zAngle = 15.0f;
    public bool flip = false;
    public AnimationCurve bob;
    //Private Variables
    private GameObject[] sprites;
    private int index = 0;
    private bool input = false;
    private bool takeBack = false;
    private bool remove = false;
    private GameObject[] dirs;
    private int amntBck = 0;
    private GameObject takeBck;

    void Start()
    {
        if (flip)
        {
            rotationSpeed = -rotationSpeed;
        }
        rotationTransform.localEulerAngles = new Vector3(xAngle, 0.0f, zAngle);
        index = spriteCount;
        sprites = new GameObject[spritePoints.Length];
    }

    void Update()
    {
        //Rotate the rotation transform
        rotationTransform.position = transform.position + Vector3.up * 0.5f;
        rotationTransform.Rotate(Vector3.up, rotationSpeed);
        
        //Input
        if (Input.GetButtonDown("Primary") && input && spriteCount >= amntBck)
        {
            foreach(GameObject d in dirs)
            {
                GameObject o = Instantiate(movingSprite, transform.position, Quaternion.LookRotation(d.transform.position - transform.position)) as GameObject;
                o.GetComponent<SpriteMovement>().refe = d;
            }
            spriteCount -= amntBck;
            if (takeBck != null && remove)
            {
                takeBck.GetComponent<Activation>().isActive = true;
                InputSet(false, 0);
                SetBack(false, 0);
            }
        }
        if (Input.GetButtonDown("Secondary") && takeBack)
        {
            for(int i = 0; i < amntBck; i++)
            {
                GameObject o = Instantiate(movingSprite, takeBck.transform.position, Quaternion.LookRotation(takeBck.transform.position - transform.position)) as GameObject;
                o.GetComponent<SpriteMovement>().refe = gameObject;
            }
            takeBck.SendMessage("Activation");
        }
        
        //Spawn/Delete sprties
        if (spriteCount > maxSpriteCount)
        {
            spriteCount = maxSpriteCount;
        }
        if (index != spriteCount)
        {
            if (index < spriteCount && index < 6)
            {
                while (index != spriteCount && index < 6)
                {
                    sprites[index] = Instantiate(standingSprite, spritePoints[index], false) as GameObject;
                    index++;
                }
            }
            else if (index > spriteCount)
            {
                while (index != spriteCount)
                {
                    index--;
                    Destroy(sprites[index]);
                }
            }
        }
        int pass = 1;
        foreach(Transform t in spritePoints)
        {
            t.rotation = Quaternion.LookRotation(-transform.forward);
            t.localPosition = new Vector3(t.localPosition.x, bob.Evaluate(Time.time), t.localPosition.z);
            pass++;
        }
    }

    public void Activation()
    {
        spriteCount++;
    }

    public void SetBack(bool set, int amountBack, GameObject refe = null, bool remve = false)
    {
        remove = remve;
        takeBack = set;
        amntBck = amountBack;
        takeBck = refe;
    }

    public void InputSet(bool set, int amountLost, GameObject[] destinations = null)
    {
        input = set;
        dirs = destinations;
        amntBck = amountLost;
    }
}
