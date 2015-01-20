using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {

	public static List<Block> blocks = new List<Block>();

	public static GameObject Put(Vector3 pos) {
		GameObject block_prefab = (GameObject) Resources.Load("Block");

		GameObject block = (GameObject) Instantiate(
			block_prefab,
			pos,
			Quaternion.identity
		);

		return block;
	}

	public Vector2 position {
		get {
			return new Vector2(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.z)
			);
		}
	}

	void Start() {
		Player.AddCollisions(gameObject);
		blocks.Add(this);
	}

	void OnDestroy() {
		Player.RemoveCollisions(gameObject);
		blocks.Remove(this);
	}

}

