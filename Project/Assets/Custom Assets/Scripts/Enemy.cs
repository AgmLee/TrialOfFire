using UnityEngine;

public class Enemy : MonoBehaviour {
    public Transform[] path;
    public float speed = 2;
    public bool pingPong = false;
    public bool moves = false;
    private Transform ownTransform;
    private Vector3 direction;
    private int current = 1;
    private bool flip = false;

    void Start()
    {
        if (!GetComponent<Rigidbody>().isKinematic)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        if (moves)
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
            }
        }
    }

    void Update()
    {
        if (moves)
        {
            ownTransform.LookAt(path[current]);
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
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.SendMessage("Hurt");
        }
    }
}
