using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	/* Characteristics */
	public bool is_exploded { get; private set; }
	public Player player { get; set; }
	public Vector2 position {
		get {
			return new Vector2(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.z)
			);
		}
	}

	public static GameObject Put(Player player) {
		GameObject bomb_prefab = (GameObject) Resources.Load("Bomb");

		Vector3 pos = player.transform.position;
		pos.y = bomb_prefab.transform.position.y;

		GameObject bomb_clone = (GameObject) Instantiate(
			bomb_prefab,
			PositionTools.AbsolutePosition(pos),
			Quaternion.identity
		);
		bomb_clone.GetComponent<Bomb>().player = player;
		bomb_clone.GetComponent<Bomb>().InitCollisions();

		return bomb_clone;
	}

	void Start() {
		is_exploded = false;
		Invoke("Boom", 2);
	}

	void InitCollisions() {
		Player.AddCollisions(gameObject);
		player.RemoveCollisionWith(gameObject);
	}

	public void Boom() {
		CancelInvoke();
		is_exploded = true;
		SoundManager.Launch("Boom");

		// Launch Fires !
		Fire.Launch(transform.position, new Vector2(0,0), 0);
		Fire.Launch(transform.position, new Vector2(-1,0), player.power);
		Fire.Launch(transform.position, new Vector2(1,0), player.power);
		Fire.Launch(transform.position, new Vector2(0,1), player.power);
		Fire.Launch(transform.position, new Vector2(0,-1), player.power);

		player.BombHaveExplode();
		Destroy(gameObject);
	}

	void OnDestroy() {
		Player.RemoveCollisions(gameObject);
	}

	// Need rigidbody on Player for works correctly...
	void OnTriggerExit(Collider other) {
		if (other == player.gameObject.collider) {
			player.AddCollisionWith(gameObject);
		}
	}

}

