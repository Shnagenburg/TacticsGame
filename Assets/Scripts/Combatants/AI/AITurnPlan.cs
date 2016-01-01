using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AITurnPlan {

	public enum Plan {
		MOVE_AND_ATTACK,
		ATTACK_AND_BACK_OFF,
		MOVE_AND_END_TURN,
		ATTACK_AND_END_TURN,
		END_TURN
	}
	public Plan TurnPlan { get; set; }
	public TileData Destination { get; set; }
	public TileData Target { get; set; }
	public MapManager map;
	Combatant combatant;
	int actionsTaken = 0;

	public AITurnPlan(Combatant combatant) {
		this.combatant = combatant;
	}

	public BattleOrder CreateNextBattleOrder() {
		actionsTaken = actionsTaken + 1;
		BattleOrder battleOrder = new BattleOrder();
		battleOrder.SourceCombatant = combatant;

		switch (this.TurnPlan) {
		case Plan.MOVE_AND_ATTACK:
			if (actionsTaken == 1) {
				battleOrder.Action = "move";
				battleOrder.TargetTile = FurthestValidTile(this.Destination).Tile;
			} else if (actionsTaken == 2) {
				battleOrder.Action = "attack";
				battleOrder.TargetTile = this.Target.Tile;
			}
			return battleOrder;
		case Plan.ATTACK_AND_BACK_OFF:
			if (actionsTaken == 1) {
				battleOrder.Action = "attack";
				battleOrder.TargetTile = this.Target.Tile;
			} else if (actionsTaken == 2) {
				battleOrder.Action = "endturn";
			}
			return battleOrder;
		case Plan.MOVE_AND_END_TURN:
			if (actionsTaken == 1) {
				battleOrder.Action = "move";
				battleOrder.TargetTile = FurthestValidTile(this.Destination).Tile;
			} else if (actionsTaken == 2) {
				battleOrder.Action = "endturn";
			}
			return battleOrder;
		case Plan.ATTACK_AND_END_TURN:
			if (actionsTaken == 1) {
				battleOrder.Action = "attack";
				battleOrder.TargetTile = this.Target.Tile;
			} else if (actionsTaken == 2) {
				battleOrder.Action = "endturn";
			}
			return battleOrder;
		case Plan.END_TURN:
			battleOrder.Action = "endturn";
			return battleOrder;
		default:
			return null;
		}
	}

	AITurnPlan WithParams(TileData attackTarget, TileData moveDestination) {
		this.Target = attackTarget;
		this.Destination = moveDestination;
		return this;
	}

	public AITurnPlan AttackAndBackOff(TileData attackTarget, TileData moveDestination) {
		WithParams(attackTarget, moveDestination);
		this.TurnPlan = Plan.ATTACK_AND_BACK_OFF;
		return this;
	}

	public AITurnPlan MoveAndAttack(TileData attackTarget, TileData moveDestination) {
		WithParams(attackTarget, moveDestination);
		this.TurnPlan = Plan.MOVE_AND_ATTACK;
		return this;
	}

	public AITurnPlan MoveAndEndTurn(TileData moveDestination) {
		WithParams(null, moveDestination);
		this.TurnPlan = Plan.MOVE_AND_END_TURN;
		return this;
	}

	public AITurnPlan AttackAndEndTurn(TileData attackTarget) {
		WithParams(attackTarget, null);
		this.TurnPlan = Plan.ATTACK_AND_END_TURN;
		return this;
	}

	public AITurnPlan EndTurn() {
		WithParams(null, null);
		this.TurnPlan = Plan.END_TURN;
		return this;
	}

	TileData FurthestValidTile(TileData destination) {
		TileData source = combatant.Tile.TileData;
		int movement = combatant.Stats.Movement;
		List<TileData> path = map.GetShortestPathThreadsafe(source, destination, TeamId.PlayerTeam);
		
		// TODO figure out how far along the path guy can go
		// If we can make the whole trek
		if (path.Count <= movement) {
			return path[path.Count - 1];				
		} else {
			// If for some reason we cannot, or go as far as you can
			return path[movement];		
		}
	}


}
