//An AI that uses a Monte Carlo search tree to try and predict moves up to thee turns ahead and choose the best option.

using UnityEngine;
using System.Collections;
using System;

public class MonteCarlo : ScriptableObject, AI
{
	GameObject boardManager;
	BoardBehaviour boardBehaviour;
	int playerNumber;

	private struct Node
	{
		public int[,] board;
		public int x;
		public int y;
		public int myScore;
		public int enemyScore;
		public Node[] next;
	}

	public MonteCarlo (GameObject boardManager, int playerNumber)
	{
		this.boardManager = boardManager;
		this.boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		this.playerNumber = playerNumber;
	}
	
	public void UpdateAI ()
	{
		Node worstNode;
		Node bestNode;


		if (boardBehaviour.currentPlayer == playerNumber) {
			//The AI simulates it's own turn, then the possible responses from the player.
			Node tree = new Node ();
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

			Node[] BestWorstNode = FindWorstAndBestNode (tree);

			bestNode = BestWorstNode [0];
			worstNode = BestWorstNode [1];

			boardBehaviour.SetTileState (bestNode.x, bestNode.y, playerNumber, true);
			boardBehaviour.TurnComplete ();

		}
	}

	Node[] FindWorstAndBestNode (Node root)
	{
		bool firstNode = true;				//Used to determine if the currently examined node is the first since it will therefore become the worst and best node.
		Node worstNode = root.next [0].next [0];
		float currentWorstNodePointScore = 0;
		Node bestNode = root.next [0].next [0];
		float currentBestNodePointScore = 0;

		//Loop through each node representing the AI's possible choices
		foreach (Node aiMove in root.next) {

			float pointTotal = 0;
			float pointAverage = 0;
			int totalMoves = 0;
			//Loop through each node representing the enemy player's possible moves
			foreach (Node enemyMove in aiMove.next) {
				pointTotal += (enemyMove.myScore - enemyMove.enemyScore);
				totalMoves++;
			}
			pointAverage = pointTotal / totalMoves;

			if (firstNode) {
				firstNode = false;
				bestNode = aiMove;
				worstNode = aiMove;
				currentBestNodePointScore = pointAverage;
				currentWorstNodePointScore = pointAverage;
			} else {
				if (pointAverage > currentBestNodePointScore) {
					currentBestNodePointScore = pointAverage;
					bestNode = aiMove;
				}
				if (pointAverage < currentWorstNodePointScore) {
					currentWorstNodePointScore = pointAverage;
					worstNode = aiMove;
				}
			}
		}

		return new Node[] {bestNode, worstNode};
	}

	Node[] GetBranches (Node root, int depth)
	{
		Node[] branches;
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
				if (boardBehaviour.CanPlaceTile (x, y, currentPlayer, root.board))
					numberOfBranches++;
			}
		}

		int currentBranch = 0;
		branches = new Node[numberOfBranches];

		//For each possible branch, simulate a turn
		for (int y = 0; y < 7; y++) {
			for (int x = 0; x < 7; x++) {
				if (boardBehaviour.CanPlaceTile (x, y, currentPlayer, root.board)) {
					Debug.Log ("Simulating move at " + x + "," + y + ", move " + currentBranch + "/" + numberOfBranches);
					branches [currentBranch] = new Node ();
					branches [currentBranch].board = new int[8, 8];
					branches [currentBranch] = SimulateTurn (x, y, currentPlayer, root);
					Debug.Log ("Successful run, depth " + depth);
					currentBranch++;
				}
			}
		}

		//For each possible branch, get their branches
		for (int i = 0; i < numberOfBranches; i++) {
			branches [i].next = GetBranches (branches [i], depth + 1);
		}

		return branches;
	}

	Node SimulateTurn (int x, int y, int player, Node root)
	{
		Node returnNode = new Node ();
		int[,] testMap = new int[8, 8];

		//Copy the root node's board state to a temporary variable
		Array.Copy (root.board, testMap, root.board.Length);

		Debug.Log ("Simulating move");
		//Simulate the move
		returnNode.board = boardBehaviour.SetTileState (x, y, player, testMap);

		//Save the actual co-ordinates of the move

		returnNode.x = x;
		returnNode.y = y;

		//Calculate the new scores
		for (int tempY = 0; tempY < 8; tempY++) {
			for (int tempX = 0; tempX < 8; tempX++) {
				if (returnNode.board [tempX, tempY] == playerNumber)
					returnNode.myScore++;
				else if (returnNode.board [tempX, tempY] != 0)
					returnNode.enemyScore++;
			}
		}

		//Return the node
		return returnNode;
	}


}
