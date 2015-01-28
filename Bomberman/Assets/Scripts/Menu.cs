using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Menu : MonoBehaviour {

	public bool play { get; set; }
	public bool network { get; private set; }
	public bool server { get; private set; }
	public NetworkManager networkManager;

	public Texture triangle;

	public GUIStyle layout_style;
	public GUIStyle title_style;
	public GUIStyle button_style;
	public GUIStyle triangle_style;
	public GUIStyle fields_style;

	string[] actions; // buttons to display on menu
	int action, players; // current action selected, number of player for the current game
	string UI, gameName, password; // current UI
	Stack<string> UIs; // precedent UI (for return)
	bool refresh;
	HostData host;

	void Start() {
		DontDestroyOnLoad(gameObject);

		refresh = false;
		play = false;
		server = false;
		host = null;
		UIs = new Stack<string>();
		action = 0;
		gameName = "Test";
		password = "";
		MainAction();
	}

	// When Bomberman Level is loaded
	void OnLevelWasLoaded(int level) {
		enabled = false;

		if (network) {
			if (server) {
				networkManager.StartServer(gameName);
			} else {
				networkManager.JoinServer(host);
			}
			return;
		}

		if (players >= 2) {
			Player.Put(new Vector3(-6,.2f,-5));
			Player.Put(new Vector3(-6,.2f,5));
		}
		if (players >= 3) {
			Player.Put(new Vector3(6,.2f,5));
		}
		if (players >= 4) {
			Player.Put(new Vector3(6,.2f,-5));
		}
	}

	// Call intern function without spaces
	void Action() {
		string str = Regex.Replace(actions[action], @"\s+", "");
		Invoke(UI + str + "Action", 0);
	}

	void Update() {
		if (Input.GetKeyDown("m")) {
			AudioListener.pause = !AudioListener.pause;
		}

		// Up & Down Selection
		if (Input.GetKeyDown("up")) {
			action = (action == 0) ? action : action - 1;
		}
		if (Input.GetKeyDown("down")) {
			action = (action == actions.Length - 1) ? action : action + 1;
		}

		// Escape for return to precedent menu
		if (Input.GetKeyDown(KeyCode.Escape) && UIs.Count > 0) {
			Return();
		}

		// Enter for action
		if (Input.GetKeyDown(KeyCode.Return)) {
			if (UI == "ReseauCreer") {
				network = true;
				server = true;
				Application.LoadLevel("Bomberman");
				return;
			} else if (UI == "ReseauRejoindre") {
				return;
			}
			string ui = UI;
			Action();
			if (UIs.Count == 0 || UIs.Peek() != ui) { UIs.Push(ui); }
			action = 0;
		}
	}

	// Used for return to precedent menu
	void Return() {
		UI = UIs.Pop();
		Invoke(UI + "Action", 0);
		action = 0;
	}

	// Actions from Main menu
	void MainAction() {
		UI = "Main";
		actions = new string[] {"Local", "Reseau", "Quitter"};
	}
	void MainLocalAction() {
		UI = "Local";
		actions = new string[] {"2 Joueurs", "3 Joueurs", "4 Joueurs"};
	}
	void MainReseauAction() {
		UI = "Reseau";
		actions = new string[] {"Creer", "Rejoindre"};
	}
	void MainQuitterAction() { Application.Quit(); }

	// Actions from Local
	void Local2JoueursAction() { Launch(2); }
	void Local3JoueursAction() { Launch(3); }
	void Local4JoueursAction() { Launch(4); }

	void Launch(int i) {
		players = i;
		Application.LoadLevel("Bomberman");
		play = true;
	}

	// Actions from Reseau
	void ReseauCreerAction() { UI = "ReseauCreer"; }
	void ReseauRejoindreAction() { UI = "ReseauRejoindre"; }

	void OnGUI () {
		title_style.fontSize = (int) (((float)Screen.width)*.18f);

		GUI.Label(
			new Rect (
				0.4f * Screen.width,
				0.05f * Screen.height,
				0.2f * Screen.width,
				0.3f * Screen.height
			), "Bomber", this.title_style
		);

		GUI.Label(
			new Rect (
				0.4f * Screen.width,
				0.25f * Screen.height,
				0.2f * Screen.width,
				0.3f * Screen.height
			), "Boy", this.title_style
		);

		if (!play) {
			GetType().GetMethod(UI + "UI").Invoke(this, null);
		} else {
			GUI.Label(
				new Rect (
					0.3f * Screen.width,
					0.7f * Screen.height,
					0.6f * Screen.width,
					0.3f * Screen.height
				), "Loading", this.button_style
			);
		}
	}

	public void MainUI() {
		GUILayout.BeginArea(new Rect(
			0.25f * Screen.width,
			0.6f * Screen.height,
			0.5f * Screen.width,
			0.4f * Screen.height
		), layout_style);

		foreach (string act in actions) {
			Lab(act);
		}

		GUILayout.EndArea();
	}

	public void LocalUI() {
		GUILayout.BeginArea(new Rect(
			0.2f * Screen.width,
			0.6f * Screen.height,
			0.7f * Screen.width,
			0.4f * Screen.height
		), layout_style);

		foreach (string act in actions) {
			Lab(act);
		}

		GUILayout.EndArea();
	}

	public void ReseauUI() {
		GUILayout.BeginArea(new Rect(
			0.2f * Screen.width,
			0.6f * Screen.height,
			0.7f * Screen.width,
			0.4f * Screen.height
		), layout_style);

		foreach (string act in actions) {
			Lab(act);
		}

		GUILayout.EndArea();
	}

	public void ReseauCreerUI() {
		GUI.Label(
			new Rect (
				0.1f * Screen.width,
				0.6f * Screen.height,
				0.4f * Screen.width,
				0.1f * Screen.height
			), "Name", this.button_style
		);

		gameName = GUI.TextField(new Rect(
			0.4f * Screen.width,
			0.6f * Screen.height,
			0.5f * Screen.width,
			0.1f * Screen.height
		), gameName, 25, fields_style);

		GUI.Label(
			new Rect (
				0.1f * Screen.width,
				0.8f * Screen.height,
				0.4f * Screen.width,
				0.1f * Screen.height
			), "Pass", this.button_style
		);

		password = GUI.PasswordField(new Rect(
			0.4f * Screen.width,
			0.8f * Screen.height,
			0.5f * Screen.width,
			0.1f * Screen.height
		), password, '*', 25, fields_style);
	}

	public void ReseauRejoindreUI() {
		if (networkManager.hostList != null) {
			for (int i = 0; i < networkManager.hostList.Length; i++) {
				if (GUI.Button(new Rect(
						0.4f * Screen.width,
						(0.6f + i*0.05f) * Screen.height,
						0.5f * Screen.width,
						0.1f * Screen.height
					), networkManager.hostList[i].gameName)) {
					
					network = true;
					host = networkManager.hostList[i];
					Application.LoadLevel("Bomberman");
				}
			}
		} else if (!refresh) {
			networkManager.RefreshHostList();
			refresh = true;
		}
	}

	void Lab(string lab) {
		GUILayout.BeginHorizontal();
		if (actions[action] == lab) GUILayout.Label(triangle, triangle_style);
		GUILayout.FlexibleSpace();
		GUILayout.Label(lab, button_style);
		GUILayout.EndHorizontal();
	}
}

