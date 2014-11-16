using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	string input_horizontal;
	string input_vertical;
	string input_fire;

	Player player;
	string last_key_pressed;

	void SetPlayerNumber(int id) {
		input_horizontal = "Horizontal" + id;
		input_vertical = "Vertical" + id;
		input_fire = "Fire" + id;

		player = (Player) Player.players[id].GetComponent<Player>();
	}

	Vector2 GetCorrectedDir(Vector2 dir) {
		float correction = .00005f;
		float speed_factor = .1f;
		float corrected_x = Mathf.Round(transform.position.x) - transform.position.x;
		float corrected_y = Mathf.Round(transform.position.z) - transform.position.z;
		float corrected_x_approx = corrected_x * speed_factor;
		float corrected_y_approx = corrected_y * speed_factor;

		if (Mathf.Abs(dir.y) > 0) {
			if (Mathf.Abs(corrected_x_approx) <= correction) {
				return new Vector2(corrected_x, dir.y);
			} else {
				return new Vector2(corrected_x_approx, dir.y);
			}
		} else { // Mathf.Abs(dir.x) > 0
			if (Mathf.Abs(corrected_x_approx) <= correction) {
				return new Vector2(dir.x, corrected_y);
			} else {
				return new Vector2(dir.x, corrected_y_approx);
			}
		}

		return dir;
	}

	bool CanMoveWithCurrentSpeed(Vector2 dir) {
		Vector2 dir_abs = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
		Vector3 pos = transform.position;
		Vector2 pos2 = new Vector2(pos.x, pos.z);
		Vector2 dest = pos2 + dir;
		Vector2 pos2_round = new Vector2(Mathf.Round(pos2.x), Mathf.Round(pos2.y));

		// in terrain
		if (!(Mathf.Abs(dest.x) <= 6 && Mathf.Abs(dest.y) <= 5)) { return false; }

		// in corridor
		if (!((dir_abs.y > 0 && Mathf.Round(Mathf.Abs(dest.x)) % 2 == 0) ||
			(dir_abs.x > 0 && Mathf.Round(Mathf.Abs(dest.y)) % 2 == 1))) {

			return false;
		}
		
		// no collisions
		// @todo : optimize
		foreach (GameObject go in player.colliders) {
			Vector2 go_pos = new Vector2(go.transform.position.x, go.transform.position.z);
			if (
				(dir.y > 0 && pos2_round.y == go_pos.y - 1 && pos2_round.x == go_pos.x) ||
				(dir.y < 0 && pos2_round.y == go_pos.y + 1 && pos2_round.x == go_pos.x) ||
				(dir.x > 0 && pos2_round.x == go_pos.x - 1 && pos2_round.y == go_pos.y) ||
				(dir.x < 0 && pos2_round.x == go_pos.x + 1 && pos2_round.y == go_pos.y)
				) {
					if (
						(dir.y < 0 && dir.y < (pos2_round - pos2).y) ||
						(dir.x < 0 && dir.x < (pos2_round - pos2).x) ||
						(dir.y > 0 && dir.y > (pos2_round - pos2).y) ||
						(dir.x > 0 && dir.x > (pos2_round - pos2).x)
						) {
						return false;
					}
			}
		}

		return true;
	}

	bool CanMove(Vector2 dir) {
		Vector2 dir_speed = dir * player.speed;
		return CanMoveWithCurrentSpeed(dir_speed);
	}

	bool Move(Vector2 dir) {
		Vector2 dir_speed = dir * player.speed;
		if (CanMoveWithCurrentSpeed(dir_speed)) {
			Vector2 dir_corrected = GetCorrectedDir(dir_speed);
			player.Move(dir_corrected);
			return true;
		} /* else if possible with less speed */

		return false;
	}

	bool LastKeyPressed(string last) {
		return last_key_pressed == last;
	}

	void Update() {
		if (Time.timeScale != 1) { return; }

		player.colliders.ForEach(Debug.Log);

		float h = Input.GetAxis(input_horizontal);
		float v = Input.GetAxis(input_vertical);

		Vector2 vh = new Vector2(1 * Mathf.Sign(h), 0);
		Vector2 vv = new Vector2(0, 1 * Mathf.Sign(v));

		if (v != 0 || h != 0) {
			if (LastKeyPressed("")) {
				if (h != 0 && CanMove(vh)) {
					Move(vh);
					// last_key_pressed = "h";
				} else if (v != 0 && CanMove(vv)) {
					Move(vv);
					// last_key_pressed = "v";
				}
			} else {
				if (LastKeyPressed("h")) {
					if (v != 0 && CanMove(vv)) {
						Move(vv);
						last_key_pressed = "v";
					} else if (h != 0 && CanMove(vh)) {
						Move(vh);
						last_key_pressed = "h";
					}
				} else { // v
					if (h != 0 && CanMove(vh)) {
						Move(vh);
						last_key_pressed = "h";
					} else if (v != 0 && CanMove(vv)) {
						Move(vv);
						last_key_pressed = "v";
					}
				}
			}
		} else {
			last_key_pressed = "";
		}

		// Put bomb
		if (Input.GetButton(input_fire)) { SendMessage("AddBomb"); }
	}

}