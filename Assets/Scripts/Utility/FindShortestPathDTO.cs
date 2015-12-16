using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindShorestPathDTO {

	public Dictionary<TileData, int> TileToDistance { get; set;}
	public Dictionary<TileData, TileData> TileToOptimalPrevious {get; set;}
	public int MoveThroughMask {get; set;}
	public TileData Destination {get; set;}

	public FindShorestPathDTO() {
		this.TileToDistance = new Dictionary<TileData, int>();
		this.TileToOptimalPrevious = new Dictionary<TileData, TileData>();
		this.MoveThroughMask = TeamId.MOVE_THROUGH_ALL;
		this.Destination = null;
	}
}
