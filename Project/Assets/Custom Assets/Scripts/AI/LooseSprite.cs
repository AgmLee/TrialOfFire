using UnityEngine;        

[RequireComponent(typeof(SphereCollider))]
public class LooseSprite : MonoBehaviour {
    public GameObject parent;
    public AudioSource aus;
    public AudioClip found;     
    void Start()
    {
        if (!parent)
        {
            gameObject.SetActive(false);
        }
    }     
   
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.SendMessage("IncreaseMax");           
            if (aus)
            {
                aus.PlayOneShot(found,  0.5f);
            }
            parent.SetActive(false);
        }
    }  
}
