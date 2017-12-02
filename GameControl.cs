using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {
	public GameObject cubePrefab;
	Vector3 cubePos;
	Vector3 NextCubePos = new Vector3 (2.5f, 11, 0);
	int gridX = 8;
	int gridY = 5;
	GameObject [,] grid;
	GameObject nextColor;
	Color[] myColors = { Color.blue, Color.red, Color.magenta, Color.yellow, Color.green};
	float gameDuration = 60;
	int turn = 1;
	float turnDuration = 2;
	int score = 0;
	//Controls

	void NextCubeSummon () {
		if (nextColor == null) {
			nextColor = Instantiate (cubePrefab, NextCubePos, Quaternion.identity);
		}
		nextColor.GetComponent<Renderer> ().material.color = myColors [Random.Range (0, myColors.Length)];
	}

	void ColorSet (GameObject myCube, Color color) {
		if (myCube == null) {
			Endgame (false);
		}
		else {

			myCube.GetComponent<Renderer> ().material.color =	nextColor.GetComponent<Renderer> ().material.color = color;
			Destroy (nextColor);
			nextColor = null;
		}
	}

	GameObject PickWhite (List<GameObject> whiteCubes) {
		//No white cubes? No problem
		if (whiteCubes.Count == 0) {
			return null;
		}
		return whiteCubes [ Random.Range (0, whiteCubes.Count)];


	}

	GameObject FindAvailable (int y){
		List<GameObject> whiteCubes = new List<GameObject> ();

		for (int x = 0; x < gridX; x++) {
			if (grid [x, y].GetComponent<Renderer> ().material.color == Color.white) { 
				whiteCubes.Add(grid[x,y]);
			}
		}
		//No white cubes? No problem
		if (whiteCubes.Count == 0) {
			return null;
		}
		return PickWhite (whiteCubes);


	}

	GameObject FindAvailable (){
		List<GameObject> whiteCubes = new List<GameObject> ();
		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				if (grid [x, y].GetComponent<Renderer> ().material.color == Color.white) { 
					whiteCubes.Add (grid [x, y]);
				}
			}
		}

		return PickWhite (whiteCubes);
	}


	void PlaceNewColor(int y){
		GameObject whiteCube = FindAvailable (y);

		ColorSet (whiteCube, nextColor.GetComponent<Renderer> ().material.color); 

	}

	void BlackCubeSummon() {
		GameObject whiteCube = FindAvailable ();

		ColorSet (whiteCube, Color.black); 

	}

	void Endgame (bool win){
		if (win) {
			//Game over...in a good way!
			print ("Congrats, a winner is you!");
		} else {
			//End the game
			print ("Oops! Game over. C'mon, let's try again!");
		}
	}

	void KeyInput(){
		int pressNumberKey = 0;

		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
			pressNumberKey = 1;
			//print ("pressed 1");
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
			pressNumberKey = 2;
			//print ("pressed 2");
		}
		if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
			pressNumberKey = 3;
			//print ("pressed 3");
		}
		if (Input.GetKeyDown (KeyCode.Alpha4) || Input.GetKeyDown (KeyCode.Keypad4)) {
			pressNumberKey = 4;
			//print ("pressed 4");
		}
		if (Input.GetKeyDown (KeyCode.Alpha5) || Input.GetKeyDown (KeyCode.Keypad5)) {
			pressNumberKey = 5;
			//print ("pressed 5");
		}
		//Place that bad boy in the row, subtracting based on a 0 - based index ((starts with 0 
		//...instead of 1))
		if (nextColor != null && pressNumberKey != 0) {
			PlaceNewColor (pressNumberKey - 1);
		}
	}

	//Gameplay set up

	void CreateGrid (){
		grid = new GameObject[gridX, gridY];

		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				//Always check everything. Even if you have to squint...
				cubePos = new Vector3 (x * 2, y * 2, 0);
				grid[x,y] = Instantiate (cubePrefab, cubePos, Quaternion.identity);
				grid[x,y].GetComponent<CubeControl> ().myX = x;
				grid[x,y].GetComponent<CubeControl> ().myY = y;
			}
		}	
	}
		

	// Use this for initialization
	void Start () {

		CreateGrid ();


	}


	// Update is called once per frame
	void Update () {

		KeyInput();

		if (Time.time > turnDuration * turn) {
			turn++;

			if (nextColor != null) {
				score -= 1;
				BlackCubeSummon ();
			}

			NextCubeSummon ();
		}
	}
}
