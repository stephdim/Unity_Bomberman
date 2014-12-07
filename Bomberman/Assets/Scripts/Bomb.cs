using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour {

	/* Characteristics */
	public bool is_exploded { get; private set; }
	public Player player { get; set; }
	public Vector2 position {
				get {
						return new Vector2 (
				Mathf.Round (transform.position.x),
				Mathf.Round (transform.position.z)
						);
				}
	}
	Vector2 dir;
	bool is_moving;

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
		is_moving = false;
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
		Vector3 pos = new Vector3 (this.position.x, transform.position.y, this.position.y);
		// Launch Fires !
		Fire.Launch(pos, new Vector2(0,0), 0);
		Fire.Launch(pos, new Vector2(-1,0), player.power);
		Fire.Launch(pos, new Vector2(1,0), player.power);
		Fire.Launch(pos, new Vector2(0,1), player.power);
		Fire.Launch(pos, new Vector2(0,-1), player.power);

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

	bool IsIndestructibleBlocCases(Vector2 v) {
		Vector2 vabs = PositionTools.Abs(v);
		return vabs.x % 2 == 1 && vabs.y % 2 == 0 || vabs.x >= 7 || vabs.y >= 6;
	}

	public void PushBomb(Player p){
		this.dir = this.position - p.position;
		is_moving = true;
	}

	void Move(Player p){
		Vector3 new_pos = new Vector3(dir.x,0,dir.y);
		Terrain terrain = GameObject.FindObjectOfType<Terrain> ();
		List<Vector2> list = new List<Vector2> ();
		foreach(GameObject go in p.colliders){
			if(go.transform.position.x - this.transform.position.x - new_pos.x == 0 
			   || go.transform.position.z - this.transform.position.z - new_pos.z == 0){
				list.Add(PositionTools.Position(go.transform.position));
			}
		}
		// Pour chaque GameObject de colliders, mettre GameObject dans liste si sur la meme ligne que deplacement bombe.
		if (!IsIndestructibleBlocCases (this.position +PositionTools.Position ( new_pos)) 
		    && !list.Contains (this.position +PositionTools.Position ( new_pos))) {
			this.transform.position = Vector3.MoveTowards (this.transform.position, this.transform.position + new_pos, 0.2f);
		} else {
			this.transform.position = new Vector3(this.position.x,this.transform.position.y,this.position.y);
			is_moving = false;
		}
	}

	void Update(){
		if (is_moving) {
			this.Move (player);
		}
	}
}

