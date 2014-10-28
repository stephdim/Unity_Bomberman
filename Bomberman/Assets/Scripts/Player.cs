using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour {

	protected int color;
	public float speed { get; private set; }

	protected int bombs_index_max;
	protected int bombs_index_current;
	public int power { get; private set; }
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
		if (Input.GetKeyDown(KeyCode.Space) && this.CanAddBomb()) {
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

