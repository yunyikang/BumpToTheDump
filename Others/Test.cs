using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PrintCharacterNames();
	}

    public void PrintCharacterNames() {
        GameObject pudge = GameObject.Find("pudge");
        Debug.Log(pudge.name);
        Debug.Log(Color.cyan);
    }
}
