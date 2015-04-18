using UnityEngine;
using System.Collections;
using System;

public class TextBehaviour : MonoBehaviour
{

	int contents;
	public TextMesh textMesh;				//The textbox
	public GameObject boardControl;			//The gameboard
	public GameObject gameController;		//The GameManager

	private BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the gameboard
	private GameManager gameManager;
	private PlayerControl player1;
	private PlayerControl player2;
	private System.Random rand;

	public int x;							//The x position of the textbox
	public int y;							//The y position of the textbox

	// Use this for initialization
	void Start ()
	{
		contents = 0;
		boardBehaviour = boardControl.GetComponent<BoardBehaviour> ();
		gameManager = gameController.GetComponent<GameManager> ();

		player1 = gameManager.GetPlayerControl (1);
		player2 = gameManager.GetPlayerControl (2);

		rand = new System.Random ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		int currentPlayerToDisplay = 1;

		if (player1.enabled && player2.enabled) {
			currentPlayerToDisplay = boardBehaviour.currentPlayer;
		}

		if (boardBehaviour.GetTileState (x, y) == 0) {
			contents = boardBehaviour.GetTileScore (x, y, currentPlayerToDisplay);
			if (contents < 10)
				textMesh.text = contents.ToString ();
			else if (contents > 9)
				textMesh.text = "+";
			else
				textMesh.text = "";
		} else
			textMesh.text = "";

		Color tempColour = textMesh.color;
		tempColour.a = (0.3f + 0.1f * contents) + (0.05F - (float)(rand.NextDouble () / 10));
		textMesh.color = tempColour;
	}
}
