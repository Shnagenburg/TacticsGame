using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleMenu : MonoBehaviour {

	public Text selector;
	public Text headerText;
	public Text optionsText;
	List<string> options = null;
	string currentSelection;
	float zOffset = 14f;
	bool canMove = false;
	bool canSelect = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (options == null) {
			return;
		}
		float input = Input.GetAxisRaw ("Vertical");
		int index = options.IndexOf (currentSelection);
		if (canMove) {
			if (input > 0) {
				index = index - 1;
				canMove = false;
			} else if (input < 0) {
				index = index + 1;
				canMove = false;
			}
			if (input != 0) {
				//onSelectionChangeAudio.Play();
			}
		}
		index = mod(index, options.Count);
		if (input == 0) {
			canMove = true;
		}
		if (currentSelection != options[index]) {
			currentSelection = options[index];
			selector.rectTransform.position = optionsText.rectTransform.position;
			//currentSelection.OnCursorOver();
		}
		
		//Vector3 target = currentSelection.GetAnchorPosition ();
		//target.z = target.z + zOffset;
		//this.transform.position = Vector3.Lerp(this.transform.position, target, Time.deltaTime * speed);
		
		if (canSelect && Input.GetButton ("Jump")) {
			//SoundBox.GetInstance().PlayOnSelectAudio();
			//currentSelection.ActivateItem();
			canSelect = false;
		}
		if (!Input.GetButton ("Jump")) {
			canSelect = true;
		}
		
	}
	
	int mod(int x, int m) {
		return (x % m + m) % m;
	}

	public void PopulateOptions(Combatant combatant) {
		//options = combatant.Options;
		string message = string.Join("\n", options.ToArray());
		headerText.text = combatant.gameObject.name;
		optionsText.text = message;

		currentSelection = options [0];	
	}
}
