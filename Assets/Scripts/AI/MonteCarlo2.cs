//A very basic Monte-Carlo style AI with a search depth of four.
using UnityEngine;
using System.Collections;
using System;

public class MonteCarlo2 : ScriptableObject, AI
{
	GameObject boardManager;		//The BoardManager
	BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the BoardManager
	int playerNumber;				//Is the AI player 1 or player 2?
	int enemyNumber;				//What is the number of the other player?

	//The placement structure is used to define a possible move that can be taken and the board that would result.
	struct Placement
	{
		public int x;
		public int y;
		public int[,] board;
	}

	// Constructor
	public MonteCarlo2 (GameObject boardManager, int playerNumber)
	{
		this.boardManager = boardManager;
		this.boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		this.playerNumber = playerNumber;

		if (playerNumber == 1)
			enemyNumber = 2;
		else
			enemyNumber = 1;
	}
	
	// Method to place a tile on the board.
	public void UpdateAI ()
	{
		int bestX = 0;		//The x co-ordinate of the best tile
		int bestY = 0;		//The y co-ordinate of the best tile
		
		int highestScore = -1;	//The highest score found so far
		int currentScore = 0;		//The score of the current tile being compared

		if (boardBehaviour.currentPlayer == playerNumber) {

			//Find all tiles that can be placed
			Placement[] firstTurnPlacements = GetPossiblePlacements (boardBehaviour.board, playerNumber);

			if (firstTurnPlacements != null) {

				//For each placement, assume the enemy picks the best scoring tile and calculate the result accordingly
				for (int i = 0; i < firstTurnPlacements.Length; i++) {
					firstTurnPlacements [i] = FindHighestScoringMove (firstTurnPlacements [i].board, enemyNumber, firstTurnPlacements [i].x, firstTurnPlacements [i].y);
				}
				//We now have an array containing the state of the gameboard corresponding to the possible move after 1 turn.
				//Next, we will simulate all possible moves stemming from each of these first moves. 

				//The secondTurnPlacements array is jagged to allow an array corresponding to each first move to be associated with each firstTurnPlacement
				Placement[][] secondTurnPlacements = new Placement[firstTurnPlacements.Length][];

				//Get all possible placements for each first turn placement
				for (int i = 0; i < firstTurnPlacements.Length; i++) {
					secondTurnPlacements [i] = GetPossiblePlacements (firstTurnPlacements [i].board, playerNumber);
					if (secondTurnPlacements [i] != null) {
						//Next, for each second turn placement, assume the enemy picks the best scoring tile and calculate the result accordingly
						for (int c = 0; c < secondTurnPlacements[i].Length; c++) {
							secondTurnPlacements [i] [c] = FindHighestScoringMove (secondTurnPlacements [i] [c].board, enemyNumber, secondTurnPlacements [i] [c].x, secondTurnPlacements [i] [c].y);
						}
					}
				}

				//Check to see if the first element of secondTurnPlacements is null. If this is the case, it means that the next move is the last that can be played.
				//We will therefore then just play the first move in the firstTurnPlacements.

				if (secondTurnPlacements [0] == null) {
					boardBehaviour.SetTileState (firstTurnPlacements [0].x, firstTurnPlacements [0].y, playerNumber);
					boardBehaviour.TurnComplete ();
				} else {
			

					//We now have a tree of sorts that contains all possible states of the game after two moves have been made, assuming the enemy player picks the highest scoring tile.
					//Next, we will look at the percentage difference in score that each of the first choices could potentially lead to.
					//We do this by going through every second turn scenario, finding the average score (positive or negative) and then comparing this to the current score.

					float[] potentialScores = new float[firstTurnPlacements.Length];

					for (int i = 0; i < firstTurnPlacements.Length; i++) {

						float scoreTotal = 0;

						for (int c = 0; c < secondTurnPlacements[i].Length; c++) {
							scoreTotal += GetScore (secondTurnPlacements [i] [c].board, playerNumber) - GetScore (secondTurnPlacements [i] [c].board, enemyNumber);
						}

						//Get the average score.
						potentialScores [i] = scoreTotal / secondTurnPlacements [i].Length;
					}

					Debug.Log ("Scores: " + potentialScores);

					//Now that we know all of the average scores that will result from each possible move, we can pick the move that will give us the best average score.

					float tempBestScore = potentialScores [0];
					int currentTurnChoice = 0;

					for (int i = 0; i < potentialScores.Length; i++) {
						if (potentialScores [i] > tempBestScore) {
							currentTurnChoice = i;
							tempBestScore = potentialScores [i];
						}
					}

					//Now, let's extract the information required to take the turn

					boardBehaviour.SetTileState (firstTurnPlacements [currentTurnChoice].x, firstTurnPlacements [currentTurnChoice].y, playerNumber);
					boardBehaviour.TurnComplete ();
				}
				/*
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

						//Now, repeat this process again.

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
			*/
				//boardBehaviour.SetTileState (bestX, bestY, playerNumber);
				//boardBehaviour.TurnComplete ();
			} else {
				boardBehaviour.TurnComplete ();
			}
		}
	}

	/// <summary>
	/// Gets the possible placements on the passed in board and returns them as an array of Placement structures.
	/// </summary>
	/// <returns>The possible placements.</returns>
	/// <param name="gameBoard">Game board.</param>
	/// <param name="player">Player.</param>
	Placement[] GetPossiblePlacements (int[,] gameBoard, int player)
	{
		Placement[] possiblePlacements;
		int numberOfPossiblePlacements = 0;

		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				if (boardBehaviour.CanPlaceTile (x, y, playerNumber, gameBoard)) {
					numberOfPossiblePlacements++;
				}
			}
		}

		if (numberOfPossiblePlacements == 0)
			return null;

		possiblePlacements = new Placement[numberOfPossiblePlacements];
		int currentPlacementNumber = 0;

		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				if (boardBehaviour.CanPlaceTile (x, y, playerNumber, gameBoard)) {
					possiblePlacements [currentPlacementNumber] = new Placement ();
					possiblePlacements [currentPlacementNumber].x = x;
					possiblePlacements [currentPlacementNumber].y = y;

					possiblePlacements [currentPlacementNumber].board = new int[8, 8];
					int[,] tempBoard = boardBehaviour.SetTileState (x, y, player, gameBoard);
					Array.Copy (tempBoard, possiblePlacements [currentPlacementNumber].board, tempBoard.Length);

					currentPlacementNumber++;
				}
			}
		}

		return possiblePlacements;
	}

	/// <summary>
	/// Finds the highest scoring move, then simulates placing it. Returns a Placement structure.
	/// </summary>
	/// <returns>The highest scoring move.</returns>
	/// <param name="gameBoard">Game board.</param>
	/// <param name="player">Player.</param>
	Placement FindHighestScoringMove (int[,] gameBoard, int player, int originalX, int originalY)
	{
		Placement highestScoringMove = new Placement ();
		int tempHighestX = 0;
		int tempHighestY = 0;
		int tempHighestScore = -1;
		int currentScore = 0;

		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				currentScore = 0;
				if (boardBehaviour.CanPlaceTile (x, y, player, gameBoard)) {
					currentScore = boardBehaviour.GetTileScore (x, y, player, gameBoard);
				}
				if (currentScore > tempHighestScore) {
					tempHighestX = x;
					tempHighestY = y;
					tempHighestScore = currentScore;
				}
			}
		}

		highestScoringMove.x = originalX;
		highestScoringMove.y = originalY;

		highestScoringMove.board = new int[8, 8];
		int[,] tempBoard = boardBehaviour.SetTileState (tempHighestX, tempHighestY, player, gameBoard);
		Array.Copy (tempBoard, highestScoringMove.board, tempBoard.Length);

		return highestScoringMove;
	}

	/// <summary>
	/// Returns the score of the current board for the passed player.
	/// </summary>
	/// <returns>The score.</returns>
	/// <param name="gameBoard">Game board.</param>
	/// <param name="player">Player.</param>
	int GetScore (int[,] gameBoard, int player)
	{
		int score = 0;

		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				if (gameBoard [x, y] == player)
					score++;
			}
		}

		return score;
	}
}
