using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combatant : MonoBehaviour {
	
	public bool animating = false;
	public Animator animator;
	Quaternion originalFacing;
	Tile currentTile;
	MapManager map;

	// Use this for initialization
	void Start () {
		map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Tile GetTile() {
		return currentTile;
	}

	public void SetTile(Tile tile) {
		this.currentTile = tile;
	}

	public void StartAttackAnimation(Tile targetTile) {
		originalFacing = this.transform.rotation;
		animator.SetTrigger("Attack");
		animating = true;
		this.transform.LookAt(targetTile.transform);
	}

	public void DoneAnimating() {
		this.transform.rotation = originalFacing;
		Debug.Log("done with attack");
		animating = false;
	}
}
