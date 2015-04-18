using UnityEngine;
using System.Collections;
using System;

public class TextBehaviour : MonoBehaviour
{

	int contents;
	public TextMesh textMesh;				//The textbox
	public GameObject boardControl;			//The gameboard
	private BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the gameboard
	private System.Random rand;
	public int x;							//The x position of the textbox
	public int y;							//The y position of the textbox

	// Use this for initialization
	void Start ()
	{
		contents = 0;
		boardBehaviour = boardControl.GetComponent<BoardBehaviour> ();
		rand = new System.Random ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (boardBehaviour.GetTileState (x, y) == 0) {
			contents = boardBehaviour.GetTileScore (x, y, 1);
			if (contents < 10)
				textMesh.text = contents.ToString ();
			else if (contents > 9)
				textMesh.text = "+";
			else
				textMesh.text = "";
		} else
			textMesh.text = "";

		Color tempColour = textMesh.color;
		tempColour.a = (0.3f + 0.1f * contents) + (0.05F - (float)(rand.NextDouble () / 10));
		textMesh.color = tempColour;
	}
}
