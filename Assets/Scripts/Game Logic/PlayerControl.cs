using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

	public bool enabled;
	public int playerNumber;

	// Use this for initialization
	void Start ()
	{
		enabled = false;
	}

	public void Initialise (int playerNumber)
	{
		this.playerNumber = playerNumber;
		enabled = true;
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
