using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {
	
	public List<GameObject> list;
	public List<Tile> tiles;
	public GameObject tilesParent;
	public int width = 4;
	public int height = 4;

	TileMap tileMap;
	public TileMap TileMap {
		get {return tileMap;}
		set {tileMap = value;}
	}
	int raycastTileMask;
	Tile currentHoverTile = null;
	List<Tile> currentHoverTiles = new List<Tile>();
	public Tile test1;
	public Tile test2;

	// Use this for initialization
	void Awake () {
		raycastTileMask = LayerMask.GetMask ("Tile");
		foreach (Transform child in tilesParent.transform) {
			list.Add(child.gameObject);
			tiles.Add(child.GetComponent<Tile>());
		}
		this.tileMap = new TileMap(tiles, width, height);
	}
	bool done = false;
	// Update is called once per frame
	void Update () {
		if (true) {
			return;
		}
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, raycastTileMask);

		//if (true && !done ) {
		//	List<Tile> path = FindShortestPath.Find(tileMap, test1, test2);
		//	Debug.Log("path length " + path.Count);
		//	path.ForEach((t) => t.OnCursorOver());
		//	done = true;
		//	return;
		//}
		if (hit) {
			//Debug.Log("Hit " + hitInfo.transform.gameObject.name);

			Tile tile = hitInfo.collider.GetComponent <Tile> ();
			
			if(tile != null) {
				Debug.Log("found tile");

				tile.OnCursorOver();	

				List<Tile> hoverTiles = FindTilesWithinRange.Find(tileMap, tile, 2, 0);
				foreach (Tile hoverTile in currentHoverTiles) {
					hoverTile.OnCursorOff();
				}
				currentHoverTiles = hoverTiles;
				foreach (Tile hoverTile in currentHoverTiles) {
					hoverTile.OnCursorOver();
                }


			}
			if (currentHoverTile == null) {				
				currentHoverTile = tile;
			} else if (currentHoverTile != null && tile != currentHoverTile) {
				currentHoverTile.OnCursorOff();
				currentHoverTile = tile;
			}
		} else {
			//Debug.Log("No hit");
			if (currentHoverTile != null) {
				currentHoverTile.OnCursorOff();
				currentHoverTile = null;
			}
		}
	}

	public List<Tile> GetTilesInRange(Tile tile, int range, bool canMoveThroughOccupied) {
		List<Tile> hoverTiles = FindTilesWithinRange.Find(tileMap, tile, range, 0, canMoveThroughOccupied);
		Debug.Log(hoverTiles.Count + "!");
		return hoverTiles;
	}

	public List<Tile> GetShortestPath(Tile source, Tile destination) {
		List<Tile> hoverTiles = FindShortestPath.Find(tileMap, source, destination);
		Debug.Log(hoverTiles.Count + "!");
		return hoverTiles;
    }
}
