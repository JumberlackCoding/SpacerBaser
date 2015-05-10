using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour 
{
	public GUISkin GameSkin;
	
	GameManagerScript gameManager = null;
	PowerManagerScript powerManager = null;


	// Create Main Bool Variables
	private bool _isTitleMenu = true;
	private bool _isSignInMenu = false;
	private bool _isFirstMenu = false;
	private bool _isGameModeMenu = false;
	private bool _isOptionsMenu = false;
	private bool _isLoadGameMenu = false;

	//Create Sub Bool Variables
	private bool _isSound = true;
	private bool _isVideo = false;
	private bool _isControl = false;
	private bool _isAbout = false;

	private bool _isLoadSurvival = false;
	private bool _isLoadClassic = true;
	private bool _isLoadEnemy = false;

	private bool _isLevelSelectMenu = false;

	private bool _isNewAccount = false;
	private bool _isNoAccount = false;
	private bool _isBadPassword = false;
	private bool _isAccountMade = false;
	
	private float soundIncreaseDelay = 0.1f;
    
	// Create Player Variables
	private string _playerName = "";
	private string _playerPassword = "";
	private string _playerRepeat = "";

	//Create Public Option Variables
	private bool musicIntroDone = true;
	private float BGMVolume = 100f;
	private float SFXVolume = 100f;
	
	public AudioClip menuBackgroundMusic_1;
	public AudioClip menuBackgroundMusic_2;
	public AudioClip menuBackgroundMusic_3;
	public AudioClip menuBackgroundMusic_4;
    
	private AudioSource audioManager;
	
	
	// Use this for initialization
	void Start () 
	{
		gameManager = GameObject.Find( "GameManager" ).GetComponent<GameManagerScript>();
		powerManager = GameObject.Find( "PowerManager" ).GetComponent<PowerManagerScript>();
		audioManager = GetComponent<AudioSource>();
		audioManager.ignoreListenerVolume = true;
	}
	
	// Update is called once per frame
	void Update () {
		if( !audioManager.isPlaying )
		{
			int music = Random.Range( 0, 9999 );
			music = music % 4;
			
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
		musicIntroDone = false;
		audioManager.Play();
		
		for( int i = 0; i < BGMVolume / 10; i++ )
		{
			if( audioManager.volume < BGMVolume/100 )
			{
				audioManager.volume += 0.1f;
				yield return new WaitForSeconds( soundIncreaseDelay );
			}
			else
			{
				break;
			}
		}
		
		musicIntroDone = true;
    }
    
    void OnGUI()
	{
		GUI.skin = GameSkin;
		TitleMenu();
		SignInMenu();
		FirstMenu();
		GameModeMenu();
		LevelSelectMenu();
		LoadGameMenu();
		OptionsMenu();

		if(_isLoadGameMenu == true || _isOptionsMenu == true || _isGameModeMenu == true)
		{
			if(GUI.Button(new Rect(10,Screen.height-35,150,25),"Back"))
			{
				_isLoadGameMenu = false;
				_isOptionsMenu = false;
				_isGameModeMenu = false;
				
				_isFirstMenu = true;
			}
		}

		if(_isLevelSelectMenu == true)
		{
			if(GUI.Button(new Rect(10,Screen.height-35,150,25),"Back"))
			{
				_isLevelSelectMenu = false;

				_isGameModeMenu = true;
			}
		}

	}

	void TitleMenu()
	{
		if(_isTitleMenu)
		{
			GUI.Label (new Rect(5,Screen.height-25,150,25),"V 0.0.1");
			GUI.Label (new Rect(Screen.width/2-200,Screen.height/2-50,50,100),"SPACER BASER","Menu Title");
			GUI.Label (new Rect(Screen.width/2-100,7*Screen.height/8-50,200,100),"--Click Anywhere to Continue--");

			if (Input.GetMouseButtonDown(0))  
			{
				_isTitleMenu = false;
				_isSignInMenu = true;
			}
		}
	}


	void SignInMenu()
	{
		if (_isSignInMenu)
		{
			//---Create Sign In Box---
			GUI.Box(new Rect(Screen.width/2-300,Screen.height/2-200,600,400),"");
			GUI.Label (new Rect(Screen.width/2-80,Screen.height/2-180,400,100),"Sign In","Menu Title");

			//---Enter User Name---
			GUI.Label (new Rect(Screen.width/2-140,Screen.height/2-100,400,100),"Player Name:");
			_playerName = GUI.TextField(new Rect(Screen.width/2-140,Screen.height/2-80,280,30),_playerName); 

			//---Enter Password---
			GUI.Label (new Rect(Screen.width/2-140,Screen.height/2-40,400,100),"Password:");
			_playerPassword = GUI.PasswordField(new Rect(Screen.width/2-140,Screen.height/2-20,280,30),_playerPassword, "*"[0],10);

			//---Skip Sign In Button (For Testing)---
			if(GUI.Button (new Rect(Screen.width/2+180,Screen.height/2+160,100,30),"Skip"))
			{
				_isSignInMenu = false;
				_isFirstMenu = true;
				_playerName = "Test";
			}

			//---Clear All Accounts Button (For Testing)---
			if(GUI.Button (new Rect(Screen.width/2-280,Screen.height/2+160,120,30),"Clear Accounts"))
			{
				PlayerPrefs.DeleteAll();
			}

			//---Sign In---
			if (_isNewAccount == false)
			{
				//---Account Created Message---
				if(_isAccountMade == true)
				{
					GUI.Label (new Rect(Screen.width/2-80,Screen.height/2+20,200,50),"Account Created","Good Message Text");
				}

				//---Error message if there is no account with that name--
				if(_isNoAccount == true)
				{
					GUI.Label (new Rect(Screen.width/2-110,Screen.height/2+20,200,50),"Account Does Not Exist","Error Message Text");
				}

				//---Error message if password doesn't match account--
				if(_isBadPassword == true)
				{
					GUI.Label (new Rect(Screen.width/2-110,Screen.height/2+20,200,50),"Invalid Password","Error Message Text");
				}

				if (GUI.Button (new Rect(Screen.width/2-120,Screen.height/2+60,240,30),"Sign In") || Input.GetKeyDown(KeyCode.Return) == true)
				{
					if(_playerName != "" && _playerPassword != "")
					{
						if(PlayerPrefs.HasKey(_playerName))
						{
							if(PlayerPrefs.GetString(_playerName)==(_playerName+_playerPassword))
							{
								_isSignInMenu = false;
								_isFirstMenu = true;
							}
							else
							{
								_isNoAccount = false;
								_isBadPassword = true;
							}
						}
						else
						{
							_isBadPassword = false;
							_isNoAccount = true;
						}
					}
					else
					{

					}
				}
				if (GUI.Button (new Rect(Screen.width/2-120,Screen.height/2+100,100,30),"New Account"))
				{
					_isNewAccount = true;
				}

				if (GUI.Button (new Rect(Screen.width/2+20,Screen.height/2+100,100,30),"Exit"))
				{
					Application.Quit ();
				}
			}

			//---Create New Account---
			else
			{
				//---Repeat Password---
				GUI.Label (new Rect(Screen.width/2-140,Screen.height/2+20,400,100),"Re-Enter Password:");
				_playerRepeat = GUI.PasswordField(new Rect(Screen.width/2-140,Screen.height/2+40,280,30),_playerRepeat, "*"[0],10);

				if(_playerPassword != _playerRepeat)
				{
					GUI.Label (new Rect(Screen.width/2-110,Screen.height/2+80,200,50),"Passwords Do Not Match","Error Message Text");
				}

				if (GUI.Button (new Rect(Screen.width/2-120,Screen.height/2+120,240,30),"Create Account") || Input.GetKeyDown(KeyCode.Return) == true)
				{
					if(_playerName != "" && _playerPassword != "" && _playerRepeat!= "")
					{
						if(_playerPassword == _playerRepeat)
						{
							_isNoAccount = false;
							PlayerPrefs.SetString(_playerName,_playerName+_playerPassword);
							_isNewAccount = false;
							_isAccountMade = true;
						}

					}
					else
					{
						//GUI.Label (new Rect(Screen.width/2-140,Screen.height/2+20,400,100),"Enter a Player Name and Password")
					}
				}
				if (GUI.Button (new Rect(Screen.width/2-120,Screen.height/2+160,100,30),"Back"))
				{
					_isNewAccount = false;
					_playerName = "";
					_playerPassword = "";
					_playerRepeat = "";
				}

				if (GUI.Button (new Rect(Screen.width/2+20,Screen.height/2+160,100,30),"Exit"))
				{
					Application.Quit ();
				}
			}


		}
	}

	void FirstMenu()
	{
		if(_isFirstMenu)
		{
			//---Game Title---
			GUI.Label (new Rect(30,75,300,25),"Spacer Baser","Menu Title");

			//---Player Welcome---
			GUI.Label (new Rect(40*(25*Screen.width/65)/65+30,40*(Screen.height-40*Screen.height/65)/65+40,25*(25*Screen.width/65)/65-60,20),"Welcome,","Sub Menu Title");
			GUI.Label (new Rect(40*(25*Screen.width/65)/65+30,40*(Screen.height-40*Screen.height/65)/65+80,25*(25*Screen.width/65)/65-60,20),"'"+_playerName+"'","Sub Menu Title");

			//---Boxes---
			GUI.Box (new Rect(25*Screen.width/65,10,1,Screen.height-10),""); //Divider Line
			GUI.Box (new Rect(10,Screen.height-40*Screen.height/65,25*Screen.width/65-10,40*Screen.height/65-10),""); //Button Box
			GUI.Box (new Rect(40*(25*Screen.width/65)/65,10,25*(25*Screen.width/65)/65,Screen.height-40*Screen.height/65-10),""); //Player Info Box
			GUI.Box (new Rect(40*(25*Screen.width/65)/65+30,40,25*(25*Screen.width/65)/65-60,40*(Screen.height-40*Screen.height/65)/65-10),""); //Player Picture Box

			//---Buttons---
			if(GUI.Button(new Rect(30,Screen.height-37*Screen.height/65,25*Screen.width/65-60,5*Screen.height/65),"New Game"))
			{
				_isFirstMenu = false;
				_isGameModeMenu = true;
			}

			if(GUI.Button(new Rect(30,Screen.height-31*Screen.height/65,25*Screen.width/65-60,5*Screen.height/65),"Load Game"))
			{
				_isLoadGameMenu = true;
				_isFirstMenu = false;
			}


			if(GUI.Button(new Rect(30,Screen.height-25*Screen.height/65,25*Screen.width/65-60,5*Screen.height/65),"Options"))
			{
				_isFirstMenu = false;
				_isOptionsMenu = true;

			}

			if(GUI.Button(new Rect(30,Screen.height-7*Screen.height/65,25*Screen.width/65-60,5*Screen.height/65),"Quit"))
			{
				Application.Quit ();
			}
		}
	}

	void GameModeMenu()
	{
		if(_isGameModeMenu)
		{

			//CUSTOM MODE
			GUI.Label(new Rect(1*Screen.width/16,Screen.height/2-280,3*Screen.width/16,30),"CUSTOM","Sub Menu Title");
			//GUI.Label(new Rect(1*Screen.width/16+75,Screen.height/2-100,3*Screen.width/16,30),"Coming Soon...","Sub Menu Title");
			if(GUI.Button(new Rect(1*Screen.width/16,Screen.height/2-250,3*Screen.width/16,500),""))
			{
				Application.LoadLevel("InitialMenu");
			}

			//STORY MODE
			GUI.Label(new Rect(13*Screen.width/32,Screen.height/2-280,3*Screen.width/16,30),"CLASSIC","Sub Menu Title");
			if(GUI.Button(new Rect(13*Screen.width/32,Screen.height/2-250,3*Screen.width/16,500),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
			}

			//ENEMY MODE
			GUI.Label(new Rect(12*Screen.width/16,Screen.height/2-280,3*Screen.width/16,30),"ENEMY","Sub Menu Title");
			GUI.Label(new Rect(12*Screen.width/16+75,Screen.height/2-100,3*Screen.width/16,30),"Coming Soon...","Sub Menu Title");
			if(GUI.Button(new Rect(12*Screen.width/16,Screen.height/2-250,3*Screen.width/16,500),""))
			{
				//Application.LoadLevel("EnemyMenu"); 
			}
		}
	}

	void LevelSelectMenu()
	{
		if(_isLevelSelectMenu)
		{
			// Level 1
			GUI.Label(new Rect(1*Screen.width/16,Screen.height/5-30,3*Screen.width/16,30),"Level 1","Sub Menu Title");
			if(GUI.Button(new Rect(1*Screen.width/16,Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level1();
			}

			GUI.Label(new Rect(4*Screen.width/16,Screen.height/5-30,6*Screen.width/16,30),"Level 2","Sub Menu Title");
			if(GUI.Button(new Rect(4*Screen.width/16,Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level2();
			}

			GUI.Label(new Rect(7*Screen.width/16,Screen.height/5-30,6*Screen.width/16,30),"Level 3","Sub Menu Title");
			if(GUI.Button(new Rect(7*Screen.width/16,Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level3();
			}

			GUI.Label(new Rect(10*Screen.width/16,Screen.height/5-30,6*Screen.width/16,30),"Level 4","Sub Menu Title");
			if(GUI.Button(new Rect(10*Screen.width/16,Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level4();
			}

			GUI.Label(new Rect(13*Screen.width/16,Screen.height/5-30,6*Screen.width/16,30),"Level 5","Sub Menu Title");
			if(GUI.Button(new Rect(13*Screen.width/16,Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level5();
			}

			GUI.Label(new Rect(1*Screen.width/16,3*Screen.height/5-30,6*Screen.width/16,30),"Level 6","Sub Menu Title");
			if(GUI.Button(new Rect(1*Screen.width/16,3*Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level6();
			}

			GUI.Label(new Rect(4*Screen.width/16,3*Screen.height/5-30,6*Screen.width/16,30),"Level 7","Sub Menu Title");
			if(GUI.Button(new Rect(4*Screen.width/16,3*Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level7();
			}

			GUI.Label(new Rect(7*Screen.width/16,3*Screen.height/5-30,6*Screen.width/16,30),"Level 8","Sub Menu Title");
			if(GUI.Button(new Rect(7*Screen.width/16,3*Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level8();
			}

			GUI.Label(new Rect(10*Screen.width/16,3*Screen.height/5-30,6*Screen.width/16,30),"Level 9","Sub Menu Title");
			if(GUI.Button(new Rect(10*Screen.width/16,3*Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				Level9();
			}

			GUI.Label(new Rect(13*Screen.width/16,3*Screen.height/5-30,6*Screen.width/16,30),"ENEMY INVASION","Sub Menu Title");
			if(GUI.Button(new Rect(13*Screen.width/16,3*Screen.height/5,2*Screen.width/16,150),""))
			{
				_isLevelSelectMenu = true;
				_isGameModeMenu = false;
				EnemyInvasion();
			}

		}

	}

	void LoadGameMenu()
	{
		if(_isLoadGameMenu)
		{
			GUI.Box(new Rect(Screen.width/2-300,Screen.height/2-200,600,400),"");

			if(GUI.Button(new Rect(Screen.width/2-300,Screen.height/2-250,150,50),"Survival"))
			{
				_isLoadSurvival = true;
				_isLoadClassic = false;
				_isLoadEnemy = false;

			}
			if(GUI.Button(new Rect(Screen.width/2-150,Screen.height/2-250,150,50),"Classic"))
			{
				_isLoadSurvival = false;
				_isLoadClassic = true;
				_isLoadEnemy = false;
			}
			if(GUI.Button(new Rect(Screen.width/2,Screen.height/2-250,150,50),"Enemy"))
			{
				_isLoadSurvival = false;
				_isLoadClassic = false;
				_isLoadEnemy = true;
			}


			if(_isLoadSurvival)
			{
				GUI.Label(new Rect(Screen.width/2-300,Screen.height/2-200,600,40),"Load Survival","Sub Menu Title");
			}
			if(_isLoadClassic)
			{
				GUI.Label(new Rect(Screen.width/2-300,Screen.height/2-200,600,40),"Load Classic","Sub Menu Title");
			}
			if(_isLoadEnemy)
			{
				GUI.Label(new Rect(Screen.width/2-300,Screen.height/2-200,600,40),"Load Enemy","Sub Menu Title");
			}

		}
	}
	

	void OptionsMenu()
	{
		if(_isOptionsMenu)
		{
			GUI.Box(new Rect(Screen.width/2-500,Screen.height-15*Screen.height/16,Screen.width/2,Screen.height-100),"");
			if(GUI.Button(new Rect(Screen.width/2-650,Screen.height-15*Screen.height/16,150,150),"Sound"))
			{
				_isSound = true;
				_isVideo = false;
				_isControl = false;
				_isAbout = false;
			}

			if(GUI.Button(new Rect(Screen.width/2-650,Screen.height-15*Screen.height/16+150,150,150),"Video"))
			{
				_isSound = false;				
				_isVideo = true;
				_isControl = false;
				_isAbout = false;
			}

			if(GUI.Button(new Rect(Screen.width/2-650,Screen.height-15*Screen.height/16+300,150,150),"Controls"))
			{
				_isSound = false;
				_isVideo = false;
				_isControl = true;
				_isAbout = false;
			}

			if(GUI.Button(new Rect(Screen.width/2-650,Screen.height-15*Screen.height/16+450,150,150),"About"))
			{
				_isSound = false;				
				_isVideo = false;
				_isControl = false;
				_isAbout = true;
			}




			if(_isSound)
			{
				GUI.Label (new Rect(Screen.width/2-490,Screen.height-15*Screen.height/16+5,100,40),"Sound Settings","Menu Title");

//				//Master Volume
//				GUI.Label (new Rect(Screen.width/2-490,Screen.height-12*Screen.height/16-25,100,40),"Master Volume","Menu Info Text");
//				masterVolume = GUI.HorizontalSlider(new Rect(Screen.width/2-490,Screen.height-12*Screen.height/16,400,40),masterVolume,0,100);
//				muteMaster = GUI.Toggle(new Rect(Screen.width/2-90,Screen.height-12*Screen.height/16-25,100,40), muteMaster, "Mute");

				//Sound Effects Volume
				GUI.Label (new Rect(Screen.width/2-490,Screen.height-10*Screen.height/16-25,100,40),"Sound Effects","Menu Info Text");
				SFXVolume = GUI.HorizontalSlider( new Rect( Screen.width / 2 - 490, Screen.height - 10 * Screen.height / 16, 400 ,40 ), SFXVolume, 0f, 100f );
//				muteFX = GUI.Toggle( new Rect ( Screen.width / 2 - 90, Screen.height - 10 * Screen.height / 16 - 25, 100, 40 ), muteFX, "Mute" );

				AudioListener.volume = SFXVolume/100;

				//Music Volume
				GUI.Label (new Rect(Screen.width/2-490,Screen.height-8*Screen.height/16-25,100,40),"Music","Menu Info Text");
				BGMVolume = GUI.HorizontalSlider( new Rect ( Screen.width / 2 - 490, Screen.height - 8 * Screen.height / 16, 400, 40 ), BGMVolume, 0f, 100f);
//				muteMusic = GUI.Toggle(new Rect(Screen.width/2-90,Screen.height-8*Screen.height/16-25,100,40), muteMusic, "Mute");

				if( musicIntroDone )
				{
					audioManager.volume = (float)BGMVolume/100;
				}
			}

			if(_isVideo)
			{
				GUI.Label (new Rect(Screen.width/2-490,Screen.height-15*Screen.height/16+5,100,40),"Video Settings","Menu Title");
			}

			if(_isControl)
			{
				GUI.Label (new Rect(Screen.width/2-490,Screen.height-15*Screen.height/16+5,100,40),"Control Settings","Menu Title");
			}

			if (_isAbout)
			{
				GUI.Label (new Rect(Screen.width/2-490,Screen.height-15*Screen.height/16+5,100,40),"About","Menu Title");
			}
		}
	}
	
	void Level1()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 50;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 10000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( -1 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level2()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 60;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 15000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 0 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level3()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 70;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 20000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 1 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level4()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 85;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 30000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 2 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level5()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 110;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 40000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 3 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level6()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 150;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 60000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 4 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level7()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 190;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 85000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 5 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level8()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 290;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 120000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 7 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void Level9()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 800;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 400;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 200000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 9 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
	void EnemyInvasion()
	{
		//---Starting Minerals---
		powerManager.currentMinerals = 20000;
		
		//---Number of Asteroids---
		gameManager.numberOfAsteroids = 400;
		
		//---Mineral Goal---
		powerManager.mineralGoal = 600000;
		
		//---Set Diffulty---
		powerManager.SetDifficulty( 30 );
		
		//---Type of Distribution---
		gameManager.typeOfDistribution = 0;
		
		Application.LoadLevel( "game" );
	}
	
}
























