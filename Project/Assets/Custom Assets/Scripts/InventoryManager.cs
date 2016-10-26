using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    //Sprites
    public int maxSpriteCount = 3;
    public int spriteCount = 0;
    //Transforms
    public Transform rotationTransform;    
    public Transform[] spritePoints;
    //References
    public GameObject standingSprite;
    public GameObject movingSprite;
    public Text UIText;
    public Image UISprite;
    public Image UIBack;
    public RectTransform UITransform;
    //Settings
    public float rotationSpeed = 5.0f;
    public float xAngle = 15.0f;
    public float zAngle = 15.0f;
    public bool flip = false;
    public float fadeSpeed = 5.0f;
    public float scaleSpeed = 5.0f;
    //Private Variables
    private float[] transperancyAmount;
    private GameObject[] sprites;
    private int index = 0;
    private bool input = false;
    private bool takeBack = false;
    private bool remove = false;
    private GameObject[] dirs;
    private int amntBck = 0;
    private GameObject takeBck;
    private float startSize = 0;
    private bool showUI = false;
    public bool activateUI = false;
    private float onTime = 0;
    private AudioSource aus;

    void Start()
    {
        aus = GetComponent<AudioSource>();
        if (flip)
        {
            rotationSpeed = -rotationSpeed;
        }
        rotationTransform.localEulerAngles = new Vector3(xAngle, 0.0f, zAngle);
        index = spriteCount;
        sprites = new GameObject[spritePoints.Length];
        if (UITransform)
        {
            startSize = UITransform.localScale.x;
            UITransform.localScale = new Vector3(0, UITransform.localScale.y, UITransform.localScale.z);
        }
        if (UISprite && UIText && UIBack)
        {
            transperancyAmount = new float[3];
            transperancyAmount[0] = UIBack.color.a;
            transperancyAmount[1] = UISprite.color.a;
            transperancyAmount[2] = UIText.color.a;
            UISprite.color = new Color(UISprite.color.r, UISprite.color.g, UISprite.color.b, 0);
            UIText.color = new Color(UIText.color.r, UIText.color.g, UIText.color.b, 0);
            UIBack.color = new Color(UIBack.color.r, UIBack.color.g, UIBack.color.b, 0);
        }
    }

    void Update()
    {   
        //Rotate the rotation transform
        rotationTransform.position = transform.position + Vector3.up * 0.5f;
        rotationTransform.Rotate(Vector3.up, rotationSpeed);

        if (input || takeBack || activateUI)
        {
            activateUI = false;
            showUI = true;
            onTime = 1;
        }

        if (onTime > 0)
        {
            onTime -= Time.deltaTime;
        }
        if (onTime <= 0)
        {
            showUI = false;
        }

        //Show UI
        if (showUI && (UISprite && UIText && UIBack))
        {
            if (UIBack.color.a < transperancyAmount[0])
            {
                UIBack.color = new Color(UIBack.color.r, UIBack.color.g, UIBack.color.b, UIBack.color.a + fadeSpeed * Time.deltaTime);
            }
            if (UISprite.color.a < transperancyAmount[1])
            {
                UISprite.color = new Color(UISprite.color.r, UISprite.color.g, UISprite.color.b, UISprite.color.a + fadeSpeed * Time.deltaTime);
            }
            if (UIText.color.a < transperancyAmount[2])
            {
                UIText.color = new Color(UIText.color.r, UIText.color.g, UIText.color.b, UIText.color.a + fadeSpeed * Time.deltaTime);
            }
        }
        else if (!showUI && (UISprite && UIText && UIBack))
        {
            if (UISprite.color.a > 0)
            {
                UISprite.color = new Color(UISprite.color.r, UISprite.color.g, UISprite.color.b, UISprite.color.a - fadeSpeed * Time.deltaTime);
                UIText.color = new Color(UIText.color.r, UIText.color.g, UIText.color.b, UIText.color.a - fadeSpeed * Time.deltaTime);
                UIBack.color = new Color(UIBack.color.r, UIBack.color.g, UIBack.color.b, UIBack.color.a - fadeSpeed * Time.deltaTime);
            }
            if (UISprite.color.a <= 0)
            {
                showUI = false;
            }
        }

        if (showUI && UITransform)
        {              
            if (UITransform.localScale.x < startSize)
            {
                UITransform.localScale = new Vector3(UITransform.localScale.x + scaleSpeed * Time.deltaTime, UITransform.localScale.y, UITransform.localScale.z);
            }
            if (UITransform.localScale.x > startSize)
            {
                UITransform.localScale = new Vector3(startSize, UITransform.localScale.y, UITransform.localScale.z);
            }
        }
        else if (!showUI && UITransform)
        {
            if (UITransform.localScale.x > 0)
            {
                UITransform.localScale = new Vector3(UITransform.localScale.x - scaleSpeed * Time.deltaTime, UITransform.localScale.y, UITransform.localScale.z);
            }
            if (UITransform.localScale.x <= 0)
            {
                UITransform.localScale = new Vector3(0, UITransform.localScale.y, UITransform.localScale.z);
                showUI = false;
            } 
        }
        //Input
        if (Input.GetButtonDown("Primary") && input && spriteCount >= amntBck)
        {
            foreach(GameObject d in dirs)
            {
                GameObject o = Instantiate(movingSprite, transform.position, Quaternion.LookRotation(d.transform.position - transform.position)) as GameObject;
                aus.Play();
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
            pass++;
        }
        UIText.text = "x" + spriteCount;
    }               

    public void Activation()
    {
        spriteCount++;
        activateUI = true;                     
    }           

    public void IncreaseMax()
    {
        maxSpriteCount++;
        spriteCount++;
        activateUI = true;
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
