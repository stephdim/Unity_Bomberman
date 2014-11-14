using UnityEngine;
using System.Collections;


public class Bomb : MonoBehaviour {

	public Player player { get; set; }
	
	void Start() {
		audio.PlayDelayed(1.2f);
		Invoke("Boom", 2);
	}


	public void Boom() {
		Terrain.instance.ExplodeBomb(this);
	}

	public void Explode() {
		this.player.BombHaveExplode();
	}

	void OnTriggerExit(Collider other) {
		this.collider.isTrigger = false;
	}

}

