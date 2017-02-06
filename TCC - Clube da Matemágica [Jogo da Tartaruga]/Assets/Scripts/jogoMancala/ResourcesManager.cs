/***************************************************************************\
Project:      Mini Mancala
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using System;
using System.Collections;

/*
The Game manager class manages all prefabs
*/
public class ResourcesManager : MonoBehaviour {

	public GameObject seedPrefab;					// Seed Prefab
	public Material[] seedMaterial;					// The different materials for the seed
	public AudioClip marbleWood;					// Sound when the seed collides with the board
	public AudioClip marble;						// Sound when the seed collides with another seed
	
	// Singleton
	private static ResourcesManager _instance;
	public static ResourcesManager instance {
		get {
			if(_instance == null) {
				_instance = FindObjectOfType<ResourcesManager>();
			}
			return _instance;
		}
	}
	
	void Awake() {
		_instance = this;
	}
	
	public Material GetRandomSeedMaterial() {
		if(seedMaterial == null || seedMaterial.Length == 0)
			return null;
			
		return seedMaterial[UnityEngine.Random.Range(0, seedMaterial.Length)];
	}
}