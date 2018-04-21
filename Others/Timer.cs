using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    [SerializeField] private Text uiText;
    //[SerializeField] private float mainTimer;

    //public static Timer instanceTimer;
    public GameObject scoreboard;
    GameObject scoreList;
    RectTransform r;
    Scoreboard scoreScript;

    bool timeToHide = true;
    bool timeToShow = false;

    float timer;
    bool canCount = true;
    bool doOnce = false;
    public bool done = false;



    void Start()
    {
        timer = 30; //SET TIME HERE

        //scoreList = GameObject.FindWithTag("ScoreList");
        //scoreScript = Scoreboard.instanceScoreboard;
        //Debug.Log(scoreScript.players + " TSTETSETT");

        //r = GameObject.FindWithTag("Scoreboardd").GetComponent<RectTransform>();


    }
    void Update()
    {
        if (timer >= 0.0f && canCount)
        {
            if (timeToHide)
            {
                //r.anchoredPosition = new Vector2(-500, 700);
                //Debug.Log(r.anchoredPosition);
                timeToHide = false;

            }
            timer -= Time.deltaTime;
            uiText.text = timer.ToString("F");
            //Debug.Log(r.anchoredPosition);
            //r = GameObject.FindWithTag("Scoreboardd").GetComponent<RectTransform>();
            //Debug.Log(r.anchoredPosition);
            //Vector2 -11.1 0
            //scoreboard.SetActive(false);
            //Debug.Log("SCOREBOARD HIDDEN");


        }

        else if (timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uiText.text = "0.00";
            timer = 0.0f;
            this.done = true;


            //scoreboard.SetActive(!scoreboard.activeSelf);
            //scoreScript.updateEntries();
            //r.anchoredPosition = new Vector2(-11, 0);



        }
    }

    
}




