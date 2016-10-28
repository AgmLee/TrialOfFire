using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Activation : MonoBehaviour {
    //Objects that it activates
    public GameObject[] objects;
    //Amount of sprites used
    public int amountNeeded = 1;
    //Sets the amountNeeded to the number of objects in objects
    public bool useAmountOfObjects = false;
    //References 
	public InventoryManager inventory;
    public bool isActive = false;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        if (useAmountOfObjects)
        {
            amountNeeded = objects.Length;
        }
    }    

    void Update()
    {
        if (isActive)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            inventory.InputSet(!isActive, amountNeeded, objects);
            inventory.SetBack(false, amountNeeded, gameObject, true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            inventory.InputSet(false, 0);
            inventory.SetBack(false, 0);
        }
    }
}
