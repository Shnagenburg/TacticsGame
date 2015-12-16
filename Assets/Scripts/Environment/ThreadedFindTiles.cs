using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadedFindTiles {

	private bool isDone = false;
	public bool IsDone { 
		get { return isDone; }
	}
	public List<TileData> TileResults {get; set;}
	private object handle = new object();
	private System.Threading.Thread thread = null;

	private Combatant combatant;
	private MapManager map;




	public void StartThreadToFindTilesInRange(Combatant combatant, MapManager map) {
		this.combatant = combatant;
		this.map = map;
		thread = new System.Threading.Thread(FindTilesInRange);
		thread.Start();

	}

	private void FindTilesInRange() {
		this.TileResults = map.GetTilesInRangeThreadsafe(combatant.Tile.TileData, combatant.Stats.Movement);
		this.TileResults = TileUtility.FilterOutOccupiedTiles(this.TileResults);	
		this.isDone = true;
	}

	public void StartThreadToFindShortestPath() {
		throw new UnityException("not implemented");
	}

	public void JoinThread() {
		if (thread == null) {
			throw new UnityException("thread not created yet");
		}
		thread.Join();
	}
}
