using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Menu : MonoBehaviour {

	public bool play { get; private set; }

	public Texture triangle;

	public GUIStyle layout_style;
	public GUIStyle title_style;
	public GUIStyle button_style;
	public GUIStyle triangle_style;

	string[] actions;
	int action, players;
	string UI;
	Stack<string> UIs;

	void Start() {
		DontDestroyOnLoad(gameObject);

		play = false;
		UIs = new Stack<string>();
		action = 0;
		MainAction();
	}

	void OnLevelWasLoaded(int level) {
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
		play = true;
		enabled = false;
	}

	void Action(string action) {
		action = Regex.Replace(action, @"\s+", "");
		Invoke(UI + action + "Action", 0);
	}

	void Update() {
		if (Input.GetKeyDown("m")) {
			AudioListener.pause = !AudioListener.pause;
		}

		if (Input.GetKeyDown("up")) {
			action = (action == 0) ? action : action - 1;
		}
		if (Input.GetKeyDown("down")) {
			action = (action == actions.Length - 1) ? action : action + 1;
		}

		if (Input.GetKeyDown(KeyCode.Escape) && UIs.Count > 0) {
			Return();
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			string ui = UI;
			Action(actions[action]);
			if (UIs.Count == 0 || UIs.Peek() != ui) { UIs.Push(ui); }
			action = 0;
		}
	}

	void Return() {
		UI = UIs.Pop();
		Invoke(UI + "Action", 0);
		action = 0;
	}

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
	void MainQuitterAction() {
		Application.Quit();
	}

	void Local2JoueursAction() { Launch(2); }
	void Local3JoueursAction() { Launch(3); }
	void Local4JoueursAction() { Launch(4); }

	void Launch(int i) {
		players = i;
		Application.LoadLevel("Bomberman");
	}

	void ReseauCreerAction() {

	}
	void ReseauRejoindreAction() {

	}

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

		GetType().GetMethod(UI + "UI").Invoke(this, null);
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

	void Lab(string lab) {
		GUILayout.BeginHorizontal();
		if (actions[action] == lab) GUILayout.Label(triangle, triangle_style);
		GUILayout.FlexibleSpace();
		GUILayout.Label(lab, button_style);
		GUILayout.EndHorizontal();
	}
}

