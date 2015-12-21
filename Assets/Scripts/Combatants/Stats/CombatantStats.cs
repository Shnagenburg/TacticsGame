using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatantStats {

	public string Name { get; set; }
	public int CurrentHealth { get; set; }
	public int MaxHealth { get; set; }
	public int AttackPower { get; set; }
	public int Accuracy { get; set; }
	public int Movement { get; set; }
	public List<StatusEffect> ActiveEffects {get; set;}
	public int AttacksPerTurn { get; set; }
	public int MovesPerTurn { get; set; }
	public TurnStats TurnStats { get; set; }

	public CombatantStats() {
		CurrentHealth = 100;
		MaxHealth = 100;
		AttackPower = 3;
		Accuracy = 95;
		Movement = 2;
		AttacksPerTurn = 1;
		MovesPerTurn = 1;
		ActiveEffects = new List<StatusEffect>();
		TurnStats = new TurnStats();
	}

	public bool IsDead() {
		return CurrentHealth <= 0;
	}

	public bool HasStatus(string statusName) {
		foreach (StatusEffect effect in ActiveEffects) {
			if (statusName.Equals(effect.Name)) {
				return true;
			}
		}
		return false;
	}

	public bool IsInAttackRange(TileData source, TileData target) {
		Debug.Log("taway " + TilesAway(source, target));
		return TilesAway(source, target) == 1;
	}

	static private int TilesAway(TileData source, TileData target) {
		return Mathf.Abs(source.Row - target.Row) + Mathf.Abs(source.Column - target.Column);
	}
}
