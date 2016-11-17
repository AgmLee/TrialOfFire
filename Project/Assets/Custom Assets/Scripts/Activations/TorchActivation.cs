using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TorchActivation : MonoBehaviour, IAction
{
    //Refernces
    public Transform emitterParent;
    public GameObject fireEmitter;
    public InventoryManager inventory;
    private GameObject fire = null;
    public AudioSource aus;
    public GameObject sprite;
    private GameObject sprRef = null;
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
    public bool IsActivated
    {
        get { return isActive; }
    }

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
        aus = GetComponent<AudioSource>();
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

    public void Activation(bool value)
    {
        isActive = !isActive;
        Activate();
    }

    //Activates all objects in objects
    void Activate()
    {
        if (aus)
        {
            aus.Play();
        }
        if (fire == null)
        {
            fire = Instantiate(fireEmitter, emitterParent, false) as GameObject;
        }
        else if (fire != null)
        {
            Destroy(fire);
        }
        if (sprite)
        {
            if (!sprRef)
            {
                sprRef = Instantiate(sprite, transform, false) as GameObject;
            }
            else if (sprRef)
            {
                Destroy(sprRef);
            }
        }
        foreach (GameObject obj in objects)
        {
            obj.SendMessage("Activation", isActive);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<InventoryManager>().InputSet(!isActive, spritesNeeded, new GameObject[1] { gameObject });
            col.gameObject.GetComponent<InventoryManager>().SetBack(isActive, spritesNeeded, gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<InventoryManager>().InputSet(false, 0);
            col.gameObject.GetComponent<InventoryManager>().SetBack(false, 0);
        }
    }
}
