using UnityEngine;
using System.Collections;

public class MenuAttackAction : MenuAction {
		
	public Menu parentMenu;
	public Combatant combatant;

	public override void Execute () {
		parentMenu.gameObject.SetActive(false);
		Debug.Log("attack! - " + combatant);
		GameObject objToSpawn = new GameObject("Attack Combatant Action");
		objToSpawn.AddComponent<AttackCombatant>();
		objToSpawn.GetComponent<AttackCombatant>().SetCombatant(combatant);
		objToSpawn.GetComponent<AttackCombatant>().battleStateTracker.previous = parentMenu.battleStateTracker;
	}
}
