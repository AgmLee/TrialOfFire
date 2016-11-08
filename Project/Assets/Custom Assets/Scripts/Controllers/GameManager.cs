using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
    //Instence of the Game Manager accessable via GameManager.inst
    public static GameManager inst;

    //Scene Tracker
    //Last Visited HUB
    private int currentHub = 0;
    public int CurrentHubWorldIndex
    {
        get { return currentHub; }
    }    
    //Current scene
    private int currentID = 0;
    public int CurrentSceneIndex
    {
        get { return currentID; }
    }
    //Pause State             
    private bool pause = false;
    public bool IsPaused
    {
        get { return pause; }
    }

    //Game Data
    private int profileNo = 0;
    private string proName = "";
    public int collectedAmount = 0;
    private int savedAmount = 0;
    private int savedID = 0;

    //Activates at the start of the application
    void Awake()
    {
        //Creates a singleton, setting the first reference and destroying all others.
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
    
    //Clears data    
    private void ClearData()
    {
        profileNo = 0;
        proName = "";
        collectedAmount = 0;
    }

    //Sends a message to the Debug log and will stop the application if needed
    public void ErrorSystem(string message, object obj = null, bool STOP = false, int SEVERITY = 0) 
    {
        //Create Debug Message
        string debugMessage = message;
        if (obj != null)
        {
            debugMessage += ": " + obj.ToString();
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
        int index = SceneManager.GetSceneByName(scene).buildIndex;
        //set currentID
        currentID = index;
        //If it is a hub, set the current hub to it
        if (ISHUB)
        {
            currentHub = index;
        }

        //Loads the scene
        SceneManager.LoadScene(index);
    }
    //Loads a scene via an index, the ISHUB bool sets the current hub to it
    public void LoadScene(int index, bool ISHUB = false)
    {
        //Show loading Screen;
        LoadingState(true);

        //If loading menu, clear data otherwise continue
        if (index == 0)
        {
            ClearData();   
        }
        else
        {   
            //Set currentID
            currentID = index;
            //If it is a hub, set the current hub to it
            if (ISHUB)
            {
                currentHub = index;
            }
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
        d.name = proName;
        d.currentID = currentHub;
        savedID = currentHub;
        d.collectedAmount = collectedAmount;
        savedAmount = collectedAmount;

        bf.Serialize(file, d);
        file.Close();
    }
    //Returns weather true if there is unsaved data
    public bool UnsavedData()
    {
        if (collectedAmount != savedAmount || savedID != currentHub)
        {
            return true;
        }
        return false;
    }

    //Pauses the game
    public void PauseGame()
    {
        //Switches pause state
        pause = !pause;
        
        if (pause)
        {
            //Set timeScale, stopping most of Time (e.g. deltaTime)
            Time.timeScale = 0;
            //Make the cursor visible and release it to the user
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //Set timeScale, reactivating most of Time (e.g. deltaTime)
            Time.timeScale = 1;
            //Make the cursor hidden and lock it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
        proName = prof.name;
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