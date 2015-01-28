using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]

public class NetworkManager : MonoBehaviour {

	public static bool enable = false;
	Menu menu;
	GameObject playerPrefab;

	const string typeName = "StephdimUnityBomberman";
	// string gameName = "Test";

	public HostData[] hostList { get; private set; }

	int playerCount = 0; // OnPlayerConnected
	// string passwordToEdit = "";

	void Start() {
		DontDestroyOnLoad(gameObject);
		menu = GameObject.FindObjectOfType<Menu>();
		playerPrefab = (GameObject) Resources.Load("Player");
	}

	Rect SRect(float x, float y, float w, float h, float sw, float sh) {
		return new Rect(x * sw, y * sh, w * sw, h * sh);
	}

	void OnGUI() {
		if (!menu.network || menu.play) return;

		GUILayout.BeginArea(
			new Rect(0, 0, Screen.width, Screen.height),
			menu.layout_style
		);
		GUILayout.FlexibleSpace(); GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();

		if (Network.isServer && playerCount >= 1) {
			if (GUILayout.Button("Launch", menu.button_style)) {
				// Launch and close new connection of others players
				networkView.RPC("Play", RPCMode.All);
			}
		} else {
			GUILayout.Label("Wait...", menu.button_style);
		}

		GUILayout.FlexibleSpace(); GUILayout.EndHorizontal(); GUILayout.FlexibleSpace();
		GUILayout.EndArea();

/*
		if (!Network.isClient && !Network.isServer) {
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")) {
				StartServer();
			}

			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts")) {
				RefreshHostList();
			}

			if (hostList != null) {
				for (int i = 0; i < hostList.Length; i++) {
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName)) {
						JoinServer(hostList[i]);
					}
				}
			}

			gameName = GUI.TextField(new Rect(10, 10, 200, 20), gameName, 25);
			passwordToEdit = GUI.PasswordField(new Rect(10, 35, 200, 20), passwordToEdit, '*', 25);
		}
*/
	}

	[RPC]
	void Play() {
		menu.play = true;
		enable = true;
		GameObject.FindObjectOfType<Terrain>().Play();
	}

	void OnMasterServerEvent(MasterServerEvent msEvent) {
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		} else if (msEvent == MasterServerEvent.RegistrationFailedGameName) {
			Debug.Log("Registration failed because an empty game name was given.");
		} else if (msEvent == MasterServerEvent.RegistrationFailedGameType) {
			Debug.Log("Registration failed because an empty game type was given.");
		} else if (msEvent == MasterServerEvent.RegistrationFailedNoServer) {
			Debug.Log("Registration failed because no server is running.");
		} else if (msEvent == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log("Registration to master server succeeded, received confirmation.");
		}
	}

	void OnServerInitialized() {
		Debug.Log("Server Initializied");
		SpawnPlayer(1);
	}
	void OnConnectedToServer() {
		Debug.Log("Server Joined");
	}
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("Disconnected from server: " + info);
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error);
	}
	void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
		Debug.Log("Could not connect to master server: " + info);
	}

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player " + playerCount++ + " connected from " + player.ipAddress + ":" + player.port);
		networkView.RPC("SpawnPlayer", player, Network.connections.Length + 1);

		Vector3[] blocks = Array.ConvertAll(GameObject.FindGameObjectsWithTag("Block"), e => e.transform.position);

		Bonus[] tmp = Array.ConvertAll(GameObject.FindGameObjectsWithTag("Bonus"), e => e.GetComponent<Bonus>());
		Vector3[] bonus_pos = Array.ConvertAll(tmp, e => e.gameObject.transform.position);
		string[] bonus_typ = Array.ConvertAll(tmp, e => e.type);

		InitTerrain(player, blocks, bonus_pos, bonus_typ);
	}
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	public void StartServer(string gameName) {
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	public void JoinServer(HostData hostData) {
		Network.Connect(hostData);
	}

	public void RefreshHostList() {
		MasterServer.RequestHostList(typeName);
	}

	private Vector3 PositionOfPlayer(int id) {
		if (id == 1) {
			return new Vector3(-6, 0.5f, -5);
		} else if (id == 2) {
			return new Vector3(6, 0.5f, -5);
		} else if (id == 3) {
			return new Vector3(6, 0.5f, 5);
		} else if (id == 4) {
			return new Vector3(-6, 0.5f, 5);
		}
		throw new ArgumentException("id must be between [1;4]", "id");
	}

	private void InitTerrain(NetworkPlayer player, Vector3[] blocks, Vector3[] bonus_pos, string[] bonus_typ) {
		foreach(Vector3 v in blocks) {
			networkView.RPC(
				"PutBlock",
				player,
				v
			);
		}

		for(int i = 0; i < bonus_pos.Length ; i++) {
			networkView.RPC(
				"PutBonus",
				player,
				bonus_pos[i],
				bonus_typ[i]
			);
		}
	}

	[RPC]
	void PutBlock(Vector3 v) {
		Block.Put(v);
	}

	[RPC]
	void PutBonus(Vector3 v, string t) {
		Bonus.Put(v, t);
	}

	[RPC]
	private void SpawnPlayer(int id) {
		Network.Instantiate(
			playerPrefab,
			PositionOfPlayer(id),
			Quaternion.identity,
			0
		);
	}
}
