/***************************************************************************\
Project:      Mini Mancala
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using System.Collections;

// This class represents 
public class Slot : MonoBehaviour {

	public int number;
	public int seeds;
	public bool house;
	public bool p1Owner;
	
	public void OnMouseDown() {
		GameManager.instance.OnSlotClick(this);
	}
	
	// highlights on mouse over
	public void OnMouseEnter() {
		Seed[] seeds = transform.GetComponentsInChildren<Seed>();
		foreach(Seed seed in seeds) {
			seed.Highlight(true);
		}
	}
	
	public void OnMouseExit() {
		Seed[] seeds = transform.GetComponentsInChildren<Seed>();
		foreach(Seed seed in seeds) {
			seed.Highlight(false);
		}
	}
}
