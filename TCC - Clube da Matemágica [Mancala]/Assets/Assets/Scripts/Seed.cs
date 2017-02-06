using UnityEngine;
using System.Collections;

public class Seed : MonoBehaviour {
	
	private AudioSource audioSource;
	public GameObject hightlight;
	
	void Awake() {
		audioSource = FindObjectOfType<AudioSource>();
	}
	
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag == "Seed") {
			audioSource.clip = ResourcesManager.instance.marble;
			audioSource.Play();
			return;
		}
		
		if(col.gameObject.tag == "Board") {
			audioSource.clip = ResourcesManager.instance.marbleWood;
			audioSource.Play();
			return;
		}
	}
	
	public void Highlight(bool value) {
		hightlight.SetActive(value);
	}
}
