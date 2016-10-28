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
            AudioSource aus = GetComponent<AudioSource>();
            if (aus && inventory.spriteCount != inventory.maxSpriteCount)
            {
                aus.Play();
            }
            if (inventory.spriteCount < inventory.maxSpriteCount)
            {
                inventory.spriteCount = inventory.maxSpriteCount;
                inventory.activateUI = true;
            }
        }
    }
}
