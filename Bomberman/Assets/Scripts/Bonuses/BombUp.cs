using UnityEngine;
using System.Collections;

public class BombUp : Bonus {

	void Start() {
		this.GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	protected override void AddBonus(Player p) {
		p.nb_bombs++;
	}

}