using UnityEngine;
using System.Collections;

public class RandomUp : Bonus {

	void Start() {
		GetComponent<MeshRenderer>().material.color = Color.magenta;
	}

	protected override void AddBonus(Player p) {
		//SoundManager.Launch("Bomb+");
		p.RandomPosition();
	}

}