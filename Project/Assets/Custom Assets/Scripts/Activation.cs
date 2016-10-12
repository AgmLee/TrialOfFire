using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Activation : MonoBehaviour {
    public GameObject[] objects;

    void Start()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null)
        {
            GameManager.inst.ErrorSystem("Collider col missing", this, true, 0);
        }
        col.isTrigger = true;
        if (objects == null)
        {
            GameManager.inst.ErrorSystem("GameObject[] objects empty", this, true, 0);
        }
    }    

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            foreach(GameObject obj in objects)
            {
                obj.SendMessage("Activation");
            }
            gameObject.SetActive(false);
        }
    }
}
