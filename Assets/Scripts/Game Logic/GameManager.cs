﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public int GameType;		//0 = player v player, 1 = player v AI, 2 = AI v AI
	public GameObject AIController1;
	public GameObject AIController2;
	public GameObject playerController1;
	public GameObject playerController2;

	private AIManager bot1;
	private AIManager bot2;
	private PlayerControl player1;
	private PlayerControl player2;

	// Use this for initialization
	void Start ()
	{

		bot1 = AIController1.GetComponent<AIManager> ();
		bot2 = AIController2.GetComponent<AIManager> ();
		player1 = playerController1.GetComponent<PlayerControl> ();
		player2 = playerController2.GetComponent<PlayerControl> ();

		switch (GameType) {

		case 0:
			player1.Initialise (1);
			player2.Initialise (2);
			break;

		case 1:
			//TODO: Allow player to choose AI to face
			player1.Initialise (1);
			bot1.Initialise (1, 2);	//Initalise 1 Greedy AI as player 2
			break;

		case 2:
			//TODO: Allow player to choose 2 AI to face off
			bot1.Initialise (1, 1); //Initialise 1 Greedy AI as player 1
			bot2.Initialise (1, 2); //Initilaise 1 Greedy AI as player 2
			break;

		}
	}

	public PlayerControl GetPlayerControl (int player)
	{
		if (player == 1)
			return player1;
		else
			return player2;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
