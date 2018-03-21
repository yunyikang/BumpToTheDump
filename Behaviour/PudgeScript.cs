using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PudgeController : NetworkBehaviour {

	public float speed;
	private Rigidbody rb;
	public bool isFlat;


	private Vector3 _playerSpawnPointpos; // player to spawn here

	// Use this for initialization
	void Start () {

	
		rb = GetComponent<Rigidbody>();
		isFlat = true;
	}


	// Update is called once per frame
	void Update () {
		//float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");

		//Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical); // 3d movement
		//rb.AddForce(movement * speed);
		//Debug.Log (isLocalPlayer);

		if (!isLocalPlayer) {

			return;
		}

		/*
		float rotate = Input.GetAxis("Horizontal") * Time.deltaTime * 20.0f;
		float move = Input.GetAxis("Vertical") * Time.deltaTime * 2f;

		transform.Rotate (0, rotate, 0);
		transform.Translate(0, 0, move);
		*/
		Vector3 tilt = Input.acceleration;
		if (isFlat) {
			tilt = Quaternion.Euler (90, 0, 0) * tilt; 
		}

		rb.AddForce (tilt);




	}



	void OnCollisionEnter(Collision c) {
		float force = 300;
		float force1 = 100;
		float force3 = 800;


		//if Meat Boy
		if (c.gameObject.name == "supermeatboyafterrig") {
			Debug.Log ("Pudge was knocked back by Meat Boy with force 500");
			Debug.Log (c.contacts[0].point.ToString());

			Vector3 dir = c.contacts [0].point - transform.position;

			dir = -dir.normalized;

			GetComponent<Rigidbody> ().AddForce (dir * force);
		}

		//if Sudo
		else if (c.gameObject.name == "sudotest2blend") {
			Debug.Log ("Pudge was knocked back by Sudo with force 200 only XD");
			Debug.Log (c.contacts[0].point.ToString());

			Vector3 dir = c.contacts [0].point - transform.position;

			dir = -dir.normalized;

			GetComponent<Rigidbody> ().AddForce (dir * force1);
		}

		else {
			Debug.Log ("Pudge was knocked back by Pudge with force 800 only XD");
			Debug.Log (c.contacts[0].point.ToString());

			Vector3 dir = c.contacts [0].point - transform.position;

			dir = -dir.normalized;

			GetComponent<Rigidbody> ().AddForce (dir * force1);
		}
	}
}
