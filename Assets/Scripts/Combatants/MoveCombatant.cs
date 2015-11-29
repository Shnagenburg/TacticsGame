using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MoveCombatant : MonoBehaviour {
		
	public BattleStateTracker battleStateTracker = new BattleStateTracker();

	enum State {
		PICKING,
		ANIMATING,
		FINISHED
	}
	System.Action<int> callbackMethod;
	State state = State.PICKING;
	Combatant combatant;
	TilePicker tilePicker = null;
	Tile sourceTile = null;
	Tile destinationTile = null;
	Tile previousHop = null;
	Tile nextHop = null;
	bool isDone = false;

    List <Tile> tilePath = null;
	float timer = 0.0f;

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
					destinationTile = tilePicker.GetResponse();
					if (destinationTile != null) {
						MapManager map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
						tilePath = map.GetShortestPath(combatant.GetTile(), destinationTile);
						state = State.ANIMATING;
						tilePicker.CleanUp();
						Destroy(tilePicker.gameObject);
						previousHop = tilePath[0];
						nextHop = tilePath[0];
					}
				}
			}
			CheckForBackButton();
			break;
		case State.ANIMATING:
			combatant.animator.SetBool("IsWalking", true);
			WalkToDestination();
            break;
		case State.FINISHED:
			combatant.animator.SetBool("IsWalking", false);
			isDone = true;
			battleStateTracker.ResetChain();
			break;
		}
	}

	void WalkToDestination() {
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
				nextHop.SetOccupant(combatant);
				sourceTile.RemoveOccupant(combatant);
				return;
			}
			nextHop = tilePath[hopIndex];
		}
	}

	void CheckForBackButton() {
		if (Input.GetButton("Fire2")) {
			Debug.Log("backing out of move");
			if (tilePicker != null) {
				tilePicker.CleanUp();
			    Destroy(tilePicker.gameObject);
			}			
			battleStateTracker.GoBackOneStep();
		}
	}

	public void SetCombatant(Combatant combatant) {
		this.combatant = combatant;
		sourceTile = combatant.GetTile();
		MapManager map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		List<Tile> tiles = map.GetTilesInRange(combatant.GetTile(), 3, false);
		tiles = TileUtility.FilterOutOccupiedTiles(tiles);

		
		GameObject objToSpawn;
		objToSpawn = new GameObject("Tile Picker");
		objToSpawn.AddComponent<TilePicker>();
		tilePicker = objToSpawn.GetComponent<TilePicker>();
		tilePicker.SetTiles(tiles);
	}

	public void SetCallback(System.Action<int> method) {
		callbackMethod = method;
	}

	public bool IsDone() {
		return isDone;
	}
}
