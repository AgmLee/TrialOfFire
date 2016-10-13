using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public PlayerController controller;
    public Text text;

    void Start()
    {
        if (controller == null)
        {
            controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        text.text = "Sprites: " + controller.spriteCount;
    }
}

