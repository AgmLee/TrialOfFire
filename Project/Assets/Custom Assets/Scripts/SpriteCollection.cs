using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Collider))]
public class SpriteCollection : MonoBehaviour {
    public int curMax = 3;
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            /*
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            if (pc.curMax > curMax)
            {
                curMax = pc.curMax;
            }    
            pc.sprites = curMax;
            */
            Debug.Log("found");
        }
    }
}
