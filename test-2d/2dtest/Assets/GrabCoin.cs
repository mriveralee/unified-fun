using UnityEngine;
using System.Collections;

public class GrabCoin : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Hero") {
			audio.Play();
			Destroy(gameObject.collider2D);
			gameObject.renderer.enabled = false;
			Destroy(gameObject, 0.47f);// Destroy the object -after- the sound played
		}
	}
}
