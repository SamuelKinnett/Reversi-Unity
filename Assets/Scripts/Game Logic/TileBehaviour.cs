using UnityEngine;
using System.Collections;

public class TileBehaviour : MonoBehaviour
{

	public GameObject boardManager;			//The boardBehaviour script to allow access to the current board state
	public GameObject gameController;		//The GameController object to allow access to the players states
	public SpriteRenderer spriteRenderer;	//The Sprite Renderer for this tile
	public int x;							//The X co-ordinate of the tile
	public int y;							//The Y co-ordinate of the tile
	public float fadeSpeed;

	Color tintColour;						//The colour to tint the square with
	int owner;								//1 = player 1, 2 = player 2, 0 = unowned
	float opacity;							//The opacity of the tile

	private BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the BoardManager 
	private GameManager gameManager;
	private PlayerControl player1;
	private PlayerControl player2;

	bool fading;							//Is the tile currently fading to another colour?
	Color currentColour;					//The current colour
	float currentOpacity;					//The current opacity
	float time;								//Used to store a time value for the purpose of fading

	#region Colours

	Color lightCyan = new Color32 (215, 255, 254, 255);
	Color deepCyan = new Color32 (39, 253, 245, 255);
	Color deepPink = new Color32 (247, 101, 184, 255);
	Color lightTan = new Color32 (255, 207, 196, 255);

	#endregion

	// Use this for initialization
	void Start ()
	{
		tintColour = lightCyan;
		opacity = 0.5f;

		boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		gameManager = gameController.GetComponent<GameManager> ();

		player1 = gameManager.GetPlayerControl (1);
		player2 = gameManager.GetPlayerControl (2);

	}
	
	// Update is called once per frame
	void Update ()
	{

		spriteRenderer.material.SetColor ("_Color", currentColour);

		owner = boardBehaviour.GetTileState (x, y);	//Who does this tile belong to?

		switch (owner) {

		case 0:
			tintColour = lightCyan;
			break;

		case 1:
			tintColour = deepPink;
			break;

		case 2:
			tintColour = deepCyan;
			break;
		}

		if (currentColour != tintColour || currentOpacity != opacity) {

			time = 0;
			fading = true;

		} else {

			fading = false;

		}

		if (fading) {

			time += Time.deltaTime;
			currentColour = Color.Lerp (currentColour, tintColour, time * fadeSpeed);
			currentOpacity = Mathf.Lerp (currentOpacity, opacity, time * fadeSpeed);

			currentColour.a = currentOpacity;
		}
	}

	// When the mouse is passed over the tile
	void OnMouseOver ()
	{

		opacity = 1;

	}

	//When the mouse leaves the tile
	void OnMouseExit ()
	{

		opacity = 0.5f;

	}

	//When the tile is clicked
	void OnMouseDown ()
	{

		if (boardBehaviour.currentPlayer == 1 && boardBehaviour.GetTileState (x, y) == 0 && player1.enabled) {
			boardBehaviour.SetTileState (x, y, 1);
			boardBehaviour.TurnComplete ();
		} else if (boardBehaviour.currentPlayer == 2 && boardBehaviour.GetTileState (x, y) == 0 && player2.enabled) {
			boardBehaviour.SetTileState (x, y, 2);
			boardBehaviour.TurnComplete ();
		}
	}
}
