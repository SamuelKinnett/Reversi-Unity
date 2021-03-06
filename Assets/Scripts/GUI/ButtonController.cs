﻿using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
	
	public TextMesh textMesh;
	public bool enabled;
	public bool selected;

	public float x;
	public float y;
	//public bool clicked;
	//public bool clickHandled;
	private Vector3 defaultScale;
	private Vector3 originalScale;
	private float maxSize;
	private System.Random rand;

	private bool fade;

	// Use this for initialization
	void Start ()
	{
		defaultScale = this.transform.localScale;
		originalScale = defaultScale;
		rand = new System.Random ();
	}

	public void CreateButton (float x, float y, string contents)
	{

		this.transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (x, y, 8.9F));
		defaultScale = originalScale;
		this.transform.localScale = defaultScale;
		maxSize = 0.70F;
		textMesh.text = contents;
		enabled = true;
		fade = true;
	}

	public void SetScale (float scale)
	{
		defaultScale = new Vector3 (scale, scale, 0);
		this.transform.localScale = defaultScale;
		maxSize = 0.70F * (defaultScale.x / originalScale.x);
	}
	// Update is called once per frame
	void Update ()
	{
		Color tempColour = textMesh.color;

		if (enabled) {
			/*
			if (clickHandled && clicked) {
				clicked = false;
				clickHandled = false;
			}
			*/
			//Flickering code
			tempColour.a = (float)(0.7 + rand.NextDouble ());
		} else
			tempColour.a = 0;

		if (enabled && !selected) {
			if (this.transform.localScale.x > defaultScale.x)
				this.transform.localScale -= new Vector3 (Time.deltaTime * 2, Time.deltaTime * 2, 0);
			else
				this.transform.localScale = defaultScale;
		} else if (selected) {
			if (this.transform.localScale.x < maxSize)
				this.transform.localScale += new Vector3 (Time.deltaTime * 2, Time.deltaTime * 2, 0);
			else
				this.transform.localScale = new Vector3 (maxSize, maxSize, 0);
		}
		textMesh.color = tempColour;
	}

	/*
	void OnMouseOver ()
	{
		if (enabled && this.transform.localScale.x < 0.75F) {
			fade = false;
		}
	}

	void OnMouseExit ()
	{
		if (enabled)
			fade = true;
	}

	void OnMouseDown ()
	{
		if (enabled) {
			clicked = true;
			clickHandled = false;
			Debug.Log ("Clicked!");
		}
	}
	*/
}
