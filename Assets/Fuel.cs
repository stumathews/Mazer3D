using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
 * Fuel script attached to fuel components will destroy themselves on collision with only the player
 */
public class Fuel : MonoBehaviour
{

	void Start () {
		
	}

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "player") return;
        Destroy(transform.gameObject);
        
        // Make a noise by telling the player to make it!
        collision.transform.gameObject.SendMessage("playAudio", "batteryCollect");
    }
	

    void OnDestroy()
    {
        // Everytime we get destroyed we need to remove fuel from the game.
        Main.FuelLeft--;
    }
}
