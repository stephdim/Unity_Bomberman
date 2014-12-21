using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timer : MonoBehaviour {

	float timer = 1;
	Vector3 pos = new Vector3(-6,3,5);
	public List<GameObject> indeblocks = new List<GameObject>();
	List<GameObject> dead_obj = new List<GameObject>();

	void Update () {
		if (IsReady () && Player.players.Count > 1) {
			Vector3 actual_pos;
			Vector3 final_pos;
			if (indeblocks.Count != 0) {
				final_pos = indeblocks [indeblocks.Count - 1].transform.position;
				actual_pos = final_pos;
			} else {
				final_pos = pos;
				actual_pos = pos;
			}
			final_pos.y = 0.25f;
			if (timer > 0) {
				timer -= Time.deltaTime;
			} else if (indeblocks.Count == 0 || (indeblocks.Count < 160 && actual_pos.y == 0.25f)) {
				foreach(Player p in Player.players){
					//tue le joueur s'il est en dessous du bloc dès que le bloc apparait.
					if(p.position == PositionTools.Position(actual_pos)){ 
						dead_obj.Add(p.gameObject);
					}
				}
				foreach(GameObject go in Player.players[0].colliders){
					if(go.transform.position == actual_pos && !indeblocks.Contains(go)){
						dead_obj.Add(go);
					}
				}
				foreach(GameObject go in dead_obj){
					Player.RemoveCollisions(go);
					Destroy(go.gameObject);
				}
				dead_obj.Clear();
				ThrowBlock ();
			} else if (actual_pos.y != 0.25f) {
				indeblocks [indeblocks.Count - 1].transform.position = Vector3.MoveTowards (actual_pos, final_pos, 0.2f);
			}
		}
	}

	bool IsReady(){
		Menu menu = GameObject.FindObjectOfType<Menu> ();
		return menu.play;
	}

	public static GameObject Put(Vector3 pos) {
		GameObject block_prefab = (GameObject) Resources.Load("IndeBlock");
		
		GameObject block = (GameObject) Instantiate(
			block_prefab,
			pos,
			Quaternion.identity
			);
		return block;
	}

	void ThrowBlock(){
		GameObject block = Put (pos);
		indeblocks.Add(block);
		Player.AddCollisions (block);

		if(pos.x == -6 && pos.z > -5){
			pos.z -= 1f;
		} else if(pos.z == -5 && pos.x < 6) {
			pos.x += 1f;
		} else if(pos.x == 6 && pos.z < 5) {
			pos.z += 1f;
		} else if(pos.z == 5 && pos.x > -5) {
			pos.x -= 1f;
		}

	}

	void OnGUI(){
		if (IsReady ()) {
			GUILayout.Box (((int)timer / 60).ToString () + ":" + ((int)timer % 60).ToString());
		}
	}
}
