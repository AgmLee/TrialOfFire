using UnityEngine;

public class Burnable : MonoBehaviour, IAction
{
    //Timers
    public float burnTime = 1.0f;
    private float timer = 0.0f;
    //Emitter
    public GameObject fireEmitter;
    //Active State
    private bool isActive = false;
    public bool IsActivated
    {
        get
        {
            return isActive;
        }
    }

    void Start()
    {
        if (fireEmitter == null)
        {
            GameManager.Instance.ErrorSystem("Missing fireEmitter", this);
        }
    }

    void Update()
    {
        //Check active state
        if (isActive)
        {
            //Increase timer
            timer += Time.deltaTime;
            if (timer >= burnTime)
            {
                //Removes the object
                gameObject.SetActive(false);
            }
        }
    }

    //Action
    public void Activation(bool value)
    {
        //Spawn emitter if it exists
        if (fireEmitter != null)
        {
            Instantiate(fireEmitter, transform, false);
        }
        //Switch active state
        isActive = !isActive;
    }
}