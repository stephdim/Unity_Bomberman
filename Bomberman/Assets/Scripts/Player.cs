using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	protected int color;
	protected int speed;
	protected int power;
	protected int nb_bomb;

	void Start() {
		this.speed = 1;
		this.power = 1;
		this.nb_bomb = 2;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			this.AddBomb();
		}
	}

	private void AddBomb() {
		Terrain.instance.AddBomb(this);
	}

	public int GetPower(){
		return this.power;
	}

	public int GetNbBomb(){
		return this.nb_bomb;
	}
}

