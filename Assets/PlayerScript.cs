using System.Linq;
using UnityEngine;

/*
 * Player can move, destroy walls it collides with, apply timer damage and play sounds
 */
public class PlayerScript : MonoBehaviour
{
    // make some noise
    public AudioClip batteryCollect;
    public AudioClip destroyWall;
    public int moveSpeed;
    public GameObject siren;

    // how long to wait until swap lights
    public float SirenLightFlashIntervalTime;
    
    // simple state tracker of colour of siren
    private bool isYellow = false;

    // colours of the siren
    public Material YellowMaterial;
    public Material RedMaterial;

	// Use this for initialization
	void Start ()
	{
	    SirenLightFlashIntervalTime = 1;
	}
    

    void OnCollisionStay(Collision collision)
    {
        // we will destroy gameobjects with the names of the wall prefabs if the player presses space and collides with it
        var validWallsNamed = new[] {"Top", "Bottom", "Left", "Right"};

        if (validWallsNamed.Any(x => x == collision.gameObject.name) && Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(collision.gameObject);
            // play destroy wall sound
            playAudio("destroyWall");
            Main.LevelTimer.ApplyTimeDisadvantage();
        }
    }

    // nice function to play sound in one way depending on the input sound clip
    void playAudio(string clipName)
    {
        if(clipName == "batteryCollect")
            GetComponent<AudioSource>().PlayOneShot(batteryCollect);
        if(clipName == "destroyWall")
            GetComponent<AudioSource>().PlayOneShot(destroyWall);
    }

	// Update is called once per frame
	void Update ()
	{

	    if (Main.endSceneStarted)
	        return; // dont update player anymore
        // Update the position of the player
	    float v = Input.GetAxis("Vertical") * Time.fixedDeltaTime * moveSpeed;
	    float h = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveSpeed;
        transform.Translate(h,0,v);

        // Swap out robot's material for the siren every second
	    SirenLightFlashIntervalTime -= Time.deltaTime;
 
	    if (SirenLightFlashIntervalTime <= 0.0f)
	    {
	        MeshRenderer gameObjectRenderer = siren.GetComponent<MeshRenderer>();
	        if (isYellow)
	        {
	            
	            gameObjectRenderer.material = RedMaterial;
	            isYellow = false;
	        }
	        else
	        {
	            gameObjectRenderer.material = YellowMaterial;
	            isYellow = true;
	        }

	        SirenLightFlashIntervalTime = 1;
	    }
	}
}
