using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenuComponentScript : MonoBehaviour
{

    public AudioClip beep;

    public GUISkin menuSkin;

    public Rect menuArea;

    public Rect playButton;

    public Rect instructionsButton;

    public Rect quitButton;

    public Texture2D LogoTexture;

    private Rect menuAreaNormalized;
    private string menupage = "main";

    void OnGUI()
    {
        GUI.BeginGroup(menuAreaNormalized);
        if (menupage == "main")
        {
            if (GUI.Button(new Rect(playButton), "Play"))
            {
                SceneManager.LoadScene("Alpha");
            }

            if (GUI.Button(new Rect(instructionsButton), "Rules"))
            {
                menupage = "instructions";
            }

            if (GUI.Button(new Rect(quitButton), "Quit"))
            {
                Application.Quit();
            }
        }

        if (menupage == "instructions")
        {
            GUI.Label(new Rect(0,0,200,400), "Collect all the fuel for your robot before the time runs out. Arrow keys to move, and rotate map with 'R'");
            if(GUI.Button(new Rect(quitButton), "Back")){
                menupage="main";
            }
        }

        GUI.EndGroup();
    }
	// Use this for initialization
	void Start () {
	    menuAreaNormalized =
	        new Rect(menuArea.x * Screen.width - (menuArea.width * 0.5f),
	            menuArea.y * Screen.height - (menuArea.height * 0.5f),
	            menuArea.width, menuArea.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
