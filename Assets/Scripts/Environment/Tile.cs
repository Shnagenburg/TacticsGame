using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {


	public Material tileSelected;
	public Material tileSecondary;
	Material tileUnSelected;
	Renderer renderer;
	int column = -1;
	int row = -1;
	int movementCost = 1;
	int height = 0;
	List<Combatant> occupants = new List<Combatant>();

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();
		tileUnSelected = renderer.material;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetColumnAndRow(int column, int row) {
		this.column = column;
		this.row = row;
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
		return row;
	}

	public int GetColumn() {
		return column;
	}

	public int GetMovementCost() {
		return movementCost;
	}

	public bool IsOccupied() {
		return occupants.Count != 0;
	}

	public void SetOccupant(Combatant combatant) {
		this.occupants.Add(combatant);
		combatant.transform.position = this.transform.position;
		combatant.SetTile(this);
	}

	public void RemoveOccupant(Combatant combatant) {
		this.occupants.Remove(combatant);
	}

	public Combatant GetOccupant() {
		if (occupants.Count > 0) {
			return occupants[0];
		}
		return null;
	}
}
