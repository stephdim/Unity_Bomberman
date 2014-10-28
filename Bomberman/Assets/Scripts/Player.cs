using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour {

	public string input_horizontal;
	public string input_vertical;
	public string input_fire;

	public float speed { get; private set; }
	public int power { get; private set; }

	protected int color;

	protected int bombs_index_max;
	protected int bombs_index_current;
	
	private bool push_bomb;

	void Start() {
		this.speed = 4;
		this.power = 0;
		this.bombs_index_max = 2;
		this.bombs_index_current = 0;
		this.push_bomb = false;
	}

	private bool CanAddBomb() {
		return this.bombs_index_current < this.bombs_index_max;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void BombHaveExplode() {
		this.bombs_index_current--;
	}

	void Update() {

		float h = Input.GetAxis(input_horizontal);
		float v = Input.GetAxis(input_vertical);

		// Movement
		if (h != 0 || v != 0) {
			Terrain.instance.MovePlayer(this, new Vector2(
				Mathf.Abs(h) > 0 ? 1 * Mathf.Sign(h) : 0,
				Mathf.Abs(v) > 0 ? 1 * Mathf.Sign(v) : 0
			));
		}

		// Put bomb
		if (Input.GetButton(input_fire) && this.CanAddBomb()) {
			this.AddBomb();
		}
	}

	private void AddBomb() {
		if (Terrain.instance.AddBomb (this)) {
			this.bombs_index_current++;
		}
	}

	// Add Bonus
	public void IncreaseBomb(){
		this.bombs_index_max++;
	}

	public void IncreasePower(){
		this.power++;
	}

	public void IncreaseSpeed(){
		this.speed += 0.5f;
	}

	public void CanPushBomb(){
		this.push_bomb = true;
	}
}

