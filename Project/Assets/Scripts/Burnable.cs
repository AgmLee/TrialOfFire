using UnityEngine;
using System.Collections;

public class Burnable : MonoBehaviour {
    //Time taken to "Burn" the object (in seconds)
    public float burnTime = 1.0f;   
    //Required amount of embers needed (W.I.P.)
    public int requiredAmmount = 0;
    //Sets the object on fire
    public bool burning = false;

    //Spawns the Flame/Smoke particle emitters
    public GameObject fireEmitter;

    //Used for spawning the emitters
    private bool active = false;
    //Timer
    private float timer = 0.0f;    
    //Material reference
    private Material mat;

    void Start() 
    {
        mat = GetComponent<MeshRenderer>().material; 
    }
    
    void Update()
    {                 
        if (burning)
        {
            //If not active it spawns the emitter
            if (!active) 
            {
                Instantiate(fireEmitter, gameObject.transform, false);
                
                active = true;
            }
            //Add to the timer
            timer += Time.deltaTime;
            if (timer > burnTime)
            {        
                //Fades the object
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, (mat.color.a - mat.color.a * Time.deltaTime));
                //Destroys the object onced faded
                if (mat.color.a < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
        }        
    }

    //Function for setting the state of the burning function
    public void SetBurnState(bool state) 
    {
        burning = state;
    }

    //Collision
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Ember")
        {
            Destroy(col.gameObject);
            burning = true;
        }
    }
}
