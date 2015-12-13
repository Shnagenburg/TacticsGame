using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatantStats {
	
	public int CurrentHealth { get; set; }
	public int MaxHealth { get; set; }
	public int AttackPower { get; set; }
	public int Accuracy { get; set; }
	public int Movement { get; set; }
	public List<StatusEffect> ActiveEffects {get; set;}

	public CombatantStats() {
		CurrentHealth = 100;
		MaxHealth = 100;
		AttackPower = 3;
		Accuracy = 95;
		Movement = 3;
		ActiveEffects = new List<StatusEffect>();
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
}
