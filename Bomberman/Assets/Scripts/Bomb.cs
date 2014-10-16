using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
	}

	public void OnTriggerExit(Collider other){
		this.collider.isTrigger = false;
	}
}

