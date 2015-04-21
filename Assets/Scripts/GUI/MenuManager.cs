using UnityEngine;
using System.Collections;

enum MenuState
{
	main,
	selectGameType,
	loadGame
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

public class MenuManager : MonoBehaviour
{

	public GameObject title;

	public GameObject startButton;
	public GameObject loadButton;
	public GameObject exitButton;
	public GameObject pvpButton;
	public GameObject pvaButton;
	public GameObject avaButton;
	public GameObject loadButton1;
	public GameObject loadButton2;
	public GameObject loadButton3;
	public GameObject loadButton4;
	public GameObject loadButton5;

	public GameObject gameController;
	public GameObject boardManager;
	public GameObject endgameInfo;
	public GameObject GameLogger;
	public Camera camera;
	public bool inMenu;

	private BoardBehaviour boardBehaviour;
	private EndgameInfoController endGameInfoController;
	private FileManager fileManager;

	private ButtonController start;
	private ButtonController load;
	private ButtonController exit;
	private ButtonController pvp;
	private ButtonController pva;
	private ButtonController ava;
	private ButtonController load1;
	private ButtonController load2;
	private ButtonController load3;
	private ButtonController load4;
	private ButtonController load5;
	int currentLoadPage;
	int highestLoadPage;

	private MenuState menuState;
	private MenuOptions1 menuOptions1;
	private MenuOptions2 menuOptions2;
	private MenuOptions3 menuOptions3;
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

		updateNeeded = true;

		start = startButton.GetComponent<ButtonController> ();
		load = loadButton.GetComponent<ButtonController> ();
		exit = exitButton.GetComponent<ButtonController> ();
		pvp = pvpButton.GetComponent<ButtonController> ();
		pva = pvaButton.GetComponent<ButtonController> ();
		ava = avaButton.GetComponent<ButtonController> ();

		load1 = loadButton1.GetComponent<ButtonController> ();
		load2 = loadButton2.GetComponent<ButtonController> ();
		load3 = loadButton3.GetComponent<ButtonController> ();
		load4 = loadButton4.GetComponent<ButtonController> ();
		load5 = loadButton5.GetComponent<ButtonController> ();
		currentLoadPage = 0;

		start.selected = true;
		
		titleManager.enabled = true;
		inMenu = true;
		camera.transform.position = new Vector3 (50F, 504F, 20);
		camera.transform.rotation = Quaternion.Euler (0, 0, 0);
		cameraController.rotationMultiplier = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (inMenu && Input.GetKeyDown (KeyCode.Escape) && menuState == MenuState.selectGameType) {
			menuState = MenuState.main;
			updateNeeded = true;
			
			pvp.selected = false;
			pva.selected = false;
			ava.selected = false;
			
		} else if (inMenu && Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();

		if (inMenu) {

			switch (menuState) {

			case MenuState.main:

				if (updateNeeded) {
					titleManager.enabled = true;
					start.CreateButton (0.5F, 0.5F, "Start Game");
					load.CreateButton (0.5F, 0.4F, "Load Game");
					exit.CreateButton (0.5F, 0.3F, "Exit Game");

					start.selected = true;
					exit.selected = false;

					pvp.enabled = false;
					pva.enabled = false;
					ava.enabled = false;

					load1.enabled = false;
					load2.enabled = false;
					load3.enabled = false;
					load4.enabled = false;
					load5.enabled = false;

					updateNeeded = false;
				}

				break;

			case MenuState.selectGameType:

				if (updateNeeded) {
					titleManager.enabled = false;
					start.enabled = false;
					load.enabled = false;
					exit.enabled = false;
					pvp.CreateButton (0.5F, 0.6F, "Player v Player");
					pva.CreateButton (0.5F, 0.5F, "Player v AI");
					ava.CreateButton (0.5F, 0.4F, "AI v AI");

					pvp.selected = true;
					pva.selected = false;
					ava.selected = false;

					updateNeeded = false;
				}

				break;

			case MenuState.loadGame:

				if (updateNeeded) {

					titleManager.enabled = false;
					start.enabled = false;
					load.enabled = false;
					exit.enabled = false;

					string[] names = fileManager.GetLogNames (currentLoadPage);

					if (names [0] != null)
						load1.CreateButton (0.5F, 0.65F, names [0]);
					else
						highestLoadPage = currentLoadPage;
					if (names [1] != null)
						load2.CreateButton (0.5F, 0.55F, names [1]);
					else
						highestLoadPage = currentLoadPage;
					if (names [2] != null)
						load3.CreateButton (0.5F, 0.45F, names [2]);
					else
						highestLoadPage = currentLoadPage;
					if (names [3] != null)
						load4.CreateButton (0.5F, 0.35F, names [3]);
					else
						highestLoadPage = currentLoadPage;
					if (names [4] != null)
						load5.CreateButton (0.5F, 0.25F, names [4]);
					else
						highestLoadPage = currentLoadPage;

					load1.selected = true;
					load2.selected = false;
					load3.selected = false;
					load4.selected = false;
					load5.selected = false;

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
					start.selected = false;
					load.selected = true;
					exit.selected = false;

					break;

				case MenuOptions1.load:

					menuOptions1 = MenuOptions1.exit;
					start.selected = false;
					load.selected = false;
					exit.selected = true;

					break;

				}

				break;

			case MenuState.selectGameType:

				switch (menuOptions2) {

				case MenuOptions2.pvp:

					menuOptions2 = MenuOptions2.pva;
					pvp.selected = false;
					pva.selected = true;
					ava.selected = false;

					break;

				case MenuOptions2.pva:

					menuOptions2 = MenuOptions2.ava;
					pvp.selected = false;
					pva.selected = false;
					ava.selected = true;
					
					break;

				}

				break;

			case MenuState.loadGame:

				switch (menuOptions3) {

				case MenuOptions3.load1:

					if (load2.enabled) {
						menuOptions3 = MenuOptions3.load2;
						load1.selected = false;
						load2.selected = true;
						load3.selected = false;
						load4.selected = false;
						load5.selected = false;
					}

					break;

				case MenuOptions3.load2:
					
					if (load3.enabled) {
						menuOptions3 = MenuOptions3.load3;
						load1.selected = false;
						load2.selected = false;
						load3.selected = true;
						load4.selected = false;
						load5.selected = false;
					}
					
					break;

				case MenuOptions3.load3:
					
					if (load4.enabled) {
						menuOptions3 = MenuOptions3.load4;
						load1.selected = false;
						load2.selected = false;
						load3.selected = false;
						load4.selected = true;
						load5.selected = false;
					}
					
					break;

				case MenuOptions3.load4:
					
					if (load5.enabled) {
						menuOptions3 = MenuOptions3.load5;
						load1.selected = false;
						load2.selected = false;
						load3.selected = false;
						load4.selected = false;
						load5.selected = true;
					}
					
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
					start.selected = true;
					load.selected = false;
					exit.selected = false;
					
					break;
					
				case MenuOptions1.exit:
					
					menuOptions1 = MenuOptions1.load;
					start.selected = false;
					load.selected = true;
					exit.selected = false;
					
					break;
					
				}
				break;
				
			case MenuState.selectGameType:
				
				switch (menuOptions2) {
					
				case MenuOptions2.pva:

					menuOptions2 = MenuOptions2.pvp;
					pvp.selected = true;
					pva.selected = false;
					ava.selected = false;
					
					break;
					
				case MenuOptions2.ava:

					menuOptions2 = MenuOptions2.pva;
					pvp.selected = false;
					pva.selected = true;
					ava.selected = false;
					
					break;
					
				}
				
				break;

			case MenuState.loadGame:
				
				switch (menuOptions3) {
					
				case MenuOptions3.load2:
					
					if (load1.enabled) {
						menuOptions3 = MenuOptions3.load1;
						load1.selected = true;
						load2.selected = false;
						load3.selected = false;
						load4.selected = false;
						load5.selected = false;
					}
					
					break;
					
				case MenuOptions3.load3:
					
					if (load2.enabled) {
						menuOptions3 = MenuOptions3.load2;
						load1.selected = false;
						load2.selected = true;
						load3.selected = false;
						load4.selected = false;
						load5.selected = false;
					}
					
					break;
					
				case MenuOptions3.load4:
					
					if (load3.enabled) {
						menuOptions3 = MenuOptions3.load3;
						load1.selected = false;
						load2.selected = false;
						load3.selected = true;
						load4.selected = false;
						load5.selected = false;
					}
					
					break;
					
				case MenuOptions3.load5:
					
					if (load4.enabled) {
						menuOptions3 = MenuOptions3.load4;
						load1.selected = false;
						load2.selected = false;
						load3.selected = false;
						load4.selected = true;
						load5.selected = false;
					}
					
					break;
					
				}
				
				break;

				
			}
		}

		if (inMenu && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space))) {
			if (start.selected) {
				start.selected = false;
				menuState = MenuState.selectGameType;
				updateNeeded = true;
			}
			if (load.selected) {
				load.selected = false;
				menuState = MenuState.loadGame;
				updateNeeded = true;
			}
			if (exit.selected)
				Application.Quit ();
			if (pvp.selected)
				StartGame (0);
			if (pva.selected)
				StartGame (1);
			if (ava.selected)
				StartGame (2);
			if (load1.selected) {
				fileManager.OpenLog (currentLoadPage * 5);
				StartGame (3);
			}
			if (load2.selected) {
				fileManager.OpenLog ((currentLoadPage * 5) + 1);
				StartGame (3);
			}
			if (load3.selected) {
				fileManager.OpenLog ((currentLoadPage * 5) + 2);
				StartGame (3);
			}
			if (load4.selected) {
				fileManager.OpenLog ((currentLoadPage * 5) + 3);
				StartGame (3);
			}
			if (load5.selected) {
				fileManager.OpenLog ((currentLoadPage * 5) + 4);
				StartGame (3);
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
	
	public void GoToMenu ()
	{
		fileManager.EndLog ();
		titleManager.enabled = true;
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

		start.selected = true;
		exit.selected = false;
		pvp.selected = false;
		pva.selected = false;
		ava.selected = false;
	}

	public void StartGame (int gameType)
	{
		boardBehaviour.filemanager = fileManager;
		fileManager.CreateLog (gameType);
		titleManager.enabled = false;
		inMenu = false;
		camera.transform.position = new Vector3 (-0.64F, -0.64F, -10);
		cameraController.rotationMultiplier = 2.5F;
		gameManager.StartGame (gameType);
	}
}
