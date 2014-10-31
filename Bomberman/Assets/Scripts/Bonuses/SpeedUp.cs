using UnityEngine;
using System.Collections;

public class SpeedUp : Bonus {
	void Start() {
		this.GetComponent<MeshRenderer>().material.color = Color.white;
	}

	protected override void AddBonus(Player p) {
		p.IncreaseSpeed();
	}
}

