using UnityEngine;
using System.Collections;
using System;

public class ScoreManager : MonoBehaviour
{

	public GameObject boardManager;
	public TextMesh textMesh;
	public int player;
	private BoardBehaviour boardBehaviour;
	private System.Random rand;
	private Vector3 defaultScale;

	// Use this for initialization
	void Start ()
	{
		boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();
		rand = new System.Random ();

		defaultScale = this.transform.localScale;

		if (player == 1)
			this.transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.05F, 1F, 8.9F));
		else
			this.transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.3F, 1F, 8.9F));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player == boardBehaviour.currentPlayer)
			this.transform.localScale = new Vector3 (0.16F, 0.16F, 0);
		else
			this.transform.localScale = defaultScale;
		if (player == 1)
			textMesh.text = "P1: " + boardBehaviour.player1Score.ToString ();
		else
			textMesh.text = "P2: " + boardBehaviour.player2Score.ToString ();


		//Flickering code
		Color tempColour = textMesh.color;
		tempColour.a = (float)(0.7 + rand.NextDouble ());
		textMesh.color = tempColour;
	}
}
