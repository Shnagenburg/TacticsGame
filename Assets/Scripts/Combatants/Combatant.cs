using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combatant : MonoBehaviour {
	
	public bool animating = false;
	public Animator animator;
	public Tile Tile { get; set; }
	public CombatantStats Stats { get; set; }
	public int TeamId { get; set; }
	Quaternion originalFacing;
	MapManager map;

	// Use this for initialization
	void Start () {
		this.Stats = new CombatantStats();
		this.TeamId = 0;
		map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapManager>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartAttackAnimation(Tile targetTile) {
		originalFacing = this.transform.rotation;
		animator.SetTrigger("Attack");
		animating = true;
		this.transform.LookAt(targetTile.transform);
	}
	
	public void StartFlinchingAnimation() {
		animator.SetBool("IsFlinching", true);
		animating = true;
	}

	public void DoneAnimating() {
		this.transform.rotation = originalFacing;
		animator.SetBool("IsFlinching", false);
		Debug.Log("done with attack");
		animating = false;
	}

	public void CheckStatus() {
		if (Stats.IsDead() && !Stats.HasStatus("dead")) {
			Stats.ActiveEffects.Add(new StatusEffect("dead"));
			animator.SetTrigger("Die");
		}
	}
}
