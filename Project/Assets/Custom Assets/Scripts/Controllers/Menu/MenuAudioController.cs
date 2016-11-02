using UnityEngine;
using UnityEngine.UI;

public class MenuAudioController : MonoBehaviour {
    /*      Int Key
     *  0 = Select Active
     *  1 = Select In-active
     *  2 = Hover
     */
    public AudioClip[] clips;
    private AudioSource aus;

    void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    public void Select(Button check)
    {        
        if (check.interactable)
        {
            aus.PlayOneShot(clips[0]);
        }
        else
        {
            aus.PlayOneShot(clips[1]);
        }   
    }
     
    public void Hover(Button check)
    {
        if (check.interactable)
        {
            aus.PlayOneShot(clips[2]);
        }   
    }
}
