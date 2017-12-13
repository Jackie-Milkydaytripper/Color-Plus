using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour {
	public int MyX, MyY;
	GameControl myGameController;
	public bool CURRENT = false;
	public bool nextColor = false;
	// Use this for initialization
	void Start () {
		myGameController = GameObject.Find ("GameControllerObject").GetComponent<GameControl> ();

	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown(){
		if (!nextColor) {
		myGameController.MouseInput (gameObject, MyX, MyY, gameObject.GetComponent<Renderer> ().material.color,CURRENT);
		//print ("x" + x + "...y: " + y); 

		}
	}
}