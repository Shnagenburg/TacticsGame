using UnityEngine;
using System.Collections;

public class MenuMoveAction : MenuAction {
 
	Menu parentMenu;

	public Combatant combatant;

	public override void Execute ()	{
		parentMenu.gameObject.SetActive(false);
		Debug.Log("Move! - " + combatant);
		GameObject objToSpawn;
		objToSpawn = new GameObject("Move Combatant Action");
		objToSpawn.AddComponent<MoveCombatant>();
		objToSpawn.GetComponent<MoveCombatant>().SetCombatant(combatant);
		objToSpawn.GetComponent<MoveCombatant>().battleStateTracker.previous = parentMenu.battleStateTracker;
	}

	public void SetMenu(Menu menu) {
		this.parentMenu = menu;
	}
}
