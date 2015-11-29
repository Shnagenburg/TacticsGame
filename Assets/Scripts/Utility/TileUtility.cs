using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileUtility {

	static public List<Tile> FilterOutOccupiedTiles(List<Tile> tiles) {
		List<Tile> toRemove = new List<Tile>();
		tiles.RemoveAll((Tile obj) => obj.IsOccupied());
		return tiles;
	}

}
