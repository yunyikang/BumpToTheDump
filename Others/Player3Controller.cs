using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Player3Controller : NetworkBehaviour {
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) {
			return;
		}

		float rotate = Input.GetAxis("Horizontal2") * Time.deltaTime * 80.0f;
		float move = Input.GetAxis("Vertical2") * Time.deltaTime * 7.0f;

		transform.Rotate (0, 0, rotate);
		transform.Translate(0, move, 0);
		/*
		float jump = Input.GetAxis ("Jump");
		Vector3 movement = new Vector3(0, jump, 0); // 3d movement
		rb.AddForce(movement * 4);
		*/

		//this is lazy implementation of jump
		if (Input.GetKeyDown (KeyCode.Space)){
			rb.velocity += 8 * Vector3.up;
		}


		
	}
	void OnCollisionEnter(Collision c) {
		float force = 1500;
		float force1 = 600;

		//if Pudge
		if (c.gameObject.name == "pudge1") {
			Debug.Log ("Sudo was knocked back by Pudge with force 1500 :(((");
			Debug.Log (c.contacts[0].point.ToString());

			Vector3 dir = c.contacts [0].point - transform.position;

			dir = -dir.normalized;

			GetComponent<Rigidbody> ().AddForce (dir * force);
		}

		//if Meat Boy
		if (c.gameObject.name == "supermeatboyafterrig") {
			Debug.Log ("Sudo was knocked back by Meat Boy with force 1200 :(((");
			Debug.Log (c.contacts[0].point.ToString());

			Vector3 dir = c.contacts [0].point - transform.position;

			dir = -dir.normalized;

			GetComponent<Rigidbody> ().AddForce (dir * force1);
		}
	}
}

