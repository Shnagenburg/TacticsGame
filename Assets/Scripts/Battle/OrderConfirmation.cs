using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrderConfirmation : MonoBehaviour {
	
	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	GameObject confirmObject = null;
	BattleOrder order = null;
	bool canConfirm = false;
	CharacterPane leftPane = null;
	CharacterPane rightPane = null;
	CombatPane centerPane = null;


	void Start () {
		Debug.Log("order confirmer!");
		battleStateTracker.gameObject = this.gameObject;
		GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
		for(int i = 0; i < canvas.transform.childCount; i++) {
			GameObject child = canvas.transform.GetChild(i).gameObject;
			if (child.name.Equals("ConfirmObject")) {
				confirmObject = child;
			}
		}
		confirmObject.SetActive(true);
	}

    void SetupCharacterPanes() {
        if (!order.Action.Equals("move")) {            
            leftPane = CharacterPane.FindLeftPane();
            rightPane = CharacterPane.FindRightPane();
            leftPane.gameObject.SetActive(true);
            rightPane.gameObject.SetActive(true);
            leftPane.Populate(order.SourceCombatant);
			if (order.TargetTile.GetOccupant() != null && order.TargetTile.GetOccupant().Stats.HasStatus("dead")
			    && order.Action.Equals("attack")) {
				rightPane.Populate(null);
			} else {
				rightPane.Populate(order.TargetTile.GetOccupant());
			}

			centerPane = CombatPane.FindCenterPane();
			centerPane.gameObject.SetActive(true);
			centerPane.Populate(order);
        }
    }

	void Update () {
		if (canConfirm && Input.GetButton("Fire1")) {			
			GameObject objToSpawn = new GameObject("BattleOrderEnactor Action");
			objToSpawn.AddComponent<BattleOrderEnactor>();
			objToSpawn.GetComponent<BattleOrderEnactor>().battleStateTracker.previous = this.battleStateTracker;
			objToSpawn.GetComponent<BattleOrderEnactor>().Enact(order);
			CleanUp();
			this.gameObject.SetActive(false);
		} else if (Input.GetButton("Fire2")) {
			CleanUp();
			battleStateTracker.GoBackOneStep();
		}
		
		if (!Input.GetButton("Fire1")) {
			canConfirm = true;
		}
	    
	}

	public void SetBattleOrder(BattleOrder order) {
		this.order = order;
		order.TargetTile.OnCursorOver();

        SetupCharacterPanes();
	}

	void CleanUp() {
		confirmObject.SetActive(false);
		order.TargetTile.OnCursorOff();
        if (leftPane != null) {
            leftPane.gameObject.SetActive(false);
		}
		if (rightPane != null) {
			rightPane.gameObject.SetActive(false);
		}
		if (centerPane != null) {
			centerPane.gameObject.SetActive(false);
		}
	}
}
