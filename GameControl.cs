using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {
	public GameObject cubePrefab,NextcubePrefab;
	Vector3 cubePos;
	Vector3 NextCubePos = new Vector3 (2.5f, 11, 0);
	int gridX = 8;
	int gridY = 5;
	GameObject [,] grid;
	GameObject nextColor;
	GameObject currentCube = null;
	public GameObject selectedCube;
	Color[] myColors = { Color.blue, Color.red, Color.magenta, Color.yellow, Color.green};
	float gameDuration = 60;
	int turn = 1;
	float turnDuration = 2;
	int score = 0;
	//Controls

	void NextCubeSummon () {
		if (nextColor == null) {
			nextColor = Instantiate (NextcubePrefab, NextCubePos, Quaternion.identity);
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

	public void MouseInput (GameObject selectedCube, int x, int y, Color cubeColor, bool CURRENT){
		if (cubeColor != Color.white && cubeColor != Color.black) {

			if (CURRENT) {
				//deactivate
				selectedCube.transform.localScale /= 1.5f;
				selectedCube.GetComponent<CubeControl>().CURRENT = false;
				currentCube = null;
			} 
			else {
				//deact. any previously active cube

				if (currentCube != null) {
					currentCube.transform.localScale /= 1.5f;
					currentCube.GetComponent<CubeControl>().CURRENT = false;
				}
				//activate
				selectedCube.transform.localScale *= 1.5f;
				selectedCube.GetComponent<CubeControl>().CURRENT = true;
				currentCube = selectedCube;
			}

		}
		//if white cube was clicked
		else if (cubeColor == Color.white && currentCube != null) {
			//This is to remind me of the distance between the Y and X variables
			int DistancebtwnX = selectedCube.GetComponent<CubeControl> ().MyX - currentCube.GetComponent<CubeControl> ().MyX;
			int DistancebtwnY = selectedCube.GetComponent<CubeControl> ().MyY - currentCube.GetComponent<CubeControl> ().MyY;

			//ONLY IF within the correct distance, including diagnolly
			if (Mathf.Abs(DistancebtwnY) <= 1 && Mathf.Abs(DistancebtwnX) <= 1) {
				//Setting to correct color, which belonged to the current cube
				selectedCube.GetComponent<Renderer>().material.color = currentCube.GetComponent<Renderer> ().material.color;
				// Selected cube gets activated...!
				selectedCube.GetComponent<CubeControl>().CURRENT = true;
				//Scales up to be a big boy...
				selectedCube.transform.localScale *= 1.5f;


				//Deactivating the previous cube to be ... White...
				currentCube.GetComponent<Renderer>().material.color = Color.white;
				// ...deactivated....and...
				currentCube.GetComponent<CubeControl> ().CURRENT = false;
				//back to its original scale
				currentCube.transform.localScale /= 1.5f;
				 
				//Current cube is now equal to a selected cube
				currentCube = selectedCube;
			}
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
				grid[x,y].GetComponent<CubeControl> ().MyX = x;
				grid[x,y].GetComponent<CubeControl> ().MyY = y;
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
//I realized that I kept calling parts of the code ("=", "&&", "().") particles, which are small connecters that help a noun or verb in a Japanese sentence.
//If the wrong one is used, it won't make sense. In order to understand C# more, I'll have to learn it just like a language.
//Although, I kinda knew that all along...