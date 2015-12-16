using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FindShortestPathThreadsafe {

	static public List<TileData> Find(TileMap tileMap, TileData sourceTile, TileData destinationTile) {
		return Find (tileMap, sourceTile, destinationTile, TeamId.MOVE_THROUGH_NONE);
	}
	static public List<TileData> Find(TileMap tileMap, TileData sourceTile, TileData destinationTile, int moveThroughMask) {
		List<TileData> shortestPath = new List<TileData>();
		List<TileData> unvisited = new List<TileData>(tileMap.TileDataList);

		FindShorestPathDTO dto = new FindShorestPathDTO();
		dto.Destination = destinationTile;
		dto.MoveThroughMask = moveThroughMask;
		if (moveThroughMask != TeamId.MOVE_THROUGH_ALL) {
			unvisited = TileUtility.FilterOutOccupiedTiles(unvisited, moveThroughMask); // TODO support enemyteam
			unvisited.Add(sourceTile);
		}

		dto.TileToDistance.Add(sourceTile, 0);
		TileData currentTile = null;
		int count = 0;
		while (unvisited.Count > 0 && count < 100) {
			count++;
			currentTile = TileWithMinDistance(dto.TileToDistance, unvisited);
			unvisited.Remove(currentTile);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.TopNeighbor(currentTile), dto);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.BottomNeighbor(currentTile), dto);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.LeftNeighbor(currentTile), dto);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.RightNeighbor(currentTile), dto);
		}
		
		Debug.Log("starting return path - " + count);
		
		TileData returnTile = destinationTile;
		shortestPath.Add(returnTile);
		count = 0;
		while (returnTile != sourceTile && count < 100) {
			count++;
			Debug.Log("finding last hop for " + returnTile.ToString());
			try {
			returnTile = dto.TileToOptimalPrevious[returnTile];
			} catch (Exception ex) {
				Debug.LogError(ex);
				Debug.LogError("oh shit error couldn't find " + returnTile.ToString());
			}
			Debug.Log("found " + returnTile);
			shortestPath.Add(returnTile);
		}
		Debug.Log("finish return path - TILEDATA");
		shortestPath.Reverse();
		return shortestPath;
	}
	
	static public void CalculateDistanceAndUpdatePath(TileData currentTile, TileData neighbor, FindShorestPathDTO dto) {		
		if (neighbor == null) {
			return;
		}
		if (neighbor.OccupiedTeam == dto.MoveThroughMask && neighbor != dto.Destination) {
			return;
		}
		// TODO i dont understand why it cant be the destination tile
		if (dto.MoveThroughMask == TeamId.MOVE_THROUGH_NONE && neighbor.OccupiedTeam != -1 && neighbor != dto.Destination) { 
			return;
		}
		Dictionary<TileData, int> tileToDistance = dto.TileToDistance;
		int totalDistance = tileToDistance[currentTile] + 1;
		if (!tileToDistance.ContainsKey(neighbor)) {
			tileToDistance.Add(neighbor, totalDistance);
			dto.TileToOptimalPrevious[neighbor] = currentTile;
		} else if (totalDistance < tileToDistance[neighbor]) {
			tileToDistance[neighbor] = totalDistance;
			dto.TileToOptimalPrevious[neighbor] = currentTile;
		}
	}
	
	static public TileData TileWithMinDistance(Dictionary<TileData, int> tileToDistance, List<TileData> unvisited) {
		KeyValuePair<TileData, int> minimum = new KeyValuePair<TileData, int>(null, int.MaxValue);
		foreach (KeyValuePair<TileData, int> entry in tileToDistance) {
			if (entry.Value < minimum.Value && unvisited.Contains(entry.Key)) {
				minimum = entry;
			}
		}
		Debug.Log("tile with min distance is " + minimum.Key);
		return minimum.Key;
	}
}
