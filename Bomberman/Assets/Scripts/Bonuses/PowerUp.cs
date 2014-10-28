using UnityEngine;
using System.Collections;

public class PowerUp : Bonus
{
	// Use this for initialization
	void Start ()
	{
		this.GetComponent<TextMesh> ().text = "Power +";
	}


	protected override void AddBonus(Player p){
		p.IncreasePower();
	}
}

