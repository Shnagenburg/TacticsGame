using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatPane : MonoBehaviour {

	public Text actionText;
	public Text damageText;
	BattleOrder order;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Populate(BattleOrder order) {
		this.order = order;
		actionText.text = order.Action;
		if (order.TargetTile.GetOccupant() != null) {
			CombatantStats stats = order.SourceCombatant.Stats;
			damageText.text = " " + stats.AttackPower + " Dmg \n " + stats.Accuracy + " % Acc";
		} else {
			damageText.text = "-/-";
		}
	}

	static public CombatPane FindCenterPane() {
		return FindCharacterPane("CombatPaneCenter");	
	}
	
	static private CombatPane FindCharacterPane(string name) {
		GameObject target = null;
		Transform parent = GameObject.FindGameObjectWithTag("CharPanes").transform;
		for (int i = 0; i < parent.childCount; i++) {
			Debug.Log("name: " + parent.GetChild(i).name);
			if (name.Equals(parent.GetChild(i).name)) {
				target = parent.GetChild(i).gameObject;
			}
		}
		return target.GetComponent<CombatPane>();
	}
}
