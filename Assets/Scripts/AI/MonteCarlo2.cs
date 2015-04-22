//A very basic Monte-Carlo style AI with a search depth of two
using UnityEngine;
using System.Collections;
using System;

public class MonteCarlo2 : ScriptableObject, AI
{
	GameObject boardManager;		//The BoardManager
	BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the BoardManager
	int playerNumber;				//Is the AI player 1 or player 2?
	int enemyNumber;				//What is the number of the other player?

	struct GameState
	{
		public int[,] board;
	}

	// Constructor
	public MonteCarlo2 (GameObject boardManager, int playerNumber)
	{
		this.boardManager = boardManager;
		this.boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		this.playerNumber = playerNumber;
	}
	
	// Method to place a tile on the board.
	public void UpdateAI ()
	{
		GameState gameState = new GameState ();
		int bestX = 0;		//The x co-ordinate of the best tile
		int bestY = 0;		//The y co-ordinate of the best tile
		
		int highestScore = -1;	//The highest score found so far
		int currentScore = 0;		//The score of the current tile being compared

		//Find all tiles that can be placed
		if (boardBehaviour.currentPlayer == playerNumber) {
			for (int y = 0; y < 8; y ++) {
				for (int x = 0; x < 8; x++) {
					if (boardBehaviour.CanPlaceTile (x, y, playerNumber)) {

						//Simulate what would happen if the ai placed a tile here and then the opponent picked the highest scoring tile.

						gameState.board = new int[8, 8];

						int tempHighestX = 0;
						int tempHighestY = 0;
						int tempHighestScore = 0;
						currentScore = 0;

						Array.Copy (boardBehaviour.board, gameState.board, boardBehaviour.board.Length);
						gameState.board = boardBehaviour.SetTileState (x, y, playerNumber, gameState.board);

						for (int x2 = 0; x2 < 8; x2++) {
							for (int y2 = 0; y2 < 8; y2++) {
								if (boardBehaviour.GetTileScore (x2, y2, enemyNumber) > tempHighestScore) {
									tempHighestX = x2;
									tempHighestY = y2;
									tempHighestScore = boardBehaviour.GetTileScore (x2, y2, enemyNumber, gameState.board);
								}
							}
						}

						gameState.board = boardBehaviour.SetTileState (x, y, enemyNumber, gameState.board);

						//Calculate the score

						for (int y2 = 0; y2 < 7; y2++) {
							for (int x2 = 0; x2 < 7; x2++) {
								if (gameState.board [x2, y2] == playerNumber)
									currentScore++;
							}
						}

						if (currentScore > highestScore) {
							bestX = x;
							bestY = y;
							highestScore = currentScore;
						}
					}
				}
			}
			
			boardBehaviour.SetTileState (bestX, bestY, playerNumber);
			boardBehaviour.TurnComplete ();
		}
	}
}
