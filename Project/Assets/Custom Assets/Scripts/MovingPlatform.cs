using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour, IAction {
    public Transform[] path;
    public float speed = 5.0f;
    private Transform ownTransform;
    private int current = 1;
    public bool pingPong = false;
    public bool requiresActivation = false;
    private bool isActive = true;
    public bool IsActivated
    {
        get { return isActive; }
    }
    private bool flip = false;
    private Vector3 direction;

    void Start()
    {
        ownTransform = transform;
        if (path.Length == 0)
        {
            Debug.Log("Path is Empty", this);
            Destroy(this);
        }
        else if (!path[0])
        {
            Debug.Log("Path is Empty", this);
            Destroy(this);
        }
        else
        {
            direction = path[current].position - ownTransform.position;
            direction.Normalize();
            if (requiresActivation)
            {
                isActive = false;
            }
            if (!GetComponent<Rigidbody>().isKinematic)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void Update()
    {
        if (isActive)
        {
            ownTransform.position += direction * speed * Time.deltaTime;
            if (Vector3.Distance(ownTransform.position, path[current].position) < 0.5f)
            {
                if (pingPong)
                {
                    if (flip)
                    {
                        current--;
                    }
                    else
                    {
                        current++;
                    }
                    if (current >= path.Length || current < 0)
                    {
                        flip = !flip;
                        if (flip)
                        {
                            current--;
                        }
                        else
                        {
                            current++;
                        }
                    }
                }
                else
                {
                    current++;
                    if (current >= path.Length)
                    {
                        current = 0;
                    }
                }
                direction = path[current].position - ownTransform.position;
                direction.Normalize();
            }
        }
        else
        {
            if (Vector3.Distance(ownTransform.position, path[0].position) > 0.05f)
            {
                ownTransform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    public void Activation()
    {
        isActive = !isActive;
        if (!isActive)
        {
            direction = path[0].position - ownTransform.position;
            direction.Normalize();
        }
    }
}
