using UnityEngine;          

public class MenuOptionsController : MonoBehaviour
{
    /*  Important Keys
     *      String Key
     *  "Hide" = State name for Hiding the menu & the name of the animation clip that hides it.
     *  "Show" = State name for Showing the menu & the name of the animation clip that hides it.
     *      
     *      Int Key
     *  0   = Audio
     *  1   = Graphics        
     */
    public Animator[] animators;
    private bool isActive = false;
    private int currentIndex = -1;
    private int previousIndex = -1;

    private bool show = false;
    void FixedUpdate()
    {
        if (previousIndex == -1)
        {
            if (currentIndex != previousIndex)
            {
                previousIndex = currentIndex;
            }
        }
        else if (isActive)
        {
            if (!animators[previousIndex].GetCurrentAnimatorStateInfo(0).IsName("Hide") && show)
            {
                animators[currentIndex].Play("Show");
                previousIndex = currentIndex;
                show = false;
            }
        } 
    }
    
    void Update()
    {
        if (!isActive && previousIndex != -1)
        {
            animators[currentIndex].Play("Hide");
            previousIndex = currentIndex = -1;
            show = false;
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    public void SwitchMenu(int newIndex)
    {
        if (newIndex != previousIndex)
        {
            currentIndex = newIndex;
            if (previousIndex != currentIndex && previousIndex != -1)
            {
                animators[previousIndex].Play("Hide");
            }
            show = true;
        }
    }  
}
