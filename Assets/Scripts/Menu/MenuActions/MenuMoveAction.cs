using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuMoveAction : MenuAction {
 
	Menu parentMenu;

	public Combatant combatant;

	public override void Execute ()	{
		parentMenu.gameObject.SetActive(false);
		Debug.Log("Move! - " + combatant);

		BattleOrder order = new BattleOrder();
		order.Action = "move";
		order.SourceCombatant = combatant;
			
		MapManager map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		//List<Tile> tiles = map.GetTilesInRange(combatant.Tile, combatant.Stats.Movement, false);
		List<Tile> tiles = map.GetTilesInRangeThreadsafe(combatant.Tile.TileData, combatant.Stats.Movement).ConvertAll(t => t.Tile);
		tiles = TileUtility.FilterOutOccupiedTiles(tiles);		

		// TODO also maybe cache tilepicker results 

		GameObject objToSpawn = new GameObject("Tile Picker - Move");
		objToSpawn.AddComponent<TilePicker>();
		TilePicker tilePicker = objToSpawn.GetComponent<TilePicker>();
		tilePicker.SetTileParamsMove(combatant);
		//tilePicker.SetTiles(tiles);
		tilePicker.battleStateTracker.previous = this.parentMenu.battleStateTracker;
		tilePicker.SetBattleOrder(order);
				
		parentMenu.CleanUp();
	}

	public void SetMenu(Menu menu) {
		this.parentMenu = menu;
	}
}
