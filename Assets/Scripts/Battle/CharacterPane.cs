using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterPane : MonoBehaviour {

	public Text nameText;
	public Text hpText;
	Combatant combatant;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Populate(Combatant combatant) {
		if (combatant == null) {
			nameText.text = "--";
			hpText.text = "-/-";
			return;
		}
		nameText.text = combatant.name;
		hpText.text = combatant.Stats.CurrentHealth + " / " + combatant.Stats.MaxHealth;
	}
	
	static public CharacterPane FindLeftPane() {
		return FindCharacterPane("CharPaneLeft");	
	}

	static public CharacterPane FindRightPane() {
		return FindCharacterPane("CharPaneRight");		
	}

	static private CharacterPane FindCharacterPane(string name) {
		GameObject target = null;
		Transform parent = GameObject.FindGameObjectWithTag("CharPanes").transform;
		for (int i = 0; i < parent.childCount; i++) {
			Debug.Log("name: " + parent.GetChild(i).name);
			if (name.Equals(parent.GetChild(i).name)) {
				target = parent.GetChild(i).gameObject;
			}
		}
		return target.GetComponent<CharacterPane>();
	}
}
