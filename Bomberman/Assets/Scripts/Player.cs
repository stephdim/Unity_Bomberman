using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// templates





	protected int color;
	protected int speed;
	public int power;
	// Use this for initialization
	void Start ()
	{
		this.speed = 1;
		this.power = 1;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			this.AddBomb ();

		}
	}


	public void AddBomb(){

		Terrain.instance.AddBomb (this);
	}


}

