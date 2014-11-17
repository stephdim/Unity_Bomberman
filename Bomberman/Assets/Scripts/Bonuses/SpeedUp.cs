using UnityEngine;
using System.Collections;

public class SpeedUp : Bonus {

	void Start() {
		GetComponent<MeshRenderer>().material.color = Color.white;
	}

	protected override void AddBonus(Player p) {
		SoundManager.Launch("Vitesse+");
		p.speed += .05f;
	}

}

