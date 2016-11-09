using UnityEngine;

public class Door : MonoBehaviour, IAction
{
    public Animator door;
    public AudioClip close;
    public float openSpeed = 3.0f;
    public bool invert = false;
    private bool isActive = false;
    public bool IsActivated
    {
        get { return isActive; }
    }
    private AudioSource aus;
    public int requirementAmout = 1;

    void Start()
    {
        aus = GetComponent<AudioSource>();
        if (invert)
        {
            isActive = true;
        }
        door.speed = openSpeed;
        door.SetBool("IsActive", isActive);
    }

    private int tally = 0;
    public void Activation(bool value)
    {
        if (value)
        {
            tally++;
        }
        else
        {
            tally--;
        }
        if (tally >= requirementAmout)
        {
            isActive = true;
            aus.Play();
            door.SetBool("IsActive", isActive);
        }
        else if (isActive)
        {
            isActive = false;
            aus.PlayOneShot(close);
            door.SetBool("IsActive", isActive);
        }
    }
}
