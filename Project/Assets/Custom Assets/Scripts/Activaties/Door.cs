using UnityEngine;

public class Door : MonoBehaviour, IAction
{
    public GameObject door;
    public Transform openPoint;
    public AudioClip close;
    public float openSpeed = 10.0f;
    public bool invert = false;
    private bool isActive = false;
    public bool IsActivated
    {
        get { return isActive; }
    }
    private AudioSource aus;
    public int requirementAmout = 1;
    private Vector3 startPoint;

    void Start()
    {
        aus = GetComponent<AudioSource>();
        startPoint = door.transform.localPosition;
        if (invert)
        {
            isActive = true;
            door.transform.localPosition = openPoint.localPosition;
        }
    }

    void Update()
    {
        if (isActive)
        {
            if (Vector3.Distance(door.transform.localPosition, openPoint.localPosition) > 0.05f)
            {
                door.transform.localPosition -= (door.transform.localPosition - openPoint.localPosition).normalized * openSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Vector3.Distance(door.transform.localPosition, startPoint) > 0.05f)
            {
                door.transform.localPosition -= (door.transform.localPosition - startPoint).normalized * openSpeed * Time.deltaTime;
            }
        }
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
            isActive = !invert;
            aus.Play();
        }
        else if ((isActive && !invert) || (!isActive && invert))
        {
            isActive = invert;
            aus.PlayOneShot(close);
        }
    }
}
