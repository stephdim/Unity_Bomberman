using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timer : MonoBehaviour {

	float timer = 30;
	Vector3 pos = new Vector3(-6,3,5);
	string dir = "S";
	public List<GameObject> indeblocks = new List<GameObject>();
	List<GameObject> dead_obj = new List<GameObject>();

	void Update () {
		if (IsReady () && Player.players.Count > 1) {
			Vector3 current_pos;
			Vector3 final_pos;
			if (indeblocks.Count != 0) {
				final_pos = indeblocks [indeblocks.Count - 1].transform.position;
				current_pos = final_pos;
			} else {
				final_pos = pos;
				current_pos = pos;
			}
			final_pos.y = 0.25f;
			if (timer > 0) {
				timer -= Time.deltaTime;
			} else if (indeblocks.Count == 0 || (indeblocks.Count < 108 && current_pos.y == 0.25f)) {
				foreach(Player p in Player.players){
					//tue le joueur s'il est en dessous du bloc dès que le bloc apparait.
					if(p.position == PositionTools.Position(current_pos)){ 
						dead_obj.Add(p.gameObject);
					}
				}
				foreach(GameObject go in Player.players[0].colliders){
					if(go.transform.position == current_pos && !indeblocks.Contains(go)){
						dead_obj.Add(go);
					}
				}
				foreach(GameObject go in dead_obj){
					Player.RemoveCollisions(go);
					//Destroy(go.gameObject);
				}
				dead_obj.Clear();
				ThrowBlock ();
			} else if (current_pos.y != 0.25f) {
				indeblocks [indeblocks.Count - 1].transform.position = Vector3.MoveTowards (current_pos, final_pos, 0.2f);
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
		foreach (GameObject go in indeblocks) {
			if( (PositionTools.Position(go.transform.position) == new Vector2(block.transform.position.x,block.transform.position.z +1f) && dir == "N") || (pos.z == 5 && pos.x == 6)){
				dir = "W";
				break;
			}
			if( (PositionTools.Position(go.transform.position) == new Vector2(block.transform.position.x,block.transform.position.z -1f) && dir == "S") || (pos.z == -5 && pos.x == -6)){
				dir = "E";
				break;
			}
			if( (PositionTools.Position(go.transform.position) == new Vector2(block.transform.position.x +1f,block.transform.position.z) && dir == "E") || (pos.z == -5 && pos.x == 6)){
				dir = "N";
				break;
			}
			if( (PositionTools.Position(go.transform.position) == new Vector2(block.transform.position.x - 1f,block.transform.position.z) && dir == "W") || (pos.z == 5 && pos.x == -6)){ 
				dir = "S";
				break;
			}
		}
		/*if(pos.x == -6 && pos.z > -5){
			pos.z -= 1f;
		} else if(pos.z == -5 && pos.x < 6) {
			pos.x += 1f;
		} else if(pos.x == 6 && pos.z < 5) {
			pos.z += 1f;
		} else if(pos.z == 5 && pos.x > -5) {
			pos.x -= 1f;
		}
*/
		if(dir == "S"){
			pos.z -= 1f;
		} else if(dir == "N") {
			pos.z += 1f;
		} else if(dir == "E") {
			pos.x += 1f;
		} else if(dir == "W") {
			pos.x -= 1f;
		}
	}

	void OnGUI(){
		if (IsReady ()) {
			GUILayout.Box (((int)timer / 60).ToString () + ":" + ((int)timer % 60).ToString());
		}
	}
}
