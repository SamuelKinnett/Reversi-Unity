using UnityEngine;
using System.Collections;
using System.IO;
using System;

public struct GameInfo
{
	public int turns;		//How many turns the game lasted
	public int gameType;	//What type of game was played
	public int aiChoice1;	//What AI was chosen as player 1? (if applicable)
	public int aiChoice2;	//What AI was chosen as player 2? (if applicable)
	public int victor;		//Which player won?
	public string date;		//The date the game was played

}

public class FileManager : MonoBehaviour
{
	private struct MapState
	{
		public int[,] map;
	}

	public GameObject BoardManager;
	public GameObject MenuController;
	public GameObject replayInfoObject;
	private BoardBehaviour boardBehaviour;
	private MenuManager menuManager;
	private ButtonController replayInfo;

	StreamReader streamReader;
	StreamWriter streamWriter;
	string[] buffer;			//Used to store turns that need to be written to an output file.

	GameInfo gameInfo;			//Structure containing information about the current game
	string gameName;			//The name of the current game
	int gameType;				//The type of game taking place
	int aiChoice1;				//The choice of AI for player 1 (if applicable)
	int aiChoice2;				//The choice of AI for player 2 (if applicable)
	int turns;					//The number of turns the current game has taken
	string date;				//The date the game was played

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
		replayInfo = replayInfoObject.GetComponent<ButtonController> ();
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
		System.DateTime dateTime;

		this.gameType = gameType;
		this.aiChoice1 = aiChoice1;
		this.aiChoice2 = aiChoice2;
		turns = 0;

		gameName = "";
		buffer = new string[64];

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
		dateTime = System.DateTime.Now;
		date = dateTime.ToString ("dd/MM/yyyy");
		gameName = dateTime.ToString ("dd-MM-yyyy_HH-mm-ss_") + typeOfGame;
		File.Create (logPath + @"\" + gameName + ".txt").Close ();
		Debug.Log ("File Created");
	}

	/// <summary>
	/// Adds the turn to the game log.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="playerNumber">Player number.</param>
	public void AddTurnToLog (int x, int y, int playerNumber)
	{
		buffer [turns] = (playerNumber + "," + x + "," + y);
		turns++;
	}

	/// <summary>
	/// Ends the game log and writes the buffer to an output file.
	/// </summary>
	public void EndLog ()
	{
		//Write the header
		streamWriter = new StreamWriter (logPath + @"\" + gameName + ".txt");
		streamWriter.WriteLine (gameType + ":" + turns + ":" + aiChoice1 + ":" + aiChoice2 + ":" + boardBehaviour.winner + ":" + date);

		//Now write the buffer to the file
		for (int i = 0; i < turns; i++) {
			streamWriter.WriteLine (buffer [i]);
		}

		//Add the terminator and then close the stream
		streamWriter.WriteLine (";");
		streamWriter.Close ();

		//Add the file to the index
		streamWriter = new StreamWriter (logPath + @"\Index.txt", true);
		streamWriter.WriteLine (gameName + ":" + gameType + ":" + turns + ":" + aiChoice1 + ":" + aiChoice2 + ":" + boardBehaviour.winner + ":" + date);
		streamWriter.Close ();
		nextFreeIndex++;
	}

	#endregion

	#region Reading

	/// <summary>
	/// Opens the game log and loads all of the moves into MapStates. Returns a GameInfo structure containing information about the game.
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
		string[] fileInfo = streamReader.ReadLine ().Split (':');
		fileName = fileInfo [0];
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
		mapState = new MapState[numberOfTurns + 1];
		PopulateTurns (numberOfTurns);
		streamReader.Close ();

		currentTurn = 0;
		gameLoaded = true;

		//Extract game information from header

		gameInfo = new GameInfo ();
		string[] headerInfo = header.Split (':');

		gameInfo.gameType = int.Parse (headerInfo [0]);
		gameInfo.turns = int.Parse (headerInfo [1]);
		gameInfo.aiChoice1 = int.Parse (headerInfo [2]);
		gameInfo.aiChoice2 = int.Parse (headerInfo [3]);
		gameInfo.victor = int.Parse (headerInfo [4]);
		gameInfo.date = headerInfo [5];

	}

	public int GetNumberOfPages ()
	{
		return (Mathf.CeilToInt (nextFreeIndex / 5F) - 1);
	}

	/// <summary>
	/// Populates the MapState array with the board's state at each turn
	/// </summary>
	/// <param name="numberOfTurns">Number of turns.</param>
	void PopulateTurns (int numberOfTurns)
	{
		string temp;

		boardBehaviour.ResetBoard ();

		mapState [0].map = new int[8, 8];

		mapState [0].map [3, 3] = 1;
		mapState [0].map [3, 4] = 2;
		mapState [0].map [4, 3] = 2;
		mapState [0].map [4, 4] = 1;

		for (int turn = 1; turn < numberOfTurns + 1; turn++) {

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
	public GameInfo[] GetLogNames (int page)
	{
		string[] fileInfo;
		GameInfo[] logNames = new GameInfo[5];

		streamReader = new StreamReader (logPath + @"\Index.txt");

		//Go through each line until we reach the line before the page we want to return
		for (int temp = 0; temp < 5 * page; temp++) {
			streamReader.ReadLine ();
		}
		//Now extract the information
		for (int i = 0; i < 5; i++) {
			try {
				fileInfo = streamReader.ReadLine ().Split (':');
				logNames [i].gameType = int.Parse (fileInfo [1]);
				logNames [i].turns = int.Parse (fileInfo [2]);
				logNames [i].aiChoice1 = int.Parse (fileInfo [3]);
				logNames [i].aiChoice2 = int.Parse (fileInfo [4]);
				logNames [i].victor = int.Parse (fileInfo [5]);
				logNames [i].date = fileInfo [6];
			} catch {
				logNames [i].gameType = -1;
			}
		}

		streamReader.Close ();

		return logNames;
	}

	#endregion

	// Update is called once per frame
	void Update ()
	{
		if (gameLoaded) {

			replayInfo.CreateButton (0.8F, 0.9F, ReturnGameInfo ());

			// Go back a turn if the left arrow or the A key are pressed.
			if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
				if (currentTurn > 0) {
					currentTurn--;
					SetTurn (currentTurn);
				}
			}
			//Move forward a turn if the right arrow or the D key are pressed.
			if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
				if (currentTurn < maxTurn) {
					currentTurn++;
					SetTurn (currentTurn);
				}
			}
			//Move forward 10 turns if the up arrow or the W key are pressed.
			if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
				if ((currentTurn + 10) < maxTurn) {
					currentTurn += 10;
					SetTurn (currentTurn);
				} else {
					currentTurn = maxTurn;
					SetTurn (currentTurn);
				}
			}
			//Move backwards 10 turns if the down arrow or the S key are pressed.
			if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
				if ((currentTurn - 10) > 0) {
					currentTurn -= 10;
					SetTurn (currentTurn);
				} else {
					currentTurn = 0;
					SetTurn (currentTurn);
				}
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				gameLoaded = false;
				menuManager = MenuController.GetComponent<MenuManager> ();
				menuManager.GoToMenu ();
			}
		} else
			replayInfo.enabled = false;
	}
		
	string ReturnGameInfo ()
	{
		string output = "";
			
		output = "Turn: " + currentTurn + " / " + gameInfo.turns;
		
		return output;
	}


	
}
