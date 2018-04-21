using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectileforward : MonoBehaviour {
    public float speed;
    public Rigidbody rb;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    

}
