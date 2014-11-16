using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public static GameObject[] players = new GameObject[4];
	public static int playerCount = 0;

	/* Characteristics */
	public float speed { get; set; }
	public int power { get; set; }
	public int nb_bombs { get; set; }
	
	public static Vector2 Position(GameObject go) {
		Player p = go.GetComponent<Player>();
		return new Vector2(
			Mathf.Round(p.transform.position.x),
			Mathf.Round(p.transform.position.z)
		);
	}

	public List<GameObject> colliders { get; private set; }

	/* Intern functionnalities */
	int bombs_index_current;

	void register() {
		int id = playerCount;
		players[id] = gameObject;
		SendMessage("SetPlayerNumber", id); // for InputPlayer
		playerCount++;
	}

	void unregister() {
		// players[id] = null;
		playerCount--;
	}

	void Start() {
		register();

		speed = .1f;
		power = 0;
		nb_bombs = 1;
		bombs_index_current = 0;
		colliders = new List<GameObject>();
	}

	void OnDestroy() {
		unregister();
	}

	public void Move(Vector2 dir) {
		transform.Translate(new Vector3(dir.x, 0, dir.y));
	}

	bool CanPutBomb() {
		return bombs_index_current < nb_bombs;
	}

	void AddBomb() {
		if (CanPutBomb()) {
			bombs_index_current++;
			Bomb.Put(this);
		}
	}

	public void BombHaveExplode() {
		if (bombs_index_current == 0) {
			Debug.LogError("Bomb of this player can't have explode...");
			return;
		}
		bombs_index_current--;
	}

	public void AddCollisionWith(GameObject gameObject) {
		colliders.Add(gameObject);
	}

	public void RemoveCollisionWith(GameObject gameObject) {
		colliders.Remove(gameObject);
	}

	public static void AddCollisions(GameObject gameObject) {
		for (int i = 0; i < playerCount ; i++) {
			players[i].GetComponent<Player>().AddCollisionWith(gameObject);
		}
	}

	public static void RemoveCollisions(GameObject gameObject) {
		for (int i = 0; i < playerCount ; i++) {
			players[i].GetComponent<Player>().RemoveCollisionWith(gameObject);
		}
	}

// Add Bonus
/*
this.audio.Stop();
audio.clip = (AudioClip) Resources.Load("Bomb+");
this.audio.Play();
*/
}

