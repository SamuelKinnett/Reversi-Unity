using UnityEngine;
using System.Collections;

public class ShadowControl : MonoBehaviour
{
	public GameObject boardShadow;
	public SpriteRenderer spriteRenderer;
	private ShadowManager shadowManager;

	float sinValue;
	float rotationSpeed = 30;
	float x;			//Anchor point X
	float y;			//Anchor point Y

	// Use this for initialization
	void Start ()
	{
		sinValue = 0;
		x = this.transform.position.x;
		y = this.transform.position.y;

		shadowManager = boardShadow.GetComponent<ShadowManager> ();

		spriteRenderer = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		spriteRenderer.material.color = Color.Lerp (shadowManager.colour1, shadowManager.colour2, shadowManager.lerpValue);
		if (shadowManager.rotating) {
			if (sinValue >= 360)
				sinValue = rotationSpeed * Time.deltaTime;
			else
				sinValue += rotationSpeed * Time.deltaTime;

			Vector3 newPosition = new Vector3 (x + (Mathf.Sin (Mathf.Deg2Rad * sinValue) * shadowManager.rotationAmmount), y + (Mathf.Cos (Mathf.Deg2Rad * sinValue) * shadowManager.rotationAmmount), shadowManager.zLayer);

			this.transform.position = newPosition;
		}
	}
}
