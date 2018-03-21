using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PudgeController : NetworkBehaviour
{

    public float speed;
    private Rigidbody rb;
    public bool isFlat;

    
    private Vector3 _playerSpawnPointpos; // player to spawn here

    /*
     Things to do:
     - basic movement (figure out a way to move without rolling) 
        - with keyboard for computer
        - accelerometer for phone
     - integrate movement with animation(s)
     - make sure map is able to be navigated by puj

     */

    // Use this for initialization
    void Start()
    {
        // prevents rolling
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // initialise rigidbody
        this.rb = GetComponent<Rigidbody>();
        // detect phone movement
        isFlat = true;
    }


    // Update is called once per frame
    void Update()
    {
        var speed = 10.0f;

        // current coordinates of player - for use later
        var position = transform.position;
       
        // movement with keyboard
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3 (-moveHorizontal, 0.0f, -moveVertical);

        // adding force to move
        this.rb.AddForce(movement * speed);

        // if (!isLocalPlayer)
        // {
        //     return;
        // }

        // Android movement section
        // Vector3 tilt = Input.acceleration;
        // if (isFlat)
        // {
        //     tilt = Quaternion.Euler(90, 0, 0) * tilt;
        // }

        // // handles accelerometer
        // rb.AddForce(tilt);

    }


    // ignoring this for now
    // void OnCollisionEnter(Collision c)
    // {
    //     float force = 300;
    //     float force1 = 100;
    //     float force3 = 800;


    //     //if Meat Boy
    //     if (c.gameObject.name == "supermeatboyafterrig")
    //     {
    //         Debug.Log("Pudge was knocked back by Meat Boy with force 500");
    //         Debug.Log(c.contacts[0].point.ToString());

    //         Vector3 dir = c.contacts[0].point - transform.position;

    //         dir = -dir.normalized;

    //         GetComponent<Rigidbody>().AddForce(dir * force);
    //     }

    //     //if Sudo
    //     else if (c.gameObject.name == "sudotest2blend")
    //     {
    //         Debug.Log("Pudge was knocked back by Sudo with force 200 only XD");
    //         Debug.Log(c.contacts[0].point.ToString());

    //         Vector3 dir = c.contacts[0].point - transform.position;

    //         dir = -dir.normalized;

    //         GetComponent<Rigidbody>().AddForce(dir * force1);
    //     }

    //     else
    //     {
    //         Debug.Log("Pudge was knocked back by Pudge with force 800 only XD");
    //         Debug.Log(c.contacts[0].point.ToString());

    //         Vector3 dir = c.contacts[0].point - transform.position;

    //         dir = -dir.normalized;

    //         GetComponent<Rigidbody>().AddForce(dir * force1);
    //     }
    // }
}
