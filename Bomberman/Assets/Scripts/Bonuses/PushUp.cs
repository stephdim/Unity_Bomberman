using UnityEngine;
using System.Collections;

public class PushUp : Bonus {

	void Start() {
		GetComponent<MeshRenderer>().material.color = Color.red;
	}
	
	protected override void AddBonus(Player p) {
		p.CanPushBomb();
	}

}

