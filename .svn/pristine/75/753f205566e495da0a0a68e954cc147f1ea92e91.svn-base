using UnityEngine;
using System.Collections;

public class VictoryScreenScript : MonoBehaviour {

	public GUISkin mainSkin;
	public GUISkin victorySkin;
	
	private float x1;
	private float x2;
	private float x3;
	private float x4;
	private float x5;
	private float x6;

	private float y1;
	private float y2;
	private float y3;
	private float y4;
	private float y5;
	private float y6;
	
	private float h1;
	private float h2;
	private float h3;
	private float h4;
	private float h5;
	private float h6;
	
	private float w1;
	private float w2;
	private float w3;
	private float w4;
	private float w5;
	private float w6;

	// Use this for initialization
	void Start () {
	
	   	x1 = Screen.width * (float)1 / 23;
		x2 = Screen.width * (float)3 / 23;
		x3 = Screen.width * (float)5 / 23;
		x4 = Screen.width * (float)7 / 23;
		x5 = Screen.width * (float)9 / 23;
		x6 = Screen.width * (float)21 / 46;
		
		y1 = Screen.height * (float)1 / 23;
		y2 = Screen.height * (float)3 / 23;
		y3 = Screen.height * (float)5 / 23;
		y4 = Screen.height * (float)7 / 23;
		y5 = Screen.height * (float)9 / 23;
		y6 = Screen.height * (float)11 / 23;

		h1 = Screen.height * (float)21 / 23;
		h2 = Screen.height * (float)17 / 23;
		h3 = Screen.height * (float)13 / 23;
		h4 = Screen.height * (float)9 / 23;
		h5 = Screen.height * (float)5 / 23;
		h6 = Screen.height * (float)1 / 23;
	
		w1 = Screen.width * (float)21 / 23;
		w2 = Screen.width * (float)17 / 23;
		w3 = Screen.width * (float)13 / 23;
		w4 = Screen.width * (float)9 / 23;
		w5 = Screen.width * (float)5 / 23;
		w6 = Screen.width * (float)2 / 23;
	
	
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
		GUI.skin = mainSkin;
		
		GUI.Box( new Rect( x1, y1, w1, h1 ), "" );
		GUI.Box( new Rect( x2, y2, w2, h2 ), "" );
		GUI.Box( new Rect( x3, y3, w3, h3 ), "" );
		GUI.Box( new Rect( x4, y4, w4, h4 ), "" );
		GUI.Box( new Rect( x5, y5, w5, h5 ), "" );
		
		GUI.skin = victorySkin;
		
		GUI.Label( new Rect( x6, y6, w6, h6 ), "VICTORY!!" );
	}
}
