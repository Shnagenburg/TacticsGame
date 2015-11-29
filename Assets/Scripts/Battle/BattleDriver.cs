using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleDriver : MonoBehaviour {

	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	
	List<Combatant> combatants = new List<Combatant>();
	Combatant activeCombatant = null;
	bool nextTurn = false;
	MapManager map;

	// Use this for initialization
	void Start () {
		battleStateTracker.gameObject = this.gameObject;
		map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		GameObject [] combatObjects = GameObject.FindGameObjectsWithTag("Combatant");
		int i = 0;
		foreach (GameObject obj in combatObjects) {
			combatants.Add(obj.GetComponent<Combatant>());			
			Tile currentTile = map.TileMap.GetMap()[0,i];
			currentTile.SetOccupant(obj.GetComponent<Combatant>());
			i++;
		}
		NextTurn();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Top level battle driver");
		if (nextTurn) {
			NextTurn();
		}		
		GameObject objToSpawn = (GameObject)Instantiate(Resources.Load("Menu"));
		objToSpawn.GetComponent<Menu>().SetCombatant(activeCombatant);
		objToSpawn.GetComponent<Menu>().battleStateTracker.previous = this.battleStateTracker;
		nextTurn = true;
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
		nextTurn = false;
	}
}
