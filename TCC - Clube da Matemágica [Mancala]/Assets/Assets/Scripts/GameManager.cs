using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 A classe Game Manager é responsável por todo comportamento do game e a máquina de estados
*/
public class GameManager : MonoBehaviour {

	public GameState state = GameState.INITIAL;				// Máquina de Estados
	public Turn turn = Turn.NONE;							// Turno do Jogador 1 ou Jogador 2
	public Slot[] slots;									// As 14 Kalahs
	public float seedsSpeed = 0.1f;							// Velocidade das sementes em cada Kalah
	
	private Slot selectedSlot;								// A kalah selecionada pelo jogador
	private Slot lastSlot;									// O último slot na qual a semente caiu
	private string message;									// Mensagem da GUI 
	private bool againstAi;									// Está jogando contra uma IA? (quebrada)
	
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
			if(againstAi && turn == Turn.PLAYER_2) {
				PlayAI();
			}
			break;
		case GameState.MOVE_SEEDS:
			break;
		case GameState.CHECK_END_CONDITION:
			CheckEndCondition();
			break;
		}
	}
	
	public void Initialize() {
		StartCoroutine(InitializeCR());
	}
	
	IEnumerator InitializeCR() {
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
	
	private void CreateSeed(Transform slotTra) {
		GameObject seed = GameObject.Instantiate(ResourcesManager.instance.seedPrefab) as GameObject;
		
		Material seedMaterial = ResourcesManager.instance.GetRandomSeedMaterial();
		
		if(seedMaterial) {
			seed.GetComponent<MeshRenderer>().material = seedMaterial;
		}
		
		PlaceSeed(slotTra, seed);
	}
	
	private void PlaceSeed(Transform slotTra, GameObject seed) {
		seed.transform.parent = slotTra;
		
		float posX = UnityEngine.Random.Range(-0.28f, 0.18f);
		float posY = 0.6f;
		float posZ = UnityEngine.Random.Range(-0.25f, 0.25f);
		
		seed.transform.localPosition = new Vector3(posX, posY, posZ);
		seed.transform.rotation = UnityEngine.Random.rotation;
		
		seed.GetComponent<Rigidbody>().velocity = Vector3.down;
	}
	
	public void OnSlotClick(Slot slot) {
		OnSlotClick(slot, false);
	}
	
	public void OnSlotClick(Slot slot, bool aiPlay) {
		if(state == GameState.TURN_START) {
			selectedSlot = slot;
			if(canSelectSlot(selectedSlot, aiPlay)) {
				message = string.Empty;
				StartCoroutine(MovingSeedsCR());
				state = GameState.MOVE_SEEDS;
			} else {
				message = Message.MSG_INVALID_SLOT;
			}
		}
	}
	
	IEnumerator MovingSeedsCR() {
		int seedsPool = selectedSlot.seeds;
		selectedSlot.seeds = 0;
		Seed[] seeds = selectedSlot.transform.GetComponentsInChildren<Seed>();
		
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
		
		if(lastSlot.seeds == 1 && !lastSlot.house && ((turn==Turn.PLAYER_1 && lastSlot.p1Owner) || (turn==Turn.PLAYER_2 && !lastSlot.p1Owner))) {
			Slot oppositeSlot = slots[GetOppositeSlot(lastSlot.number)];
			seedsPool = oppositeSlot.seeds;
			oppositeSlot.seeds = 0;
			
			if(seedsPool > 0) {
				Debug.Log("Pegou as sementes do outro Jogador");
				seeds = oppositeSlot.transform.GetComponentsInChildren<Seed>();
				
				Slot playerHole = turn==Turn.PLAYER_1?slots[7]:slots[0];
				
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
	
	private void CheckEndCondition() {
		bool slotsP1Empty = true;
		bool slotsP2Empty = true;
		
		for(int i = 1;i<7;i++) {
			if(slots[i].seeds > 0) {
				slotsP1Empty = false;
				break;
			}
		}

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
			if(turn == Turn.PLAYER_1 && lastSlot.number == 7 ||
			   turn == Turn.PLAYER_2 && lastSlot.number == 0) {
				message = Message.MSG_PLAY_AGAIN;
			} else {
				turn = turn == Turn.PLAYER_1?Turn.PLAYER_2:Turn.PLAYER_1;
			}
			state = GameState.TURN_START;
		}
	}
	
	private void PlayAI() {
		List<int> bestMovements = new List<int>();
		List<int> goodMovements = new List<int>();
		List<int> possibleMovements = new List<int>();

		for(int i = 8; i<14; i++) {
			if(slots[i].seeds > 0) {
				possibleMovements.Add(i);
			}
		}
		
		foreach(int i in possibleMovements) {
			Slot slot = slots[i];
			
			if(slot.seeds + i == 14) {
				bestMovements.Add(i);
			}
			
			int seedsPool = slot.seeds;
			int endSlotNum = slot.number;
			while(seedsPool > 0) {
				endSlotNum = GetNextSlot(endSlotNum);
				seedsPool --;
			}
			Slot endSlot = slots[endSlotNum];
			if(endSlot.seeds == 0 && !endSlot.p1Owner && !endSlot.house) {
				Slot oppositeSlot = slots[GetOppositeSlot(endSlotNum)];
				if(oppositeSlot.seeds > 0) {
					bestMovements.Add(i);
				}
			}
		}
		
		foreach(int i in possibleMovements) {
			for(int j = 9; j<14; j++) {
				if(slots[j].seeds + i == 13) {
					goodMovements.Add(i);
					break;
				}
			}
		}
		
		int clickSlot = 8;
		if(bestMovements.Count > 0) {
			clickSlot = bestMovements[UnityEngine.Random.Range(0, bestMovements.Count)];
		} else if(goodMovements.Count > 0) {
			clickSlot = goodMovements[UnityEngine.Random.Range(0, goodMovements.Count)];
		} else {
			clickSlot = possibleMovements[UnityEngine.Random.Range(0, possibleMovements.Count)];
		}
		
		OnSlotClick(slots[clickSlot], true);
	}
	
	private int GetNextSlot(int slotNum) {
		if(slotNum == 13)
			return 0;
			
		return slotNum + 1;
	}
	
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
			
			againstAi = GUI.Toggle(new Rect (Screen.width/2 - 50, Screen.height/2 + 100, 100, 50), againstAi, "IA");
		}
		
		if(state == GameState.END) {
			if(GUI.Button (new Rect (Screen.width/2 - 50, Screen.height/2 + 10, 100, 50), Message.BTN_START)) {
				OnRestartClick();
			}
		}
	}
}
