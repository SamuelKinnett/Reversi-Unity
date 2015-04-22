using UnityEngine;
using System.Collections;

public class EndgameInfoController : MonoBehaviour
{

	public GameObject boardManager;
	public TextMesh textMesh;
	public bool enabled;
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
		enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Color tempColour = textMesh.color;

		if (boardBehaviour.currentPlayer == 0) {

			switch (boardBehaviour.winner) {

			case 1:
				textMesh.text = "Player 1 Wins!";
				winnerFound = true;
				enabled = true;
				break;

			case 2:
				textMesh.text = "Player 2 Wins!";
				winnerFound = true;
				enabled = true;
				break;

			case 3:
				textMesh.text = "It's a Draw!";
				winnerFound = true;
				enabled = true;
				break;

			default:
				enabled = false;
				break;

			}

		}

		if (enabled) {
			tempColour.a += Time.deltaTime;
			textMesh.color = tempColour;
		} else {
			tempColour.a = 0;
			textMesh.color = tempColour;
			winnerFound = false;
		}

	}
}