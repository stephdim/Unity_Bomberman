using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

	public void OnTriggerExit(Collider other) {
		this.collider.isTrigger = false;
	}

}

