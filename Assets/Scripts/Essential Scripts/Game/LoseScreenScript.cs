using UnityEngine;
using System.Collections;

public class LoseScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			Application.Quit();
		}
	}
	
	void OnGUI()
	{
		GUI.Label( new Rect( 50, 50, 50, 50 ), "You lose " );	
	}
}
