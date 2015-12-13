using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public TileData TileData {get; set;}
	public Material tileSelected;
	public Material tileSecondary;
	Material tileUnSelected;
	Renderer renderer;
    
	List<Combatant> occupants = new List<Combatant>();

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();
		tileUnSelected = renderer.material;
		if (this.TileData == null) {
		    this.TileData = new TileData();
		    this.TileData.Tile = this;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetColumnAndRow(int column, int row) {
		if (this.TileData == null) {
			this.TileData = new TileData();
			this.TileData.Tile = this;
        }
        this.TileData.Column = column;
		this.TileData.Row = row;
	}

	public void OnCursorOver() {
		renderer.material = tileSelected;
	}

	public void OnCursorOff() {
		renderer.material = tileUnSelected;
	}

	public void SetSecondaryColor() {
		renderer.material = tileSecondary;
	}

	public int GetRow() {
		return this.TileData.Row;
	}

	public int GetColumn() {
		return this.TileData.Column;
	}

	public int GetMovementCost() {
		return this.TileData.MovementCost;
	}

	public bool IsOccupied() {
		return occupants.Count != 0;
	}

	public void SetOccupant(Combatant combatant) {
		this.occupants.Add(combatant);
		combatant.transform.position = this.transform.position;
		combatant.Tile  = this;
		this.TileData.OccupiedTeam = combatant.TeamId;
	}

	public void RemoveOccupant(Combatant combatant) {
		this.occupants.Remove(combatant);
		this.TileData.OccupiedTeam = TeamId.NoOccupant;
	}

	public Combatant GetOccupant() {
		if (occupants.Count > 0) {
			return occupants[0];
		}
		return null;
	}

	public string LocationString() {
		return "r:" + this.GetRow() + " c:" + this.GetColumn();
	}
}
