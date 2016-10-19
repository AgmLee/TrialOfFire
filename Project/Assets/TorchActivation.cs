using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TorchActivation : MonoBehaviour
{
    //Refernces
    public Transform emitterParent;
    public GameObject fireEmitter;
    private GameObject fire = null;
    private RBCharacterController player = null;
    //Activations
    public GameObject[] objects;
    //Active State
    private bool isActive = false;
    private bool acceptInput = false;
    public int spritesNeeded = 1;
    //Settings
    public bool useAmountInObjects = false;
    public bool invert = false;
        
    void Start()
    {
        if (useAmountInObjects)
        {
            spritesNeeded = objects.Length;
        }
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
            if (Input.GetButtonDown("Primary") && player.spriteCount >= spritesNeeded)
            {
                if (!isActive)
                {
                    isActive = true;
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

            if (player == null)
            {
                player = col.gameObject.GetComponent<RBCharacterController>();
            }
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
