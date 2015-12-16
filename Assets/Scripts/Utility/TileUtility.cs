using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileUtility {

	static public List<Tile> FilterOutOccupiedTiles(List<Tile> tiles) {
		tiles.RemoveAll((Tile obj) => obj.IsOccupied());
		return tiles;
	}

	static public List<TileData> FilterOutOccupiedTiles(List<TileData> tiles) {
		tiles.RemoveAll((TileData obj) => obj.OccupiedTeam != TeamId.NoOccupant);
		return tiles;
	}

	static public List<TileData> FilterOutOccupiedTiles(List<TileData> tiles, int teamId) {
		if (teamId == TeamId.MOVE_THROUGH_NONE) {
			foreach (TileData t in tiles) {
				if (t.OccupiedTeam != -1) {
					Debug.Log("filtering out " + t.ToString());
				}
			}

			tiles.RemoveAll((TileData obj) => obj.OccupiedTeam != -1);
		} else {
			tiles.RemoveAll((TileData obj) => obj.OccupiedTeam == teamId);
		}
		return tiles;
	}

}
