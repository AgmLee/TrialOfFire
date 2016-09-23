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
        Vector3 pos = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        Vector3 offset = pos + (direction * offsetXZ) + (offsetY * Vector3.up);
        myTransform.position = Vector3.Lerp(myTransform.position, offset, 5)/* Time.deltaTime*/;
        myTransform.forward = Vector3.Lerp(direction, newDir, 5);
    }

    public void SetCameraDir(Vector3 newDirection)
    {
        newDir = newDirection;
    }
}
