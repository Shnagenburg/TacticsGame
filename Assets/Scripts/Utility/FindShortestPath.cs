using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindShortestPath {
	
	static public List<Tile> Find(TileMap tileMap, Tile sourceTile, Tile destinationTile) {
		return Find (tileMap, sourceTile, destinationTile, false);
	}
	static public List<Tile> Find(TileMap tileMap, Tile sourceTile, Tile destinationTile, bool moveThroughOccupied) {
		List<Tile> shortestPath = new List<Tile>();

		Dictionary<Tile, int> tileToDistance = new Dictionary<Tile, int>();
		Dictionary<Tile, Tile> tileToOptimalPrevious = new Dictionary<Tile, Tile>();
		List<Tile> unvisited = new List<Tile>(tileMap.GetTiles());
		if (!moveThroughOccupied) {
		    unvisited = TileUtility.FilterOutOccupiedTiles(unvisited);
		    unvisited.Add(sourceTile);
		}

		tileToDistance.Add(sourceTile, 0);

		Tile currentTile = null;
		int count = 0;
		while (unvisited.Count > 0 && count < 100) {
			count++;
			currentTile = TileWithMinDistance(tileToDistance, unvisited);
			unvisited.Remove(currentTile);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.TopNeighbor(currentTile), tileToDistance, tileToOptimalPrevious, moveThroughOccupied);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.BottomNeighbor(currentTile), tileToDistance, tileToOptimalPrevious, moveThroughOccupied);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.LeftNeighbor(currentTile), tileToDistance, tileToOptimalPrevious, moveThroughOccupied);
			CalculateDistanceAndUpdatePath(currentTile, tileMap.RightNeighbor(currentTile), tileToDistance, tileToOptimalPrevious, moveThroughOccupied);
		}
		
		Debug.Log("starting return path - " + count);
			
		Tile returnTile = destinationTile;
		shortestPath.Add(returnTile);
		count = 0;
		while (returnTile != sourceTile && count < 100) {
			count++;
			Debug.Log("finding last hop for " + returnTile.TileData);
			returnTile = tileToOptimalPrevious[returnTile];
			Debug.Log("found " + returnTile.LocationString());
			shortestPath.Add(returnTile);
		}
		Debug.Log("finish return path");
		shortestPath.Reverse();
		return shortestPath;
	}

	static public void CalculateDistanceAndUpdatePath(Tile currentTile, Tile neighbor, 
	                                                  Dictionary<Tile, int> tileToDistance, Dictionary<Tile, Tile> tileToOptimalPrevious, 
	                                                  bool moveThroughOccupied) {

		if (neighbor == null || (neighbor.IsOccupied() && !moveThroughOccupied)) {
			return;
		}
		int totalDistance = tileToDistance[currentTile] + 1;
		if (!tileToDistance.ContainsKey(neighbor)) {
			tileToDistance.Add(neighbor, totalDistance);
			tileToOptimalPrevious[neighbor] = currentTile;
		} else if (totalDistance < tileToDistance[neighbor]) {
			tileToDistance[neighbor] = totalDistance;
			tileToOptimalPrevious[neighbor] = currentTile;
		}
	}

	static public Tile TileWithMinDistance(Dictionary<Tile, int> tileToDistance, List<Tile> unvisited) {
		KeyValuePair<Tile, int> minimum = new KeyValuePair<Tile, int>(null, int.MaxValue);
		foreach (KeyValuePair<Tile, int> entry in tileToDistance) {
			if (entry.Value < minimum.Value && unvisited.Contains(entry.Key)) {
				minimum = entry;
			}
		}
		Debug.Log("tile with min distance is " + minimum.Key.TileData);
		return minimum.Key;
	}

}
