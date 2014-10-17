using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Fire : MonoBehaviour {

	private int timer;
	private Vector2 direction;
	private Vector2 position;
	private int power;

	//TODO Modify because we can't create an instance of Fire (it's impossible to do new Fire()).
	public Fire(Vector2 direction, Vector2 position, int power){
		this.direction = direction;
		this.position = position;
		this.power = power;
	}

	void Start(){
		this.timer = 2 * 60;
	}
	void Update() {
		if (this.timer == 0) {
			this.FireSpread(this.gameObject);
			timer = 2 * 60;
		}
		this.timer--;
	}
	
	//TODO Move those 2 functions in Terrain.cs because it's this script who fixes the conflict between all Object
	//TODO create a function DeplaceFire
	public void DestroyBomb(GameObject bomb, Player p) {
		Terrain terrain = Terrain.instance;
		if (bomb != null) {
			Vector3 pos = terrain.GetVector3Position(bomb.transform.position);
			terrain.terrain.Remove(terrain.GetRealPosition (pos));
			terrain.dico_bomb[p].Remove(bomb);
			Destroy(bomb.gameObject);
		}
	}

	public void FireSpread(GameObject bomb){
		Terrain terrain = Terrain.instance;
		Player[] players = GameObject.FindObjectsOfType<Player> ();
		Player p = players[0];
		foreach(Player player in players){
			if(terrain.dico_bomb[player].Contains(bomb)){
			p = player;
			}
		}
		Vector3 pos = bomb.transform.position;
		this.DestroyBomb(bomb.gameObject, p);
		foreach (Player player in players) {
			if (terrain.GetRealPosition (player.transform.position) == terrain.GetRealPosition (pos)) {
				Destroy (player.gameObject);
			}
		}
		Vector2 up = new Vector2(1f,0f);
		Vector2 down = new Vector2(-1f,0f);
		Vector2 right = new Vector2(0f,1f);
		Vector2 left = new Vector2(0f,-1f);
		this.DirectionalFire(up,terrain.GetRealPosition(pos),p);
		this.DirectionalFire(down,terrain.GetRealPosition(pos),p);
		this.DirectionalFire(left,terrain.GetRealPosition(pos),p);
		this.DirectionalFire(right,terrain.GetRealPosition(pos),p);
	}


	void DirectionalFire(Vector2 dir, Vector2 pos, Player player){
		Terrain terrain = Terrain.instance;
		Vector2 fire_pos = new Vector2 (pos.x, pos.y);
		bool is_alive = true;
		int iter = player.GetPower ();
		while (iter >= 0 && is_alive) {
			if(terrain.IsOccupied(fire_pos) && !terrain.IsIndestructibleBlocCases(fire_pos)){
				if(terrain.dico_bomb[player].Contains(terrain.GetGameObject(fire_pos))){
					terrain.dico_bomb[player].Remove(terrain.GetGameObject(fire_pos));
					this.FireSpread(terrain.GetGameObject(fire_pos));
				}
				else{
					Destroy(terrain.GetGameObject(fire_pos).gameObject);
					terrain.RemoveGameObject(fire_pos);
				}
				is_alive = false;
			}
			if(terrain.IsIndestructibleBlocCases(fire_pos)){
				is_alive = false;
			}
			for(int i = 0; i< terrain.players.Length; i++){
				Player p = terrain.players[i];
				if(p != null){
					if(terrain.GetRealPosition(p.transform.position) == fire_pos){
						terrain.players[i] = null;
						Destroy(p.gameObject);
						is_alive = false;
					}
				}
			}
			fire_pos += dir;
			iter--;
		}
	}
}