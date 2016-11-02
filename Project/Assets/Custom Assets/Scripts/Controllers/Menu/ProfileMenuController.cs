using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ProfileMenuController : MonoBehaviour {
    public UIProfile[] ui;
    public Sprite[] textures;
    public Button play;
    public Button delete;
    private Profile[] profiles;
    private bool active = false;
    private Image lastSelected;
    private int id = -1;

    void Start()
    {
        profiles = new Profile[3] {
            new Profile("New Profile"),
            new Profile("New Profile"),
            new Profile("New Profile")
        };
        //Load Profiles  
        for(int i = 0; i < profiles.Length; i++)
        {
            ui[i].name.text = profiles[i].name;
            if (profiles[i].currentID != -1)
            {
                ui[i].thumbnail.sprite = textures[1];
            }
            else
            {
                ui[i].thumbnail.sprite = textures[0];
            }
        }      
    }

    void Update()
    {
        if (!active && lastSelected)
        {
            lastSelected.enabled = false;                                     
            lastSelected = null;
            play.interactable = false;
            delete.interactable = false;
            id = -1;
        }
        else if (active && (!play.interactable || delete.interactable))
        {
            play.interactable = true;
            delete.interactable = true;
        }
    }

    public void SelectProfile(Image img)
    {    
        if (img != lastSelected)
        {   
            img.enabled = true;                                  
            if (lastSelected)
            {   
                lastSelected.enabled = false;                                        
            }
            lastSelected = img;
            active = true;
        }
    }
    public void SetID(int value)
    {
        id = value;
    }

    public void SetActive(bool value)
    {
        active = value;
    }
    
}
                           
[Serializable]
public class UIProfile
{
    public Image thumbnail;
    public Text name;
}
               
public class Profile
{
    public string name;
    public List<bool> completedLevels;  
    public int currentID;     

    public Profile(string _name) { name = _name; currentID = -1; }
}