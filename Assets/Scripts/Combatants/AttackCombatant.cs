using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackCombatant : MonoBehaviour {
	
	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	
	enum State {
		PICKING,
		ANIMATING,
		FINISHED
	}

	State state = State.PICKING;
	Combatant combatant;
	TilePicker tilePicker = null;
	Tile targetTile = null;
	bool isDone = false;

	// Use this for initialization
	void Start () {
		battleStateTracker.gameObject = this.gameObject;	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case State.PICKING:
			if (tilePicker != null) {
				if (tilePicker.IsDone()) {
					targetTile = tilePicker.GetResponse();
					if (targetTile != null) {
						combatant.StartAttackAnimation(targetTile);
						state = State.ANIMATING;
						tilePicker.CleanUp();
						Destroy(tilePicker.gameObject);
					}
				}
			}
			CheckForBackButton();
			break;
		case State.ANIMATING:
			if (!combatant.animating) {
				state = State.FINISHED;
			}
			break;
		case State.FINISHED:
			isDone = true;
			battleStateTracker.ResetChain();
			break;
		}
	}
	
	void CheckForBackButton() {
		if (Input.GetButton("Fire2")) {
			Debug.Log("backing out of attack");
			if (tilePicker != null) {
				tilePicker.CleanUp();
				Destroy(tilePicker.gameObject);
			}			
			battleStateTracker.GoBackOneStep();
		}
	}

	public void DoneWithAnimation() {

	}
	
	public void SetCombatant(Combatant combatant) {
		this.combatant = combatant;
		MapManager map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		List<Tile> tiles = map.GetTilesInRange(combatant.GetTile(), 1, true);
		tiles.Remove(combatant.GetTile());

		GameObject objToSpawn;
		objToSpawn = new GameObject("Tile Picker");
		objToSpawn.AddComponent<TilePicker>();
		tilePicker = objToSpawn.GetComponent<TilePicker>();
		tilePicker.SetTiles(tiles);
	}
}
