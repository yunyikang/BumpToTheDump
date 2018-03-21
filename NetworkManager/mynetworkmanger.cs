using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;

public class mynetworkmanager : NetworkManager {

	public Button player1Button;
	public Button player2Button;


	int avatarIndex = 0;
	// Use this for initialization
	void Start () {
		player1Button.onClick.AddListener (delegate {
			AvatarPicker (player1Button.name);
		});
		player2Button.onClick.AddListener (delegate {
			AvatarPicker (player2Button.name);
		});
	
		
	}
	
	// Update is called once per frame
	void AvatarPicker(string buttonName) {
		switch (buttonName) {
		case "Player 1":
			avatarIndex = 0;
			break;
		case "Player 2":
			avatarIndex = 1;
			break;
		case "Player 3":
			avatarIndex = 2;
			break;
		}
		
		playerPrefab = spawnPrefabs [avatarIndex];
		
	}

    //public override void onClientConnect(NetworkConnection conn)
    //{
    //    characterSelectionCanvas.enabled = false;
    //    IntegerMessage msg = new IntegerMessage(avatarIndex);
    //    if (!clientLoadedScene)
    //    {

    //        ClientScene.Ready(conn);
    //        if (autoCreatePlayer)
    //        {
    //            ClientScene.AddPlayer(conn, 0, msg);

    //        }
    //    }
    //}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,
		NetworkReader extraMessageReader){
		int id = 0;
		if (extraMessageReader != null) {
			IntegerMessage i = extraMessageReader.ReadMessage<IntegerMessage> ();
			id = i.value;
		}
		GameObject playerPrefab = spawnPrefabs [id];

		GameObject player;
		Transform startPos = GetStartPosition ();
		if (startPos != null) {
			player = (GameObject)Instantiate (playerPrefab, startPos.position, startPos.rotation);
		} else {
			player = (GameObject)Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
		}
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}
}
