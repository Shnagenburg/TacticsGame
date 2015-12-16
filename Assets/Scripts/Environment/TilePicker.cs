using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilePicker : MonoBehaviour {

	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	BattleOrder battleOrder = null;

	ThreadedFindTiles calc = null;

	List<Tile> tiles = null;
	Tile hoverTile = null;
	Tile response = null;
	bool isDone = false;
	bool canGoBack = false;

	// Use this for initialization
	void Start () {
		this.battleStateTracker.gameObject = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (calc != null && calc.IsDone) {
			calc.JoinThread();
			SetTiles(calc.TileResults.ConvertAll(t => t.Tile));
		}
		if (tiles == null) {
			return;
		}
		CheckReturned();
		if (hoverTile != null) {
			hoverTile.OnCursorOver();
		}
		hoverTile = GetTileMouseIsOver();
		if (hoverTile != null) {
			hoverTile.SetSecondaryColor();
		}
		if (Input.GetButton("Fire1") && hoverTile != null) {
			Debug.Log("tile picker done");			
			CleanUp();
			isDone = true;
			battleOrder.TargetTile = hoverTile;
			
			GameObject objToSpawn = new GameObject("OrderConfirm Action");
			objToSpawn.AddComponent<OrderConfirmation>();
			objToSpawn.GetComponent<OrderConfirmation>().battleStateTracker.previous = this.battleStateTracker;
			objToSpawn.GetComponent<OrderConfirmation>().SetBattleOrder(battleOrder);
			this.gameObject.SetActive(false);
		}
		CheckForBackButton();
	}

	Tile GetTileMouseIsOver() {	
		Tile hoverTile = null;
		int layerMask = LayerMask.GetMask("Tile");
		RaycastHit hitInfo = new RaycastHit();
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, layerMask)) {
			Debug.Log("hit tile!");
			Tile tile = hitInfo.collider.GetComponent<Tile>();			
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
	
	void CheckForBackButton() {
		if (canGoBack && Input.GetButton("Fire2")) {
			Debug.Log("backing out of tile picker");
			CleanUp();
			battleStateTracker.GoBackOneStep();
		}
		if (!Input.GetButton("Fire2")) {
			canGoBack = true;
		}
	}

	public void SetBattleOrder(BattleOrder battleOrder) {
		this.battleOrder = battleOrder;
	}

	public void SetTileParamsMove(Combatant combatant) {
		calc = new ThreadedFindTiles();
		calc.StartThreadToFindTilesInRange(combatant, MapManager.FindMapManager());
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

	void CheckReturned() {
		if (isDone) {
			isDone = false;
			canGoBack = false;
			HighlightTiles();
		}
	}

	public bool IsDone() {
		return isDone;
	}

	public Tile GetResponse() {
		return response;
	}

	public void CleanUp() {
		if (calc != null) {
		    calc.JoinThread();
		}
		UnlightTiles();
	}
}
