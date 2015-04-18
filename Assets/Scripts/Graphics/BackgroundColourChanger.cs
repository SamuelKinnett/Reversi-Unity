using UnityEngine;
using System.Collections;

public class BackgroundColourChanger : MonoBehaviour
{
	
	public float time; //How long it takes for the background colour to switch between the two colours.
	float currentTime; //Used to track the current colour in relation to time.
	
	public Camera camera;
	public Color colour1;
	public Color colour2;
	
	// Use this for initialization
	void Start ()
	{
		if (colour1 == null)
			colour1 = new Color32 (39, 253, 245, 255);
		if (colour2 == null)
			colour2 = new Color32 (247, 101, 184, 255);
		currentTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentTime += Time.deltaTime;

		if (currentTime >= time) {
			currentTime = 0;
		}

		
		float currentIntensity = (360F / time) * currentTime * Mathf.Deg2Rad;
		
		camera.backgroundColor = Color.Lerp (colour1, colour2, 0.5f + (Mathf.Sin (currentIntensity) / 2));
	}
}
