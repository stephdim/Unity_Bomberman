using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terrain : MonoBehaviour {

	List<string> list_bonus;
	bool paused;

	void Start() {
		list_bonus = (List<string>) ReflectiveEnumerator.GetEnumerableOfType<Bonus>();

		Play();
		Init();
	}

	void Update() {
		if (Input.GetKeyDown("p")) {
			Pause();
		}

		if (Input.GetKeyDown("m")) {
			AudioListener.pause = !AudioListener.pause;
		}

		/*if (Input.GetKeyDown ("b")) {
			Player.players[0].transform.position = PositionTools.AbsoluteDirection(new Vector2(3,-5));
			Player.players[1].transform.position = PositionTools.AbsoluteDirection(new Vector2(-6,4));
			Bomb b1 = (Player.players[0]).AddBomb();
			Bomb b2 = Player.players[1].AddBomb();
			Player.players[0].transform.position = PositionTools.AbsoluteDirection(new Vector2(4,-5));
			Player.players[1].transform.position = PositionTools.AbsoluteDirection(new Vector2(-6,5));
			b1.PushBomb(Player.players[0]);
			b2.PushBomb(Player.players[1]);
		}*/
	}

	public static bool IsIndestructibleBlocCases(Vector2 v) {
		Vector2 vabs = PositionTools.Abs(v);
		Timer timer  = GameObject.FindObjectOfType<Timer> ();
		foreach (GameObject b in timer.indeblocks){
			if(PositionTools.Position(b.transform.position) == v) return true;
		}
		return vabs.x % 2 == 1 && vabs.y % 2 == 0 ;
	}

	bool IsForbidden(Vector2 v) {
		Vector2 vabs = PositionTools.Abs(v);
		bool isStartCasesForPlayers = (
			(vabs.x == 5 && vabs.y == 5) ||
			(vabs.x == 6 && (vabs.y == 4 || vabs.y == 5))
			);
		return isStartCasesForPlayers || IsIndestructibleBlocCases(v);
	}

	void Init() {

		// stock all available cases
		// select random cases
		// add powers randomly

		int nb_block = Random.Range(75,96); // 101 max

		List<Vector2> positions = new List<Vector2>();
		for (int x = -6; x <= 6; x++) {
			for (int y = -5; y <= 5; y++) {
				Vector2 v = new Vector2(x, y);
				if (!IsForbidden(v)) { positions.Add(v); }
			}
		}

		for (int x = 0; x < nb_block; x++) {
			int rand = Random.Range(0, positions.Count);
			Vector2 pos = positions[rand];
			Vector3 v = new Vector3(pos.x, 0.25f, pos.y);
			positions.RemoveAt(rand);

			// add block
			Block.Put(v);

			// add bonus 1/4
			if (Random.Range(0,4) == 0) {
				Bonus.Put(v, list_bonus[(Random.Range(0, this.list_bonus.Count))]);
			}
		}
	}

	void Play() {
		paused = false;
		Time.timeScale = 1;
	}

	void Pause() {
		Time.timeScale = paused ? 1 : 0;
		paused = !paused;
	}

	bool IsOver() {
		return  Player.players.Count < 2;
	}

	bool IsReady(){
		Menu menu = GameObject.FindObjectOfType<Menu> ();
		return menu.play;
	}

	void OnGUI() {

		// Menu pause
		if (paused && IsReady()) {

			GUI.Box(
				new Rect(
					0.4f * Screen.width,
					0.6f * Screen.height,
					0.2f * Screen.width,
					0.3f * Screen.height
				),
				"Pause"
			);

			if (GUI.Button(
					new Rect(
						0.41f * Screen.width,
						0.7f * Screen.height,
						0.18f * Screen.width,
						0.05f * Screen.height
					),
					"Retour au jeu"
				)) {

				Pause();
			}

			if (GUI.Button(
					new Rect(
						0.41f * Screen.width,
						0.75f * Screen.height,
						0.18f * Screen.width,
						0.05f * Screen.height
					),
					"Retour au menu"
				)) {

				Application.LoadLevel("Menu");
			}

			if (GUI.Button(
					new Rect(
						0.41f * Screen.width,
						0.8f * Screen.height,
						0.18f * Screen.width,
						0.05f * Screen.height
					),
					"Quitter"
				)) {

				Application.Quit();
			}
		}

		// Menu Game Over
		if (IsReady () && IsOver()) {
			if (IsOver()) {
				Time.timeScale = 0.0f;
			}

			GUI.Box(
				new Rect(
					0.4f * Screen.width,
					0.6f * Screen.height,
					0.2f * Screen.width,
					0.3f * Screen.height
				),
				"Game Over"
			);

			if (GUI.Button(
					new Rect(
						0.41f * Screen.width,
						0.7f * Screen.height,
						0.18f * Screen.width,
						0.05f * Screen.height
					),
					"Nouvelle Partie"
				)) {
				Application.LoadLevel("Bomberman");
			}

			if (GUI.Button(
					new Rect(
						0.41f * Screen.width,
						0.75f * Screen.height,
						0.18f * Screen.width,
						0.05f * Screen.height
					),
					"Retour au Menu"
				)) {

				Application.LoadLevel("Menu");
			}

			if (GUI.Button(
					new Rect(
						0.41f * Screen.width,
						0.8f * Screen.height,
						0.18f * Screen.width,
						0.05f * Screen.height
					),
					"Quitter"
				)) {

				Application.Quit();
			}
		}
	}

}
