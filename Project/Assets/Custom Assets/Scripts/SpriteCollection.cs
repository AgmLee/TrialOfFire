using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class SpriteCollection : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            pc.spriteCount = pc.maxSpriteCount;
        }
    }
}
