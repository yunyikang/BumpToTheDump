using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class countdownscript : MonoBehaviour {
	[SerializeField] private Text uiText;
	[SerializeField] private float mainTimer;


	GameObject goScore;
	public float timer;
	private bool canCount = true;
	public bool doOnce = false;

	void Start(){
        //goScore = GameObject.FindWithTag("Scoreboard");
	}

	void Update(){
		if (timer >= 0.0f && canCount){
				timer -= Time.deltaTime;
				uiText.text = timer.ToString ("F");
			goScore.SetActive (false);


			}

		else if (timer <= 0.0f && !doOnce)
		{
			canCount = false;
			doOnce = true;
				uiText.text = "0.00";
				timer = 0.0f;

		
			goScore.SetActive (!goScore.activeSelf);


		}
	}
}




