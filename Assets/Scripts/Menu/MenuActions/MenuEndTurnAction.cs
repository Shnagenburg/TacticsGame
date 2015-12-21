using UnityEngine;
using System.Collections;

public class MenuEndTurnAction : MenuAction {

	public Menu parentMenu;

	public override void Execute () {
		Debug.Log("end turn!");
		BattleOrder order = new BattleOrder();
		order.SourceCombatant = parentMenu.Combatant;
		order.Action = "endturn";
		
		GameObject enactor = new GameObject("BattleOrderEnactor Action");
		enactor.AddComponent<BattleOrderEnactor>();
		enactor.GetComponent<BattleOrderEnactor>().battleStateTracker.previous = this.parentMenu.battleStateTracker;
		enactor.GetComponent<BattleOrderEnactor>().Enact(order);
		
		parentMenu.CleanUp();
	}
}
