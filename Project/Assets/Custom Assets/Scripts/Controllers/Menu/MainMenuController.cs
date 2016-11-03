using UnityEngine;
using UnityEngine.UI;    

public class MainMenuController : MonoBehaviour {
    /*  Important Keys
     *      String Key
     *  "Hide" = State name for Hiding the menu & the name of the animation clip that hides it.
     *  "Show" = State name for Showing the menu & the name of the animation clip that hides it.
     *      
     *      Int Key
     *  0   = Quit Menu
     *  1   = Options Menu
     *  2   = Play Menu
     *  3   = Delete Menu
     */
    public Animator[] animators;
    private int currentIndex = -1;
    private int previousIndex = -1;
    private AudioSource aus;

    private bool show = false;
    void FixedUpdate()
    {
        if (aus)
        {
            aus.volume -= Time.deltaTime;
        }
        if (previousIndex == -1)
        {
            if (previousIndex != currentIndex)
            {
                previousIndex = currentIndex;
            } 
        }
        else if (exit)
        {
            if (!animators[previousIndex].GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                #if DEBUG
                    Debug.Log("Has Quit");
                    exit = false;
                    previousIndex = -1;
                    currentIndex = -1;
                #else
                    Application.Quit();
                #endif
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
            previousIndex = -1;
            currentIndex = -1;
            exit = false;
            show = false;
        }
        else if (newIndex != previousIndex)
        {   
            currentIndex = newIndex;
            if (previousIndex != currentIndex && previousIndex != -1)
            {
                animators[previousIndex].Play("Hide");
            }                 
            show = true;
        }
    }

    public void Fade()
    {
        aus = GetComponent<AudioSource>();
    }

    private bool exit = false;
    public void Quit()
    {
        exit = true;
        animators[previousIndex].Play("Hide");                   
    }
}
