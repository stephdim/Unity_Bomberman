﻿using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof (NetworkView))]

public class NetworkManager : MonoBehaviour {

	public static bool enable = false;
	public GameObject playerPrefab;

	const string typeName = "StephdimUnityBomberman";
	string gameName = "Test";

	HostData[] hostList;

	int playerCount = 0; // OnPlayerConnected
	string passwordToEdit = "";

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

			gameName = GUI.TextField(new Rect(10, 10, 200, 20), gameName, 25);
			passwordToEdit = GUI.PasswordField(new Rect(10, 35, 200, 20), passwordToEdit, '*', 25);
		}
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
	}
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	private void StartServer() {
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	private void JoinServer(HostData hostData) {
		Network.Connect(hostData);
	}

	private void RefreshHostList() {
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

	[RPC]
	private void initTerrain() {}

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