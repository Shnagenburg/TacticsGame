using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilePicker : MonoBehaviour {

	public class TilePickerResponse {
		Tile tile;
	}

	List<Tile> tiles = null;
	Tile hoverTile = null;
	Tile response = null;
	bool isDone = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (tiles == null) {
			return;
		}
		if (hoverTile != null) {
			hoverTile.OnCursorOver();
		}
		hoverTile = GetTileMouseIsOver();
		if (hoverTile != null) {
			hoverTile.SetSecondaryColor();
		}
		if (Input.GetButton("Fire1") && hoverTile != null) {
			Debug.Log("tile picker done");
			isDone = true;
			response = hoverTile;
		}
	}

	Tile GetTileMouseIsOver() {	
		Tile hoverTile = null;
		int layerMask = LayerMask.GetMask("Tile");
		RaycastHit hitInfo = new RaycastHit();
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, layerMask)) {
			Debug.Log("hit tile!");
			Tile tile = hitInfo.collider.GetComponent <Tile> ();			
			if(tile != null) {
				hoverTile = tile;
			} else {
				Debug.LogWarning("Non tile object with tile mask: " + hitInfo.collider.gameObject);
			}
		}
		if (tiles.Contains(hoverTile)) {
			return hoverTile;
		}
		return null;
	}

	public void SetTiles(List<Tile> tiles) {
		this.tiles = tiles;
		HighlightTiles();
	}

	void HighlightTiles() {		
		foreach (Tile tile in tiles) {
			tile.OnCursorOver();
		}
	}

	void UnlightTiles() {		
		foreach (Tile tile in tiles) {
			tile.OnCursorOff();
		}
	}

	public bool IsDone() {
		return isDone;
	}

	public Tile GetResponse() {
		return response;
	}

	public void CleanUp() {
		UnlightTiles();
	}
}
