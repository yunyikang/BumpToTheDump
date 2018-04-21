using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterOffline : MonoBehaviour {

	public float speed;
	private Rigidbody rb;


	public abstract void getInput();
	public abstract void move ();

}
	