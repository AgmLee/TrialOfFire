using UnityEngine;
public class InventoryManager : MonoBehaviour {
    //Sprites
    public int maxSpriteCount = 3;
    public int spriteCount = 0;
    //Transforms
    public Transform rotationTransform;
    public Transform[] spritePoints;
    //References
    public GameObject sprite;
    //Settings
    public float rotationSpeed = 5.0f;
    public float xAngle = 15.0f;
    public float zAngle = 15.0f;
    public bool flip = false;
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
        if (index != spriteCount && index < 6)
        {
            if (index < spriteCount)
            {
                while (index != spriteCount)
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
    }
}
