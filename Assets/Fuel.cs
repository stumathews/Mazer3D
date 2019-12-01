using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{

    
    // Use this for initialization
	void Start () {
		
	}

    void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag == "player")
        {
            Destroy(transform.gameObject);
            
            collision.transform.gameObject.SendMessage("playAudio", "batteryCollect");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        Main.FuelLeft--;
    }
}
