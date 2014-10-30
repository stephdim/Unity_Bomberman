using UnityEngine;
using System.Collections;

public class SpeedUp : Bonus {
	void Start() {
		this.GetComponent<TextMesh>().text = "Speed +";
	}

	protected override void AddBonus(Player p) {
		p.IncreaseSpeed();
	}
}

