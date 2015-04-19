using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour
{

	public TextMesh textMesh;
	public bool enabled;

	private System.Random rand;

	// Use this for initialization
	void Start ()
	{
		enabled = true;
		rand = new System.Random ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.5F, 0.7F, 8.9F));

		Color tempColour = textMesh.color;

		if (enabled) {
			//Flickering code
			tempColour.a = (float)(0.8 + rand.NextDouble ());
		} else
			tempColour.a = 0;

		textMesh.color = tempColour;
	}
}
