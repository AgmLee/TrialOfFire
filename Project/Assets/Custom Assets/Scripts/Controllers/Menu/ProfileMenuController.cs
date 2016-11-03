using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ProfileMenuController : MonoBehaviour {
    public UIProfile[] ui;
    public Sprite[] textures;
    public Button play;
    public Button createMain;
    public Button createSub;
    public Button delete;
    public InputField input;
    private Profile[] profiles;
    private bool active = false;
    private bool inSub = false;
    private Image lastSelected;
    private int id = -1;

    void Start()
    {
        //Create Empty Profiles
        profiles = new Profile[3] {
            new Profile("New Profile"),
            new Profile("New Profile"),
            new Profile("New Profile")
        };
        
        //Load Profiles 
        for (int i = 0; i < 3; i++)
        {
            string current = "/ProfileData/profile" + (i + 1) + ".dat";
            if (File.Exists(Application.persistentDataPath + current))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + current, FileMode.Open);

                DATA d = (DATA)bf.Deserialize(file);

                file.Close();

                profiles[i].currentID = d.currentID;
                profiles[i].collectedAmount = d.collectedAmount;
                profiles[i].name = d.name;
            }
        }
        
        //Load UI Elemets
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
        play.gameObject.SetActive(false);
        createMain.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!active && lastSelected)
        {
            lastSelected.enabled = false;                                     
            lastSelected = null;
            play.gameObject.SetActive(false);
            createMain.gameObject.SetActive(true);
            createMain.interactable = false;
            delete.interactable = false;
            inSub = false;
            id = -1;
            input.text = "";
        }
        else if (active)
        {
            if (profiles[id].currentID != -1)
            {
                delete.interactable = true;
                play.gameObject.SetActive(true);
                createMain.gameObject.SetActive(false);
            }
            else
            {
                play.gameObject.SetActive(false);
                createMain.gameObject.SetActive(true);
            }
            if (!inSub)
            {
                input.text = "";
            }
            if (input.text != "")
            {
                createSub.interactable = true;
            }
            else
            {
                createSub.interactable = false;
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
            createMain.interactable = true;
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
    public void DeleteProfile()
    {
        profiles[id] = new Profile("New Game");
        ui[id].thumbnail.sprite = textures[0];
        ui[id].name.text = profiles[id].name;
        delete.interactable = false;
    }
    public void StartGame()
    {
        GameManager.inst.LoadGame(profiles[id], id + 1);
        #if DEBUG
        Debug.Log("Loaded Scene");
        #endif
    }
    public void CreateProfile()
    {
        profiles[id].name = input.text;
        profiles[id].currentID = 0;

        ui[id].name.text = profiles[id].name;
        ui[id].thumbnail.sprite = textures[1];
        input.text = "";
    }
    public void SetSub(bool value)
    {
        inSub = value;
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
    public int collectedAmount;
    public int currentID;     

    public Profile(string _name) { name = _name; currentID = -1; }
}