using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	void Update(){
		this.transform.Rotate (0, 2, 0, Space.World);
	}
	void OnTriggerEnter(Collider other) {
		Player p = other.GetComponent<Player>();
		if (p != null) {
			this.AddBonus(p);
			Destroy(this.gameObject);
		}
	}

	protected virtual void AddBonus(Player p) {}

}

