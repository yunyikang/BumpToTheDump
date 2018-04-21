using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PudgeOffline : MonoBehaviour {
	public float speed;
	private Rigidbody rb;
	public bool isFlat;
    float maxDashTime = 1.0f;
    float currentDashTime;
    Vector3 moveDirection;
    float increment = 0.1f;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
		isFlat = true;
        currentDashTime = maxDashTime;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Translate (Input.acceleration.x, 0, -Input.acceleration.z);
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical); // 3d movement
        transform.Translate(movement);

		/*
		 Debug.Log (isLocalPlayer);

		if (!isLocalPlayer) {

			return;
		}
        */
        /*
		float rotate = Input.GetAxis("Horizontal") * Time.deltaTime * 20.0f;
		float move = Input.GetAxis("Vertical") * Time.deltaTime * 2f;

		transform.Rotate (0, rotate, 0);
		transform.Translate(0, 0, move);
        */
        //use skill
        /*
        Debug.Log(currentDashTime);

        if (Input.GetButtonDown("Dash")) {
            float currentDashTime = 0.0f;
        }
        if (currentDashTime < maxDashTime) {
            moveDirection = new Vector3(0, 0, 1.0f);
            currentDashTime += increment;
        }
        else {
            moveDirection = Vector3.zero;
        }
        transform.Translate(moveDirection);
        */
        if (Input.GetButtonDown("Dash")) {
            transform.position += new Vector3(0.0f, 0.1f, 0.0f * Time.deltaTime);
        }
        
		/*Vector3 tilt = Input.acceleration;
		if (isFlat) {
			tilt = Quaternion.Euler (90, 0, 0) * tilt; 
		}

		rb.AddForce (tilt);
		*/
        /*
		Vector3 dir = Vector3.zero;
		dir.x = -Input.acceleration.y;
		dir.z = Input.acceleration.x;
		if (dir.sqrMagnitude > 1)
			dir.Normalize ();
		dir *= Time.deltaTime;
		transform.Translate (dir * speed);
		*/
	}

	//public override void getInput() {
	//}
	//public override void move() {
		
	//}
}
