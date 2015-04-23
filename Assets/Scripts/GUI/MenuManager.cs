using UnityEngine;
using System.Collections;

enum MenuState
{
	main,
	selectGameType,
	loadGame,
	selectPVA,
	selectAVA1,
	selectAVA2
}

enum MenuOptions1
{
	start,
	load,
	exit
}

enum MenuOptions2
{
	pvp,
	pva,
	ava
}

enum MenuOptions3
{
	load1,
	load2,
	load3,
	load4,
	load5
}

enum AiChoiceOptions
{
	GreedyAI,
	MonteCarloAI
}

public class MenuManager : MonoBehaviour
{

	public GameObject title;

	public GameObject button1Object;
	public GameObject button2Object;
	public GameObject button3Object;
	public GameObject button4Object;
	public GameObject button5Object;
	public GameObject pageIndicator;
	public GameObject versionInfo;

	public GameObject gameController;
	public GameObject boardManager;
	public GameObject endgameInfo;
	public GameObject GameLogger;
	public Camera camera;
	public bool inMenu;

	private BoardBehaviour boardBehaviour;
	private EndgameInfoController endGameInfoController;
	private FileManager fileManager;

	private ButtonController button1;
	private ButtonController button2;
	private ButtonController button3;
	private ButtonController button4;
	private ButtonController button5;

	private ButtonController pageViewer;
	private ButtonController version;

	int currentLoadPage;
	int highestLoadPage;
	GameInfo gameInfo;

	private MenuState menuState;
	private MenuOptions1 menuOptions1;
	private MenuOptions2 menuOptions2;
	private MenuOptions3 menuOptions3;
	private AiChoiceOptions aiChoice1;
	private AiChoiceOptions aiChoice2;
	private bool updateNeeded;

	private TitleManager titleManager;
	private GameManager gameManager;
	private MoveCamera cameraController;

	// Use this for initialization
	void Start ()
	{
		titleManager = title.GetComponent<TitleManager> ();
		gameManager = gameController.GetComponent<GameManager> ();
		fileManager = GameLogger.GetComponent<FileManager> ();
		boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		endGameInfoController = endgameInfo.GetComponent<EndgameInfoController> ();
		cameraController = camera.GetComponent<MoveCamera> ();

		menuState = MenuState.main;
		menuOptions1 = MenuOptions1.start;
		menuOptions2 = MenuOptions2.pvp;
		menuOptions3 = MenuOptions3.load1;
		aiChoice1 = AiChoiceOptions.GreedyAI;
		aiChoice2 = AiChoiceOptions.GreedyAI;

		updateNeeded = true;

		button1 = button1Object.GetComponent<ButtonController> ();
		button2 = button2Object.GetComponent<ButtonController> ();
		button3 = button3Object.GetComponent<ButtonController> ();
		button4 = button4Object.GetComponent<ButtonController> ();
		button5 = button5Object.GetComponent<ButtonController> ();
		pageViewer = pageIndicator.GetComponent<ButtonController> ();
		version = versionInfo.GetComponent<ButtonController> ();

		currentLoadPage = 0;

		button1.selected = true;
		
		titleManager.enabled = true;
		inMenu = true;
		camera.transform.position = new Vector3 (50F, 504F, 20);
		camera.transform.rotation = Quaternion.Euler (0, 0, 0);
		cameraController.rotationMultiplier = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (inMenu && Input.GetKeyDown (KeyCode.Escape) && (menuState == MenuState.selectGameType || menuState == MenuState.loadGame)) {
			menuState = MenuState.main;
			updateNeeded = true;
			
			button1.selected = true;
			button2.selected = false;
			button3.selected = false;
			button4.selected = false;
			button5.selected = false;
			
		} else if (inMenu && Input.GetKeyDown (KeyCode.Escape) && (menuState == MenuState.selectPVA || menuState == MenuState.selectAVA1)) {
			menuState = MenuState.selectGameType;
			updateNeeded = true;

			button1.selected = true;
			button2.selected = false;
			button3.selected = false;
			button4.selected = false;
			button5.selected = false;

		} else if (inMenu && Input.GetKeyDown (KeyCode.Escape) && (menuState == MenuState.selectAVA2)) {
			menuState = MenuState.selectAVA1;
			updateNeeded = true;
			
			button1.selected = true;
			button2.selected = false;
			button3.selected = false;
			button4.selected = false;
			button5.selected = false;
			
		} else if (inMenu && Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();

		if (inMenu) {

			switch (menuState) {

			case MenuState.main:

				if (updateNeeded) {
					titleManager.enabled = true;
					version.CreateButton (0.5F, 0.1F, "Reversi V0.1a, ©1988 Samuel Kinnett Software");
					button1.CreateButton (0.5F, 0.5F, "Start Game");
					button2.CreateButton (0.5F, 0.4F, "Load Game");
					button3.CreateButton (0.5F, 0.3F, "Exit Game");
					button4.enabled = false;
					button5.enabled = false;

					menuOptions1 = MenuOptions1.start;

					button1.selected = true;
					button2.selected = false;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;

					pageViewer.enabled = false;

					updateNeeded = false;
				}

				break;

			case MenuState.selectGameType:

				if (updateNeeded) {
					titleManager.enabled = false;
					version.enabled = false;

					button1.CreateButton (0.5F, 0.6F, "Player v Player");
					button2.CreateButton (0.5F, 0.5F, "Player v AI");
					button3.CreateButton (0.5F, 0.4F, "AI v AI");
					button4.enabled = false;
					button5.enabled = false;

					menuOptions2 = MenuOptions2.pvp;

					button1.selected = true;
					button2.selected = false;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;

					updateNeeded = false;
				}

				break;

			case MenuState.loadGame:

				if (updateNeeded) {

					titleManager.enabled = false;
					version.enabled = false;
					string output;

					GameInfo[] names = fileManager.GetLogNames (currentLoadPage);

					button1.enabled = false;
					button2.enabled = false;
					button3.enabled = false;
					button4.enabled = false;
					button5.enabled = false;

					if (names [0].gameType > -1) {
						output = ReturnGameInfo (names [0]);
						button1.CreateButton (0.5F, 0.65F, output);
						button1.SetScale (0.3F);
					}
					if (names [1].gameType > -1) {
						output = ReturnGameInfo (names [1]);
						button2.CreateButton (0.5F, 0.55F, output);
						button2.SetScale (0.3F);
					}
					if (names [2].gameType > -1) {
						output = ReturnGameInfo (names [2]);
						button3.CreateButton (0.5F, 0.45F, output);
						button3.SetScale (0.3F);
					}
					if (names [3].gameType > -1) {
						output = ReturnGameInfo (names [3]);
						button4.CreateButton (0.5F, 0.35F, output);
						button4.SetScale (0.3F);
					}
					if (names [4].gameType > -1) {
						output = ReturnGameInfo (names [4]);
						button5.CreateButton (0.5F, 0.25F, output);
						button5.SetScale (0.3F);
					}

					menuOptions3 = MenuOptions3.load1;

					button1.selected = true;
					button2.selected = false;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;

					highestLoadPage = fileManager.GetNumberOfPages ();

					pageViewer.CreateButton (0.5F, 0.1F, "Page " + (currentLoadPage + 1) + " / " + (highestLoadPage + 1));

					updateNeeded = false;
				}
				break;

			case MenuState.selectPVA:

				if (updateNeeded) {

					titleManager.enabled = false;
					version.enabled = false;

					button1.CreateButton (0.5F, 0.8F, "Select Opponent");
					button2.CreateButton (0.5F, 0.6F, "Greedy AI");
					button3.CreateButton (0.5F, 0.5F, "Monte-Carlo AI");
					button4.enabled = false;
					button5.enabled = false;
					
					aiChoice1 = AiChoiceOptions.GreedyAI;
					
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					updateNeeded = false;
				}
				break;

			case MenuState.selectAVA1:
				
				if (updateNeeded) {
					
					titleManager.enabled = false;
					version.enabled = false;
					
					button1.CreateButton (0.5F, 0.8F, "Select First AI");
					button2.CreateButton (0.5F, 0.6F, "Greedy AI");
					button3.CreateButton (0.5F, 0.5F, "Monte-Carlo AI");
					button4.enabled = false;
					button5.enabled = false;
					
					aiChoice1 = AiChoiceOptions.GreedyAI;
					
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					updateNeeded = false;
				}
				break;

			case MenuState.selectAVA2:
				
				if (updateNeeded) {
					
					titleManager.enabled = false;
					version.enabled = false;
					
					button1.CreateButton (0.5F, 0.8F, "Select Second AI");
					button2.CreateButton (0.5F, 0.6F, "Greedy AI");
					button3.CreateButton (0.5F, 0.5F, "Monte-Carlo AI");
					button4.enabled = false;
					button5.enabled = false;
					
					aiChoice2 = AiChoiceOptions.GreedyAI;
					
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					updateNeeded = false;
				}
				break;

			}
		} else {
			if (Input.GetKeyDown (KeyCode.Escape) && boardBehaviour.winner != 0) {
				inMenu = true;
				endGameInfoController.enabled = false;
				GoToMenu ();
			} else {
				//TODO: Add code for a pause menu
			}
		}

		/*
		if (start.clicked) {
			start.clickHandled = true;
			menuState = MenuState.selectGameType;
			updateNeeded = true;
		}
		if (exit.clicked) {
			exit.clickHandled = true;
			Application.Quit ();
		}
		*/

		if (inMenu && (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S))) {
			switch (menuState) {

			case MenuState.main:

				switch (menuOptions1) {

				case MenuOptions1.start:

					menuOptions1 = MenuOptions1.load;
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;

					break;

				case MenuOptions1.load:

					menuOptions1 = MenuOptions1.exit;
					button1.selected = false;
					button2.selected = false;
					button3.selected = true;
					button4.selected = false;
					button5.selected = false;

					break;

				}

				break;

			case MenuState.selectGameType:

				switch (menuOptions2) {

				case MenuOptions2.pvp:

					menuOptions2 = MenuOptions2.pva;
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;

					break;

				case MenuOptions2.pva:

					menuOptions2 = MenuOptions2.ava;
					button1.selected = false;
					button2.selected = false;
					button3.selected = true;
					button4.selected = false;
					button5.selected = false;
					
					break;

				}

				break;

			case MenuState.loadGame:

				switch (menuOptions3) {

				case MenuOptions3.load1:

					if (button2.enabled) {
						menuOptions3 = MenuOptions3.load2;
						button1.selected = false;
						button2.selected = true;
						button3.selected = false;
						button4.selected = false;
						button5.selected = false;
					}

					break;

				case MenuOptions3.load2:
					
					if (button3.enabled) {
						menuOptions3 = MenuOptions3.load3;
						button1.selected = false;
						button2.selected = false;
						button3.selected = true;
						button4.selected = false;
						button5.selected = false;
					}
					
					break;

				case MenuOptions3.load3:
					
					if (button4.enabled) {
						menuOptions3 = MenuOptions3.load4;
						button1.selected = false;
						button2.selected = false;
						button3.selected = false;
						button4.selected = true;
						button5.selected = false;
					}
					
					break;

				case MenuOptions3.load4:
					
					if (button5.enabled) {
						menuOptions3 = MenuOptions3.load5;
						button1.selected = false;
						button2.selected = false;
						button3.selected = false;
						button4.selected = false;
						button5.selected = true;
					}
					
					break;

				}

				break;

			case MenuState.selectPVA:

				switch (aiChoice1) {

				case AiChoiceOptions.GreedyAI:
					aiChoice1 = AiChoiceOptions.MonteCarloAI;
					button1.selected = false;
					button2.selected = false;
					button3.selected = true;
					button4.selected = false;
					button5.selected = false;

					break;

				}

				break;

			case MenuState.selectAVA1:

				switch (aiChoice1) {
					
				case AiChoiceOptions.GreedyAI:
					aiChoice1 = AiChoiceOptions.MonteCarloAI;
					button1.selected = false;
					button2.selected = false;
					button3.selected = true;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				}

				break;

			case MenuState.selectAVA2:

				switch (aiChoice2) {
					
				case AiChoiceOptions.GreedyAI:
					aiChoice2 = AiChoiceOptions.MonteCarloAI;
					button1.selected = false;
					button2.selected = false;
					button3.selected = true;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				}

				break;

			}
		}

		if (inMenu && (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))) {
			switch (menuState) {
				
			case MenuState.main:
				
				switch (menuOptions1) {
					
				case MenuOptions1.load:
					
					menuOptions1 = MenuOptions1.start;
					button1.selected = true;
					button2.selected = false;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				case MenuOptions1.exit:
					
					menuOptions1 = MenuOptions1.load;
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				}
				break;
				
			case MenuState.selectGameType:
				
				switch (menuOptions2) {
					
				case MenuOptions2.pva:

					menuOptions2 = MenuOptions2.pvp;
					button1.selected = true;
					button2.selected = false;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				case MenuOptions2.ava:

					menuOptions2 = MenuOptions2.pva;
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				}
				
				break;

			case MenuState.loadGame:
				
				switch (menuOptions3) {
					
				case MenuOptions3.load2:
					
					if (button1.enabled) {
						menuOptions3 = MenuOptions3.load1;
						button1.selected = true;
						button2.selected = false;
						button3.selected = false;
						button4.selected = false;
						button5.selected = false;
					}
					
					break;
					
				case MenuOptions3.load3:
					
					if (button2.enabled) {
						menuOptions3 = MenuOptions3.load2;
						button1.selected = false;
						button2.selected = true;
						button3.selected = false;
						button4.selected = false;
						button5.selected = false;
					}
					
					break;
					
				case MenuOptions3.load4:
					
					if (button3.enabled) {
						menuOptions3 = MenuOptions3.load3;
						button1.selected = false;
						button2.selected = false;
						button3.selected = true;
						button4.selected = false;
						button5.selected = false;
					}
					
					break;
					
				case MenuOptions3.load5:
					
					if (button4.enabled) {
						menuOptions3 = MenuOptions3.load4;
						button1.selected = false;
						button2.selected = false;
						button3.selected = false;
						button4.selected = true;
						button5.selected = false;
					}
					
					break;
					
				}
				
				break;

			case MenuState.selectPVA:
				
				switch (aiChoice1) {
					
				case AiChoiceOptions.MonteCarloAI:
					aiChoice1 = AiChoiceOptions.GreedyAI;
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				}
				
				break;
				
			case MenuState.selectAVA1:
				
				switch (aiChoice1) {
					
				case AiChoiceOptions.MonteCarloAI:
					aiChoice1 = AiChoiceOptions.GreedyAI;
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				}
				
				break;
				
			case MenuState.selectAVA2:
				
				switch (aiChoice2) {
					
				case AiChoiceOptions.MonteCarloAI:
					aiChoice2 = AiChoiceOptions.GreedyAI;
					button1.selected = false;
					button2.selected = true;
					button3.selected = false;
					button4.selected = false;
					button5.selected = false;
					
					break;
					
				}
				
				break;
				
			}
		}

		if (inMenu && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space))) {

			switch (menuState) {

			case MenuState.main:
				switch (menuOptions1) {

				case MenuOptions1.start:
					button1.selected = false;
					menuState = MenuState.selectGameType;
					updateNeeded = true;
					break;

				case MenuOptions1.load:
					button2.selected = false;
					menuState = MenuState.loadGame;
					updateNeeded = true;
					break;

				case MenuOptions1.exit:
					Application.Quit ();
					break;
				}
				break;

			case MenuState.selectGameType:
				switch (menuOptions2) {

				case MenuOptions2.pvp:
					StartGame (0);
					break;

				case MenuOptions2.pva:
					menuState = MenuState.selectPVA;
					updateNeeded = true;
					//StartGame (1);
					break;

				case MenuOptions2.ava:
					menuState = MenuState.selectAVA1;
					updateNeeded = true;
					//StartGame (2);
					break;
				}
				break;

			case MenuState.loadGame:

				switch (menuOptions3) {

				case MenuOptions3.load1:
					fileManager.OpenLog (currentLoadPage * 5);
					StartGame (3);
					break;

				case MenuOptions3.load2:
					fileManager.OpenLog ((currentLoadPage * 5) + 1);
					StartGame (3);
					break;

				case MenuOptions3.load3:
					fileManager.OpenLog ((currentLoadPage * 5) + 2);
					StartGame (3);
					break;

				case MenuOptions3.load4:
					fileManager.OpenLog ((currentLoadPage * 5) + 3);
					StartGame (3);
					break;

				case MenuOptions3.load5:
					fileManager.OpenLog ((currentLoadPage * 5) + 4);
					StartGame (3);
					break;

				}
				break;

			case MenuState.selectPVA:
				StartGame (1, (int)aiChoice1 + 1);
				break;

			case MenuState.selectAVA1:
				menuState = MenuState.selectAVA2;
				updateNeeded = true;
				break;

			case MenuState.selectAVA2:
				StartGame (2, (int)aiChoice1 + 1, (int)aiChoice2 + 1);
				break;

			}
		}

		if (inMenu && (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) && menuState == MenuState.loadGame) {
			if (currentLoadPage > 0) {
				currentLoadPage--;
				updateNeeded = true;
			}
		}

		if (inMenu && (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) && menuState == MenuState.loadGame) {
			if (currentLoadPage < highestLoadPage) {
				currentLoadPage++;
				updateNeeded = true;
			}
		}
	}

	string ReturnGameInfo (GameInfo info)
	{
		string output = "";

		output = info.date + " - ";

		switch (info.gameType) {
		case 0:
			output += "PvP, ";
			break;
			
		case 1:
			output += "PvA, ";
			break;
			
		case 2:
			output += "AvA, ";
			break;
		}

		output += info.turns + " turns, ";

		switch (info.victor) {

		case 1:
			output += "P1 win";
			break;

		case 2:
			output += "P2 win";
			break;

		case 3:
			output += "Draw";
			break;
		}

		return output;
	}

	public void GoToMenu ()
	{
		try {
			fileManager.EndLog ();
		} catch {
			Debug.Log ("Tried to close a non-existant filestream");
		}
		titleManager.enabled = true;
		boardBehaviour.winner = 0;
		endGameInfoController.enabled = false;
		inMenu = true;
		menuState = MenuState.main;
		menuOptions1 = MenuOptions1.start;
		menuOptions2 = MenuOptions2.pvp;
		camera.transform.position = new Vector3 (50F, 50F, 20);
		camera.transform.rotation = Quaternion.Euler (0, 0, 0);
		cameraController.rotationMultiplier = 0;
		gameManager.EndGame ();
		boardBehaviour.ResetBoard ();
		updateNeeded = true;

		button1.selected = true;
		button2.selected = false;
		button3.selected = false;
		button4.selected = false;
		button5.selected = false;
	}

	public void StartGame (int gameType, int AI1 = 0, int AI2 = 0)
	{
		boardBehaviour.filemanager = fileManager;
		if (gameType != 3)
			fileManager.CreateLog (gameType);
		titleManager.enabled = false;
		inMenu = false;
		camera.transform.position = new Vector3 (-0.64F, -0.64F, -10);
		cameraController.rotationMultiplier = 2.5F;
		gameManager.StartGame (gameType, AI1, AI2);
	}
}
