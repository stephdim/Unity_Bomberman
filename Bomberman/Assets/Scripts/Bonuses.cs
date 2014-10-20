using UnityEngine;
using System.Collections;

public class Bonuses : MonoBehaviour
{

	private int kind;

	// Use this for initialization
	void Start ()
	{
		this.Bonus ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void Bonus(){

		int i = Random.Range (1, 4);
		this.kind = i;
		switch (i) {
		case 1 : this.GetComponent<TextMesh>().text = "Bomb +";break;
		case 2 : this.GetComponent<TextMesh>().text = "Speed +";break;
		case 3 : this.GetComponent<TextMesh>().text = "Power +";break;
		default : this.GetComponent<TextMesh>().text = "Push +";break;
		}

	}

	public int GetKindOfBonus(){
		return this.kind;
	}

	private void OnTriggerEnter(Collider other){
		Player p = other.GetComponent<Player>();
		if (p != null) {
			p.AddBonus (this);
			Destroy (this.gameObject);
		}
	}
}

