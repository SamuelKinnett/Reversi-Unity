using UnityEngine;
using System.Collections;

public class EndgameInfoController : MonoBehaviour
{

	public GameObject boardManager;
	public TextMesh textMesh;
	private BoardBehaviour boardBehaviour;

	bool winnerFound = false;

	// Use this for initialization
	void Start ()
	{
		textMesh.text = "";
		boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();

		Color tempColour = textMesh.color;
		tempColour.a = 0;
		textMesh.color = tempColour;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!winnerFound) {
			if (boardBehaviour.currentPlayer == 0) {

				switch (boardBehaviour.winner) {

				case 1:
					textMesh.text = "Player 1 Wins!";
					winnerFound = true;
					break;

				case 2:
					textMesh.text = "Player 2 Wins!";
					winnerFound = true;
					break;

				case 3:
					textMesh.text = "It's a Draw!";
					winnerFound = true;
					break;

				}

			}
		} else {
			Color tempColour = textMesh.color;
			tempColour.a += Time.deltaTime;
			textMesh.color = tempColour;
		}
	}
}
