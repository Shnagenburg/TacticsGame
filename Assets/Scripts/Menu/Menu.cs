using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

	public BattleStateTracker battleStateTracker = new BattleStateTracker();
	public MenuSelector menuSelector;
	public bool IsDone {get; set;}
	List<MenuItem> menuItems = new List<MenuItem>();
	Combatant combatant;
	public Combatant Combatant {
		get {return combatant;} 
		set {combatant = value;}
	}
	CharacterPane leftPane = null;

	// Use this for initialization
	void Start () {
		battleStateTracker.gameObject = this.gameObject;
		GameObject parentCanvas = GameObject.FindGameObjectWithTag("Canvas");
		this.gameObject.transform.SetParent(parentCanvas.transform, false);
		name = "Battle Menu Action";
		IsDone = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (menuItems.Count == 0) {
			Default();
		}
		CheckReturned();
	}
	
	void CheckReturned() {
		if (IsDone) {
			IsDone = false;
			EnablePane();
		}
	}

	void EnablePane() {
		leftPane.gameObject.SetActive(true);
		leftPane.Populate(combatant);
	}

	void Default() {
		Debug.Log("building menu");

		menuItems = new List<MenuItem>();
		GameObject menuItemObject;
		MenuItem menuItem;
		//MenuAction menuAction;
		if (combatant.Stats.TurnStats.CanAttack()) {

			MenuAttackAction attackAction = new MenuAttackAction();
			attackAction.parentMenu = this;
			attackAction.combatant = combatant;

			menuItemObject = (GameObject)Instantiate(Resources.Load("MenuItem"));
			menuItemObject.transform.parent = this.gameObject.transform;
			menuItem = menuItemObject.GetComponent<MenuItem>();
			menuItem.SetText("Attack");
			menuItem.name = menuItem.menuItemText.text;
			menuItem.transform.localRotation = Quaternion.identity;
			menuItem.transform.localPosition = Vector3.zero;
			menuItem.transform.localScale = Vector3.one;
			menuItem.SetMenuAction(attackAction);
			menuItems.Add(menuItem);
			
		}

		if (combatant.Stats.TurnStats.CanMove()) {
			//menuAction = new MenuMoveAction();
			MenuMoveAction moveAction = new MenuMoveAction();
			moveAction.SetMenu(this);
			moveAction.combatant = combatant;

			menuItemObject = (GameObject)Instantiate(Resources.Load("MenuItem"));
			menuItemObject.transform.parent = this.gameObject.transform;
			menuItem = menuItemObject.GetComponent<MenuItem>();
			menuItem.SetText("Move");
			menuItem.name = menuItem.menuItemText.text;
			menuItem.transform.localRotation = Quaternion.identity;
			menuItem.transform.localPosition = Vector3.zero;
			menuItem.transform.localScale = Vector3.one;
			menuItem.SetMenuAction(moveAction);
			menuItems.Add(menuItem);
		}
		
		//menuAction = new MenuEndTurnAction();
		MenuEndTurnAction endTurnAction = new MenuEndTurnAction();
		endTurnAction.parentMenu = this;
		menuItemObject = (GameObject)Instantiate(Resources.Load("MenuItem"));
		menuItemObject.transform.parent = this.gameObject.transform;
		menuItem = menuItemObject.GetComponent<MenuItem>();
		menuItem.SetText("End Turn");
		menuItem.name = menuItem.menuItemText.text;
		menuItem.transform.localRotation = Quaternion.identity;
		menuItem.transform.localPosition = Vector3.zero;
		menuItem.transform.localScale = Vector3.one;
		menuItem.SetMenuAction(endTurnAction);
		menuItems.Add(menuItem);

		int i = 0;
		foreach(MenuItem item in menuItems) {
			Vector3 pos = item.transform.position;
			pos.y = pos.y - (item.menuItemText.fontSize * i);
			item.transform.position = pos;
			i = i + 1;
		}
		menuSelector.SetMenuItems(menuItems);
	}

	public void SetCombatant(Combatant combatant) {
		Debug.Log("setting combant -" + combatant + "-");
		this.combatant = combatant;
		
		leftPane = CharacterPane.FindLeftPane();
		EnablePane();
	}

	public void CleanUp() {
		if (leftPane != null) {
			leftPane.gameObject.SetActive(false);
		}
		IsDone = true;
	}
}
