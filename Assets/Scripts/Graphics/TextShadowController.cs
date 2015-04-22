using UnityEngine;
using System.Collections;

public class TextShadowController : MonoBehaviour
{

	public ShadowManager textShadow;
	public TextMesh textMesh;
	public GameObject title;

	private ShadowManager shadowManager;
	private TitleManager titleManager;
	
	float sinValue;
	float x;			//Anchor point X
	float y;			//Anchor point Y
	
	// Use this for initialization
	void Start ()
	{
		sinValue = 0;
		
		shadowManager = textShadow.GetComponent<ShadowManager> ();
		titleManager = title.GetComponent<TitleManager> ();

		textMesh = this.GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Color tempColor;

		this.transform.position = Camera.main.ViewportToWorldPoint (new Vector3 (0.5F, 0.7F, 8.9F));

		x = 0.5F;
		y = 0.7F;

		if (titleManager.enabled) {
			tempColor = textMesh.color;
			tempColor.a = 1;
			textMesh.color = tempColor;
			textMesh.color = Color.Lerp (shadowManager.colour1, shadowManager.colour2, shadowManager.lerpValue);
			if (shadowManager.rotating) {
				if (sinValue == 360)
					sinValue = 1;
				else
					sinValue++;
			
				Vector3 newPosition = Camera.main.ViewportToWorldPoint (new Vector3 (x + (Mathf.Sin (Mathf.Deg2Rad * sinValue) * shadowManager.rotationAmmount), y + (Mathf.Cos (Mathf.Deg2Rad * sinValue) * shadowManager.rotationAmmount), shadowManager.zLayer));
			
				this.transform.position = newPosition;
			}
		} else {
			tempColor = textMesh.color;
			tempColor.a = 0;
			textMesh.color = tempColor;
		}
	}
}
