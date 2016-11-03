using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameManager : MonoBehaviour {
    //Instence of the Game Manager accessable via GameManager.inst
    public static GameManager inst;

    //Current HubIndex
    private int currentHub = 0;
    public int CurrentHubWorldIndex
    {
        get { return currentHub; }
    }    
    //Pause State             
    private bool pause = false;
    public bool IsPaused
    {
        get { return pause; }
    }

    //Game Data
    private int profileNo = 0;
    private string name;
    private List<bool> completedLevels;
    private int collectedAmount;

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

    //Activate at the start of the application (After Awake)
    void Start()
    {
        loadScreen = GetComponentInChildren<Animator>();
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
        //Show loading Screen;
        LoadingState(true);
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

    //Saves data from the game
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/ProfileData/profile" + profileNo + ".dat");

        DATA d = new DATA();
        d.name = name;
        d.currentID = currentHub;
        d.collectedAmount = collectedAmount;

        bf.Serialize(file, d);
        file.Close();
    }
    
    //Pauses the game
    public void PauseGame()
    {
        pause = !pause;
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    //Displays Loading screen (True = show, false = hide)
    private Animator loadScreen;
    public void LoadingState(bool state)
    {   
        if (state)
        {
            loadScreen.Play("Start", 0);
            loadScreen.Play("Start", 1);
        }
        else
        {
            loadScreen.Play("Stop", 1);
        }
    }

    //Loads data from a Profile
    public void LoadGame(Profile prof, int profNo)
    {
        currentHub = prof.currentID;
        name = prof.name;
        collectedAmount = prof.collectedAmount;
        profileNo = profNo;
    }
}   

//Class for saving/loading
[Serializable]
class DATA
{
    public string name;
    public int collectedAmount;
    public int currentID;
}