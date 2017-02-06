/***************************************************************************\
Project:      Mini Mancala
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
The Game manager class is responbile for the all the game behavior and state machine control
*/
public class GameManager : MonoBehaviour {

	public GameState state = GameState.INITIAL;				// State machine
	public Turn turn = Turn.NONE;							// Player 1 or Player 2 turn
	public Slot[] slots;									// The 14 slots
	public float seedsSpeed = 0.1f;							// How fast are the seeds placed
	
	private Slot selectedSlot;								// The player selected slot
	private Slot lastSlot;									// The last slot where the seed landed on
	private string message;									// GUI message
	private bool againstAi;									// Are you playing against an AI?
	
	// Singleton
	private static GameManager _instance;
	public static GameManager instance {
		get {
			if(_instance == null) {
				_instance = FindObjectOfType<GameManager>();
			}
			return _instance;
		}
	}
	
	void Awake() {
		_instance = this;
	}

	public enum Turn {
		NONE,
		PLAYER_1,
		PLAYER_2
	}

	public enum GameState {
		INITIAL,
		START,
		TURN_START,
		MOVE_SEEDS,
		CHECK_END_CONDITION,
		END,
	}

	void Update() {
		switch (state) {
		case GameState.INITIAL:
			break;
		case GameState.START:
			state = GameState.TURN_START;
			break;
		case GameState.TURN_START:
			// Waits for OnSlotClick
			// If is against an AI, calculates the best movement and plays for this player
			if(againstAi && turn == Turn.PLAYER_2) {
				PlayAI();
			}
			break;
		case GameState.MOVE_SEEDS:
			// Waits for the coroutine MovingSeedsCR
			break;
		case GameState.CHECK_END_CONDITION:
			CheckEndCondition();
			break;
		}
	}
	
	// Initializes the board
	public void Initialize() {
		StartCoroutine(InitializeCR());
	}
	
	// Places the pieces with a small delay
	IEnumerator InitializeCR() {
		// Starts populating the slots
		for(int i = 0; i < 14; i++) {
			Slot slot = slots[i];
			if(!slot.house)	{
				for(int s = 0; s<4; s++) {
					CreateSeed(slot.transform);
					yield return new WaitForSeconds(0.001f);
				}
				slot.seeds = 4;
			}
		}
		turn = Turn.PLAYER_1;
		state = GameState.TURN_START;
	}
	
	// Created a random seed object
	private void CreateSeed(Transform slotTra) {
		GameObject seed = GameObject.Instantiate(ResourcesManager.instance.seedPrefab) as GameObject;
		
		// Randomizes the seed material
		Material seedMaterial = ResourcesManager.instance.GetRandomSeedMaterial();
		
		if(seedMaterial) {
			seed.GetComponent<MeshRenderer>().material = seedMaterial;
		}
		
		PlaceSeed(slotTra, seed);
	}
	
	// Places a seed randomly inside a slot
	private void PlaceSeed(Transform slotTra, GameObject seed) {
		seed.transform.parent = slotTra;
		
		float posX = UnityEngine.Random.Range(-0.28f, 0.18f);
		float posY = 0.6f;
		float posZ = UnityEngine.Random.Range(-0.25f, 0.25f);
		
		seed.transform.localPosition = new Vector3(posX, posY, posZ);
		seed.transform.rotation = UnityEngine.Random.rotation;
		
		// Starts dropping fast
		seed.GetComponent<Rigidbody>().velocity = Vector3.down;
	}
	
	public void OnSlotClick(Slot slot) {
		OnSlotClick(slot, false);
	}
	
	// Action when the player clicks a slot
	public void OnSlotClick(Slot slot, bool aiPlay) {
		if(state == GameState.TURN_START) {
			selectedSlot = slot;
			//checks if the player can select the slot
			if(canSelectSlot(selectedSlot, aiPlay)) {
				message = string.Empty;
				StartCoroutine(MovingSeedsCR());
				state = GameState.MOVE_SEEDS;
			} else {
				message = Message.MSG_INVALID_SLOT;
			}
		}
	}
	
	// Seeds movement animation
	IEnumerator MovingSeedsCR() {
		int seedsPool = selectedSlot.seeds;
		selectedSlot.seeds = 0;
		Seed[] seeds = selectedSlot.transform.GetComponentsInChildren<Seed>();
		
		// Place the seeds in another place not to be shown
		foreach(Seed seed in seeds) {
			seed.transform.position = Vector3.one * 100;
			seed.Highlight(false);
		}
		
		int nextSlot = selectedSlot.number;
		
		while(seedsPool > 0) {
			nextSlot = GetNextSlot(nextSlot);
			
			GameObject seedGo = seeds[seedsPool -1].gameObject;
			
			slots[nextSlot].seeds ++;
			seedsPool --;
			
			PlaceSeed(slots[nextSlot].transform, seedGo);
			
			yield return new WaitForSeconds(seedsSpeed);
		}
		
		lastSlot = slots[nextSlot];
		
		// If the final seed lands on an empty hole on your side, grabs all seeds on the opposite site and places in your mancala
		if(lastSlot.seeds == 1 && !lastSlot.house && ((turn==Turn.PLAYER_1 && lastSlot.p1Owner) || (turn==Turn.PLAYER_2 && !lastSlot.p1Owner))) {
			Slot oppositeSlot = slots[GetOppositeSlot(lastSlot.number)];
			seedsPool = oppositeSlot.seeds;
			oppositeSlot.seeds = 0;
			
			if(seedsPool > 0) {
				Debug.Log("Got the other player seeds");
				seeds = oppositeSlot.transform.GetComponentsInChildren<Seed>();
				
				Slot playerHole = turn==Turn.PLAYER_1?slots[7]:slots[0];
				
				// Place the seeds in another place not to be shown
				foreach(Seed seed in seeds) {
					seed.transform.position = Vector3.one * 100;
				}
				
				while(seedsPool > 0) {
					playerHole.seeds ++;
					
					PlaceSeed(playerHole.transform, seeds[seedsPool - 1].gameObject);
					
					seedsPool --;
					yield return new WaitForSeconds(seedsSpeed);
				}
			}
		}		
		
		state = GameState.CHECK_END_CONDITION;
		
		yield return null;
	}
	
	private int GetOppositeSlot(int slot) {
		return 14 - slot;
	}
	
	// Verifies if the player slots are all empty
	private void CheckEndCondition() {
		bool slotsP1Empty = true;
		bool slotsP2Empty = true;
		
		// Player 1 Slots
		for(int i = 1;i<7;i++) {
			if(slots[i].seeds > 0) {
				slotsP1Empty = false;
				break;
			}
		}

		// Player 2 Slots
		for(int i = 8;i<14;i++) {
			if(slots[i].seeds > 0) {
				slotsP2Empty = false;
				break;
			}
		}
		
		if(slotsP1Empty || slotsP2Empty) {
			int seedsP1 = slots[7].seeds;
			int seedsP2 = slots[0].seeds;
			
			if(seedsP1 == seedsP2) {
				message = String.Format(Message.MSG_DRAW);	
			} else {
				message = String.Format(Message.MSG_WINNER, seedsP1 > seedsP2?"1":"2");
			}
			state = GameState.END;
		} else {
			// checks if landed on the player house and starts the next turn
			// Is the players final slot?
			if(turn == Turn.PLAYER_1 && lastSlot.number == 7 ||
			   turn == Turn.PLAYER_2 && lastSlot.number == 0) {
				message = Message.MSG_PLAY_AGAIN;
			} else {
				turn = turn == Turn.PLAYER_1?Turn.PLAYER_2:Turn.PLAYER_1;
			}
			state = GameState.TURN_START;
		}
	}
	
	// AI player calculates the best possible movement and plays
	private void PlayAI() {
		// The best movements (when you can play again)
		List<int> bestMovements = new List<int>();
		// The good movements (when you can place seeds to get the best movements)
		List<int> goodMovements = new List<int>();
		// The possible movements
		List<int> possibleMovements = new List<int>();
		
		// Cycle between the possible slots
		for(int i = 8; i<14; i++) {
			if(slots[i].seeds > 0) {
				possibleMovements.Add(i);
			}
		}
		
		// Tries to pick the best movements from the possibilities
		foreach(int i in possibleMovements) {
			Slot slot = slots[i];
			
			// Should calculate if the last seed ends on the ending hole
			// 8 + 6 = 14, 9 + 5 = 14, 10 + 4 = 14 and so on...
			if(slot.seeds + i == 14) {
				bestMovements.Add(i);
			}
			
			// Another best movement is to capture the opposing player seeds
			// Forecast the ending seed position
			int seedsPool = slot.seeds;
			int endSlotNum = slot.number;
			while(seedsPool > 0) {
				endSlotNum = GetNextSlot(endSlotNum);
				seedsPool --;
			}
			Slot endSlot = slots[endSlotNum];
			if(endSlot.seeds == 0 && !endSlot.p1Owner && !endSlot.house) {
				// Now I need to check if the opposing slot if there is at least one seed
				Slot oppositeSlot = slots[GetOppositeSlot(endSlotNum)];
				if(oppositeSlot.seeds > 0) {
					bestMovements.Add(i);
				}
			}
		}
		
		// Forecast the good movements
		foreach(int i in possibleMovements) {
			// Starts at position 9 since pos 8 is the minimum
			for(int j = 9; j<14; j++) {
				// Needs to place at least one piece
				if(slots[j].seeds + i == 13) {
					goodMovements.Add(i);
					break;
				}
			}
		}
		
		int clickSlot = 8;
		// After calculating choses the best, good then possible movements. The random AI levels can be placed here
		if(bestMovements.Count > 0) {
			clickSlot = bestMovements[UnityEngine.Random.Range(0, bestMovements.Count)];
		} else if(goodMovements.Count > 0) {
			clickSlot = goodMovements[UnityEngine.Random.Range(0, goodMovements.Count)];
		} else {
			clickSlot = possibleMovements[UnityEngine.Random.Range(0, possibleMovements.Count)];
		}
		
		OnSlotClick(slots[clickSlot], true);
	}
	
	// Grabs the next cycling slot.
	private int GetNextSlot(int slotNum) {
		if(slotNum == 13)
			return 0;
			
		return slotNum + 1;
	}
	
	// Condition for selecting the slot. The player must be the owner and the slot must have at least one seed
	private bool canSelectSlot(Slot slot, bool aiPlay) {
		return (turn == Turn.PLAYER_1 && slot.p1Owner) || (turn == Turn.PLAYER_2 && !slot.p1Owner && (!againstAi || aiPlay)) && slot.seeds > 0;
	}
	
	public void OnRestartClick() {
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void OnGUI() {
		GUIStyle fontStyle = new GUIStyle();
		fontStyle.normal.textColor = Color.green;
		
		if(!String.IsNullOrEmpty(message)) {
			GUI.Label(new Rect(Screen.width/2, Screen.height - 100, 500, 20), message, fontStyle);
		}
		
		if(turn != Turn.NONE) {
			GUI.Label(new Rect(10, 10, 100, 20), String.Format(Message.MSG_TURN, (turn == Turn.PLAYER_1? "1" : "2")), fontStyle);
			GUI.Label(new Rect(10, 30, 200, 20), String.Format(Message.MSG_SCORE, "1", slots[7].seeds), fontStyle);
			GUI.Label(new Rect(10, 50, 200, 20), String.Format(Message.MSG_SCORE, "2", slots[0].seeds), fontStyle);
		}
	
		if(state == GameState.INITIAL) {
			if(GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 + 10, 100, 50), Message.BTN_START)) {
				Initialize();
			}
			
			againstAi = GUI.Toggle(new Rect (Screen.width/2 - 50, Screen.height/2 + 100, 100, 50), againstAi, "AI");
		}
		
		if(state == GameState.END) {
			if(GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 + 10, 100, 50), Message.BTN_START)) {
				OnRestartClick();
			}
		}
	}
}
