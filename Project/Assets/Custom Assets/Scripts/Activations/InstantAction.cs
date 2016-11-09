using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InstantAction : MonoBehaviour {
    public InstantActionObject[] objs;

    void Start()
    {
        if (objs.Length == 0)
        {
            GameManager.Instance.ErrorSystem("actObj length zero", this);
            gameObject.SetActive(false);
        }
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            foreach(InstantActionObject iao in objs)
            {
                iao.obj.SendMessage("Activation", iao.value);
            }
        }
    }
}

[Serializable]
public struct InstantActionObject
{
    public GameObject obj;
    public bool value;
}