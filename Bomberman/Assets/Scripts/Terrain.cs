using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terrain : MonoBehaviour
{
	private static Terrain _instance;

	private Terrain (){}

	public static Terrain instance {
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<Terrain>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}


	 void Awake(){
				if (_instance == null) {
						_instance = this;
						DontDestroyOnLoad (this);
				} else if (this != _instance) {

						Destroy (this.gameObject);
				}
		}


	public GameObject bomb;
	public Dictionary<GameObject,Player> dico_bomb = new Dictionary<GameObject, Player>();
	public GameObject destroyable_block;
	private const int length = 13;
	private const int width = 11;
	public Dictionary<Vector2,GameObject> terrain;
	public List<Vector2> forbidden = new List<Vector2>();
	public List<Vector2> indestructible_block = new List<Vector2>();




	void Start(){
		this.terrain = new Dictionary<Vector2,GameObject> ();
		// Cases libres
		forbidden.Add (new Vector2 (5, 5));
		forbidden.Add (new Vector2 (-5, 5));
		forbidden.Add (new Vector2 (5, -5));
		forbidden.Add (new Vector2 (-5, -5));
		forbidden.Add (new Vector2 (6, 4));
		forbidden.Add (new Vector2 (-6, 4));
		forbidden.Add (new Vector2 (6, -4));
		forbidden.Add (new Vector2 (-6, -4));
		forbidden.Add (new Vector2 (6, 5));
		forbidden.Add (new Vector2 (-6, 5));
		forbidden.Add (new Vector2 (6, -5));
		forbidden.Add (new Vector2 (-6, -5));
		// Cases contenant un bloc indestructible
		forbidden.Add (new Vector2 (5, 4));
		forbidden.Add (new Vector2 (3, 4));
		forbidden.Add (new Vector2 (1, 4));
		forbidden.Add (new Vector2 (-1, 4));
		forbidden.Add (new Vector2 (-3, 4));
		forbidden.Add (new Vector2 (-5, 4));
		forbidden.Add (new Vector2 (5, 2));
		forbidden.Add (new Vector2 (3, 2));
		forbidden.Add (new Vector2 (1, 2));
		forbidden.Add (new Vector2 (-1, 2));
		forbidden.Add (new Vector2 (-3, 2));
		forbidden.Add (new Vector2 (-5, 2));
		forbidden.Add (new Vector2 (5, 0));
		forbidden.Add (new Vector2 (3, 0));
		forbidden.Add (new Vector2 (1, 0));
		forbidden.Add (new Vector2 (-1, 0));
		forbidden.Add (new Vector2 (-3, 0));
		forbidden.Add (new Vector2 (-5, 0));
		forbidden.Add (new Vector2 (5, -2));
		forbidden.Add (new Vector2 (3, -2));
		forbidden.Add (new Vector2 (1, -2));
		forbidden.Add (new Vector2 (-1, -2));
		forbidden.Add (new Vector2 (-3, -2));
		forbidden.Add (new Vector2 (-5, -2));
		forbidden.Add (new Vector2 (5, -4));
		forbidden.Add (new Vector2 (3, -4));
		forbidden.Add (new Vector2 (1, -4));
		forbidden.Add (new Vector2 (-1, -4));
		forbidden.Add (new Vector2 (-3, -4));
		forbidden.Add (new Vector2 (-5, -4));
		// Cases contenant un bloc indestructible
		indestructible_block.Add (new Vector2 (5, 4));
		indestructible_block.Add (new Vector2 (3, 4));
		indestructible_block.Add (new Vector2 (1, 4));
		indestructible_block.Add (new Vector2 (-1, 4));
		indestructible_block.Add (new Vector2 (-3, 4));
		indestructible_block.Add (new Vector2 (-5, 4));
		indestructible_block.Add (new Vector2 (5, 2));
		indestructible_block.Add (new Vector2 (3, 2));
		indestructible_block.Add (new Vector2 (1, 2));
		indestructible_block.Add (new Vector2 (-1, 2));
		indestructible_block.Add (new Vector2 (-3, 2));
		indestructible_block.Add (new Vector2 (-5, 2));
		indestructible_block.Add (new Vector2 (5, 0));
		indestructible_block.Add (new Vector2 (3, 0));
		indestructible_block.Add (new Vector2 (1, 0));
		indestructible_block.Add (new Vector2 (-1, 0));
		indestructible_block.Add (new Vector2 (-3, 0));
		indestructible_block.Add (new Vector2 (-5, 0));
		indestructible_block.Add (new Vector2 (5, -2));
		indestructible_block.Add (new Vector2 (3, -2));
		indestructible_block.Add (new Vector2 (1, -2));
		indestructible_block.Add (new Vector2 (-1, -2));
		indestructible_block.Add (new Vector2 (-3, -2));
		indestructible_block.Add (new Vector2 (-5, -2));
		indestructible_block.Add (new Vector2 (5, -4));
		indestructible_block.Add (new Vector2 (3, -4));
		indestructible_block.Add (new Vector2 (1, -4));
		indestructible_block.Add (new Vector2 (-1, -4));
		indestructible_block.Add (new Vector2 (-3, -4));
		indestructible_block.Add (new Vector2 (-5, -4));
		this.AddBlocks ();
	}

	void Update(){

	}

	public Vector2 GetRealPosition(Vector3 v){
		float x = Mathf.Ceil (v.x - 0.5f);
		float z = Mathf.Ceil (v.z - 0.5f);
		return new Vector2 (x,z);
	}

	public Vector3 GetVector3Position(Vector3 v){
		float x = Mathf.Ceil (v.x - 0.5f);
		float z = Mathf.Ceil (v.z - 0.5f);
		return new Vector3 (x, v.y, z);
	}
	public bool IsOccupied(Vector2 v){
		return this.terrain.ContainsKey(v);
	}

	public bool IsOccupied(Vector3 v){
		return this.IsOccupied (this.GetRealPosition (v));
	}

	private GameObject MakeBomb(Player player){
		Vector3 v1 = this.GetVector3Position (player.transform.position);
		GameObject bomb_clone = (GameObject)Instantiate (this.bomb, v1, Quaternion.identity);
		bomb_clone.SetActive (true);
		if (bomb_clone != null) {
						this.dico_bomb.Add (bomb_clone, player);
				}
		return bomb_clone;
	}

	public bool AddBomb(Player player){
		Vector3 player_pos = player.transform.position;
		if (!this.IsOccupied (player_pos)) {
			this.terrain.Add (this.GetRealPosition (player_pos), this.MakeBomb (player));
			return true;
		}
		return false;
	}


	void AddBlocks(){
		int nb_block = Random.Range (200, 220);
		for (int i = 0; i< nb_block; i++) {
			int x = Random.Range(-6,7);
			int z = Random.Range(-5,6);
			Vector3 v = new Vector3 (x, 0.5f, z);
			Vector2 v2 = GetRealPosition(v);
			if (!this.IsOccupied (v2) && !forbidden.Contains(v2)) {
				GameObject block = (GameObject)Instantiate (this.destroyable_block, v, Quaternion.identity);
				block.SetActive (true);
				this.terrain.Add (v2, block);
			}
		}
	}
}

