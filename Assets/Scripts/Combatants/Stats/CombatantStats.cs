using UnityEngine;
using System.Collections;

public class CombatantStats {
	
	public int CurrentHealth { get; set; }
	public int MaxHealth { get; set; }
	public int AttackPower { get; set; }
	public int Accuracy { get; set; }

	public CombatantStats() {
		CurrentHealth = 100;
		MaxHealth = 100;
		AttackPower = 65;
		Accuracy = 95;
	}

}
