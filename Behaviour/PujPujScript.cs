using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PujPujScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        Destroy(this.gameObject);
    }
}
