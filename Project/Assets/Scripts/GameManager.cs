using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager inst;

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
