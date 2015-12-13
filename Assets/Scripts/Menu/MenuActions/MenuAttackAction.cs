using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuAttackAction : MenuAction {
		
	public Menu parentMenu;
	public Combatant combatant;

	public override void Execute () {
		parentMenu.gameObject.SetActive(false);
		Debug.Log("attack! - " + combatant);

		BattleOrder order = new BattleOrder();
		order.Action = "attack";
		order.SourceCombatant = combatant;
		
		MapManager map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		List<Tile> tiles = map.GetTilesInRange(combatant.Tile, 1, true);
		tiles.Remove(combatant.Tile);
		
		GameObject objToSpawn = new GameObject("Tile Picker - Attack");
		objToSpawn.AddComponent<TilePicker>();
		TilePicker tilePicker = objToSpawn.GetComponent<TilePicker>();
		tilePicker.SetTiles(tiles);
		tilePicker.battleStateTracker.previous = this.parentMenu.battleStateTracker;
		tilePicker.SetBattleOrder(order);

		parentMenu.CleanUp();
	}
}
