using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreList : MonoBehaviour {

	public GameObject PlayerScoreboarditem;

	Transform scoreboardlist;



	

	// Use this for initialization
	void Start () {



			GameObject gO = (GameObject)Instantiate (PlayerScoreboarditem, scoreboardlist);


		



	}
	
	// Update is called once per frame
	void Update () {

		//goScore.transform.SetParent (this.transform);
		//goScore.transform.Find ("Player").GetComponent<Text> ().text = name;

		//goScore.transform.Find ("Survived(player1): ").GetComponent<Text> ().text = lives;


	

		
	}
}
