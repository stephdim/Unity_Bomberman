using UnityEngine;
using System.Collections;

public class PushUp : Bonus {
	void Start() {
		this.GetComponent<MeshRenderer>().material.color = Color.red;
	}
	
	protected override void AddBonus(Player p) {
		p.CanPushBomb();
	}
}

