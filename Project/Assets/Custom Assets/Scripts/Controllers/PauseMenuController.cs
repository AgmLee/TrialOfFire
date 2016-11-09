using UnityEngine;      

public class PauseMenuController : MonoBehaviour { 
    /*      KEY
     * -1 = Hide All
     *  0 = Audio
     *  1 = Save Complete 
     *  2 = Exit Message
     *  3 = Unsaved Message
     *  4 = Restart Message
     */
    public Animator[] animators;  
    //Buttons
    public GameObject exitLvl;
    public GameObject exitGme;
    public GameObject restart;
    //Canvas
    public GameObject canvas;

    private int currentIndex = -1;
    private int previousIndex = -1;

    void Start()
    {
        exitLvl.SetActive(false);
        exitGme.SetActive(false);
        restart.SetActive(false);
        canvas.SetActive(false);
    }                           
    void OnApplicationFocus(bool focus)
    {
        if (!focus && GameManager.Instance.IsPaused)
        {
            GameManager.Instance.PauseGame();
            canvas.SetActive(true);
            SwitchMenu(0);
        }
    }
    private float timer = 0;
    private bool show = false;   
    void Update()
    {    
        //Activates objects based on 
        if (GameManager.Instance.IsPaused)
        {      
            if (GameManager.Instance.CurrentSceneIndex != GameManager.Instance.CurrentHubWorldIndex)
            {
                exitGme.SetActive(false);
                exitLvl.SetActive(true);
                restart.SetActive(true);
            }
            else
            {
                exitLvl.SetActive(false);
                restart.SetActive(false);
                exitGme.SetActive(true);
            }
            canvas.SetActive(true);
        }   
        else
        {
            exitLvl.SetActive(false);
            exitGme.SetActive(false);
            restart.SetActive(false);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (!GameManager.Instance.IsPaused)
            {
                GameManager.Instance.PauseGame();
                canvas.SetActive(true);
            }
            else
            {
                if (previousIndex != -1)
                {
                    SwitchMenu(-1);
                }
                else
                {
                    exitLvl.SetActive(false);
                    exitGme.SetActive(false);
                    restart.SetActive(false);
                    canvas.SetActive(false);
                    GameManager.Instance.PauseGame();
                }
            }
        }               

        timer += Time.unscaledDeltaTime;

        if (timer > 0.1f)
        {
            timer = 0;
            switch (state)
            {
                //Normal
                case 0:
                    if (previousIndex == -1)
                    {
                        if (previousIndex != currentIndex)
                        {
                            previousIndex = currentIndex;
                        }
                    }
                    else if (currentIndex == -1)
                    {
                        if (!animators[previousIndex].GetCurrentAnimatorStateInfo(0).IsName("Hide"))
                        {
                            exitLvl.SetActive(false);
                            exitGme.SetActive(false);
                            restart.SetActive(false);
                            canvas.SetActive(false);
                            previousIndex = -1;
                            GameManager.Instance.PauseGame();
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
                 break;
                //Exit to Menu
                case 1:
                    if (!animators[previousIndex].GetCurrentAnimatorStateInfo(0).IsName("Hide"))
                    {
                        exitLvl.SetActive(false);
                        exitGme.SetActive(false);
                        restart.SetActive(false);
                        canvas.SetActive(false);
                        previousIndex = -1;
                        state = 0;
                        GameManager.Instance.PauseGame();
                        GameManager.Instance.LoadScene(0);
                    }
                break;
                //Exit to Hub
                case 2:
                    if (!animators[previousIndex].GetCurrentAnimatorStateInfo(0).IsName("Hide"))
                    {
                        exitLvl.SetActive(false);
                        exitGme.SetActive(false);
                        restart.SetActive(false);
                        canvas.SetActive(false);
                        previousIndex = -1;
                        state = 0;
                        GameManager.Instance.PauseGame();
                        GameManager.Instance.LoadScene(GameManager.Instance.CurrentHubWorldIndex);
                    }
                break;
                //Restart Level
                case 3:if (!animators[previousIndex].GetCurrentAnimatorStateInfo(0).IsName("Hide"))
                    {
                        exitLvl.SetActive(false);
                        exitGme.SetActive(false);
                        restart.SetActive(false);
                        canvas.SetActive(false);
                        previousIndex = -1;
                        state = 0;
                        GameManager.Instance.PauseGame();
                        GameManager.Instance.LoadScene(GameManager.Instance.CurrentSceneIndex);
                    }
                break;
            }
        }
    }


    private int state = 0;
    public void SwitchMenu(int newIndex)
    {
        if (newIndex == -1)
        {
            if (previousIndex != -1)
            {
                animators[previousIndex].Play("Hide");
            }
            currentIndex = -1;
            show = false;
        }
        else if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            if (previousIndex != currentIndex && previousIndex != -1)
            {
                animators[previousIndex].Play("Hide");
            }
            show = true;
        }
    }
    //Save Game
    public void Save()
    {
        GameManager.Instance.SaveGame();
        SwitchMenu(1);
    }    
    //Exit to Menu
    public void ShowExit()
    {
        if (GameManager.Instance.UnsavedData())
        {
            SwitchMenu(3);
        }
        else
        {
            SwitchMenu(2);
        }
    }
    //Restart Level
    public void Restart()
    {
        SwitchMenu(-1);
        state = 3;
    }
    //Exit to Hub / Exit to Menu
    public void Exit()
    {
        if (exitLvl.activeSelf)
        {
            SwitchMenu(-1);
            state = 2;
        }
        else
        {
            SwitchMenu(-1);
            state = 1;
        }
    }
    //Resume
    public void Resume()
    {
        if (previousIndex != -1)
        {
            SwitchMenu(-1);
        }
        else
        {
            exitLvl.SetActive(false);
            exitGme.SetActive(false);
            restart.SetActive(false);
            canvas.SetActive(false);
            GameManager.Instance.PauseGame();
        }
    }
}
