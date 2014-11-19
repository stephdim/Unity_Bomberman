using UnityEngine;
using System.Collections;


public class Bomb : MonoBehaviour {

	public Player player { get; set; }
	
	void Start() {
		Invoke("Boom", 2);
	}


	public void Boom() {
		Camera.main.GetComponent<CameraAudio> ().BoomSound ();
		Terrain.instance.ExplodeBomb(this);
	}

	public void Explode() {
		this.player.BombHaveExplode();
	}

	void OnTriggerExit(Collider other) {
		this.collider.isTrigger = false;
	}

}

