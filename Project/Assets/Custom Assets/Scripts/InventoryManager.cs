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
    public GameObject sprite;
    //Settings
    public float rotationSpeed = 5.0f;
    public float xAngle = 15.0f;
    public float zAngle = 15.0f;
    public bool flip = false;
    public AnimationCurve bob;
    //Private Variables
    private GameObject[] sprites;
    private int index = 0;
    
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
        rotationTransform.position = transform.position + Vector3.up * 0.5f;
        rotationTransform.Rotate(Vector3.up, rotationSpeed);
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
                    sprites[index] = Instantiate(sprite, spritePoints[index], false) as GameObject;
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
}
