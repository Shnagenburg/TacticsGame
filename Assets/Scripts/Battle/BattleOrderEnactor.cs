using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleOrderEnactor : MonoBehaviour {

	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	enum State {
		MOVING,
		ATTACKING,
		FINISHED
	}
	State state = State.MOVING;
	BattleOrder battleOrder = null;
	Tile previousHop = null;
	Tile nextHop = null;
	List <Tile> tilePath = null;
	float timer = 0.0f;
	float maxTimer = 0.0f;

	// Use this for initialization
	void Start () {
		Debug.Log("order enactor!");
		battleStateTracker.gameObject = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (battleOrder == null) {
			return;
		}
		switch (state) {
		case State.ATTACKING:
			timer += Time.deltaTime;
			if (!battleOrder.SourceCombatant.animating && timer > maxTimer) {
				state = State.FINISHED;
			}
			break;
		case State.MOVING:
			WalkToDestination();
			break;
		case State.FINISHED:
			battleStateTracker.ResetChain();
			break;
		}
	}

	public void Enact(BattleOrder battleOrder) {
		this.battleOrder = battleOrder;
		if ("attack".Equals(battleOrder.Action)) {
			battleOrder.SourceCombatant.StartAttackAnimation(battleOrder.TargetTile);
			state = State.ATTACKING;

			//TODO shuffle this somewhere else
			ApplyDamage();

		} else if ("move".Equals(battleOrder.Action)) {
			MapManager map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
			tilePath = map.GetShortestPath(battleOrder.SourceCombatant.GetTile(), battleOrder.TargetTile);
			previousHop = tilePath[0];
			nextHop = tilePath[0];
			state = State.MOVING;
		}
	}

	void ApplyDamage() {
		CombatantStats attackerStats = battleOrder.SourceCombatant.Stats;
		Combatant targetCombatant = battleOrder.TargetTile.GetOccupant();
		if (targetCombatant == null) {
			maxTimer = 0.0f;
			return;
		}
		int damage = attackerStats.AttackPower;
		targetCombatant.Stats.CurrentHealth = targetCombatant.Stats.CurrentHealth - damage;
		
		GameObject objToSpawn = (GameObject)Instantiate(Resources.Load("DamageText"));
		objToSpawn.SetActive(true);
		objToSpawn.GetComponent<DamageNumber>().SetNumber(damage);
		objToSpawn.transform.position = targetCombatant.transform.position + new Vector3(0, 1, 0);
		maxTimer = objToSpawn.GetComponent<DamageNumber>().duration;


	}

	void WalkToDestination() {
		Combatant combatant = battleOrder.SourceCombatant;
		Vector3 sourcePos = previousHop.transform.position;
		Vector3 destinationPos = nextHop.transform.position;
		timer = timer + Time.deltaTime;
		combatant.transform.position = Vector3.Lerp(sourcePos, destinationPos, timer * 2.0f);
		if (combatant.transform.position == destinationPos) {
			timer = 0;
			previousHop = nextHop;
			int hopIndex = tilePath.IndexOf(nextHop);
			hopIndex = hopIndex + 1;
			if (hopIndex >= tilePath.Count) {
				state = State.FINISHED;
				Tile sourceTile = combatant.GetTile();
				nextHop.SetOccupant(combatant);
				sourceTile.RemoveOccupant(combatant);
				return;
			}
			nextHop = tilePath[hopIndex];
		}
	}
}
