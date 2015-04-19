using UnityEngine;
using System.Collections;

enum MenuState
{
	main,
	selectGameType
}

enum MenuOptions1
{
	start,
	exit
}

enum MenuOptions2
{
	pvp,
	pva,
	ava
}

public class MenuManager : MonoBehaviour
{

	public GameObject title;

	public GameObject startButton;
	public GameObject exitButton;
	public GameObject pvpButton;
	public GameObject pvaButton;
	public GameObject avaButton;

	public GameObject gameController;
	public GameObject boardManager;
	public GameObject endgameInfo;
	public Camera camera;
	public bool inMenu;

	private BoardBehaviour boardBehaviour;
	private EndgameInfoController endGameInfoController;

	private ButtonController start;
	private ButtonController exit;
	private ButtonController pvp;
	private ButtonController pva;
	private ButtonController ava;

	private MenuState menuState;
	private MenuOptions1 menuOptions1;
	private MenuOptions2 menuOptions2;
	private bool updateNeeded;

	private TitleManager titleManager;
	private GameManager gameManager;
	private MoveCamera cameraController;

	// Use this for initialization
	void Start ()
	{
		titleManager = title.GetComponent<TitleManager> ();
		gameManager = gameController.GetComponent<GameManager> ();
		boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		endGameInfoController = endgameInfo.GetComponent<EndgameInfoController> ();
		cameraController = camera.GetComponent<MoveCamera> ();

		menuState = MenuState.main;
		menuOptions1 = MenuOptions1.start;
		menuOptions2 = MenuOptions2.pvp;

		updateNeeded = true;

		start = startButton.GetComponent<ButtonController> ();
		exit = exitButton.GetComponent<ButtonController> ();
		pvp = pvpButton.GetComponent<ButtonController> ();
		pva = pvaButton.GetComponent<ButtonController> ();
		ava = avaButton.GetComponent<ButtonController> ();

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
					exit.CreateButton (0.5F, 0.4F, "Exit Game");

					start.selected = true;
					exit.selected = false;

					pvp.enabled = false;
					pva.enabled = false;
					ava.enabled = false;
					updateNeeded = false;
				}

				break;

			case MenuState.selectGameType:

				if (updateNeeded) {
					titleManager.enabled = false;
					start.enabled = false;
					exit.enabled = false;
					pvp.CreateButton (0.5F, 0.6F, "Start Game");
					pva.CreateButton (0.5F, 0.5F, "Start Game");
					ava.CreateButton (0.5F, 0.4F, "Start Game");

					pvp.selected = true;
					pva.selected = false;
					ava.selected = false;

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

				start.selected = false;
				exit.selected = true;

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

			}
		}

		if (inMenu && (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))) {
			switch (menuState) {
				
			case MenuState.main:
				
				start.selected = true;
				exit.selected = false;
				
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
				
			}
		}

		if (inMenu && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space))) {
			if (start.selected) {
				start.selected = false;
				menuState = MenuState.selectGameType;
				updateNeeded = true;
			}
			if (exit.selected) {
				Application.Quit ();
			}
			if (pvp.selected) {
				StartGame (0);
			}
			if (pva.selected) {
				StartGame (1);
			}
			if (ava.selected) {
				StartGame (2);
			}

		}
	}
	
	public void GoToMenu ()
	{
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
		titleManager.enabled = false;
		inMenu = false;
		camera.transform.position = new Vector3 (-0.64F, -0.64F, -10);
		cameraController.rotationMultiplier = 2.5F;
		gameManager.StartGame (gameType);
	}
}
