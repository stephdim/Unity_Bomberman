using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Fire {

	public Vector2 position { get; private set; }
	private Vector2 direction;
	private int life;

	public Fire(Vector2 position, Vector2 direction, int life) {
		this.direction = direction;
		this.position = position + direction;
		this.life = life;
	}

	public bool isDead() {
		return this.life <= 0;
	}

	public void Move() {
		if (this.isDead()) {
			return;
		}
		this.position += this.direction;
		this.life--;
	}

}
