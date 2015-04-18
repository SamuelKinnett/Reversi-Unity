//A simple greedy AI that plays Reversi. The AI will always pick the tile that will give it the most immediate points.

using UnityEngine;
using System.Collections;

public class GreedyAI : MonoBehaviour {

	public GameObject boardManager;			//The BoardManager
	private BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the BoardManager
	public int playerNumber;				//is the AI player 1 or player 2?

	// Use this for initialization
	void Start () {
		boardBehaviour = boardManager.GetComponent<BoardBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {

		int highestX = 0;		//The x co-ordinate of the current highest scoring tile
		int highestY = 0;		//The y co-ordinate of the current highest scoring tile

		int highestScore = 0;	//The highest score found so far
		int currentScore;		//The score of the current tile being compared

		if (boardBehaviour.currentPlayer == playerNumber) {
			for (int y = 0; y < 8; y ++) {
				for (int x = 0; x < 8; x++) {
					currentScore = boardBehaviour.GetTileScore(x, y, playerNumber);

					if (currentScore > highestScore && boardBehaviour.GetTileState(x, y) == 0) {
						highestScore = currentScore;
						highestX = x;
						highestY = y;
					}
				}
			}

			boardBehaviour.SetTileState(highestX, highestY, playerNumber);
			boardBehaviour.TurnComplete();
		}

	}
}
