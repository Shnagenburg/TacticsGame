using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuItem : MonoBehaviour {

	public Text menuItemText;

	MenuAction menuAction;

	public void OnSelect() {
		if (menuAction == null) {
			Debug.LogWarning(this.name + " has no menu action");
			return;
		}
		menuAction.Execute();
	}

	public void SetMenuAction(MenuAction menuAction) {
		this.menuAction = menuAction;
	}

	public void SetText(string text) {
		this.menuItemText.text = text;
	}


}
