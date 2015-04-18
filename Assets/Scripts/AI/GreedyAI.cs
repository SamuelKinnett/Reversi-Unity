//A simple greedy AI that plays Reversi. The AI will always pick the tile that will give it the most immediate points.

using UnityEngine;
using System.Collections;

public class GreedyAI : ScriptableObject, AI
{

	GameObject boardManager;		//The BoardManager
	BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the BoardManager
	int playerNumber;				//Is the AI player 1 or player 2?
	bool firstMove;					//Is this the first move of the AI?

	// Constructor
	public GreedyAI (GameObject boardManager, int playerNumber)
	{
		this.boardManager = boardManager;
		this.boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		this.playerNumber = playerNumber;
		firstMove = true;
	}
	
	// Method to place a tile on the board.
	public void UpdateAI ()
	{
		if (firstMove && boardBehaviour.currentPlayer == playerNumber) {
			//Place the first tile randomly.

			System.Random rand = new System.Random ();
			int xChoice = rand.Next (0, 8);
			int yChoice = rand.Next (0, 8);

			boardBehaviour.SetTileState (xChoice, yChoice, playerNumber);
			boardBehaviour.TurnComplete ();
			firstMove = false;
		} else {
			int highestX = 0;		//The x co-ordinate of the current highest scoring tile
			int highestY = 0;		//The y co-ordinate of the current highest scoring tile

			int highestScore = 0;	//The highest score found so far
			int currentScore;		//The score of the current tile being compared

			if (boardBehaviour.currentPlayer == playerNumber) {
				for (int y = 0; y < 8; y ++) {
					for (int x = 0; x < 8; x++) {
						currentScore = boardBehaviour.GetTileScore (x, y, playerNumber);

						if (currentScore > highestScore && boardBehaviour.GetTileState (x, y) == 0) {
							highestScore = currentScore;
							highestX = x;
							highestY = y;
						}
					}
				}

				boardBehaviour.SetTileState (highestX, highestY, playerNumber);
				boardBehaviour.TurnComplete ();
			}
		}
	}
}
