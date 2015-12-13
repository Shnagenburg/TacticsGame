using UnityEngine;
using System.Collections;

public class TileData {

	public int Column { get; set; }
	public int Row { get; set; }
	public int MovementCost { get; set; }
	public int Height { get; set; }
	public Tile Tile { get; set; }
	public int OccupiedTeam { get; set;}

	public TileData() {
		this.Column = -1;
		this.Row = -1;
		this.MovementCost = 1;
		this.Height = 0;
		this.Tile = null;
		this.OccupiedTeam = -1;
	}

	public string ToString() {
		return "TileData-r:" + this.Row + ",c:" + this.Column + ",Occp:" + this.OccupiedTeam;
	}

}
