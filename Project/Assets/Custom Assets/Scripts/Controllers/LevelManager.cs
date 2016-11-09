using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    private Vector3 defaultPos = Vector3.zero;
    private GameObject player = null;

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start ()
    {
        if (GameManager.Instance.IsLoadActive)
        {
            GameManager.Instance.LoadingState(false);
        }
        player = GameObject.FindGameObjectWithTag("Player") as GameObject;
        if (player)
        {
            defaultPos = player.transform.position;
        }
    }

    public void SpawnPlayer ()
    {
        player.transform.position = defaultPos;
    }

    public void SpawnPlayer (Vector3 position)
    {
        player.transform.position = position;
    }
}
