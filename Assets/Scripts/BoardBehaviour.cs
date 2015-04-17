using UnityEngine;
using System.Collections;

public class BoardBehaviour : MonoBehaviour {

	public int currentPlayer;	//1 = player 1, 2 = player 2
	int [,] board;				//The gameboard. A 0 indicates an uncaptured tile, whilst a 1 or a 2 indicates ownership by player 1 or 2 respectively.

	// Use this for initialization
	void Start () {

		board = new int[8, 8];

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Get the owner of the tile at the passed co-ordinates
	public int GetTileState(int x, int y) {

		return board [x, y];

	}

	//Set the state of the tile at the passed co-ordinates and update all other tiles accordingly
	public void SetTileState(int x, int y, int owner) {

		board [x, y] = owner;

		#region Test Right
		int length = 0;
		bool endFound = false;

		for (int currentX = x + 1; currentX < 8; currentX ++) {

			if (board[currentX, y] == owner || endFound == true) {
				endFound = true;
			}
			else
			{
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				board[currentX, y] = owner;
			}
		}
		#endregion

		#region Test Left
		length = 0;
		endFound = false;

		for (int currentX = x - 1; currentX >= 0; currentX --) {
			
			if (board[currentX, y] == owner || endFound == true) {
				endFound = true;
			}
			else
			{
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentX = x - 1; currentX > x - length; currentX --) {
				board[currentX, y] = owner;
			}
		}
		#endregion

		#region Test Upwards
		length = 0;
		endFound = false;


		for (int currentY = y - 1; currentY >= 0; currentY --) {
			
			if (board[x, currentY] == owner || endFound == true) {
				endFound = true;
			}
			else
			{
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentY = y - 1; currentY >= y - length; currentY --) {
				board[x, currentY] = owner;
			}
		}
		#endregion

		#region Test Downwards
		length = 0;
		endFound = false;
		
		
		for (int currentY = y + 1; currentY < 8; currentY ++) {
			
			if (board[x, currentY] == owner || endFound == true) {
				endFound = true;
			}
			else
			{
				length++;
			}
		}
		if (endFound) {
			length++;
			for (int currentY = y + 1; currentY < y + length; currentY ++) {
				board[x, currentY] = owner;
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

			if (board[currentX, diagY] == owner || endFound == true)
				endFound = true;
			else
				length++;
			}
		}

		diagY = y;

		if (endFound) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				if (diagY > 0)
				{
					diagY--;
					board[currentX, diagY] = owner;
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
				
			if (board[currentX, diagY] == owner || endFound == true)
				endFound = true;
			else
				length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			length++;
			for (int currentX = x - 1; currentX > x - length; currentX --) {
				if (diagY > 0)
				{
					diagY--;
					board[currentX, diagY] = owner;
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
				
				if (board[currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			length++;
			for (int currentX = x + 1; currentX < x + length; currentX ++) {
				if (diagY < 7)
				{
					diagY++;
					board[currentX, diagY] = owner;
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
				
				if (board[currentX, diagY] == owner || endFound == true)
					endFound = true;
				else
					length++;
			}
		}
		
		diagY = y;
		
		if (endFound) {
			length++;
			for (int currentX = x - 1; currentX >= x - length; currentX --) {
				if (diagY < 7)
				{
					diagY++;
					board[currentX, diagY] = owner;
				}
			}
		}
		#endregion

	}

	//Returns an integer representing the number of tiles that will be converted should a tile be placed at the passed co-ordinates
	public int GetTileScore(int x, int y, int owner) {

		return 0;
	}
}
