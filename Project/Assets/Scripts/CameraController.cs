using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform playerTransform;
    public Vector3 direction = Vector3.forward;
    public float offsetXZ, offsetY, offsetA;

    private Transform myTransform;
    private Vector3 newDir;

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

        newDir = direction;
    }

    void Update()
    {
        if (newDir != direction)
        {
            direction = Vector3.Lerp(direction, newDir, Vector3.Distance(direction, newDir));
        }
        Vector3 pos = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        Vector3 offset = pos + (offsetXZ * -direction) + (offsetY * Vector3.up);
        myTransform.position = offset;
        myTransform.LookAt(new Vector3(playerTransform.position.x, playerTransform.position.y + offsetA, playerTransform.position.z));
    }

    public void SetCameraDir(Vector3 newDirection)
    {
        newDir = newDirection;
    }
}
