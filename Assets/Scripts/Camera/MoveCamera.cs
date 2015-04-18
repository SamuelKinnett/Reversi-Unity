using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

	public Camera camera;
	public float rotationMultiplier;

	float currentSinValue;

	// Use this for initialization
	void Start ()
	{
		currentSinValue = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentSinValue == 360)
			currentSinValue = 1;
		else
			currentSinValue++;
		this.transform.rotation = Quaternion.AngleAxis (Mathf.Sin (currentSinValue * Mathf.Deg2Rad) * rotationMultiplier, Vector3.forward);
	}
}
