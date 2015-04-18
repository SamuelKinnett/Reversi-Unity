using UnityEngine;
using System.Collections;

public class BoardBehaviour : MonoBehaviour
{

	public int currentPlayer;	//1 = player 1, 2 = player 2, 0 = neither (this can be used to add waiting times and to pause / stop the game)
	int[,] board;				//The gameboard. A 0 indicates an uncaptured tile, whilst a 1 or a 2 indicates ownership by player 1 or 2 respectively.

	int nextPlayer;				//The player to swith to after waiting
	public int player1Score;	//Player 1's score
	public int player2Score;	//Player 2's score
	float time;					//Used to wait after a turn has been played
	bool switching;				//True when the turn is currently switching

	public int winner;					//0 = no current winner, 1 = player 1, 2 = player 2, 3 = draw

	// Use this for initialization
	void Start ()
	{

		board = new int[8, 8];

	}
	
	// Update is called once per frame
	void Update ()
	{
		bool boardFull = true;
		time += Time.deltaTime;
		
		if (switching) {
			if (time >= 1) {
				currentPlayer = nextPlayer;
				switching = false;
			}
		}

		int player1NewScore = 0;
		int player2NewScore = 0;
		
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				switch (board [x, y]) {
				case 0:
					boardFull = false;
					break;

				case 1:
					player1NewScore++;
					break;

				case 2:
					player2NewScore++;
					break;
				}
			}
		}

		if (boardFull) {
			currentPlayer = 0;

			if (player1NewScore > player2NewScore)
				winner = 1;
			else if (player2NewScore < player1NewScore)
				winner = 2;
			else
				winner = 3;
		}

		player1Score = player1NewScore;
		player2Score = player2NewScore;
		
	}

	//Get the owner of the tile at the passed co-ordinates
	public int GetTileState (int x, int y)
	{

		return board [x, y];

	}

	//Set the state of the tile at the passed co-ordinates and update all other tiles accordingly
	public void SetTileState (int x, int y, int owner)
	{

		board [x, y] = owner;

		#region Test Right
		int length = 0;
		bool endFound = false;

		for (int currentX = x + 1; currentX < 8; currentX ++) {

			if (board [currentX, y] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				board [currentX, y] = owner;
			}
		}
		#endregion

		#region Test Left
		length = 0;
		endFound = false;

		for (int currentX = x - 1; currentX >= 0; currentX --) {
			
			if (board [currentX, y] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentX = x - 1; currentX > x - length; currentX --) {
				board [currentX, y] = owner;
			}
		}
		#endregion

		#region Test Upwards
		length = 0;
		endFound = false;


		for (int currentY = y - 1; currentY >= 0; currentY --) {
			
			if (board [x, currentY] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentY = y - 1; currentY >= y - length; currentY --) {
				board [x, currentY] = owner;
			}
		}
		#endregion

		#region Test Downwards
		length = 0;
		endFound = false;
		
		
		for (int currentY = y + 1; currentY < 8; currentY ++) {
			
			if (board [x, currentY] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentY = y + 1; currentY < y + length; currentY ++) {
				board [x, currentY] = owner;
			}
		}
		#endregion

		#region Test Up-Right
		length = 0;
		endFound = false;

		int diagY = y;

		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY > 0) {
				diagY--;

				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}

		diagY = y;

		if (endFound) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				if (diagY > 0) {
					diagY--;
					board [currentX, diagY] = owner;
				}
			}
		}
		#endregion

		#region Up-Left
		length = 0;
		endFound = false;
		
		diagY = y;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY > 0) {
				diagY--;
				
				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			length++;
			for (int currentX = x - 1; currentX > x - length; currentX --) {
				if (diagY > 0) {
					diagY--;
					board [currentX, diagY] = owner;
				}
			}
		}
		#endregion

		#region Down-Right
		length = 0;
		endFound = false;
		
		diagY = y;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY < 7) {
				diagY++;
				
				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				if (diagY < 7) {
					diagY++;
					board [currentX, diagY] = owner;
				}
			}
		}
		#endregion

		#region Down-Left
		length = 0;
		endFound = false;
		
		diagY = y;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY < 7) {
				diagY++;
				
				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			length++;
			for (int currentX = x - 1; currentX >= x - length; currentX --) {
				if (diagY < 7) {
					diagY++;
					board [currentX, diagY] = owner;
				}
			}
		}
		#endregion

	}

	//Returns an integer representing the number of tiles that will be converted should a tile be placed at the passed co-ordinates
	public int GetTileScore (int x, int y, int owner)
	{

		int score = 1;	//The ammount of tiles that will be converted should this tile be captured. Default 1 since 1 will always be captured.

		#region Test Right
		int length = 0;
		bool endFound = false;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			
			if (board [currentX, y] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			score += length;
		}
		#endregion

		#region Test Left
		length = 0;
		endFound = false;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			
			if (board [currentX, y] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			score += length;
		}
		#endregion
		
		#region Test Upwards
		length = 0;
		endFound = false;
		
		
		for (int currentY = y - 1; currentY >= 0; currentY --) {
			
			if (board [x, currentY] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			score += length;
		}
		#endregion
		
		#region Test Downwards
		length = 0;
		endFound = false;
		
		
		for (int currentY = y + 1; currentY < 8; currentY ++) {
			
			if (board [x, currentY] == owner || endFound == true) {
				endFound = true;
			} else {
				length++;
			}
		}
		if (endFound) {
			score += length;
		}
		#endregion
		
		#region Test Up-Right
		length = 0;
		endFound = false;
		
		int diagY = y;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY > 0) {
				diagY--;
				
				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			score += length;
		}
		#endregion
		
		#region Up-Left
		length = 0;
		endFound = false;
		
		diagY = y;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY > 0) {
				diagY--;
				
				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			score += length;
		}
		#endregion
		
		#region Down-Right
		length = 0;
		endFound = false;
		
		diagY = y;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY < 7) {
				diagY++;
				
				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			score += length;
		}
		#endregion
		
		#region Down-Left
		length = 0;
		endFound = false;
		
		diagY = y;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY < 7) {
				diagY++;
				
				if (board [currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			score += length;
		}
		#endregion


		return score;
	}

	public void TurnComplete ()
	{
		if (currentPlayer == 1)
			nextPlayer = 2;
		else
			nextPlayer = 1;
				
		currentPlayer = 0;
		switching = true;
		time = 0;
	}
}
