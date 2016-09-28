using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    //Instence of the Game Manager accessable via GameManager.inst
    public static GameManager inst;

    //Sets the instance of the game manager and destroys itself if a different one exists
    void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (inst != this)
        {
            Destroy(gameObject);
        }
    }
}
