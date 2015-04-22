//An AI that uses a Monte Carlo search tree to try and predict moves up to thee turns ahead and choose the best option.

using UnityEngine;
using System.Collections;
using System;

public class MonteCarlo : ScriptableObject, AI
{
	GameObject boardManager;
	BoardBehaviour boardBehaviour;
	int playerNumber;

	private struct node
	{
		public int[,] board;
		public int myScore;
		public int enemyScore;
		public node[] next;
	}

	public MonteCarlo (GameObject boardManager, int playerNumber)
	{
		this.boardManager = boardManager;
		this.boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		this.playerNumber = playerNumber;
	}
	
	public void UpdateAI ()
	{
		//The AI simulates it's own turn, then the possible responses from the player.
		node tree = new node ();
		tree.board = new int[8, 8];
		if (playerNumber == 1) {
			tree.myScore = boardBehaviour.player1Score;
			tree.enemyScore = boardBehaviour.player2Score;
		} else {
			tree.myScore = boardBehaviour.player2Score;
			tree.enemyScore = boardBehaviour.player1Score;
		}
		Array.Copy (boardBehaviour.board, tree.board, boardBehaviour.board.Length);

		//Create a tree containing every possible move the AI can make right now and every subsequent move the enemy could make.
		tree.next = GetBranches (tree, 1);

	}

	node[] GetBranches (node root, int depth)
	{
		node[] branches;
		int currentPlayer;
		int numberOfBranches = 0;

		//Prevents the simulation from running too deep.
		if (depth > 2)
			return null;

		//Work out who the current player is
		if (playerNumber == 1)
		if (depth == 1)
			currentPlayer = 1;
		else
			currentPlayer = 2;
		else
			if (depth == 1)
			currentPlayer = 2;
		else
			currentPlayer = 1;

		//Calculate how many possible moves there are and thus how many branches we need
		for (int y = 0; y < 7; y++) {
			for (int x = 0; x < 7; x++) {
				if (boardBehaviour.CanPlaceTile (x, y, currentPlayer))
					numberOfBranches++;
			}
		}

		int currentBranch = 0;
		branches = new node[numberOfBranches];

		//For each possible branch, simulate a turn
		for (int y = 0; y < 7; y++) {
			for (int x = 0; x < 7; x++) {
				if (boardBehaviour.CanPlaceTile (x, y, currentPlayer)) {
					branches [currentBranch] = SimulateTurn (x, y, currentPlayer, root);
				}
			}
		}

		//For each possible branch, get their branches
		for (int i = 0; i < numberOfBranches; i++) {
			branches [i].next = GetBranches (branches [i], depth + 1);
		}

		return branches;
	}

	node SimulateTurn (int x, int y, int player, node root)
	{
		node returnNode = new node ();
		int[,] testMap = new int[8, 8];

		//Copy the root node's board state to a temporary variable
		Array.Copy (root.board, testMap, root.board.Length);

		//Simulate the move
		testMap = boardBehaviour.SetTileState (x, y, player, testMap);

		//Calculate the new scores
		for (int tempY = 0; tempY < 8; tempY++) {
			for (int tempX = 0; tempX < 8; tempX++) {
				if (testMap [tempX, tempY] == playerNumber)
					returnNode.myScore++;
				else if (testMap [tempX, tempY] != 0)
					returnNode.enemyScore++;
			}
		}

		//Return the node
		return returnNode;
	}
}
