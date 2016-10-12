using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    //Instence of the Game Manager accessable via GameManager.inst
    public static GameManager inst;

    //Current HubIndex
    public int CurrentHubWorldIndex
    {
        get { return currentHub; }
    }                 
    private int currentHub = 0;
    
    //List of levels, false = locked, true = unlocked
    //Level Amount = the amount of locked Levels
    private List<bool> levels;
    private int amountOfLevels = 1;

    //Activates at application stop
    void OnApplicationQuit()
    {
        SaveGame();
    }

    //Activates at the start of the application (After Awake)
    void Start() 
    {
        levels = new List<bool>(amountOfLevels);
        for (int i = 0; i < amountOfLevels; i++)
        {
            if (i > 0)
            {
                levels[i] = false;
            }
            else
            {
                levels[i] = true;
            }
        }
    }

    //Activates at the start of the application
    void Awake()
    {
        //Sets the instance of the game manager and destroys itself if a different one exists
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

    //Sends a message to the Debug log and will stop the application if needed
    public void ErrorSystem(string message, object obj = null, bool STOP = false, int SEVERITY = 0) 
    {
        //Create Debug Message
        string debugMessage = message;
        if (obj != null)
        {
            debugMessage += ": " + obj;
        }

        //Log Error
        Debug.Log(debugMessage);

        //Stop based on severity (e.g. Level) if wanted
        if (STOP)
        {
            switch(SEVERITY)
            {
                //RETURN TO CURRENT HUB
                case 0:
                    SceneManager.LoadScene(currentHub);
                    break;
                //RETURN TO MENU
                case 1:
                    SceneManager.LoadScene(0);
                    break;
                //QUIT
                default:
                    Application.Quit();
                    break;
            }
        }
    }

    //Loads a scene via the scene name, the ISHUB bool sets the current hub to it
    public void LoadScene(string scene, bool ISHUB = false)
    {
        //If it is a hub, set the current hub to it
        if (ISHUB)
        {
            currentHub = SceneManager.GetSceneByName(scene).buildIndex;
        }

        //Loads the scene
        SceneManager.LoadScene(scene);
    }
    //Loads a scene via an index, the ISHUB bool sets the current hub to it
    public void LoadScene(int index, bool ISHUB = false) 
    {
        //If it is a hub, set the current hub to it
        if (ISHUB)
        {
            currentHub = index;
        }

        //Loads the scene
        SceneManager.LoadScene(index);
    }

    //Loads data into the game
    public void LoadGame()
    {
        //
    }

    //Saves data from the game
    public void SaveGame()
    {
        //
    }
}   

//Class for saving/loading
class DATA
{
    public List<bool> levels;
    public int currentHub;          
}