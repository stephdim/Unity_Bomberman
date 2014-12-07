using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public static List<Player> players = new List<Player>();

	/* Characteristics */
	public int id { get; private set; }
	public float speed { get; set; }
	public int power { get; set; }
	public int nb_bombs { get; set; }
	public bool can_push { get; set; }
	public List<GameObject> colliders { get; private set; }
	public Vector2 position {
		get {
			return new Vector2(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.z)
			);
		}
	}

	public static GameObject Put(Vector3 v) {
		GameObject player_prefab = (GameObject) Resources.Load("Player");
		GameObject player_clone = (GameObject) Instantiate(
			player_prefab,
			v,
			Quaternion.identity
		);
		return player_clone;
	}

	/* Intern functionnalities */
	int bombs_index_current;

	void register() {
		id = players.Count;
		players.Add(this);
		SendMessage("SetInputPlayer", this); // for InputPlayer
	}

	void unregister() {
		players.Remove(this);
	}

	void Start() {
		register();

		speed = .1f;
		power = 0;
		nb_bombs = 1;
		bombs_index_current = 0;
		colliders = Block.blocks.ConvertAll<GameObject>(b => b.gameObject);
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

	public void CanPushBomb() {
		can_push = true;
	}

	void AddBomb() {
		List<GameObject> bombs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bomb"));
		if (CanPutBomb() && !bombs.Exists(go => position == new Vector2(go.transform.position.x, go.transform.position.z))) {
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
		players.ForEach(p => p.AddCollisionWith(gameObject));
	}

	public static void RemoveCollisions(GameObject gameObject) {
		players.ForEach(p => p.RemoveCollisionWith(gameObject));
	}

	void PushBomb(Vector2 dir){
		if (can_push) {
			foreach (GameObject obj in colliders) {
				Bomb bomb = obj.GetComponent<Bomb> ();
				if (bomb != null) {
					if (bomb.position == this.position + dir) {
						bomb.PushBomb (this);
						return;
					}
				}
			}
		}
	}
}

