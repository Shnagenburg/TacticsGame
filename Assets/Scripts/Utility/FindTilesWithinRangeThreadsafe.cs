using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindTilesWithinRangeThreadsafe : MonoBehaviour {

	static public List<TileData> Find(TileMap tileMap, TileData sourceTile, int movement, int jumpHeight) {
		return Find(tileMap, sourceTile, movement, jumpHeight, TeamId.MOVE_THROUGH_NONE);
	}
	
	static public List<TileData> Find(TileMap tileMap, TileData sourceTile, int movement, int jumpHeight, int moveThroughMask) {
		FindTilesWithinRangeDTO dto = new FindTilesWithinRangeDTO();
		dto.JumpHeight = jumpHeight;
		dto.MoveThroughMask = moveThroughMask;
		movement = movement + 1;
		
		List<TileData> visited = new List<TileData>();
		FindHelper(tileMap, sourceTile, movement, dto);
		foreach(TileData t in dto.TileToMaxMovement.Keys) {
			visited.Add(t);
		}
		return visited;
	}
	
	static public void FindHelper(TileMap tileMap, TileData sourceTile, int movement, FindTilesWithinRangeDTO dto) {
		if (!dto.TileToMaxMovement.ContainsKey(sourceTile)) {
			dto.TileToMaxMovement[sourceTile] = movement;
		} else {
			int maxMove = dto.TileToMaxMovement[sourceTile];
			if (movement > maxMove) {
				dto.TileToMaxMovement[sourceTile] = movement;
			} else {
				return;
			}
		}
		//if (visited.Contains(sourceTile)) {
		//	return;
		//}
		//visited.Add(sourceTile);
		movement = movement - sourceTile.MovementCost;
		if (movement <= 0) {
			return;
		}
		if (IsTraversable(tileMap.BottomNeighbor(sourceTile), dto)) {
			FindHelper(tileMap, tileMap.BottomNeighbor(sourceTile), movement, dto);
		}
		if (IsTraversable(tileMap.TopNeighbor(sourceTile), dto)) {
			FindHelper(tileMap, tileMap.TopNeighbor(sourceTile), movement, dto);
		}
		if (IsTraversable(tileMap.LeftNeighbor(sourceTile), dto)) {
			FindHelper(tileMap, tileMap.LeftNeighbor(sourceTile), movement, dto);
		}
		if (IsTraversable(tileMap.RightNeighbor(sourceTile), dto)) {
			FindHelper(tileMap, tileMap.RightNeighbor(sourceTile), movement, dto);
		}
	}
	
	static public bool IsTraversable(TileData tileData, FindTilesWithinRangeDTO dto) {
		if (tileData == null) {
			return false;
		}
		if (tileData.OccupiedTeam == dto.MoveThroughMask) {
			return false;
		}
		if (dto.MoveThroughMask == TeamId.MOVE_THROUGH_NONE && tileData.OccupiedTeam != -1) { 
			return false;
		}
		return true;
	}
}
