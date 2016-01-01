using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleOrder {

	public Combatant SourceCombatant { get; set; }
	public Tile TargetTile { get; set; }
	public string Action { get; set; }
	public List<TileData> PrecalculatedPath { get; set; }


}
