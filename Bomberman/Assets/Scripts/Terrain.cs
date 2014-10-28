using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Terrain : Singleton
 * 
 * @todo: add description of Terrain class here...
 * 
 */
public class Terrain : MonoBehaviour {

	private static Terrain _instance;

	private Terrain() {}

	public static Terrain instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<Terrain>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	
	void Awake() {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this);
		} else if (this != _instance) {
			Destroy(this.gameObject);
		}
	}

	private const int length = 13;
	private const int width = 11;

	public GameObject bomb;
	public GameObject destroyable_block;
	public GameObject bonus;
	private Dictionary<Vector2,GameObject> terrain;
	private List<Fire> fires;

	private Player[] players;
	void Start() {
		this.terrain = new Dictionary<Vector2,GameObject>();
		this.fires = new List<Fire>();
		this.AddBlocks();
//		this.players = new Player[4];
		this.players = GameObject.FindObjectsOfType<Player> ();
		// this.AddBonus();
	}

	private void FireUpdate() {
		List<Fire> firesToRemove = new List<Fire>();
		List<Bomb> bombsToExplode = new List<Bomb>();

		foreach (Fire fire in this.fires) {

			if (this.terrain.ContainsKey(fire.position)) {
				GameObject gameObject = this.terrain[fire.position];
				this.terrain.Remove(fire.position);

				// check if bomb
				Bomb bomb = gameObject.GetComponent<Bomb>();
				if (bomb != null) {
					bombsToExplode.Add(bomb);
				}

				Destroy(gameObject);
				firesToRemove.Add(fire);
				continue;
			} else if (this.IsIndestructibleBlocCases(fire.position)) {
				firesToRemove.Add(fire);
				continue;
			} else{
				for(int i = 0; i<this.players.Length; i++){
					Player p = this.players[i];
					if(p != null){
						if(this.GetRelativePosition(p.transform.position) == fire.position){
							this.players[i] = null;
							Destroy (p.gameObject);
						}
					}
				}
			}

			if (fire.isDead()) {
				firesToRemove.Add(fire);
			} else {
				fire.Move();
			}
		}

		foreach (Fire fire in firesToRemove) {
			this.fires.Remove(fire);
		}

		foreach (Bomb bomb in bombsToExplode) {
			this.ExplodeBomb(bomb);
		}
	}

	void Update() {
		FireUpdate();
	}

	public Vector3 GetAbsolutePosition(Vector3 v) {
		float x = Mathf.Ceil(v.x - 0.5f);
		float z = Mathf.Ceil(v.z - 0.5f);
		return new Vector3(x, v.y, z);
	}

	public Vector2 GetRelativePosition(Vector3 v) {
		Vector3 tmp = this.GetAbsolutePosition(v);
		return new Vector2(tmp.x,tmp.z);
	}

	private Vector2 Vector2Abs(Vector2 v) {
		return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
	}

	private bool IsIndestructibleBlocCases(Vector2 v) {
		Vector2 vabs = Vector2Abs(v);
		return vabs.x % 2 == 1 && vabs.y % 2 == 0;
	}

	private bool IsForbidden(Vector2 v) {
		Vector2 vabs = Vector2Abs(v);
		bool isStartCasesForPlayers = (
			(vabs.x == 5 && vabs.y == 5) ||
			(vabs.x == 6 && (vabs.y == 4 || vabs.y == 5))
		);
		return isStartCasesForPlayers || this.IsIndestructibleBlocCases(v);
	}

	public bool IsOccupied(Vector2 v) {
		return this.terrain.ContainsKey(v);
	}

	public bool IsOccupied(Vector3 v) {
		return this.IsOccupied(this.GetRelativePosition(v));
	}

	private GameObject MakeBomb(Player player) {
		GameObject bomb_clone = (GameObject) Instantiate(
			this.bomb,
			this.GetAbsolutePosition(player.transform.position),
			Quaternion.identity
		);

		bomb_clone.GetComponent<Bomb>().player = player;
		bomb_clone.SetActive(true);

		return bomb_clone;
	}

	public bool AddBomb(Player player) {
		Vector3 player_pos = player.transform.position;
		if (!this.IsOccupied(player_pos)) {
			this.terrain.Add(this.GetRelativePosition(player_pos), this.MakeBomb(player));
			return true;
		}
		return false;
	}
	
	public void ExplodeBomb(Bomb bomb) {
		Vector2 pos = this.GetRelativePosition(bomb.transform.position);

		// Remove bomb from terrain & destroy GameObject
		this.terrain.Remove(pos);
		bomb.Explode();
		Destroy(bomb.gameObject);

		for(int i = 0; i<this.players.Length; i++){
			Player p = this.players[i];
			if(p != null){
				if(this.GetRelativePosition(p.transform.position) == pos){
					this.players[i] = null;
					Destroy (p.gameObject);
				}
			}
		}
		// Launch Fires !
		this.fires.Add(new Fire(pos, new Vector2(-1,0), bomb.player.power));
		this.fires.Add(new Fire(pos, new Vector2(1,0), bomb.player.power));
		this.fires.Add(new Fire(pos, new Vector2(0,1), bomb.player.power));
		this.fires.Add(new Fire(pos, new Vector2(0,-1), bomb.player.power));
	}
	
	private void AddBlocks() {
		int nb_block = 200;
		for (int i = 0; i < nb_block; i++) {
			int x = Random.Range(-6,7);
			int z = Random.Range(-5,6);
			Vector3 v = new Vector3(x, 0.5f, z);
			Vector2 v2 = GetRelativePosition(v);
			if (!this.IsOccupied(v2) && !this.IsForbidden(v2)) {
				GameObject block = (GameObject) Instantiate(
					this.destroyable_block,
					v,
					Quaternion.identity
				);
				block.SetActive(true);
				this.terrain.Add(v2, block);
			}
		}
	}

	public GameObject GetGameObject(Vector2 v){
		return this.terrain [v];
	}

	public bool RemoveGameObject(Vector2 v){
		if (this.IsOccupied(v)) {
			this.terrain.Remove(v);
			return true;
		}
		return false;
	}

	private bool IsOutOfTerrain(Vector2 v) {
		return v.x < -6 || v.x > 6 || v.y > 5 || v.y < -5;
	}

	public bool CanMove(Vector2 pos) {
		return (
			!this.IsOccupied(pos) &&
			!this.IsIndestructibleBlocCases(pos) &&
			!this.IsOutOfTerrain(pos)
			// @todo : and there is a path
		);
	}

	public Vector2 GetNextRelativePosition(Vector2 pos, Vector2 dir) {
		Vector2 relative_pos = PositionTools.RelativePosition(pos);
		Vector2 next = relative_pos + dir;
		if (this.CanMove(next)) {
			return next;
		} else {
			return relative_pos;
		}
	}

	public void MovePlayer(Player player, Vector2 dir) {
		Vector2 position = PositionTools.Position(player.transform.position);
		Vector2 next_relative_position = this.GetNextRelativePosition(
			position,
			dir
		);
		Vector3 absolute_next_position = PositionTools.AbsoluteDirection(next_relative_position);
		player.transform.position = Vector3.MoveTowards(
			player.transform.position,
			absolute_next_position,
			Time.deltaTime * player.speed
		);
	}

	// Bonuses

	private void AddBonus(){
		int nb_bonus = 40;
		for (int i = 0; i < nb_bonus; i++) {
			int x = Random.Range (-6, 7);
			int z = Random.Range (-5, 6);
			Vector3 v = new Vector3 (x, 0.5f, z);
			Vector2 v2 = GetRelativePosition (v);
			if (this.IsOccupied (v2)) {
					GameObject bonus = (GameObject)Instantiate (
						this.bonus,
						v,
						Quaternion.identity
					);
					bonus.SetActive (true);
			} else {
					i--;
			}
		}
	}
}
