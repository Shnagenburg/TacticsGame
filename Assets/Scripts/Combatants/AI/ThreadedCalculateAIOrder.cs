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
		Debug.Log("Creating AI Order...");
		if (!combatant.Stats.TurnStats.HasAITurnPlan() ) {
			Debug.Log("Creating turn plan...");
			combatant.Stats.TurnStats.AITurnPlan = CreateTurnPlan();
			combatant.Stats.TurnStats.AITurnPlan.map = map;			
			Debug.Log("finished creating turn plan: " + combatant.Stats.TurnStats.AITurnPlan.TurnPlan);
		}
		Debug.Log("Onto exection of plan...");

		// Execute Plan
		this.BattleOrder = combatant.Stats.TurnStats.AITurnPlan.CreateNextBattleOrder();
		isDone = true;
	}

	private Dictionary<Combatant, TileData> FindTargetsInMoveRangeToAttack() {
		int movement = combatant.Stats.Movement;
		TileData sourceTileData = combatant.Tile.TileData;
		List<Combatant> targets = BattleDriver.CurrentBattleDriver.Combatants;
		targets = targets.Where(t => t.TeamId == TeamId.PlayerTeam).ToList();
		
		List<Combatant> reachableCombatants = new List<Combatant>();
		Dictionary<Combatant, TileData> combatantToAdjacentTile = new Dictionary<Combatant, TileData>();
		foreach (Combatant target in targets) {
			// Find all adjacent unoccupied tiles to these bad guys
			List<TileData> adjacent = map.TileMap.GetNeighbors(target.Tile.TileData);
			adjacent = adjacent.Where(t => t.OccupiedTeam == TeamId.NoOccupant).ToList();
			
			// Figure out all options we can get to in this turn
			List<TileData> reachableTiles = adjacent.Where(t => TilesAway(sourceTileData, t) <= movement)
				.OrderBy(t => TilesAway(sourceTileData, t)).ToList();
			if (reachableTiles.Count > 0) {
				// TODO check if there is a valid path to these tiles
				combatantToAdjacentTile.Add(target, reachableTiles[0]);
			}
		}
		return combatantToAdjacentTile;
	}

	private Dictionary<Combatant, TileData> FindTargetsByClosest() {
		int movement = combatant.Stats.Movement;
		TileData sourceTileData = combatant.Tile.TileData;
		List<Combatant> targets = BattleDriver.CurrentBattleDriver.Combatants;
		targets = targets.Where(t => t.TeamId == TeamId.PlayerTeam).ToList();
		
		List<Combatant> reachableCombatants = new List<Combatant>();
		Dictionary<Combatant, TileData> combatantToAdjacentTile = new Dictionary<Combatant, TileData>();
		foreach (Combatant target in targets) {
			// Find all adjacent unoccupied tiles to these bad guys
			List<TileData> adjacent = map.TileMap.GetNeighbors(target.Tile.TileData);
			adjacent = adjacent.Where(t => t.OccupiedTeam == TeamId.NoOccupant).ToList();
			
			// Figure out all options we can get to in this turn
			List<TileData> reachableTiles = adjacent.OrderBy(t => TilesAway(sourceTileData, t)).ToList();
			if (reachableTiles.Count > 0) {
				// TODO check if there is a valid path to these tiles
				combatantToAdjacentTile.Add(target, reachableTiles[0]);
			}
		}
		return combatantToAdjacentTile;
	}

	private Combatant FindBestTargetToHit(List<Combatant> targets) {
		Combatant primeTarget = null;
		// figure out who we want to hit (lowest hp)
		foreach (Combatant target in targets) {
			if (primeTarget == null) {
				primeTarget = target;
			} else if (primeTarget.Stats.CurrentHealth > target.Stats.CurrentHealth) {
				primeTarget = target;
			}
		}
		return primeTarget;
	}

	private AITurnPlan CreateTurnPlan() {
		TileData sourceTileData = combatant.Tile.TileData;
		AITurnPlan result = new AITurnPlan(combatant);
		// Formulate Plan
		
		// Can we hit anyone without moving? If so, mark them for attack
		// Then if possible, retreat backwards
		List<Combatant> targets = BattleDriver.CurrentBattleDriver.Combatants;
		targets = targets.Where(t => t.TeamId == TeamId.PlayerTeam) //
			.Where(t => combatant.Stats.IsInAttackRange(sourceTileData, t.Tile.TileData)).ToList();
		if (targets.Count > 0) {
			Combatant preferredTarget = FindBestTargetToHit(targets);
			return result.AttackAndEndTurn(preferredTarget.Tile.TileData); // TODO change to attack and back off
		}
		
		// Can we hit anyone this turn by moving? If so, mark them as our target and move
		Dictionary<Combatant, TileData> targetToTiledata = FindTargetsInMoveRangeToAttack();
		if (targetToTiledata.Count > 0) {
			Combatant preferredCloseTarget = FindBestTargetToHit(targetToTiledata.Keys.ToList());
			return result.MoveAndAttack(preferredCloseTarget.Tile.TileData, targetToTiledata[preferredCloseTarget]);
		}				

		// Who is the closest target we can't reach? Move towards them and end turn
		targetToTiledata = FindTargetsByClosest();
		if (targetToTiledata.Count > 0) {
			Combatant preferredCloseTarget = FindBestTargetToHit(targetToTiledata.Keys.ToList());
			// TODO check if path exists
			List<TileData> path = map.GetShortestPathThreadsafe(combatant.Tile.TileData, 
			                                                    targetToTiledata[preferredCloseTarget], TeamId.PlayerTeam);
			TileData destination = null;
			// Find how far along the path we can move
			for (int i = combatant.Stats.Movement; i > 0; i--) {
				if (path[i].OccupiedTeam == TeamId.NoOccupant) {
					destination = path[i];
					break;
				}
			}
			if (destination != null) {
				return result.MoveAndEndTurn(destination);
			}
		}

		// Nothing we can do
		return result.EndTurn();
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
