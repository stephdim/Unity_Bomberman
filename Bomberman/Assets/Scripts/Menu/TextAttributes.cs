using UnityEngine;
using System.Collections;

public class TextAttributes : MonoBehaviour
{

/*	Color enter_color = Color.white;
	Color leave_color = Color.green;


	void OnMouseEnter(){
		this.button_style.normal.textColor = enter_color;
	}

	void OnMouseExit(){
		this.button_style.normal.textColor = leave_color;
	}
*/
	public GUIStyle title_style;
	public GUIStyle button_style;

	void OnGUI () {

		GUI.Label (new Rect (Screen.width/2-30, Screen.height/10, 80, 20), "BomberBoy", this.title_style);

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(Screen.width/2 - 140,Screen.height/2,270,this.button_style.fontSize), "Jouer", this.button_style)) {
			Application.LoadLevel("Bomberman");
		}
		
		// Make the second button.
		if(GUI.Button(new Rect(Screen.width/2 - 260,Screen.height/2 + 100,500,this.button_style.fontSize), "Commandes", this.button_style)) {
			Application.LoadLevel(0);
		}

		if(GUI.Button(new Rect(Screen.width/2 - 160,Screen.height/2 + 200,300,this.button_style.fontSize), "Quitter", this.button_style)) {
			Application.Quit();
		}
	}
}

