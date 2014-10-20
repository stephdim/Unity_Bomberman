using UnityEngine;
using System.Collections;

public class Move {
	
	private Player player;
	
	public Move(Player player) {
		this.player = player;
	}
	
	public void Update () {
		
		// Movement Left/Right (x)
		if (Input.GetKey(KeyCode.LeftArrow)) {
			Terrain.instance.MovePlayer(this.player, new Vector2(-1,0));
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			Terrain.instance.MovePlayer(this.player, new Vector2(1,0));
		}
		
		// Movement Up/Down (z)
		if (Input.GetKey(KeyCode.UpArrow)) {
			Terrain.instance.MovePlayer(this.player, new Vector2(0,1));
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			Terrain.instance.MovePlayer(this.player, new Vector2(0,-1));
		}

	}
}

