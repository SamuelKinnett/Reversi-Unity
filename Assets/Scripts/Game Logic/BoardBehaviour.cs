using UnityEngine;
using System.Collections;

public class BoardBehaviour : MonoBehaviour
{
	public FileManager filemanager;

	public int currentPlayer;	//1 = player 1, 2 = player 2, 0 = neither (this can be used to add waiting times and to pause / stop the game), 3 = switching
	public int[,] board;		//The gameboard. A 0 indicates an uncaptured tile, whilst a 1 or a 2 indicates ownership by player 1 or 2 respectively.

	int nextPlayer;				//The player to swith to after waiting
	public int player1Score;	//Player 1's score
	public int player2Score;	//Player 2's score
	float time;					//Used to wait after a turn has been played
	bool switching;				//True when the turn is currently switching
	bool movePossible;			//True if the current player is capable of moving

	public int winner;			//0 = no current winner, 1 = player 1, 2 = player 2, 3 = draw

	// Use this for initialization
	void Start ()
	{

		board = new int[8, 8];
		board [3, 3] = 1;
		board [3, 4] = 2;
		board [4, 3] = 2;
		board [4, 4] = 1;

		movePossible = true;

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

		if (boardFull || !movePossible) {
			currentPlayer = 0;

			if (player1NewScore > player2NewScore)
				winner = 1;
			else if (player2NewScore > player1NewScore)
				winner = 2;
			else
				winner = 3;
		} else
			winner = 0;

		player1Score = player1NewScore;
		player2Score = player2NewScore;
		
	}

	//Returns true if the current player can make a move
	private bool MovePossible (int currentPlayer)
	{
		if (currentPlayer == 0)
			return true;

		bool canMove = false;

		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				if (GetTileScore (x, y, currentPlayer) > 0)
					canMove = true;
			}
		}

		return canMove;
	}

	//Get the owner of the tile at the passed co-ordinates
	public int GetTileState (int x, int y)
	{

		return board [x, y];

	}

	public bool CanPlaceTile (int x, int y, int owner)
	{
		if (GetTileScore (x, y, owner) > 0)
			return true;
		else
			return false;
	}

	//Set the state of the tile at the passed co-ordinates and update all other tiles accordingly
	public void SetTileState (int x, int y, int owner, bool log = true)
	{
		if (log)
			filemanager.AddTurnToLog (x, y, owner);
		int enemy;
		board [x, y] = owner;

		if (owner == 1)
			enemy = 2;
		else
			enemy = 1;

		#region Test Right
		int length = 0;
		bool endFound = false;
		bool endSearch = false;
		bool enemyPassed = false;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			
			if ((board [currentX, y] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [currentX, y] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				board [currentX, y] = owner;
			}
		}
		#endregion

		#region Test Left
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;

		for (int currentX = x - 1; currentX >= 0; currentX --) {
			
			if ((board [currentX, y] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [currentX, y] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			length++;
			for (int currentX = x - 1; currentX > x - length; currentX --) {
				board [currentX, y] = owner;
			}
		}
		#endregion

		#region Test Upwards
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;



		for (int currentY = y - 1; currentY >= 0; currentY --) {
			
			if ((board [x, currentY] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [x, currentY] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			length++;
			for (int currentY = y - 1; currentY >= y - length; currentY --) {
				board [x, currentY] = owner;
			}
		}
		#endregion

		#region Test Downwards
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		for (int currentY = y + 1; currentY < 8; currentY ++) {
			
			if ((board [x, currentY] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [x, currentY] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			length++;
			for (int currentY = y + 1; currentY < y + length; currentY ++) {
				board [x, currentY] = owner;
			}
		}
		#endregion

		#region Test Up-Right
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;

		int diagY = y - 1;

		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY > 0) {

				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}
				diagY--;
			}
		}

		diagY = y - 1;

		if (endFound && enemyPassed) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				if (diagY > 0) {
					board [currentX, diagY] = owner;
					diagY--;
				}
			}
		}
		#endregion

		#region Up-Left
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		diagY = y - 1;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY > 0) {

				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}
				diagY--;
			}
		}
		
		diagY = y - 1;
		
		if (endFound) {
			length++;
			for (int currentX = x - 1; currentX > x - length; currentX --) {
				if (diagY > 0) {
					board [currentX, diagY] = owner;
					diagY--;
				}
			}
		}
		#endregion

		#region Down-Right
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		diagY = y + 1;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY < 7) {

				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}
				diagY++;
			}
		}
		
		diagY = y + 1;
		
		if (endFound) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				if (diagY < 7) {
					board [currentX, diagY] = owner;
					diagY++;
				}
			}
		}
		#endregion

		#region Down-Left
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		diagY = y + 1;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY < 7) {
				
				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}
				diagY++;
			}
		}
		
		diagY = y + 1;
		
		if (endFound && enemyPassed) {
			length++;
			for (int currentX = x - 1; currentX >= x - length; currentX --) {
				if (diagY < 7) {
					board [currentX, diagY] = owner;
					diagY++;
				}
			}
		}
		#endregion

	}

	//Returns an integer representing the number of tiles that will be converted should a tile be placed at the passed co-ordinates
	public int GetTileScore (int x, int y, int owner)
	{

		int score = 0;	//The ammount of tiles that will be converted should this tile be captured.
		int enemy;

		if (owner == 1)
			enemy = 2;
		else
			enemy = 1;

		#region Test Right
		int length = 0;
		bool endFound = false;
		bool endSearch = false;
		bool enemyPassed = false;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			
			if ((board [currentX, y] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [currentX, y] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			score += length;
		}
		#endregion

		#region Test Left
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			
			if ((board [currentX, y] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [currentX, y] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			score += length;
		}
		#endregion
		
		#region Test Upwards
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		for (int currentY = y - 1; currentY >= 0; currentY --) {
			
			if ((board [x, currentY] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [x, currentY] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			score += length;
		}
		#endregion
		
		#region Test Downwards
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		for (int currentY = y + 1; currentY < 8; currentY ++) {
			
			if ((board [x, currentY] == owner || endFound == true) && !endSearch) {
				endFound = true;
				endSearch = true;
			} else if (board [x, currentY] == enemy && !endSearch) {
				enemyPassed = true;
				length++;
			} else {
				endSearch = true;
			}
		}
		if (endFound && enemyPassed) {
			score += length;
		}
		#endregion
		
		#region Test Up-Right
		length = 0;
		endFound = false;
		endSearch = false;
		enemyPassed = false;
		
		int diagY = y - 1;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY > 0) {
				
				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}
				diagY--;
			}
		}
		
		diagY = y;
		
		if (endFound && enemyPassed) {
			score += length;
		}
		#endregion
		
		#region Up-Left
		length = 0;
		endSearch = false;
		endFound = false;
		
		diagY = y - 1;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY > 0) {
				
				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}

				diagY--;
			}
		}
		
		diagY = y;
		
		if (endFound && enemyPassed) {
			score += length;
		}
		#endregion
		
		#region Down-Right
		length = 0;
		endSearch = false;
		endFound = false;
		
		diagY = y + 1;
		
		for (int currentX = x + 1; currentX < 8; currentX ++) {
			if (diagY < 7) {
				
				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}
				diagY++;
			}
		}
		
		diagY = y;
		
		if (endFound && enemyPassed) {
			score += length;
		}
		#endregion
		
		#region Down-Left
		length = 0;
		endSearch = false;
		endFound = false;
		
		diagY = y + 1;
		
		for (int currentX = x - 1; currentX >= 0; currentX --) {
			if (diagY < 7) {
				
				if ((board [currentX, diagY] == owner || endFound == true) && !endSearch) {
					endFound = true;
					endSearch = true;
				} else if (board [currentX, diagY] == enemy && !endSearch) {
					enemyPassed = true;
					length++;
				} else {
					endSearch = true;
				}
				diagY++;
			}
		}
		
		diagY = y;
		
		if (endFound && enemyPassed) {
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
				
		currentPlayer = 3;
		switching = true;
		movePossible = MovePossible (nextPlayer);
		time = 0;
	}

	public void ResetBoard ()
	{
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				board [x, y] = 0;
			}
		}
		board [3, 3] = 1;
		board [3, 4] = 2;
		board [4, 3] = 2;
		board [4, 4] = 1;

		player1Score = 0;
		player2Score = 0;
		winner = 0;
		currentPlayer = 1;
	}
}
