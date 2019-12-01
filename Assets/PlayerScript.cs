using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public AudioClip batteryCollect;
    public AudioClip destroyWall;
    public int moveSpeed;
    public GameObject siren;

    public float targetTime;
    private bool isYellow = false;
    public Material YellowMaterial;
    public Material RedMaterial;

	// Use this for initialization
	void Start ()
	{
	    targetTime = 1;
	}
    

    void OnCollisionStay(Collision collision)
    {
        var validWallsNamed = new[] {"Top", "Bottom", "Left", "Right"};

        if (validWallsNamed.Any(x => x == collision.gameObject.name) && Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(collision.gameObject);
            playAudio("destroyWall");
        }
    }

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

        // Update the position of the player
	    float v = Input.GetAxis("Vertical") * Time.fixedDeltaTime * moveSpeed;
	    float h = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveSpeed;
        transform.Translate(h,0,v);

	    targetTime -= Time.deltaTime;
 
	    if (targetTime <= 0.0f)
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

	        targetTime = 1;
	    }
	}
}
