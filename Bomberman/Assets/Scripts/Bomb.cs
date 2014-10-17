using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public Player player { get; set; }
	private int timer;

	void Start() {
		this.timer = 2 * 60;
	}

	void Update() {
		this.timer--;
		if (this.timer <= 0) {
			Terrain.instance.ExplodeBomb(this);
		}
	}

	public void Explode() {
		this.player.BombHaveExplode();
	}

	void OnTriggerExit(Collider other) {
		this.collider.isTrigger = false;
	}

}

