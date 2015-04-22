using UnityEngine;
using System.Collections;

public class AIManager : MonoBehaviour
{

	public int AIChoice;			//Currently, 0 = No AI (playing against a human), 1 = Greedy AI
	public GameObject boardManager;
	public int playerNumber;
	public bool enabled;			//Is the AI currently being used

	AI bot;

	// Use this for initialization
	void Start ()
	{
		enabled = false;
	}

	// Disregard the previous comment, use this for initialisation in code
	public void Initialise (int choice, int playerNumber)
	{
		AIChoice = choice;
		this.playerNumber = playerNumber;

		switch (AIChoice) {
	
		case 1:
			bot = new GreedyAI (boardManager, playerNumber);
			enabled = true;
			break;

		case 2:
			bot = new MonteCarlo (boardManager, playerNumber);
			enabled = true;
			break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (enabled) {
			bot.UpdateAI ();
		}
	}
}
