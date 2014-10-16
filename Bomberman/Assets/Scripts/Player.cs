using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	protected int color;
	protected int speed;
	public int power;

	void Start() {
		this.speed = 1;
		this.power = 1;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			this.AddBomb();
		}
	}

	public void AddBomb() {
		Terrain.instance.AddBomb(this);
	}
	
}

