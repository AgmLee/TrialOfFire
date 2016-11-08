using UnityEngine;      

public class PauseMenuController : MonoBehaviour { 
    /*      KEY
     * -1 = Hide All
     *  0 = Main Show
     *  1 = Audio
     *  2 = Save Complete 
     *  3 = Exit
     */
    public Animator[] animators;  
    //Buttons
    public GameObject exitLvl;
    public GameObject exitGme;
    //Canvas
    public GameObject canvas;

    private int currentIndex = -1;
    private int previousIndex = -1;

    void Start()
    {
        exitGme.SetActive(false);
        exitLvl.SetActive(false);
    }                           
    private bool runOnce = true;
    void Update()
    {                     
        //Activates objects based on 
        if (GameManager.inst.IsPaused)
        {      
            if (GameManager.inst.CurrentSceneIndex != GameManager.inst.CurrentHubWorldIndex)
            {
                exitGme.SetActive(false);
                exitLvl.SetActive(true);
            }
            else
            {
                exitLvl.SetActive(false);
                exitGme.SetActive(true);
            }
            canvas.SetActive(true);
            if (runOnce)
            {
                SwitchMenu(0);
                runOnce = false;
            }
        }   
        else
        {
            exitLvl.SetActive(false);
            exitGme.SetActive(false);   
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (!GameManager.inst.IsPaused)
            {
                GameManager.inst.PauseGame();
                canvas.SetActive(true);
                SwitchMenu(0);
            }
            else
            {
                SwitchMenu(-1);
            }
        }           
    }
    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            GameManager.inst.PauseGame();
            canvas.SetActive(true);
            SwitchMenu(0);
        }
    }
    private bool hide = false;
    private bool show = false;
    void FixedUpdate()
    {
        if (hide)
        {
            if (!animators[0].GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                hide = false;
                canvas.SetActive(false);
                GameManager.inst.PauseGame();
                runOnce = true;
            }
        }
        else if (previousIndex == -1 && currentIndex > 0)
        {
            if (previousIndex != currentIndex)
            {
                previousIndex = currentIndex;
            }
        }
        else
        {
            if (!animators[previousIndex].GetCurrentAnimatorStateInfo(0).IsName("Hide") && show)
            {
                animators[currentIndex].Play("Show");
                previousIndex = currentIndex;
                show = false;
            }
        }
    }           
        
    public void SwitchMenu(int newIndex)
    {
        if (newIndex == -1)
        {
            if (previousIndex != -1)
            {
                animators[previousIndex].Play("Hide");
            }
            if (previousIndex != 0)
            {
                animators[0].Play("Hide");
            }
            previousIndex = -1;
            currentIndex = -1;
            show = false;    
            hide = true;
        }
        else if (newIndex != previousIndex)
        {   
            if (!canvas.activeSelf)
            {
                canvas.SetActive(true);
            }
            currentIndex = newIndex;
            if (previousIndex != currentIndex && previousIndex != -1)
            {
                animators[previousIndex].Play("Hide");
            }                 
            show = true;
        }
    } 
    public void Save()
    {
        #if DEBUG
            Debug.Log("Saved");
        #else
            GameManager.inst.SaveGame();
        #endif
        animators[1].Play("Show");
    }    
    public void Exit()
    {
        if (exitLvl.activeSelf)
        {
            #if DEBUG
                Debug.Log("Return to Hub");
            #else   
                GameManager.inst.LoadScene(GameManager.inst.CurrentHubWorldIndex);
            #endif
        }
        else
        {
            #if DEBUG
                Debug.Log("Return to Menu");
            #else                            
                if (GameManager.inst.UnsavedData())
                {
                    //Show Unsaved Data Message
                }
                else
                {
                    GameManager.inst.LoadScene(0);
                }
            #endif
        }
    }
}
