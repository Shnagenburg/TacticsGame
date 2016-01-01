using UnityEngine;
using System.Collections;

public class TurnStats {

	public int MovesLeft { get; set; }
	public int AttacksLeft { get; set; }
	public AITurnPlan AITurnPlan { get; set; }
	CombatantStats combatantStats = null;

	public TurnStats() {
		MovesLeft = 0;
		AttacksLeft = 0;
		this.AITurnPlan = null;
	}

	public void Reset(CombatantStats stats) {
		this.combatantStats = stats;
		this.MovesLeft = stats.MovesPerTurn;
		this.AttacksLeft = stats.AttacksPerTurn;
		this.AITurnPlan = null;
	}

	public bool CanDoSomething() {
		if (MovesLeft > 0 || AttacksLeft > 0) {
			return true;
		}
		return false;
	}

	public bool CanMove() {
		return MovesLeft > 0;
	}

	public bool CanAttack() {
		return AttacksLeft > 0;
	}

	public void ConsumeMovement() {
		this.MovesLeft = this.MovesLeft - 1;
	}

	public void ConsumeAttack() {
		this.AttacksLeft = this.AttacksLeft - 1;
	}

	public bool HasDoneAnything() {
		if (this.MovesLeft == combatantStats.MovesPerTurn && 
		    this.AttacksLeft == combatantStats.AttacksPerTurn) {
			return false;
		}
		return true;
	}

	public void EndTurn() {
		MovesLeft = 0;
		AttacksLeft = 0;
	}

	public bool HasAITurnPlan() {
		return this.AITurnPlan != null;
	}

	public string ToString() { 
		return "Atks: " + AttacksLeft + " Moves: " + MovesLeft;
	}

}
