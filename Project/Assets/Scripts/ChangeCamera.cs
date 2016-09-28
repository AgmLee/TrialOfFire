using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {
    //New Position for the camera
    public Vector3 newPosition;
    //Reference for the gameCamera
    public GameObject gameCamera;
           
    //Sets the reference for the camera, destroying itself if nothing is found
    void Start()
    {
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (gameCamera == null)
        {
            Destroy(gameObject);
        }
    }

    //Trigger collider, sends a message to change the position
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            gameCamera.SendMessage("SetCameraDir", newPosition);
        }
    }
}
