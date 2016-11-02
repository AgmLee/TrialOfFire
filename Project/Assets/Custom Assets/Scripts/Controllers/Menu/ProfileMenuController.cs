using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ProfileMenuController : MonoBehaviour {
    public UIProfile[] ui;
    public Sprite[] textures;
    public Button play;
    public Button delete;
    public Animator delMenu;
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
        profiles[0].currentID = 0;
        profiles[0].name = "TEST"; 
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
        play.GetComponentInChildren<Text>().text = "Play Game";
    }

    void Update()
    {
        if (!active && lastSelected)
        {
            lastSelected.enabled = false;                                     
            lastSelected = null;
            play.interactable = false;
            delete.interactable = false;
            play.GetComponentInChildren<Text>().text = "Play Game";
            id = -1;
        }
        else if (active)
        {
            if (profiles[id].currentID != -1)
            {
                delete.interactable = true;
                play.GetComponentInChildren<Text>().text = "Play Game";
            }
            else
            {
                play.GetComponentInChildren<Text>().text = "Create Profile";
            }
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
            delete.interactable = false;
            play.interactable = true;
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
    public void PlayAnimation(bool hide)
    {
        if (hide && delMenu.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            delMenu.Play("Hide");
        }
        else if (delMenu.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            delMenu.Play("Show");
        }
    }
    public void DeleteProfile()
    {
        if (delMenu.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            profiles[id] = new Profile("New Profile");
            delMenu.Play("Hide");
        }
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