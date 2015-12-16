using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindTilesWithinRangeDTO {

	public int JumpHeight { get; set; }
	public Dictionary<TileData, int> TileToMaxMovement { get; set; }
	public int MoveThroughMask { get; set; }

	public FindTilesWithinRangeDTO() {
		this.JumpHeight = 0;
		this.TileToMaxMovement = new Dictionary<TileData, int>();
		this.MoveThroughMask = -1;
	}
		
}
