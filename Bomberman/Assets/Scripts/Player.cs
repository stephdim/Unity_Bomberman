using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string input_horizontal;
	public string input_vertical;
	public string input_fire;

	public float speed { get; private set; }
	public int power { get; private set; }

	protected int color;

	protected int bombs_index_max;
	protected int bombs_index_current;

	void Start() {
		this.speed = 5;
		this.power = 0;
		this.bombs_index_max = 2;
		this.bombs_index_current = 0;
	}

	private bool CanAddBomb() {
		return this.bombs_index_current < this.bombs_index_max;
	}

	public void BombHaveExplode() {
		this.bombs_index_current--;
	}

	void Update() {

		float h = Input.GetAxis(input_horizontal);
		float v = Input.GetAxis(input_vertical);

		// Movement
		if (h != 0 || v != 0) {

//			Terrain.instance.MovePlayer(this, new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));

			Terrain.instance.MovePlayer(this, new Vector2(
				Mathf.Abs(h) > 0 ? 1 * Mathf.Sign(h) : 0,
				Mathf.Abs(v) > 0 ? 1 * Mathf.Sign(v) : 0
			));
/*
			this.transform.position = Vector3.MoveTowards(
				this.transform.position,
				this.transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) ,
				Time.deltaTime * speed
			);
*/
		}

		// Put bomb
		if (Input.GetButton(input_fire) && this.CanAddBomb()) {
			this.AddBomb();
		}
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

