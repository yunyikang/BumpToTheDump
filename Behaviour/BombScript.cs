using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Destroy on collision
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Deploying Bomb");
            if (other.gameObject.name == "MeatBoy(Clone)")
            {

            }
            else
            {
                Debug.Log("Exploding bomb");
                Network.Destroy(gameObject);
                Destroy(this.gameObject);
            }
            
        }
        
    }
}
