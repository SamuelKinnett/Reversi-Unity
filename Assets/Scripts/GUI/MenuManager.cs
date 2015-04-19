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
	public Camera camera;
	public bool inMenu;

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

	private float cooldownTimer;				//Used to wait for the key to be released.

	// Use this for initialization
	void Start ()
	{
		titleManager = title.GetComponent<TitleManager> ();
		gameManager = gameController.GetComponent<GameManager> ();
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
		if (inMenu) {

			cooldownTimer += Time.deltaTime;

			switch (menuState) {

			case MenuState.main:

				if (updateNeeded) {
					titleManager.enabled = true;
					start.CreateButton (0.5F, 0.5F, "Start Game");
					exit.CreateButton (0.5F, 0.4F, "Exit Game");
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
					pvp.CreateButton (0.5F, 0.55F, "Start Game");
					pva.CreateButton (0.5F, 0.45F, "Start Game");
					ava.CreateButton (0.5F, 0.35F, "Start Game");
					updateNeeded = false;
				}

				break;
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

		if (cooldownTimer > 0.1) {
			if (inMenu && Input.GetKey (KeyCode.Escape) && menuState == MenuState.selectGameType) {
				menuState = MenuState.main;
				updateNeeded = true;
			} else if (inMenu)
				Application.Quit ();

			if (inMenu && (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S))) {
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

			if (inMenu && (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W))) {
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

			if (inMenu && (Input.GetKey (KeyCode.Return) || Input.GetKey (KeyCode.Space))) {

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
