using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	/* Characteristics */
	public Player player { get; set; }

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
		// bomb_clone.GetComponent<Bomb>().InitCollisions();

		return bomb_clone;
	}

	void Start() {
		Invoke("Boom", 2);
		Time.timeScale = .5f;
	}

	void InitCollisions() {
		Player.AddCollisions(gameObject);
		player.RemoveCollisionWith(gameObject);
	}

	public void Boom() {
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

