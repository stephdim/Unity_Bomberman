using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	public static GameObject Put(Vector3 pos, string type) {
		GameObject bonus_prefab = (GameObject) Resources.Load("Bonus");

		GameObject bonus = (GameObject) Instantiate(
			bonus_prefab,
			pos,
			Quaternion.identity
		);
		bonus.AddComponent(type);

		return bonus;
	}

	void Update(){
		transform.Rotate(0, 2, 0, Space.World);
	}

	void OnTriggerEnter(Collider other) {
		Player p = other.GetComponent<Player>();
		if (p != null) {
			AddBonus(p);
			Destroy(gameObject);
		}
	}

	protected virtual void AddBonus(Player p) {}

}

