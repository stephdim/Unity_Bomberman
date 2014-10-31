using UnityEngine;
using System;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "stephdim_Unity_Bomberman";
	private const string gameName = "Test";

	private HostData[] hostList;
	public GameObject playerPrefab;
	private int playerNumber = 0;

	private void StartServer() {
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	void OnServerInitialized() {
		Debug.Log("Server Initializied");
		SpawnPlayer();
	}

	void OnGUI() {
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
		}
	}

	private void RefreshHostList() {
		MasterServer.RequestHostList(typeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent) {
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}

	private void JoinServer(HostData hostData) {
		Network.Connect(hostData);
	}

	void OnConnectedToServer() {
		Debug.Log("Server Joined");
		SpawnPlayer();
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

	private void SpawnPlayer() {
		playerNumber++;
		Network.Instantiate(
			playerPrefab,
			PositionOfPlayer(playerNumber),
			Quaternion.identity,
			0
		);
	}
}
