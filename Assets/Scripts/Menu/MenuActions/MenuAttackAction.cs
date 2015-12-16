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
		//List<Tile> tiles = map.GetTilesInRange(combatant.Tile, 1, true);
		List<Tile> tiles = map.GetTilesInRangeThreadsafe(combatant.Tile.TileData, 1, TeamId.MOVE_THROUGH_ALL)
			.ConvertAll(t => t.Tile);
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
