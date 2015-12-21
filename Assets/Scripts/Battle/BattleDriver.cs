
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleDriver : MonoBehaviour {

	static public BattleDriver CurrentBattleDriver { get; set; }
	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	public List<Combatant> Combatants { 
		get {return combatants;}
	}
	List<Combatant> combatants = new List<Combatant>();
	Combatant activeCombatant = null;
	bool nextTurn = false;
	MapManager map;

	// Use this for initialization
	void Start () {
		BattleDriver.CurrentBattleDriver = this;
		battleStateTracker.gameObject = this.gameObject;
		map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		GameObject [] combatObjects = GameObject.FindGameObjectsWithTag("Combatant");
		int i = 0;
		foreach (GameObject obj in combatObjects) {
			combatants.Add(obj.GetComponent<Combatant>());			
			Tile currentTile = map.TileMap.GetMap()[i*2, i*2];
			currentTile.SetOccupant(obj.GetComponent<Combatant>());
			i++;
		}
		for (int j = 0; j < combatants.Count; j++) {
			if (j % 2 == 0) {
				combatants[j].TeamId = TeamId.EnemyTeam;
			} else {
				combatants[j].TeamId = TeamId.PlayerTeam;
			}
		}
		NextTurn();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Top level battle driver");
		CheckVictory();
		Debug.Log("Active combatant has - " + activeCombatant.Stats.TurnStats.ToString());
		if (!activeCombatant.Stats.TurnStats.CanDoSomething()) {
			NextTurn();
			return;
		}
		if (activeCombatant.Stats.HasStatus("dead")) {
			Debug.Log("combatant " + activeCombatant + " is ded! skipping turn...");
			nextTurn = true;
			return;
		}

		if (activeCombatant.TeamId == TeamId.PlayerTeam) {
			GameObject objToSpawn = (GameObject)Instantiate(Resources.Load("Menu"));
			objToSpawn.GetComponent<Menu>().SetCombatant(activeCombatant);
			objToSpawn.GetComponent<Menu>().battleStateTracker.previous = this.battleStateTracker;
		} else {
			GameObject objToSpawn = new GameObject("AI Director");
			objToSpawn.AddComponent<AIDirector>();
			objToSpawn.GetComponent<AIDirector>().SetCombatant(activeCombatant);
			objToSpawn.GetComponent<AIDirector>().battleStateTracker.previous = this.battleStateTracker;

		}
		this.gameObject.SetActive(false);
	}

	void NextTurn() {
		Debug.Log("finishing turn of - " + activeCombatant);
		if (activeCombatant == null) {
			activeCombatant = combatants[0];
		} else {
			int i = combatants.IndexOf(activeCombatant);
			i = (i + 1) % combatants.Count;
			activeCombatant = combatants[i];
		}
		Debug.Log("now turn of - " + activeCombatant);
		activeCombatant.Stats.TurnStats.Reset(activeCombatant.Stats);
		nextTurn = false;
	}

	void CheckVictory() {
		bool teamZeroAlive = false;
		bool teamOneAlive = false;
		foreach (Combatant combatant in combatants) {
			if (!combatant.Stats.HasStatus("dead")) {
				if (combatant.TeamId == 0) {
					teamZeroAlive = true;
				} else {
					teamOneAlive = true;
				}
			}
		}
		if (!teamZeroAlive && !teamOneAlive) {
			Debug.Log("Draw!");
		} else if (teamOneAlive) {
			Debug.Log("game over!");
		} else if (teamZeroAlive) {
			Debug.Log("you win!");
		}
	}
}
