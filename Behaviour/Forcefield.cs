
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forcefield : MonoBehaviour {

    public Animator animation;
    public GameObject go;
    // Use this for initialization
    void Start ()
    {
        go.GetComponent<Animation>().Play("Forcefield");

    }

}
