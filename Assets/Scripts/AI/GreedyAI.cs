//A simple greedy AI that plays Reversi. The AI will always pick the tile that will give it the most immediate points.

using UnityEngine;
using System.Collections;

public class GreedyAI : ScriptableObject, AI
{

	GameObject boardManager;		//The BoardManager
	BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the BoardManager
	int playerNumber;				//Is the AI player 1 or player 2?

	// Constructor
	public GreedyAI (GameObject boardManager, int playerNumber)
	{
		this.boardManager = boardManager;
		this.boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		this.playerNumber = playerNumber;
	}
	
	// Method to place a tile on the board.
	public void UpdateAI ()
	{
		int highestX = -1;		//The x co-ordinate of the current highest scoring tile
		int highestY = -1;		//The y co-ordinate of the current highest scoring tile

		int highestScore = 0;	//The highest score found so far
		int currentScore;		//The score of the current tile being compared

		if (boardBehaviour.currentPlayer == playerNumber) {
			for (int y = 0; y < 8; y ++) {
				for (int x = 0; x < 8; x++) {
					if (boardBehaviour.CanPlaceTile (x, y, playerNumber)) {
						currentScore = boardBehaviour.GetTileScore (x, y, playerNumber);

						if (currentScore > highestScore && boardBehaviour.GetTileState (x, y) == 0) {
							highestScore = currentScore;
							highestX = x;
							highestY = y;
						}
					}
				}
			}

			if (highestX != -1 && highestY != -1) {
				boardBehaviour.SetTileState (highestX, highestY, playerNumber);
				boardBehaviour.TurnComplete ();
			}
		}
	}
}
