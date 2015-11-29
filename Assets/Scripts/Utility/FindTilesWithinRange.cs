using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindTilesWithinRange {

	
	static public List<Tile> Find(TileMap tileMap, Tile sourceTile, int movement, int jumpHeight) {
		return Find(tileMap, sourceTile, movement, jumpHeight, false);
	}

	static public List<Tile> Find(TileMap tileMap, Tile sourceTile, int movement, int jumpHeight, bool moveThroughOccupiedTiles) {
		movement = movement + 1;

		List<Tile> visited = new List<Tile>();
		Dictionary<Tile, int> tileToMaxMovement = new Dictionary<Tile, int>();
		FindHelper(tileMap, sourceTile, movement, jumpHeight, tileToMaxMovement, moveThroughOccupiedTiles);
		foreach(Tile t in tileToMaxMovement.Keys) {
			visited.Add(t);
		}
		return visited;
	}

	static public void FindHelper(TileMap tileMap, Tile sourceTile, int movement, int jumpHeight, Dictionary<Tile, int> tileToMaxMovement,
	                              bool moveThroughOccupiedTiles) {
		if (!tileToMaxMovement.ContainsKey(sourceTile)) {
			tileToMaxMovement[sourceTile] = movement;
		} else {
			int maxMove = tileToMaxMovement[sourceTile];
			if (movement > maxMove) {
				tileToMaxMovement[sourceTile] = movement;
			} else {
				return;
			}
		}
		//if (visited.Contains(sourceTile)) {
		//	return;
		//}
		//visited.Add(sourceTile);
		movement = movement - sourceTile.GetMovementCost();
		if (movement <= 0) {
			return;
		}
		if (IsTraversable(tileMap.BottomNeighbor(sourceTile), moveThroughOccupiedTiles)) {
			FindHelper(tileMap, tileMap.BottomNeighbor(sourceTile), movement, jumpHeight, tileToMaxMovement, moveThroughOccupiedTiles);
		}
		if (IsTraversable(tileMap.TopNeighbor(sourceTile), moveThroughOccupiedTiles)) {
			FindHelper(tileMap, tileMap.TopNeighbor(sourceTile), movement, jumpHeight, tileToMaxMovement, moveThroughOccupiedTiles);
		}
		if (IsTraversable(tileMap.LeftNeighbor(sourceTile), moveThroughOccupiedTiles)) {
		    FindHelper(tileMap, tileMap.LeftNeighbor(sourceTile), movement, jumpHeight, tileToMaxMovement, moveThroughOccupiedTiles);
		}
		if (IsTraversable(tileMap.RightNeighbor(sourceTile), moveThroughOccupiedTiles)) {
			FindHelper(tileMap, tileMap.RightNeighbor(sourceTile), movement, jumpHeight, tileToMaxMovement, moveThroughOccupiedTiles);
		}
	}

	static public bool IsTraversable(Tile tile, bool moveThroughOccupiedTiles) {
		return (tile != null && ( !tile.IsOccupied() || moveThroughOccupiedTiles));
	}

}
