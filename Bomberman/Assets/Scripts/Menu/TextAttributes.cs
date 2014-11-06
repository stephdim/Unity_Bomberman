using UnityEngine;
using System.Collections;

public class TextAttributes : MonoBehaviour
{
	
	public GUIStyle title_style;
	public GUIStyle button_style;

	void Update() {
		
	}


	void OnGUI () {

		GUI.Label (new Rect (0.4f * Screen.width,0.6f * Screen.width/10,0.2f * Screen.width,0.3f * Screen.height), "BomberBoy", this.title_style);

		if(GUI.Button(new Rect(0.4f * Screen.width,2.1f * Screen.width/10,0.2f * Screen.width,this.button_style.fontSize), "Jouer", this.button_style)) {
			Application.LoadLevel("Bomberman");
		}

		if(GUI.Button(new Rect(0.4f * Screen.width,2.6f * Screen.width/10,0.2f * Screen.width,this.button_style.fontSize), "Quitter", this.button_style)) {
			Application.Quit();
		}
	}
}

