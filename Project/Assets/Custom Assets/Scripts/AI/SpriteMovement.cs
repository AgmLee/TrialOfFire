using UnityEngine;

public class SpriteMovement : MonoBehaviour {
    public GameObject refe;
    public float speed = 5.0f;
    private Transform ownTrans;
    private Vector3 dir;

    void Start()
    {
        ownTrans = transform;
        dir = ownTrans.position - refe.transform.position;
        ownTrans.rotation = Quaternion.LookRotation(-dir);
    }

    void Update()
    {
        ownTrans.LookAt(refe.transform);
        ownTrans.position += ownTrans.forward * speed * Time.deltaTime;
        if (Vector3.Distance(refe.transform.position, ownTrans.position) < 0.5f)
        {
            refe.SendMessage("Activation");
            Destroy(gameObject);
        }
    }
}
