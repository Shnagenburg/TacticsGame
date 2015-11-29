using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	// singleton pattern from https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager?playlist=17150
	
	public static BattleManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

	enum Phase {
		CHOOSE_COMMAND,
		CHOOSE_TARGET,
		EXECUTING
	}

	public BattleMenu menu;
	List<Combatant> combatants = new List<Combatant>();
	Combatant activeCombatant = null;
	Phase phase = Phase.CHOOSE_COMMAND;
	bool nextTurn = true;

	//Awake is always called before any Start functions
	void Awake () {
		//Check if instance already exists
		if (instance == null) {             
			//if not, set instance to this
			instance = this;
		} else if (instance != this) {   //If instance already exists and it's not this:            
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);    
		}
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);
		
		//Call the InitGame function to initialize the first level 
		InitGame ();
	}

	void Start() {
		GameObject [] combatObjects = GameObject.FindGameObjectsWithTag("Combatant");
		foreach (GameObject obj in combatObjects) {
			combatants.Add(obj.GetComponent<Combatant>());
		}
	}

	void NextTurn() {
		if (activeCombatant == null) {
			activeCombatant = combatants[0];
		} else {
			int i = combatants.IndexOf(activeCombatant);
			i = (i + 1) % combatants.Count;
			activeCombatant = combatants[0];
		}
		//menu.PopulateOptions(activeCombatant);
	}
	
	//Initializes the game for each level.
	void InitGame () {
	}

	void Update () {
		if (nextTurn) {
			nextTurn = false;
			NextTurn();
		}
		switch (phase) {
		case Phase.CHOOSE_COMMAND:
//			menu.gameObject.SetActive(true);
			break;
		case Phase.CHOOSE_TARGET:
			break;
		case Phase.EXECUTING:
			break;
		}
	}
}
