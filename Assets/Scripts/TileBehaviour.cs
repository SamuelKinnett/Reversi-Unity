using UnityEngine;
using System.Collections;

public class TileBehaviour : MonoBehaviour {

	public GameObject boardManager;			//The boardBehaviour script to allow access to the current board state
	public SpriteRenderer spriteRenderer;	//The Sprite Renderer for this tile
	public int x;							//The X co-ordinate of the tile
	public int y;							//The Y co-ordinate of the tile
	public float fadeSpeed;

	Color tintColour;						//The colour to tint the square with
	int owner;								//1 = player 1, 2 = player 2, 0 = unowned
	float opacity;							//The opacity of the tile
	private BoardBehaviour boardBehaviour;	//The BoardBehaviour script of the BoardManager 

	bool fading;							//Is the tile currently fading to another colour?
	Color currentColour;					//The current colour
	float currentOpacity;					//The current opacity
	float time;								//Used to store a time value for the purpose of fading

	// Use this for initialization
	void Start () {

		tintColour = Color.white;
		opacity = 0.5f;
		boardBehaviour = boardManager.GetComponent<BoardBehaviour> ();

	}
	
	// Update is called once per frame
	void Update () {

		spriteRenderer.material.SetColor ("_Color", currentColour);

		owner = boardBehaviour.GetTileState (x, y);	//Who does this tile belong to?

		switch (owner) {

		case 0:
			tintColour = Color.white;
			break;

		case 1:
			tintColour = Color.red;
			break;

		case 2:
			tintColour = Color.blue;
			break;
		}

		if (currentColour != tintColour || currentOpacity != opacity) {

			time = 0;
			fading = true;

		} else {

			fading = false;

		}

		if (fading) {

			time += Time.deltaTime;
			currentColour = Color.Lerp(currentColour, tintColour, time * fadeSpeed);
			currentOpacity = Mathf.Lerp (currentOpacity, opacity, time * fadeSpeed);

			currentColour.a = currentOpacity;
		}
	}

	// When the mouse is passed over the tile
	void OnMouseOver() {

		opacity = 1;

	}

	//When the mouse leaves the tile
	void OnMouseExit() {

		opacity = 0.5f;

	}

	//When the tile is clicked
	void OnMouseDown() {

		if (boardBehaviour.currentPlayer == 1) {
			boardBehaviour.SetTileState(x, y, 1);
		}
	}
}
