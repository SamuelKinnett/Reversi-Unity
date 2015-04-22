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

		tree.next = GetBranches (tree, 1);

	}

	node[] GetBranches (node root, int depth)
	{
		node[] branches;
		int currentPlayer;
		int numberOfBranches;

		//Prevents the simulation from running too deep.
		if (depth > 2)
			return null;

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


		for (int y = 0; y < 7; y++) {
			for (int x = 0; x < 7; x++) {
				if (boardBehaviour.CanPlaceTile (x, y, currentPlayer))
					numberOfBranches++;
			}
		}

		int currentBranch = 0;
		branches = new node[numberOfBranches];

		for (int y = 0; y < 7; y++) {
			for (int x = 0; x < 7; x++) {
				if (boardBehaviour.CanPlaceTile (x, y, currentPlayer)) {
					branches [currentBranch] = SimulateTurn (x, y, currentPlayer, root);
				}
			}
		}

		return branches;
	}

	node SimulateTurn (int x, int y, int player, node root)
	{
		int[,] testMap = new int[8, 8];

		Array.Copy (root.board, testMap, root.board.Length);
	}
}
