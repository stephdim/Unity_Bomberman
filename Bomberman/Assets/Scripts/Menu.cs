using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private bool play;

	void Start() {
		play = false;
	}

	void OnGUI() {
		if (!play) {

			GUILayout.Box("Choose number of player");

			if (GUILayout.Button("2 players")) {
				Player.Put(new Vector3(-6,.2f,-5));
				Player.Put(new Vector3(-6,.2f,5));
				play = true;
			}

			if (GUILayout.Button("3 players")) {
				Player.Put(new Vector3(-6,.2f,-5));
				Player.Put(new Vector3(-6,.2f,5));
				Player.Put(new Vector3(6,.2f,5));
				play = true;
			}

			if (GUILayout.Button("4 players")) {
				Player.Put(new Vector3(-6,.2f,-5));
				Player.Put(new Vector3(-6,.2f,5));
				Player.Put(new Vector3(6,.2f,5));
				Player.Put(new Vector3(6,.2f,-5));
				play = true;
			}
		}

	}

}