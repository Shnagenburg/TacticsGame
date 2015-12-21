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
	float timer = 0.0f;
	const float MAX_TIME = 2.0f;
	bool autoConfirm = false;


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
		confirmObject.SetActive(!autoConfirm);
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
		if (autoConfirm) {
			timer += Time.deltaTime;
			if (timer > MAX_TIME) {
				CreateEnactor();
				CleanUp();
				return;
			}
		}
		if (canConfirm && Input.GetButton("Fire1")) {	
			CreateEnactor();
			CleanUp();
		} else if (Input.GetButton("Fire2")) {
			CleanUp();
			battleStateTracker.GoBackOneStep();
		}
		
		if (!Input.GetButton("Fire1")) {
			canConfirm = true;
		}
	    
	}

	private void CreateEnactor() {
		GameObject objToSpawn = new GameObject("BattleOrderEnactor Action");
		objToSpawn.AddComponent<BattleOrderEnactor>();
		objToSpawn.GetComponent<BattleOrderEnactor>().battleStateTracker.previous = this.battleStateTracker;
		objToSpawn.GetComponent<BattleOrderEnactor>().Enact(order);
	}
	
	public void SetBattleOrder(BattleOrder order) {
		this.SetBattleOrder(order, false);
	}

	public void SetBattleOrder(BattleOrder order, bool autoConfirm) {
		this.order = order;
		order.TargetTile.OnCursorOver();

        SetupCharacterPanes();
		this.autoConfirm = autoConfirm;
	}

	void CleanUp() {
		timer = 0;
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
		this.gameObject.SetActive(false);
	}
}
