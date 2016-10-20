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
    private bool allowInput = false;

    void Start()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null)
        {
            GameManager.inst.ErrorSystem("Collider \'col\' missing", this, true, 0);
        }
        col.isTrigger = true;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        if (objects == null)
        {
            GameManager.inst.ErrorSystem("GameObject[] \'objects\' empty", this, true, 0);
        }
        if (useAmountOfObjects)
        {
            amountNeeded = objects.Length;
        }
    }    

    void Update()
    {
        if (allowInput)
        {
            if (Input.GetButtonDown("Primary") && inventory.spriteCount >= amountNeeded)
            {
                inventory.spriteCount -= amountNeeded;
                foreach (GameObject obj in objects)
                {
                    obj.SendMessage("Activation");
                }
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            allowInput = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            allowInput = false;
        }
    }
}
