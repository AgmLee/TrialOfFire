using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //Type
    public enum Type { LazyBones, SnooberNoodle, AngryTurtle }
    public Type type;
    //Multiple Types
    public float speed = 2;
    private Vector3 direction;
    private Transform ownTransform;
    private bool element = false;
    //Snoober
    public Transform[] path;
    public bool pingPong = false;
    private int current = 1;
    //Angry
    public float dropTime = 3;
    public float dropHeight = 2;
    private float timer = 0;
    private Vector3 start;
    private Vector3 destination;
    private bool move = false;
    private bool up = false;
    //Private

    void Start()
    {
        if (!GetComponent<Rigidbody>().isKinematic)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        ownTransform = transform;
        if (type == Type.SnooberNoodle)
        {
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
        else if (type == Type.AngryTurtle)
        {
            destination = ownTransform.position - Vector3.up * dropHeight;
            start = ownTransform.position;
        }
    }

    void Update()
    {
        if (type == Type.SnooberNoodle)
        {
            ownTransform.LookAt(path[current]);
            ownTransform.position += direction * speed * Time.deltaTime;
            if (Vector3.Distance(ownTransform.position, path[current].position) < 0.5f)
            {
                if (pingPong)
                {
                    if (element)
                    {
                        current--;
                    }
                    else
                    {
                        current++;
                    }
                    if (current >= path.Length || current < 0)
                    {
                        element = !element;
                        if (element)
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
        else if (type == Type.AngryTurtle)
        {
            timer += Time.deltaTime;
            if (element && timer >= dropTime && !move)
            {
                direction = destination - ownTransform.position;
                direction.Normalize();
                move = true;
                up = false;
            }
            if (timer >= dropTime && !move)
            {
                direction = start - ownTransform.position;
                direction.Normalize();
                move = true;
                up = true;
            }
            if (move)
            {
                if (up)
                {
                    if (Vector3.Distance(ownTransform.position, start) > 0.05f)
                    {
                        ownTransform.position += direction * speed * Time.deltaTime;
                    }
                    else
                    {
                        move = false;
                        up = false;
                    }
                }
                else
                {
                    if (Vector3.Distance(ownTransform.position, destination) > 0.05f)
                    {
                        ownTransform.position += direction * speed * Time.deltaTime;
                    }
                    else
                    {
                        move = false;
                        up = true;
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (type != Type.AngryTurtle)
            {
                col.gameObject.SendMessage("Hurt");
            }
            else
            {
                element = true;
                timer = 0;
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (type == Type.AngryTurtle)
            {
                element = false;
                timer = 0;
            }
        }
    }
}
