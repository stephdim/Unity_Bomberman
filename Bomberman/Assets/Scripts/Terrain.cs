using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Terrain : Singleton
 * 
 * @todo: add description of Terrain class here...
 * 
 * @comment: maybe use "relative" position for terrain and "absolute" for unity scene
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
	private int timer = 2 * 60;
	public Dictionary<Vector2,GameObject> terrain;
	public Player[] players = new Player[4];
	public List<Fire> fires = new List<Fire>();
	public Dictionary<GameObject,Player> dico_bomb = new Dictionary<GameObject, Player>();
	private List<GameObject> bombs = new List<GameObject>();


	void Start() {
		this.terrain = new Dictionary<Vector2,GameObject>();
		players = GameObject.FindObjectsOfType<Player>();
		this.AddBlocks ();
	}


	void Update(){
	}

	public Vector2 GetRealPosition(Vector3 v) {
		float x = Mathf.Ceil(v.x - 0.5f);
		float z = Mathf.Ceil(v.z - 0.5f);
		return new Vector2(x,z);
	}

	public Vector3 GetVector3Position(Vector3 v) {
		float x = Mathf.Ceil(v.x - 0.5f);
		float z = Mathf.Ceil(v.z - 0.5f);
		return new Vector3(x, v.y, z);
	}

	private bool IsForbidden(Vector2 v) {
		Vector2 vabs = new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
		bool isStartCasesForPlayers = (
			(vabs.x == 5 && vabs.y == 5) ||
			(vabs.x == 6 && (vabs.y == 4 || vabs.y == 5))
		);
		bool isIndestructibleBlocCases = (vabs.x % 2 == 1 && vabs.y % 2 == 0);
		return isStartCasesForPlayers || isIndestructibleBlocCases;
	}

	public bool IsIndestructibleBlocCases(Vector2 v){
		Vector2 vabs = new Vector2 (Mathf.Abs (v.x), Mathf.Abs (v.y));
		return (vabs.x % 2 == 1 && vabs.y % 2 == 0);
	}

	public bool IsOccupied(Vector2 v) {
		return this.terrain.ContainsKey(v);
	}

	public bool IsOccupied(Vector3 v) {
		return this.IsOccupied(this.GetRealPosition(v));
	}

	private GameObject MakeBomb(Player player) {
		Vector3 v1 = this.GetVector3Position(player.transform.position);
		GameObject bomb_clone = (GameObject) Instantiate(this.bomb, v1, Quaternion.identity);
		bomb_clone.SetActive(true);
		this.dico_bomb.Add(bomb_clone, player);
		this.bombs.Add (bomb_clone);
		return bomb_clone;
	}

	public bool AddBomb(Player player) {
		Vector3 player_pos = player.transform.position;
		if (!this.IsOccupied(player_pos)) {
			this.terrain.Add(this.GetRealPosition(player_pos), this.MakeBomb(player));
			return true;
		}
		return false;
	}

	private void AddBlocks() {
		int nb_block = 200;
		for (int i = 0; i < nb_block; i++) {
			int x = Random.Range(-6,7);
			int z = Random.Range(-5,6);
			Vector3 v = new Vector3(x, 0.5f, z);
			Vector2 v2 = GetRealPosition(v);
			if (!this.IsOccupied(v2) && !this.IsForbidden(v2)) {
				GameObject block = (GameObject) Instantiate(this.destroyable_block, v, Quaternion.identity);
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

	//TODO : Create a function which check if a GameObject is on the same case than a Fire.
}

