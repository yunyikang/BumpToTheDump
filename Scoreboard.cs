using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Prototype.NetworkLobby;





public class Scoreboard : NetworkBehaviour {

    public static Scoreboard instanceScoreboard = null;

    public GameObject scoreboardList;
    public Text[] texts;
    public LayoutElement[] entries;
    public int players;

    LobbyManager manager;


    void Awake()
    {
        if (instanceScoreboard == null)
        {
            instanceScoreboard = this;

        }   
        else if (instanceScoreboard != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        manager = LobbyManager.s_Singleton;
        //players = 2;
        scoreboardList = GameObject.FindWithTag("ScoreList");
        texts = scoreboardList.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++) {
            texts[i].text = manager.scoreList[i];
        }
        entries = scoreboardList.GetComponentsInChildren<LayoutElement>();


    }

    // Update is called once per frame
    void Update () {

	}


    //Get and Set Kills
    public void setSudoKills(int s)
    {
        texts[1].text = s.ToString();
        Debug.Log(texts[1].text);
    }
    public string getSudoKills()
    {
        return texts[1].text;
    }

    public void setPudgeKills(int s)
    {
        texts[4].text = s.ToString();
        Debug.Log(texts[4].text);
    }
    public string getPudgeKills()
    {
        return texts[4].text;
    }

    public void setMeatKills(int s)
    {
        texts[7].text = s.ToString();
        Debug.Log(texts[7].text);
    }
    public string getMeatKills()
    {
        return texts[7].text;
    }

    //Get and Set Lives
    public void setSudoLives(int s)
    {
        texts[2].text = s.ToString();
        Debug.Log(texts[2].text);
    }
    public string getSudoLives()
    {
        return texts[2].text;
    }
    public void setPudgeLives(int s)
    {
        texts[5].text = s.ToString();
        Debug.Log(texts[5].text);
    }
    public string getPudgeLives()
    {
        return texts[5].text;
    }
    public void setMeatLives(int s)
    {
        texts[8].text = s.ToString();
        Debug.Log(texts[8].text);
    }
    public string getMeatLives()
    {
        return texts[8].text;
    }

    public void setPlayers(int i) {
        this.players = i;
    }
    public int getPlayers(int i)
    {
        return this.players;
    }

    public void updateEntries()
    {
        if (players == 1)
        {
            GameObject.FindWithTag("Player2").SetActive(false);
            GameObject.FindWithTag("Player3").SetActive(false);

        }
        else if (players == 2)
        {
            GameObject.FindWithTag("Player3").SetActive(false);

        }
        
    }


}
