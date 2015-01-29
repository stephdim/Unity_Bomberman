using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]

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
		GameObject player_prefab;
		if (players.Count > 0) {
			player_prefab = (GameObject)Resources.Load ("Player1");
		} else {
			player_prefab = (GameObject)Resources.Load ("Player");
		}
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

	}

	void unregister() {
		players.Remove(this);
	}

	void Awake() {
		register();
	}

	void Start() {
		SendMessage("SetInputPlayer", this); // for InputPlayer
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
		if (NetworkManager.enable) {
			networkView.RPC(
				"AddBombNetwork",
				RPCMode.All,
				id,
				transform.position
			);
		} else {
			AddBombAux(transform.position);
		}
	}

	[RPC]
	void AddBombNetwork(int id, Vector3 pos) {
		foreach(Player p in players) {
			if (p.id == id) {
				p.AddBombAux(pos);
				break;
			}
		}
	}

	void AddBombAux(Vector3 pos) {
		List<GameObject> bombs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bomb"));
		if (CanPutBomb() && !bombs.Exists(go => position == new Vector2(go.transform.position.x, go.transform.position.z))) {
			bombs_index_current++;
			Bomb.Put(this, pos);
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

	public void RandomPosition(){
		Vector3 pos = this.transform.position;
		int i = Random.Range (0, players.Count);
		this.transform.position = players [i].transform.position;
		players [i].transform.position = pos;
	}
}

