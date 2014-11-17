using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Fire : MonoBehaviour {

	/* Characteristics */
	int life;
	Vector2 dir;

	public static void Launch(Vector3 pos, Vector2 dir, int life) {
		GameObject fire_prefab = Resources.Load("Fire") as GameObject;

		Vector3 dest = pos + new Vector3(dir.x, 0, dir.y);

		// check in terrain
		if (Mathf.Abs(dest.x) > 6 || Mathf.Abs(dest.z) > 5) { return; }

		// check not in indestructible block
		if (Mathf.Abs(dest.x) % 2 == 1 && Mathf.Abs(dest.z) % 2 == 0) { return; }

		GameObject fire_effect = (GameObject) Instantiate(
			fire_prefab,
			dest,
			Quaternion.identity
		);
		fire_effect.GetComponent<Fire>().Init(dir, life);
	}

	void Init(Vector2 dir, int life) {
		this.dir = dir;
		this.life = life;

		SpreadFire();
	}

	void SpreadFire() {
		particleSystem.Play();
		SoundManager.Launch("FFFrrr");

		// check if player (don't stop explosion)
		Vector2 pos = new Vector2(transform.position.x, transform.position.z);
		Player.players.FindAll(p => p.position == pos).ForEach(p => Destroy(p.gameObject));

		// check if in block or bomb (stop explosion)
		bool have_destroy = false;
		List<Block> blocks = Block.blocks.FindAll(b => b.position == pos);
		if (blocks.Count > 0) {
			blocks.ForEach(b => Destroy(b.gameObject));
			have_destroy = true;
		}

		if (have_destroy) {
			Die();
			return;
		}

		if (life > 0) {
			Fire.Launch(transform.position, dir, life - 1);
		}

		Invoke("Die", .4f);
	}

	void Die() {
		Destroy(gameObject);
	}

	// Not sufficient, need to control things before instanciate
	void OnTriggerEnter(Collider other) {
		// kill powers, blocks, bombermans, bombs
		Destroy(other.gameObject);
		Destroy(gameObject);
	}

}
