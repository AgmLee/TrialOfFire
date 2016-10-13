using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class SpriteCollection : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			RBCharacterController pc = col.gameObject.GetComponent<RBCharacterController>();
            pc.spriteCount = pc.maxSpriteCount;
        }
    }
}
