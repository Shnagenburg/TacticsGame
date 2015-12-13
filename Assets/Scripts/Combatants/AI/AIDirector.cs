using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class AIDirector : MonoBehaviour {

	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	Combatant combatant;
	BattleOrder battleOrder;
	CharacterPane leftPane = null;
	ThreadedCalculateAIOrder calc = null;

	static public Mutex MainLoop = new Mutex(true);


	// Use this for initialization
	void Start () {
		this.battleStateTracker.gameObject = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    if (calc != null && calc.IsDone) {
			Finish();
		}
	}

	public void SetCombatant(Combatant combatant) {
		this.combatant = combatant;
		
		leftPane = CharacterPane.FindLeftPane();
		EnablePane();
		CreateBattleOrder();
	}

	void CreateBattleOrder() {		
		
		MapManager map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		calc = new ThreadedCalculateAIOrder(combatant, map);
	}

	void Finish() {		
		GameObject objToSpawn = new GameObject("BattleOrderEnactor Action");
		objToSpawn.AddComponent<BattleOrderEnactor>();
		objToSpawn.GetComponent<BattleOrderEnactor>().battleStateTracker.previous = this.battleStateTracker;
		objToSpawn.GetComponent<BattleOrderEnactor>().Enact(calc.BattleOrder);
		Debug.Log("enacting... " + calc.BattleOrder);
		
		this.gameObject.SetActive(false);
	}

	void EnablePane() {
		leftPane.gameObject.SetActive(true);
		leftPane.Populate(combatant);
	}
}
