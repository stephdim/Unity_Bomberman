using UnityEngine;
using System.Collections;

public class PowerUp : Bonus {
	void Start() {
		this.GetComponent<TextMesh>().text = "Power +";
	}

	protected override void AddBonus(Player p) {
		p.IncreasePower();
	}
}

