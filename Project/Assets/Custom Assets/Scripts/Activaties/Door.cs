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
    void Start()
    {
        aus = GetComponent<AudioSource>();
        if (invert)
        {
            isActive = !isActive;
        }
        door.SetBool("invert", invert);
        door.SetBool("IsActive", isActive);
        door.speed = openSpeed;
    }

    public void Activation()
    {
        isActive = !isActive;
        if (isActive)
        {
            aus.Play();
        }
        else
        {
            aus.PlayOneShot(close);
        }
        door.SetBool("IsActive", isActive);
    }
}
