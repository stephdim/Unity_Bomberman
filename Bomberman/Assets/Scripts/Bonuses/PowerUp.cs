using UnityEngine;
using System.Collections;

public class PowerUp : Bonus {
	void Start() {
		this.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}

	protected override void AddBonus(Player p) {
		p.IncreasePower();
	}
}

