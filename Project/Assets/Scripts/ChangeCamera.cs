using UnityEngine;

public class ChangeCamera : MonoBehaviour {
    //New Position for the camera
    public Vector3[] newOffsets;
    //Reference for the gameCamera
    public GameObject gameCamera;
    //Index
    private int index = 0;  

           
    //Sets the reference for the camera, destroying itself if nothing is found
    void Start()
    {
        //Gets reference to the camera
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        //Error Checking
        if (gameCamera == null)
        {
            GameManager.inst.ErrorSystem("Main Camera not found", this);
            Destroy(this);
        }
    }

    //Trigger collider, sends a message to change the position
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            index++;
            if (index >= newOffsets.Length)
            {
                index = 0;
            }
            gameCamera.SendMessage("SetCameraDir", newOffsets[index]);
        }
    }
}
