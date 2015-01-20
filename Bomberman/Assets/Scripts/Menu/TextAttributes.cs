using UnityEngine;
using System.Collections;

public class TextAttributes : MonoBehaviour {

	public Texture triangle;

	public GUIStyle layout_style;
	public GUIStyle title_style;
	public GUIStyle button_style;
	public GUIStyle triangle_style;

	string[] actions = new string[] {"Local", "Reseau", "Quitter"};
	int action;
	string UI = "Main";

	void Start() {
		action = 0;
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

		if (Input.GetKeyDown(KeyCode.Return)) {
			Invoke(actions[action] + "Action", 0);
		}
	}

	void LocalAction() {
		UI = "Local";
		actions = new string[] {"2 Joueurs", "3 Joueurs", "4 Joueurs"};
	}
	void ResauAction() {
		UI = "Reseau";
	}
	void QuitterAction() {
		Application.Quit();
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

		if (UI == "Main") {
			MainUI();
		} else if (UI == "Local") {
			LocalUI();
		} else if (UI == "Network") {
			NetworkUI();
		}
	}

	void MainUI() {
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

	void LocalUI() {
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

	void NetworkUI() {

	}

	void Lab(string lab) {
		GUILayout.BeginHorizontal();
		if (actions[action] == lab) GUILayout.Label(triangle, triangle_style);
		GUILayout.FlexibleSpace();
		GUILayout.Label(lab, button_style);
		GUILayout.EndHorizontal();
	}
}

