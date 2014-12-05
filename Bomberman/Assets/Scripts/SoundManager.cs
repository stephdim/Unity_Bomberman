using UnityEngine;
using System;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip bomb_plus;
	public AudioClip boom;
	public AudioClip fffrrr;
	public AudioClip level1;
	public AudioClip pouvoir_plus;
	public AudioClip vitesse_plus;

	public static void Launch(string sound) {
		Camera.main.GetComponent<SoundManager>().Play(sound);
	}

	public void Play(string sound) {
		AudioClip audioClip;
		float volume = 1;
		if (sound == "Bomb+") {
			audioClip = bomb_plus;
		} else if (sound == "Boom") {
			audioClip = boom;
			volume = .6f;
		} else if (sound == "FFFrrr") {
			audioClip = fffrrr;
		} else if (sound == "Level1") {
			audioClip = level1;
		} else if (sound == "Pouvoir+") {
			audioClip = pouvoir_plus;
		} else if (sound == "Vitesse+") {
			audioClip = vitesse_plus;
		} else {
			throw new ArgumentException("sound not found", "sound");
		}
		AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
	}

	void Start() {
		audio.clip = level1;
		audio.Play();
	}

}