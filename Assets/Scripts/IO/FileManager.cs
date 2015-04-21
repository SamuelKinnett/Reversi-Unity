using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileManager : MonoBehaviour
{
	private struct MapState
	{
		public int[,] map;
	}

	public GameObject BoardManager;
	public GameObject MenuController;
	private BoardBehaviour boardBehaviour;
	private MenuManager menuManager;

	StreamReader streamReader;
	StreamWriter streamWriter;

	string mainDirectory;		//The directory the program is located in without an ending backslash, e.g. C:\Users\Test\Documents\Reversi
	string logPath;				//The path for the \Logs folder
	bool gameLoaded;
	int currentTurn;
	int maxTurn;

	int nextFreeIndex;			//The next free index number
	MapState[] mapState;

	// Use this for initialization
	void Start ()
	{
		boardBehaviour = BoardManager.GetComponent<BoardBehaviour> ();
		gameLoaded = false;

		mainDirectory = Directory.GetCurrentDirectory ();
		Debug.Log ("Current Directory = " + mainDirectory);

		//Check if the logs directory exists
		if (!Directory.Exists (mainDirectory + @"\Logs"))
			Directory.CreateDirectory (mainDirectory + @"\Logs");

		logPath = mainDirectory + @"\Logs";

		//Check if the index file exists, update index number if it does
		if (!File.Exists (logPath + @"\Index.txt")) {
			File.Create (logPath + @"\Index.txt").Close ();
			nextFreeIndex = 0;
		} else {
			streamReader = new StreamReader (logPath + @"\Index.txt");
			while (!streamReader.EndOfStream) {
				streamReader.ReadLine ();
				nextFreeIndex++;
			}
			streamReader.Close ();
			Debug.Log ("Index loaded");
		}
	}

	#region Writing

	/// <summary>
	/// Creates a new game log and opens a StreamWriter to it.
	/// </summary>
	/// <param name="gameType">Game type.</param>
	/// <param name="aiChoice1">Ai choice1.</param>
	/// <param name="aiChoice2">Ai choice2.</param>
	public void CreateLog (int gameType, int aiChoice1 = 0, int aiChoice2 = 0)
	{
		string typeOfGame = "";
		string gameName = "";

		switch (gameType) {

		case 0:
			typeOfGame = "pvp";
			break;

		case 1:
			typeOfGame = "pva";
			break;

		case 2:
			typeOfGame = "ava";
			break;
		}

		//Create the file
		gameName = System.DateTime.Now.ToString ("dd-MM-yyyy_HH-mm-ss_") + typeOfGame;
		File.Create (logPath + @"\" + gameName + ".txt").Close ();
		Debug.Log ("File Created");
		//Add the file to the index
		streamWriter = new StreamWriter (logPath + @"\Index.txt", true);
		streamWriter.WriteLine (gameName);
		streamWriter.Close ();
		nextFreeIndex++;

		//Add some game info to the header
		streamWriter = new StreamWriter (logPath + @"\" + gameName + ".txt");
		streamWriter.WriteLine (gameType + ":" + aiChoice1 + ":" + aiChoice2);
	}

	/// <summary>
	/// Adds the turn to the game log.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="playerNumber">Player number.</param>
	public void AddTurnToLog (int x, int y, int playerNumber)
	{
		streamWriter.WriteLine (playerNumber + "," + x + "," + y);
	}

	/// <summary>
	/// Ends the game log and closes the StreamWriter.
	/// </summary>
	public void EndLog ()
	{
		streamWriter.WriteLine (";");
		streamWriter.Close ();
	}

	#endregion

	#region Reading

	/// <summary>
	/// Opens the game log and loads all of the moves into MapStates.
	/// </summary>
	/// <param name="index">Index.</param>
	public void OpenLog (int index)
	{
		string fileName = "";
		int numberOfTurns = 0;

		streamReader = new StreamReader (logPath + @"\Index.txt");

		//Go down the lines until we get to the line before the index
		for (int i = 0; i < index; i++)
			streamReader.ReadLine ();

		//Now, read the index line in order to get the filename
		fileName = streamReader.ReadLine ();
		streamReader.Close ();

		//Find and open the log file, read the header
		streamReader = new StreamReader (logPath + @"\" + fileName + ".txt");
		string header = streamReader.ReadLine ();
		//Get the number of turns
		while (!streamReader.ReadLine().Contains (";"))
			numberOfTurns++;
		streamReader.Close ();

		//update maxTurn
		maxTurn = numberOfTurns;

		//Load every turn into the MapState array
		streamReader = new StreamReader (logPath + @"\" + fileName + ".txt");
		streamReader.ReadLine ();
		mapState = new MapState[numberOfTurns];
		PopulateTurns (numberOfTurns);
		streamReader.Close ();

		currentTurn = 0;
		gameLoaded = true;

	}

	/// <summary>
	/// Populates the MapState array with the board's state at each turn
	/// </summary>
	/// <param name="numberOfTurns">Number of turns.</param>
	void PopulateTurns (int numberOfTurns)
	{
		string temp;

		boardBehaviour.ResetBoard ();

		for (int turn = 0; turn < numberOfTurns; numberOfTurns++) {

			temp = streamReader.ReadLine ();

			string[] contents = temp.Split (',');

			boardBehaviour.SetTileState (int.Parse (contents [1]), int.Parse (contents [2]), int.Parse (contents [0]), false);

			mapState [turn].map = new int[8, 8];

			Array.Copy (boardBehaviour.board, mapState [turn].map, boardBehaviour.board.Length);

		}

		Array.Copy (mapState [0].map, boardBehaviour.board, mapState [0].map.Length);
	}

	/// <summary>
	/// Sets the board to the state it was at the specified turn.
	/// </summary>
	/// <param name="turn">Turn.</param>
	public void SetTurn (int turn)
	{
		Array.Copy (mapState [turn].map, boardBehaviour.board, mapState [turn].map.Length);
	}

	/// <summary>
	/// Returns a string of five log names, offset by five * the page number
	/// </summary>
	/// <returns>The log names.</returns>
	/// <param name="page">Page.</param>
	public string[] GetLogNames (int page)
	{
		string[] logNames = new string[5];

		streamReader = new StreamReader (logPath + @"\Index.txt");
		for (int temp = 0; temp < 5 * page; temp++)
			streamReader.ReadLine ();
		for (int i = 0; i < 5; i++) {
			try {
				logNames [i] = streamReader.ReadLine ();
			} catch {
				logNames [i] = null;
			}
		}

		return logNames;
	}

	#endregion

	// Update is called once per frame
	void Update ()
	{
		if (gameLoaded) {
			if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
				if (currentTurn > 0) {
					currentTurn--;
					SetTurn (currentTurn);
				}
			}
			if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
				if (currentTurn < maxTurn) {
					currentTurn++;
					SetTurn (currentTurn);
				}
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				gameLoaded = false;
				menuManager = MenuController.GetComponent<MenuManager> ();
				menuManager.GoToMenu ();
			}
		}
	}
	
}
