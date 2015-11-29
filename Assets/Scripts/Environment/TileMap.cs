using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap {
	
	Tile [,] tileMap;
	List<Tile> tileList;
	int width;
	int height;

	public TileMap(List<Tile> tiles, int width, int height) {
		this.tileList = tiles;
		this.width = width;
		this.height = height;
		BuildTileMap();
	}	
	
	void BuildTileMap() {
		tileMap = new Tile[width,height];
		Dictionary<int, List<Tile>> rowOfTilesMap = new Dictionary<int, List<Tile>>();
		Debug.Log(tileList.Count);
		foreach (Tile tile in tileList) {
			int z = Mathf.RoundToInt(tile.gameObject.transform.position.z);
			Debug.Log("z: " + z + " Tile: " + tile);
			if (!rowOfTilesMap.ContainsKey(z)) {
				Debug.Log("created row at " + z);
				rowOfTilesMap.Add(z, new List<Tile>());
			}			
			rowOfTilesMap[z].Add(tile);			
		}
		foreach (KeyValuePair<int, List<Tile>> row in rowOfTilesMap) {
			if (row.Value.Count != width) {
				Debug.LogError("Row of tiles has inconsistent size: " + row.Value.Count + " , expected: " + width 
				               + " , rowNo: " + row.Key);
			}
		}
		foreach (List<Tile> row in rowOfTilesMap.Values) {
			row.Sort((tileA, tileB) => tileA.gameObject.transform.position.x.CompareTo(tileB.gameObject.transform.position.x));
		}
		List<int> rowOrder = new List<int>();
		foreach(int i in rowOfTilesMap.Keys) {
			rowOrder.Add(i);
		}
		rowOrder.Sort();

		for (int i = 0; i < rowOrder.Count; i++) {
			int rowIndex = rowOrder[i];
			List<Tile> rowOfTiles = rowOfTilesMap[rowIndex];
			for (int j = 0; j < rowOfTiles.Count; j++) {
				Tile tile = rowOfTiles[j];
				tile.SetColumnAndRow(j, i);
				tileMap[j, i] = tile;
			}
		}

		int rowLength = tileMap.GetLength(0);
		int colLength = tileMap.GetLength(1);		
		for (int i = 0; i < rowLength; i++) {
			string name = "";
			for (int j = 0; j < colLength; j++)	{
				name += tileMap[i, j] + "   |   ";
			}
			Debug.Log(name);
		}
	}

	public Tile[,] GetMap() {
		return tileMap;
	}

	public List<Tile> GetTiles() {
		return tileList;
	}

	public int GetWidth() {
		return width;
	}

	public int GetHeight() {
		return height;
	}

	public Tile TopNeighbor(Tile sourceTile) {
		return GetTileOrNull(sourceTile.GetColumn(), sourceTile.GetRow() + 1);
	}

	public Tile BottomNeighbor(Tile sourceTile) {
		return GetTileOrNull(sourceTile.GetColumn(), sourceTile.GetRow() - 1);
	}
	
	public Tile LeftNeighbor(Tile sourceTile) {
		return GetTileOrNull(sourceTile.GetColumn() - 1, sourceTile.GetRow());
	}
	
	public Tile RightNeighbor(Tile sourceTile) {
		return GetTileOrNull(sourceTile.GetColumn() + 1, sourceTile.GetRow());
	}

	public Tile GetTileOrNull(int column, int row) {		
		if (IsInBounds(column, row)) {
			return GetMap()[column, row];
		}
		return null;
	}

	public bool IsInBounds(int column, int row) {
		if (column < 0 || row < 0 || column >= this.GetWidth() || row >= this.GetHeight()) {
			return false;
		}
		return true;
	}
} 
