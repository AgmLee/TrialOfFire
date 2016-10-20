using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TorchActivation : MonoBehaviour
{
    //Refernces
    public Transform emitterParent;
    public GameObject fireEmitter;
    public InventoryManager inventory;
    private GameObject fire = null;
    //Activations
    public GameObject[] objects;
    //Active State
    private bool isActive = false;
    private bool acceptInput = false;
    public int spritesNeeded = 1;
    //Settings
    public bool useAmountInObjects = false;
    public bool invert = false;
    public bool useOnce = false;
        
    void Start()
    {
        if (useAmountInObjects)
        {
            spritesNeeded = objects.Length;
        }
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        if (invert)
        {
            isActive = true;
            Activate();
        }
    }

    void Update()
    {
        if (acceptInput)
        {
            if (Input.GetButtonDown("Primary") && inventory.spriteCount >= spritesNeeded)
            {
                if (!isActive)
                {
                    inventory.spriteCount -= spritesNeeded;
                    isActive = true;
                    Activate();
                }
            }
            else if (Input.GetButtonDown("Secondary") && !useOnce)
            {
                if (isActive)
                {
                    isActive = false;
                    if (inventory.spriteCount != inventory.maxSpriteCount)
                    {
                        inventory.spriteCount += spritesNeeded;
                    }
                    Activate();
                }
            }
        }
    }

    //Activates all objects in objects
    void Activate()
    {
        if (fire == null)
        {
            fire = Instantiate(fireEmitter, emitterParent, false) as GameObject;
        }
        else if (fire != null)
        {
            Destroy(fire);
        }
        foreach (GameObject obj in objects)
        {
            obj.SendMessage("Activation");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            acceptInput = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            acceptInput = false;
        }
    }
}
