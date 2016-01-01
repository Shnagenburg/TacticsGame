using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuSelector : MonoBehaviour {

	public Text selectorText;
	List<MenuItem> menuItems = new List<MenuItem>();
	MenuItem currentSelection = null;
	bool canSelect = false;
	bool canMove = false;

	void Update () {
		if (menuItems == null || menuItems.Count == 0) {
			Debug.Log("no items");
			return;
		}
		float input = Input.GetAxisRaw ("Vertical");
		int index = menuItems.IndexOf (currentSelection);
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
		index = mod(index, menuItems.Count);
		if (input == 0) {
			canMove = true;
		}
		if (currentSelection != menuItems[index]) {
			currentSelection = menuItems[index];
			Vector3 newPos = currentSelection.transform.position;
			newPos.x -= 20;
			this.transform.position = newPos;
			//currentSelection.OnCursorOver();
		}
		
		//Vector3 target = currentSelection.GetAnchorPosition ();
		//target.z = target.z + zOffset;
		//this.transform.position = Vector3.Lerp(this.transform.position, target, Time.deltaTime * speed);
		
		if (canSelect && Input.GetButton ("Jump")) {
			//SoundBox.GetInstance().PlayOnSelectAudio();
			currentSelection.OnSelect();
			canSelect = false;
		}
		if (!Input.GetButton ("Jump")) {
			canSelect = true;
		}
		//Debug.Log("cs: " + currentSelection.name);
		
	}
	
	int mod(int x, int m) {
		return (x % m + m) % m;
	}

	public void SetMenuItems(List<MenuItem> menuItems) {
		this.menuItems = menuItems;
	}
}
