using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ThreadedCalculateAIOrder {

	private bool isDone = false;
	public bool IsDone { 
		get { return isDone; }
	}
	public BattleOrder BattleOrder { get; set; }
	private Combatant combatant;
	private MapManager map;
	private Dictionary<Combatant, int> targetToDistance;
	 
	private object handle = new object();
	private System.Threading.Thread thread = null;

	public ThreadedCalculateAIOrder(Combatant combatant, MapManager map) {
		this.combatant = combatant;
		this.map = map;
		thread = new System.Threading.Thread(Calculate);
		thread.Start();
	}

	public void Calculate() {


		List<Combatant> targets = BattleDriver.CurrentBattleDriver.Combatants;
		targets = targets.Where(t => t.TeamId == TeamId.PlayerTeam).ToList();
		targetToDistance = targets.ToDictionary(t => t, t => TilesAway(combatant, t));
		KeyValuePair<Combatant, int> nearestTarget = targetToDistance.OrderBy(kp => kp.Value).ToList()[0];
		Debug.Log("marybutt");
		if (nearestTarget.Value > 1) {
			// Move to player
			Debug.Log("sp between " + combatant.Tile.LocationString() + " " + nearestTarget.Key.Tile.LocationString());

			//List<Tile> path = map.GetShortestPath(combatant.Tile, nearestTarget.Key.Tile);			
			List<TileData> path = map.GetShortestPathThreadsafe(combatant.Tile.TileData, nearestTarget.Key.Tile.TileData, TeamId.PlayerTeam);
			Debug.Log("done!");
			path.ForEach(p => Debug.Log(p.ToString()));

			BattleOrder = new BattleOrder();
			BattleOrder.SourceCombatant = combatant;
			BattleOrder.Action = "move";
			BattleOrder.TargetTile = path[1].Tile;

		} else {
			// attack player	
			BattleOrder = new BattleOrder();
			BattleOrder.SourceCombatant = combatant;
			BattleOrder.Action = "attack";
			BattleOrder.TargetTile = nearestTarget.Key.Tile;
		}

		isDone = true;

	}

	int TilesAway(Combatant source, Combatant target) {
		return Mathf.Abs(source.Tile.GetRow() - target.Tile.GetRow()) 
			+ Mathf.Abs(source.Tile.GetColumn() - target.Tile.GetColumn());
	}
}
