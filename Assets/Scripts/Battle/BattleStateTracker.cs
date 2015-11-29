using UnityEngine;
using System.Collections;

public class BattleStateTracker {

	public GameObject gameObject;
	public BattleStateTracker previous;

	public void ResetChain() {
		if (previous != null) {
			GameObject.Destroy(gameObject);
			previous.ResetChain();
		} else {
			gameObject.SetActive(true);
		}
	}

	public void GoBackOneStep() {		
		if (previous != null) {
			GameObject.Destroy(gameObject);
			previous.gameObject.SetActive(true);
		} else {
			Debug.LogWarning("No step to go back to for " + gameObject);
		}
	}

}
