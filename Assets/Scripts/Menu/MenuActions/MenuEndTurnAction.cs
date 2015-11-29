using UnityEngine;
using System.Collections;

public class MenuEndTurnAction : MenuAction {

	public Menu parentMenu;

	public override void Execute () {
		Debug.Log("end turn!");
		parentMenu.battleStateTracker.ResetChain();
	}
}
