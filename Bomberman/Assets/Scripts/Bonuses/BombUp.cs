using UnityEngine;
using System.Collections;

public class BombUp : Bonus {

	void Start() {
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	protected override void AddBonus(Player p) {
		p.nb_bombs++;
	}

}