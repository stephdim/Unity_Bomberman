using UnityEngine;
using System.Collections;

public class BombUp : Bonus
{

	void Start(){
		this.GetComponent<TextMesh> ().text = "Bomb +";
	}
	
	protected override void AddBonus(Player p){
		p.IncreaseBomb();
	}
}