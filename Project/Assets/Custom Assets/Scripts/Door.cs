using UnityEngine;

public class Door : MonoBehaviour, IAction
{
    public Animator door;
    public bool invert = false;
    private bool isActive = false;
    public bool IsActivated
    {
        get { return isActive; }
    }

    void Start()
    {
        if (invert)
        {
            isActive = !isActive;
        }
        door.SetBool("invert", invert);
        door.SetBool("IsActive", isActive);
    }

    public void Activation()
    {
        isActive = !isActive;
        door.SetBool("IsActive", isActive);
    }
}
