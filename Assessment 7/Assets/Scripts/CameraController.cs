using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform playerTransform;
    public Vector3 offsetPos = new Vector3(0, 3, -5);
    public float offsetAngle = 1.25f;

    private Transform myTransform;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
            {
                playerTransform = obj.GetComponent<Transform>();
            }
        }

        if (myTransform == null)
        {
            myTransform = gameObject.transform;
        }

        if (myTransform == null || playerTransform == null)
        {
            Debug.Log("Missing Transforms");
            Destroy(this);
        }
    }

    void Update()
    {

        myTransform.position = new Vector3(playerTransform.position.x + offsetPos.x, playerTransform.position.y + offsetPos.y, playerTransform.position.z + offsetPos.z);
        myTransform.LookAt(playerTransform.position + (Vector3.up * offsetAngle));
    }
}
