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
			this.FireSpread();
			timer = 2 * 60;
		}
		this.timer--;
	}
	
	//TODO Move those 2 functions in Terrain.cs because it's this script who fixes the conflict between all Object
	//TODO create a function DeplaceFire
	public void DestroyBomb(GameObject bomb) {
		Terrain terrain = Terrain.instance;
		if (bomb != null) {
			Vector3 pos = terrain.GetVector3Position(bomb.transform.position);
			terrain.terrain.Remove(terrain.GetRealPosition (pos));
			Destroy(bomb.gameObject);
		}
	}

	//TODO Modify because all bombs are destroy at the same time.
	public void FireSpread(){
		Terrain terrain = Terrain.instance;
		Player[] players = GameObject.FindObjectsOfType<Player> ();
		foreach (GameObject bomb in terrain.dico_bomb.Keys) {
			Player p = terrain.dico_bomb [bomb];
			Vector3 pos = bomb.transform.position;
			this.DestroyBomb(bomb.gameObject);
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
		terrain.dico_bomb.Clear ();
	}


	void DirectionalFire(Vector2 dir, Vector2 pos, Player player){
		Terrain terrain = Terrain.instance;
		Vector2 fire_pos = new Vector2 (pos.x, pos.y);
		bool is_alive = true;
		int iter = player.GetPower ();
		while (iter >= 0 && is_alive) {
			if(terrain.IsOccupied(fire_pos) && !terrain.IsIndestructibleBlocCases(fire_pos)){
				Destroy(terrain.GetGameObject(fire_pos).gameObject);
				terrain.RemoveGameObject(fire_pos);
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