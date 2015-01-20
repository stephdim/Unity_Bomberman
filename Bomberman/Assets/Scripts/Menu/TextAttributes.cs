using UnityEngine;
using System.Collections;

public class TextAttributes : MonoBehaviour {

	public GUIStyle title_style;
	public GUIStyle button_style;
	public AudioClip title_sound;

	void Start(){
		audio.PlayOneShot(title_sound);
		if (Input.GetKeyDown("m")) {
			AudioListener.pause = !AudioListener.pause;
		}
	}

	void OnGUI () {
		GUI.Label(
			new Rect (
				0.4f * Screen.width,
				0.1f * Screen.height,
				0.2f * Screen.width,
				0.3f * Screen.height
			), "BomberBoy", this.title_style
		);

		if (GUI.Button(
				new Rect(
					0.4f * Screen.width,
					0.45f * Screen.height,
					0.2f * Screen.width,
					this.button_style.fontSize
				), "Jouer", this.button_style)
			) {

			Application.LoadLevel("Bomberman");

		}

		if (GUI.Button(
				new Rect(
					0.4f * Screen.width,
					0.7f * Screen.height,
					0.2f * Screen.width,
					this.button_style.fontSize
				), "Quitter", this.button_style)
			) {

			Application.Quit();

		}
	}
}

