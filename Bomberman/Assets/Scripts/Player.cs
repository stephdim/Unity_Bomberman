using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	protected int color;
	public float speed { get; private set; }

	protected int bombs_index_max;
	protected int bombs_index_current;
	public int power { get; private set; }

	private Move move;

	void Start() {
		this.speed = .2f;
		this.power = 0;
		this.bombs_index_max = 2;
		this.bombs_index_current = 0;
		move = new Move(this);
	}

	private bool CanAddBomb() {
		return this.bombs_index_current < this.bombs_index_max;
	}

	public void BombHaveExplode() {
		this.bombs_index_current--;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) && this.CanAddBomb()) {
			this.AddBomb();
		}
		this.move.Update();
	}

	private void AddBomb() {
		this.bombs_index_current++;
		Terrain.instance.AddBomb(this);
	}

	public void AddBonus(Bonuses bonus){
		int kind = bonus.GetKindOfBonus ();
		if (kind == 1) {
				this.bombs_index_max++;
		} else if (kind == 2) {
				this.speed++;
		} else if (kind == 3) {
			this.power++;
		} else {
		}
	}
}

