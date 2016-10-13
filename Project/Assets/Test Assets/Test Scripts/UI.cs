using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	public RBCharacterController controller;
    public Text text;

    void Start()
    {
        if (controller == null)
        {
			controller = GameObject.FindGameObjectWithTag("Player").GetComponent<RBCharacterController>();
        }
    }

    void Update()
    {
        text.text = "Sprites: " + controller.spriteCount;
    }
}

