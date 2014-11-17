using UnityEngine;
using System.Collections;

public class PowerUp : Bonus {

	void Start() {
		GetComponent<MeshRenderer>().material.color = Color.yellow;
	}

	protected override void AddBonus(Player p) {
		p.power++;
	}

}

