using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class SpriteCollection : MonoBehaviour {
    public InventoryManager inventory;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            inventory.spriteCount = inventory.maxSpriteCount;
        }
    }
}
