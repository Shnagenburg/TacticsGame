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
	private Dictionary<TileData, int> tileToDistance;
	 
	private object handle = new object();
	private System.Threading.Thread thread = null;

	public ThreadedCalculateAIOrder(Combatant combatant, MapManager map) {
		this.combatant = combatant;
		this.map = map;
		thread = new System.Threading.Thread(Calculate);
		thread.Start();
	}

	public void Calculate() {
		//List<Combatant> targets = BattleDriver.CurrentBattleDriver.Combatants;
		//targets = targets.Where(t => t.TeamId == TeamId.PlayerTeam).ToList();
		//targetToDistance = targets.ToDictionary(t => t, t => TilesAway(combatant, t));
		//KeyValuePair<Combatant, int> nearestTarget = targetToDistance.OrderBy(kp => kp.Value).ToList()[0];

		int movement = combatant.Stats.Movement;
		if (!combatant.Stats.TurnStats.CanMove() && combatant.Stats.TurnStats.CanAttack()) {
			KeyValuePair<Combatant, TileData> preferredTarget = FindTargetInRangeToAttack();

			if (preferredTarget.Key != null) {
				Debug.Log("att");
				// attack player	
				BattleOrder = new BattleOrder();
				BattleOrder.SourceCombatant = combatant;
				BattleOrder.Action = "attack";
				BattleOrder.TargetTile = preferredTarget.Key.Tile;
			} else {
				Debug.Log("end");
				// nothing to do	
				BattleOrder = new BattleOrder();
				BattleOrder.SourceCombatant = combatant;
				BattleOrder.Action = "endturn";
			}
			isDone = true;

			return;
		}
		KeyValuePair<Combatant, TileData> nearestTarget = FindClosetTargetToAttack();
		Combatant target = nearestTarget.Key;
		TileData destination = nearestTarget.Value;
		if (target != null && destination != null) {
			// Move to player
			Debug.Log("sp between " + combatant.Tile.LocationString() + " " + destination.Tile.LocationString());

			//List<Tile> path = map.GetShortestPath(combatant.Tile, nearestTarget.Key.Tile);			
			List<TileData> path = map.GetShortestPathThreadsafe(combatant.Tile.TileData, destination, TeamId.PlayerTeam);
			Debug.Log("done!");

			BattleOrder = new BattleOrder();
			BattleOrder.SourceCombatant = combatant;
			BattleOrder.Action = "move";

			// TODO figure out how far along the path guy can go
			// If we can make the whole trek
			if (path.Count <= movement) {
				BattleOrder.TargetTile = path[path.Count - 1].Tile;				
			} else {
				// If for some reason we cannot, or go as far as you can
				BattleOrder.TargetTile = path[movement].Tile;		
			}

		} else if (target != null && destination == null) {
			// attack player	
			BattleOrder = new BattleOrder();
			BattleOrder.SourceCombatant = combatant;
			BattleOrder.Action = "attack";
			BattleOrder.TargetTile = target.Tile;

		} else if (target == null && destination == null) {
			BattleOrder = new BattleOrder();
			BattleOrder.SourceCombatant = combatant;
			BattleOrder.Action = "endturn";
		} else {
			// attack player	
			BattleOrder = new BattleOrder();
			BattleOrder.SourceCombatant = combatant;
			BattleOrder.Action = "attack";
			BattleOrder.TargetTile = nearestTarget.Key.Tile;
		}

		isDone = true;

	}

	private KeyValuePair<Combatant, TileData> FindClosetTargetToAttack() {
		int movement = combatant.Stats.Movement;
		TileData sourceTileData = combatant.Tile.TileData;
		// TODO process:
		// Find all enemies guys
		List<Combatant> targets = BattleDriver.CurrentBattleDriver.Combatants;
		targets = targets.Where(t => t.TeamId == TeamId.PlayerTeam).ToList();

		List<Combatant> reachableCombatants = new List<Combatant>();
		Dictionary<Combatant, List<TileData>> combatantToAdjacentTiles = new Dictionary<Combatant, List<TileData>>();
		foreach (Combatant target in targets) {
			// Find all adjacent unoccupied tiles to these bad guys
			List<TileData> adjacent = map.TileMap.GetNeighbors(target.Tile.TileData);
			adjacent = adjacent.Where(t => t.OccupiedTeam == TeamId.NoOccupant).ToList();
			
			// Figure out all options we can get to in this turn
			List<TileData> reachableTiles = adjacent.Where(t => TilesAway(sourceTileData, t) <= movement).ToList();
			if (reachableTiles.Count > 0) {
				reachableCombatants.Add(target);
			}
			if (adjacent.Count > 0) {
				combatantToAdjacentTiles.Add(target, adjacent);
			}
		}

		if (combatantToAdjacentTiles.Count == 0) {
			Debug.Log("AI couldn't find anyone they could reach");
			return new KeyValuePair<Combatant, TileData>(null, null);
		}
		Combatant primeTarget = null;
		// figure out who we want to hit (lowest hp)
		foreach (Combatant target in reachableCombatants) {
		    if (primeTarget == null) {
				primeTarget = target;
			} else if (primeTarget.Stats.CurrentHealth > target.Stats.CurrentHealth) {
				primeTarget = target;
			}
		}
		// If we found someone we can hit this turn, go hit them.
		if (primeTarget != null) {
			Debug.Log("AI can reach this turn to attack: " + primeTarget.Stats.Name);
			TileData closetAdjacentTile = combatantToAdjacentTiles[primeTarget].OrderBy(t => TilesAway(sourceTileData, t)).ToList()[0];

			// If we don't need to move
			if (closetAdjacentTile == sourceTileData) {
				return new KeyValuePair<Combatant, TileData>(primeTarget, null);
			} else { 
				return new KeyValuePair<Combatant, TileData>(primeTarget, closetAdjacentTile);
			}
		} else {
			Debug.Log("AI couldn't find any reachable targets this turn");
			
			Combatant alternateTarget = null;
			// figure out who we want to hit (lowest hp)
			foreach (Combatant target in combatantToAdjacentTiles.Keys) {
				if (alternateTarget == null) {
					alternateTarget = target;
				} else if (alternateTarget.Stats.CurrentHealth > target.Stats.CurrentHealth) {
					alternateTarget = target;
				}
			}
			TileData closetAdjacentTile = combatantToAdjacentTiles[alternateTarget].OrderBy(t => TilesAway(sourceTileData, t)).ToList()[0];
			// If we don't need to move
			if (closetAdjacentTile == sourceTileData) {
				return new KeyValuePair<Combatant, TileData>(alternateTarget, null);
			} else { 
				return new KeyValuePair<Combatant, TileData>(alternateTarget, closetAdjacentTile);
			}
		}

		//targets.ForEach(t => map.TileMap.AddNeighbors(t.Tile.TileData, validTilesToStandOn)); // fixup, track neighbor sets

		// move into the closest space to hit said person

		// return this closet space and the person we want to hit

		//targetToDistance = targets.ToDictionary(t => t, t => TilesAway(combatant, t));

		//KeyValuePair<Combatant, int> nearestTarget = targetToDistance.OrderBy(kp => kp.Value).ToList()[0];

		//return nearestTarget;
		// TODO::future if adjacent ally, perhaps move further away to allow ally to get close?
	}

	private KeyValuePair<Combatant, TileData> FindTargetInRangeToAttack() {
		int movement = combatant.Stats.Movement;
		TileData sourceTileData = combatant.Tile.TileData;

		// Find all enemies guys
		List<Combatant> targets = BattleDriver.CurrentBattleDriver.Combatants;
		targets = targets.Where(t => t.TeamId == TeamId.PlayerTeam).ToList();
		Debug.Log("targets: " + targets.Count);

		List<Combatant> reachableCombatants = new List<Combatant>();
		reachableCombatants = targets.Where(t => combatant.Stats.IsInAttackRange(sourceTileData, t.Tile.TileData)).ToList();
		Debug.Log("reachable: " + reachableCombatants.Count);
		Combatant primeTarget = null;
		// figure out who we want to hit (lowest hp)
		foreach (Combatant target in reachableCombatants) {
			if (primeTarget == null) {
				primeTarget = target;
			} else if (primeTarget.Stats.CurrentHealth > target.Stats.CurrentHealth) {
				primeTarget = target;
			}
		}
		TileData targetTileData = null;
		if (primeTarget != null) {
			targetTileData = primeTarget.Tile.TileData; 
		}
		return new KeyValuePair<Combatant, TileData>(primeTarget, targetTileData);
	}

	private int TilesAway(Combatant source, Combatant target) {
		return Mathf.Abs(source.Tile.GetRow() - target.Tile.GetRow()) 
			+ Mathf.Abs(source.Tile.GetColumn() - target.Tile.GetColumn());
	}

	private int TilesAway(TileData source, TileData target) {
		return Mathf.Abs(source.Row - target.Row) + Mathf.Abs(source.Column - target.Column);
	}

	public void JoinThread() {
		thread.Join();
	}
}
