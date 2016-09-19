using UnityEngine;
using System.Collections;
using System;

public class InputEvent : MonoBehaviour {

	void OnEnable() {
		InputController.moveEvent += OnMoveEvent;
		InputController.fireEvent += OnFireEvent;
	}

	void OnDisable() {
		InputController.moveEvent -= OnMoveEvent;
		InputController.fireEvent -= OnFireEvent;
	}

	private void OnMoveEvent(object sender, InfoEventArgs<Vec> e) {
		Debug.Log("Move " + e.info.ToString());
	}

	private void OnFireEvent(object sender, InfoEventArgs<int> e) {
		Debug.Log("Fire " + e.info.ToString());
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
