using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {

    public Vector3 direction;

    private GameObject camera;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera == null)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            camera.SendMessage("SetCameraDir", direction);
        }
    }
}
