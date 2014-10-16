using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Fire : MonoBehaviour
{
	private int timer = 2 * 60;
	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.timer == 0) {

			this.FireSpread();
			timer = 2 * 60;
		}
		this.timer--;
	}

	public void DestroyBomb(GameObject bomb){
		Terrain terrain = Terrain.instance;
		if (bomb != null) {
			Vector3 pos = terrain.GetVector3Position (bomb.transform.position);
			terrain.terrain.Remove (terrain.GetRealPosition (pos));
			Destroy (bomb.gameObject);
		}
	}

	public void FireSpread(){
		Terrain terrain = Terrain.instance;
		Player[] players = GameObject.FindObjectsOfType<Player> ();
		foreach (GameObject bomb in terrain.dico_bomb.Keys) {
			Player p = terrain.dico_bomb [bomb];
			Vector3 pos = bomb.transform.position;

			Vector2 pos_up = terrain.GetRealPosition (new Vector3 (pos.x + 1, pos.y, pos.z));
			Vector2 pos_down = terrain.GetRealPosition (new Vector3 (pos.x - 1, pos.y, pos.z));
			Vector2 pos_left = terrain.GetRealPosition (new Vector3 (pos.x, pos.y, pos.z - 1));
			Vector2 pos_right = terrain.GetRealPosition (new Vector3 (pos.x, pos.y, pos.z + 1));
			this.DestroyBomb(bomb.gameObject);
			bool can_spread = true;
			foreach (Player player in players) {
				if (terrain.GetRealPosition (player.transform.position) == terrain.GetRealPosition (pos)) {
					Destroy (player.gameObject);
				}
			}
			while (pos_up.x <= pos.x + p.power && can_spread) {
				if (terrain.terrain.ContainsKey (pos_up)) {
					Destroy (terrain.terrain [pos_up].gameObject);
					terrain.terrain.Remove (pos_up);
					can_spread = false;
				}
				if (terrain.indestructible_block.Contains (pos_up)) {
					can_spread = false;
				}
				foreach (Player player in players) {
					if (terrain.GetRealPosition (player.transform.position) == pos_up) {
						Destroy (player.gameObject);
					}
				}
				pos_up.x += 1;
			}
			can_spread = true;
			while (pos_down.x >=-(pos.x + p.power) && can_spread) {
				if (terrain.terrain.ContainsKey (pos_down)) {
					Destroy (terrain.terrain [pos_down].gameObject);
					terrain.terrain.Remove (pos_down);
					can_spread = false;
				}
				if (terrain.indestructible_block.Contains (pos_down)) {
					can_spread = false;
				}
				foreach (Player player in players) {
					if (terrain.GetRealPosition (player.transform.position) == pos_down) {
						Destroy (player.gameObject);
					}
				}
				pos_down.x -= 1;
			}
			can_spread = true;
			while (pos_left.y >=-(pos.z + p.power) && can_spread) {
				if (terrain.terrain.ContainsKey (pos_left)) {
					Destroy (terrain.terrain [pos_left].gameObject);
					terrain.terrain.Remove (pos_left);
					can_spread = false;
				}
				if (terrain.indestructible_block.Contains (pos_left)) {
					can_spread = false;
				}
				foreach (Player player in players) {
					if (terrain.GetRealPosition (player.transform.position) == pos_left) {
						Destroy (player.gameObject);
					}
				}
				pos_left.y -= 1;
			}
			can_spread = true;
			while (pos_right.y <= (pos.z + p.power) && can_spread) {
				if (terrain.terrain.ContainsKey (pos_right)) {
					Destroy (terrain.terrain [pos_right].gameObject);
					terrain.terrain.Remove (pos_right);
					can_spread = false;
				}
				if (terrain.indestructible_block.Contains (pos_right)) {
					can_spread = false;
				}
				foreach (Player player in players) {
					if (terrain.GetRealPosition (player.transform.position) == pos_right) {
						Destroy (player.gameObject);
					}
				}
				pos_right.y += 1;
			}
			can_spread = true;
		}
		terrain.dico_bomb.Clear ();
	}
}

