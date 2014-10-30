using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Player p = other.GetComponent<Player>();
		if (p != null) {
			this.AddBonus(p);
			Destroy(this.gameObject);
		}
	}

	protected virtual void AddBonus(Player p) {}

}

