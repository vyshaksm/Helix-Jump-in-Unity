using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	bool gameStarted = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameStarted && Input.GetMouseButtonDown(0)) {
			FindObjectOfType<Text>().transform.parent.gameObject.SetActive(false);
			Ball.Move = gameStarted = true;
		}
	}
}
