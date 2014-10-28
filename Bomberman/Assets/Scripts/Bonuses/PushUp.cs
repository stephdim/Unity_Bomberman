using UnityEngine;
using System.Collections;

public class PushUp : Bonus
{

	void Start(){
		this.GetComponent<TextMesh> ().text = "Push +";
	}
	
	protected override void AddBonus(Player p){
		p.CanPushBomb();
	}
}

