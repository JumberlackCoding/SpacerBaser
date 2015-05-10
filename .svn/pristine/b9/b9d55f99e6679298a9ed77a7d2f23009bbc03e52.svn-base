using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	public GUISkin option1Skin;
	private GameManagerScript gameManager = null;
	private PowerManagerScript powerManager = null;
	
	public AudioClip menuBackgroundMusic_1;
	public AudioClip menuBackgroundMusic_2;
	public AudioClip menuBackgroundMusic_3;
	public AudioClip menuBackgroundMusic_4;
	
	private AudioSource audioManager;
	
	private float hSliderValue = 0.0f;
	private int hSliderValueInt = 0;
	private float vSliderValue = 0.0f;
	private float tSliderValue = 0.0f;
	private int tSliderValueInt = 0;
	private string tAsteroidType = "";
	private float sizeSliderValue = 0.0f;
	private int sizeSliderValueInt = 0;
	
	private float diffValue = 0.0f;
	private float gameLength = 0.0f;
//	private float hSValue = 0.0F;
//	private float vSValue = 0.0F;
//	private float screenXPos1 = 1/19f;
	private float screenXPos3 = 9/19f;
	private float screenYPos1 = 17/19f;
//	private float screenYPos2 = 16/19f;
//	private float screenYPos3 = 18/19f;
//	
//	private float screenYPos4 = 14/19f;
//	private float screenYPos5 = 12/19f;
	
	private float soundIncreaseDelay = 0.1f;
	
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find( "GameManager" ).GetComponent<GameManagerScript>();
		powerManager = GameObject.Find( "PowerManager" ).GetComponent<PowerManagerScript>();
		audioManager = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if( !audioManager.isPlaying )
		{
			int music = Random.Range( 0, 9999 );
			music = music % 2;
			
			switch( music )
			{
				case 0:
					audioManager.clip = menuBackgroundMusic_1;
					break;
				case 1:
					audioManager.clip = menuBackgroundMusic_2;
					break;
				case 2:
					audioManager.clip = menuBackgroundMusic_3;
					break;
				case 3:
					audioManager.clip = menuBackgroundMusic_4;
					break;
				default:
					print( "music entry error in GUI script" );
					break;
			}
			audioManager.volume = 0f;
			StartCoroutine( musicEntry() );
		}
	}
	
	
	private IEnumerator musicEntry()
	{
		audioManager.Play();
		audioManager.volume = 0.1f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.2f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.3f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.4f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.5f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.6f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.7f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.8f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 0.9f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		
		audioManager.volume = 1.0f;
		
		yield return new WaitForSeconds( soundIncreaseDelay );
		if( audioManager.clip == menuBackgroundMusic_2 )
		{
			audioManager.volume = 1.2f;
			
			yield return new WaitForSeconds( soundIncreaseDelay );
			
			audioManager.volume = 1.4f;
			
			yield return new WaitForSeconds( soundIncreaseDelay );
			
			audioManager.volume = 1.6f;
			
			yield return new WaitForSeconds( soundIncreaseDelay );
			
			audioManager.volume = 1.8f;
			
			yield return new WaitForSeconds( soundIncreaseDelay );
			
			audioManager.volume = 2.0f;
			
			yield return new WaitForSeconds( soundIncreaseDelay );
		}
	}
	
	
	
	void OnGUI()
	{


		//==========================================================
		//==================CREATE PAGE TITLE TEXT==================
		//==========================================================

		//---Sets Current Skin for the Following GUI Functions
		GUI.skin = option1Skin;

		GUI.Label ( new Rect ( Screen.width/2-200, Screen.height*1/16, Screen.width/2+100, 400), "CUSTOM GAME","Menu Title");

		//==========================================================
		//==================CREATE BACKGROUND BOX===================
		//==========================================================
		GUI.Box(new Rect(Screen.width*1/20,Screen.height*3/16,Screen.width*18/20,Screen.height*11/16),"");


		//==========================================================
		//==============CREATE RESOURCES SETTINGS TITLE=============
		//==========================================================
		GUI.Label( new Rect( Screen.width*30/200, Screen.height*4/16, Screen.width*2/20, Screen.height*1/16),"RESOURCES","Sub Menu Title");


		//===============CREATE ASTEROID COUNT SLIDER===============
		//---Create Main Label 'Asteroid Count'---
		
		//---Creates Horizontal Slider for Number of Asteroids---
		hSliderValue = GUI.HorizontalSlider( new Rect( Screen.width*30/200, Screen.height*7/16, 160, 10 ), hSliderValue, 0.0F, 800.0F );
		//---Turn Slider Value to an Integer---
		hSliderValueInt = (int)( hSliderValue + 0.5 );
		//---Display Number of Asteroids
		GUI.Label( new Rect( Screen.width*25/200, Screen.height*6/16, 160, 30 ), "Asteroid Count: " + hSliderValueInt,"Sub Menu Title" );
//		GUI.Label( new Rect( Screen.width*50/200+10, Screen.height*6/16, 120, 50 ), hSliderValueInt.ToString(),"Sub Menu Title");

		//===============CREATE SIZE OF ASTEROIDS SLIDER==================
		
		//---Creates Horizontal Slider for Size of Asteroids---
		sizeSliderValue = GUI.HorizontalSlider( new Rect( Screen.width*30/200, Screen.height*9/16, 160, 10 ), sizeSliderValue, 0.0F, 800.0F );
		//---Turn Slider Value to an Integer---
		sizeSliderValueInt = (int)( sizeSliderValue + 0.5 );
		//---Display Size of Asteroids
		GUI.Label( new Rect( Screen.width*20/200, Screen.height*8/16, 160, 30 ), "Asteroid Average Size: " + sizeSliderValueInt,"Sub Menu Title" );
//		GUI.Label( new Rect( Screen.width*57/200+10, Screen.height*8/16, 120, 50 ), sizeSliderValueInt.ToString(),"Sub Menu Title");

		//===============CREATE DISTRIBUTION TYPE DROP DOWN===============
		
		//add a drop down box... need too look into that Thursday afternoon when i have time

		//Old Distribution Code to make a Slider for the time being
		//=============================================================
		//===============CREATE DISTRIBUTION TYPE SLIDER===============
		//=============================================================

		//Create Horizontal Slider to Set Between Different Distribution Types
		tSliderValue = GUI.HorizontalSlider( new Rect( Screen.width*30/200, Screen.height*11/16, 160, 10 ), tSliderValue, 0.0F, 3.0F );
		//Lock Slider to Certain Values
		if( tSliderValue < 0.5f )
		{
			tSliderValue = 0f;
		}
		else if ( tSliderValue >= 0.5f && tSliderValue <= 1.5f )
		{
			tSliderValue = 1f;
		}
		else if ( tSliderValue >= 1.5f && tSliderValue <= 2.5f )
		{
			tSliderValue = 2f;
		}
		else if ( tSliderValue >= 2.5f )
		{
			tSliderValue = 3f;
		}
		//---Turn Slider Value to an Ineger---
		tSliderValueInt = (int)tSliderValue;
		//---Display Label for Current Slider Setting---
		if( tSliderValueInt == 0 )
		{
			tAsteroidType = "Uniform";
		}
		else if( tSliderValueInt == 1 )
		{
			tAsteroidType = "Clustered";
		}
		else if( tSliderValueInt == 2 )
		{
			tAsteroidType = "Horizontal Belts";
		}
		else if( tSliderValueInt == 3 )
		{
			tAsteroidType = "Vertical Belts";
		}
		//---Display Label---
		GUI.Label( new Rect( Screen.width*20/200, Screen.height*10/16, 160, 30 ), "Distribution Type: " + tAsteroidType,"Sub Menu Title" );
//		GUI.Label( new Rect( Screen.width*49/200, Screen.height*10/16, 120, 50 ), tAsteroidType ,"Sub Menu Title");




		//==========================================================
		//================CREATE GAME SETTINGS TITLE================
		//==========================================================
		GUI.Label( new Rect( Screen.width*95/200, Screen.height*4/16, Screen.width*2/20, Screen.height*1/16),"MAP","Sub Menu Title");


		//===============CREATE LENGTH OF GAME SLIDER==================
		//---Creates Horizontal Slider for Game Length---
		gameLength = GUI.HorizontalSlider( new Rect( Screen.width*90/200, Screen.height*7/16, 160, 10 ), gameLength, 0.0F, 170000.0F );
		//---Determine Length Based on Slider Position---
		string gameLen = "";
		if( gameLength <= 34000 )
		{
			gameLen = "Short";
		}
		else if( gameLength <= 68000 )
		{
			gameLen = "Shortish";
		}
		else if( gameLength <= 102000 )
		{
			gameLen = "Medium";
		}
		else if( gameLength <= 136000 )
		{
			gameLen = "Longish";
		}
		else if( gameLength <= 170000 )
		{
			gameLen = "Long";
		}
		//---Display Game Length---
		GUI.Label( new Rect( Screen.width*84/200, Screen.height*6/16, 160, 30 ), "Game Length: " + gameLen, "Sub Menu Title" );
//		GUI.Label( new Rect( Screen.width*107/200, Screen.height*6/16, 120, 50 ), gameLen, "Sub Menu Title" );

		//===============CREATE MAP (BACKGROUND) DROP DOWN==================
		//look up drop boxes


		//==========================================================
		//===============CREATE ENEMY SETTINGS TITLE================
		//==========================================================
		GUI.Label( new Rect( Screen.width*160/200, Screen.height*4/16, Screen.width*2/20, Screen.height*1/16),"ENEMIES","Sub Menu Title");

		//===============CREATE GAME DIFFICULTY SLIDER==================
		//---Creates Horizontal Slider for Difficulty---
		diffValue = GUI.HorizontalSlider( new Rect( Screen.width*158/200, Screen.height*7/16, 160, 10 ), diffValue, 0.0F, 4.0F );
		//---Display Text Based on Slider Position---
		string difficulty = "";
		if( diffValue < 0.5f )
		{
			diffValue = 0f;
		}
		else if ( diffValue >= 0.5f && diffValue < 1.5f )
		{
			diffValue = 1f;
		}
		else if ( diffValue >= 1.5f && diffValue < 2.5f )
		{
			diffValue = 2f;
		}
		else if ( diffValue >= 2.5f && diffValue < 3.5f )
		{
			diffValue = 3f;
		}
		else if ( diffValue >= 3.5f )
		{
			diffValue = 4f;
		}

		switch( (int)diffValue )
		{
			case 0:
				difficulty = "Cake";
				break;
			case 1:
				difficulty = "Pie";
				break;
			case 2:
				difficulty = "So-so";
				break;
			case 3:
				difficulty = "Hard";
				break;
			case 4:
				difficulty = "Killer";
				break;
			default:
				difficulty = "Failed";
				break;
		}
		//---Display Game Difficulty---
		GUI.Label( new Rect( Screen.width*156/200, Screen.height*6/16, 160, 30 ), "Difficulty: " + difficulty,"Sub Menu Title" );
//		GUI.Label( new Rect( Screen.width*171/200, Screen.height*6/16, 120, 50 ), difficulty , "Sub Menu Title");
		


		

	



		//========================================================================
		//===============CREATE 'RANDOM NUMBER OF ASTEROIDS' SLIDER===============
		//========================================================================

	//	//---Create Vertical Slider to set Between Random/Not Random Number of Asteroids---
	//	vSliderValue = GUI.VerticalSlider( new Rect( Screen.width * screenXPos2, Screen.height * screenYPos1 - 15, 10, 40 ), vSliderValue, 1.0F, 0.0F );
	//	//---Lock Slider to Certain Values---
	//	if( vSliderValue < 0.5f )
	//	{
	//		vSliderValue = 0f;
	//	}
	//	else if ( vSliderValue >= 0.5f )
	//	{
	//		vSliderValue = 1f;
	//	}
	//	//---Turn Slider Value to an Integer---
	//	vSliderValueInt = (int)vSliderValue;
	//	//---Pick Label Based on Current Slider Setting---
	//	if( vSliderValueInt == 0 )
	//	{
	//		vAsteroidCount = hSliderValueInt.ToString();
	//	}
	//	else
	//	{
	//		vAsteroidCount = "Random";
	//	}
	//	//---Display Label---
	//	//GUI.Label( new Rect( Screen.width * screenXPos2 + 20, Screen.height * screenYPos1 - 5, 100, 30 ), vAsteroidCount );


		//======================================================
		//===============CREATE START GAME BUTTON===============
		//======================================================

		//---Create Start Button and Check if it is Pressed---
		if( GUI.Button( new Rect( Screen.width * screenXPos3, Screen.height * screenYPos1, 90, 30 ), "Start Game" ) )
		{
			//+++++Set the Value of the Slider Variables in the Game Manager+++++

			//---Number of Asteroids---
			if( vSliderValue == 0 )
			{
				gameManager.numberOfAsteroids = (int)hSliderValueInt;
			}
			else
			{
				gameManager.numberOfAsteroids = Random.Range( 10, 200 );
			}
			//---Type of Distribution---
			gameManager.typeOfDistribution = (int)tSliderValueInt;

			int gameLengthInt = (int)gameLength + 30000;
			
			powerManager.currentMinerals = 800;
			powerManager.mineralGoal = (int)gameLengthInt;
			powerManager.SetDifficulty( (int)diffValue );





			Application.LoadLevel( "game" );
		}
	}
}
