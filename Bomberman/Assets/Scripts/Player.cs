using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	protected int color;
	protected int speed;
	protected int power;

	void Start() {
		this.speed = 1;
		this.power = 1;
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
}

