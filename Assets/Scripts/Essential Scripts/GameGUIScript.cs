using UnityEngine;
using System.Collections;

public class GameGUIScript : MonoBehaviour {

	public GUISkin mainSkin;
//	public GUISkin powerSkin;
	public GUISkin mineralSkin;
	public GUISkin structSelectSkin;
	public GUISkin structSelectionTitleSkin;
	
	public LayerMask NodeMask;
	public LayerMask StructureMask;
	public LayerMask AsteroidMask;
	public LayerMask NotBeam;
	public LayerMask EverythingMask;
	
	/*=======STRUCTURES=========*/
	public GameObject solarPreFab;
	public GameObject nodePreFab;
	public GameObject minerPreFab;
	public GameObject storagePreFab;
	public GameObject healerPreFab;
	public GameObject turretPreFab;
	public GameObject missilePreFab;
	public GameObject energybeamPreFab;
	
	/*========ENEMIES=========*/
	public GameObject enemyExploderPreFab;
	public GameObject enemyBasicPreFab;
	public GameObject enemySwarmerPreFab;
	public GameObject enemyTankPreFab;
	public GameObject enemyCloaker;
	
	/*==========Worms==========*/	
	public GameObject mediumWormPreFab;
	
	/*=====RangeIndicator=====*/
	public GameObject miningRangeIndicatorPreFab;
	public GameObject turretRangeIndicatorPreFab;
	public GameObject repairRangeIndicatorPreFab;
	public GameObject energyRangeIndicatorPreFab;
	
	public AudioClip gameMusic_1;
	public AudioClip gameMusic_2;
	
	public Texture solar;
	public Texture miner;
	public Texture healer;
	public Texture missiles;
	public Texture turret;
	public Texture node;
	public Texture storage;
	
	public Texture tutorialPage1;
	public Texture tutorialPage2;
	public Texture tutorialPage3;
	public Texture tutorialPage4;
	public Texture tutorialPage5;
	public Texture tutorialPage6;
	
	public int nodeCost;
	public int generatorCost;
	public int batteryCost;
	public int repairStationCost;
	public int turretCost;
	public int missileLauncherCost;
	public int minerCost;
	
	public float cameraZoom;

    public bool paused = false;
	
    //private float x1;
    //private float x2;
	private float x3;
	private float x4;
	private float x5;
	private float x6;
	private float x7;
	private float x8;
	private float x9;
	private float x10;
//	private float x11;
	private float px;
	private float mx;
	
	private float w1;
	private float w2;
	private float w3;
	private float w4;
//	private float w5;
	private float pw;
	private float mw;
	
	private float h1;
//	private float h2;
	private float ph;
	private float mh;
	
	private float y1;
    //private float y2;
	private float py;
	private float my;

    [SerializeField]
    private float cameraDrag;

    //private float powerPercent;
    //private float currPowFloat;
	private float mousePosTopDown;
	
	private int tutorialPageNum = 1;
    
//	private GameManagerScript gameManager;
	private PowerManagerScript powerManager;
	
	private Vector3 mousePosition;
	private Vector3 mousePosOnScreen;
	private Vector3 newCameraPosition;
    private Vector3 newBackgroundPosition;
    private Vector3 cameraPos;
    private Vector3 cameraPosOld;
    private Vector3 cameraVelocity;
	private float cameraDragSpeed;
	
	private GameObject miningRangeIndicator;
	private GameObject turretRangeIndicator;
	private GameObject repairRangeIndicator;
	private GameObject energyRangeIndicator;

    private GameObject backGround;
	
	private GameObject currentSelection;
	private GameObject currentlyPickedUp;
	private GameObject[] beamsArr;
	private bool[] beamsGood;
	
	private AudioSource audioManager;
	private float soundIncreaseDelay;
	private float BGMVolume = 100f;
	private float SFXVolume = 100f;
	
	private CircleCollider2D currentlyPickedCol;
	private SpriteRenderer currentlyPickedSprite;
    private Camera cameraComp;
	
	private bool placed = true;
	private bool tutorial = false;
	private bool options = false;
	private bool musicIntroDone = true;
    private bool doDragSlow = false;

    // structure upgrade bools
    private bool solarCollectorUpgradeBool = false;
    private bool minerUpgradeBool = false;
    private bool solarBatteryUpgradeBool = false;
    private bool repairStationUpgradeBool = false;
    private bool turretUpgradeBool = false;
	
	private bool miningRangeDisplay = false;
	private bool energyRangeDisplay = false;
	private bool repairRangeDisplay = false;
	private bool turretRangeDisplay = false;

    private bool gotBackground = false;

	void Awake()
	{
		powerManager = GameObject.Find( "PowerManager" ).GetComponent<PowerManagerScript>();
		audioManager = GetComponent<AudioSource>();
        cameraComp = GetComponent<Camera>();
	}

	// Use this for initialization
	void Start () {
		soundIncreaseDelay = 0.1f;

        mineralSkin.label.fontSize = 14;

		currentSelection = null;
		currentlyPickedUp = null;
		cameraDragSpeed = 0.0785f*Camera.main.orthographicSize/2.5f;		//Mathf.Pow(Camera.main.orthographicSize,6)/27000;
		
		audioManager.ignoreListenerVolume = true;
		
//		gameManager = GameObject.Find( "GameManager" ).GetComponent<GameManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
        // set positions based on screen width so all GUI elements will be relocated and resized depending to screen size
        //x1 = Screen.width * (float)( 1f / 50f );
        //x2 = Screen.width * (float)( 7f / 50f );
        x3 = Screen.width * (float)( 11f / 50f );
        x4 = Screen.width * (float)( 15f / 50f );
        x5 = Screen.width * (float)( 19f / 50f );
        x6 = Screen.width * (float)( 23f / 50f );
        x7 = Screen.width * (float)( 27f / 50f );
        x8 = Screen.width * (float)( 31f / 50f );
        x9 = Screen.width * (float)( 35f / 50f );
        x10 = Screen.width * (float)( 39f / 50f );
        //		x11 = Screen.width * (float)( 43f / 50f );

        w1 = Screen.width * (float)( 4f / 50f );
        w2 = Screen.width * (float)( 10f / 50f );
        w3 = Screen.width * (float)( 28f / 50f );
        w4 = w3 * (float)( 13f / 15f );
        //		w5 = 2f;

        h1 = Screen.height * (float)( 10f / 50f );
        //		h2 = Screen.height * (float)( 20f / 50f );

        y1 = Screen.height * (float)( 15f / 19f );
        //y2 = Screen.height * (float)( 7f / 19f );

        //pause button
        px = Screen.width * (float)17/19;
        py = Screen.height * (float)1/19;
        pw = Screen.width * (float)1/19;
        ph = Screen.height * (float)1/19;

        // menu box when paused
        mx = Screen.width * (float)8/19;
        my = Screen.height * (float)5/19;
        mw = Screen.width * (float)3/19;
        mh = Screen.height * (float)8/19;

        if( !gotBackground )
        {
            backGround = GameObject.Find( "Background" );
            gotBackground = true;
        }




        // camera movement
        if( Input.GetMouseButton( 1 ) )
        {
            if( newCameraPosition == Vector3.zero )
            {
                Vector3 dragPanning = mousePosition - Input.mousePosition.FromMainScreenToWorld();
                dragPanning.z = 0;
                transform.position += dragPanning;
                Cursor.visible = false;

                backGround.transform.position += dragPanning / 2;
                doDragSlow = false;
            }
        }
        if( Input.GetMouseButtonUp( 1 ) )
        {
            Cursor.visible = true;

            if( newCameraPosition == Vector3.zero )
            {
                doDragSlow = true;
            }
        }
        
        if( ( newCameraPosition == Vector3.zero ) && ( doDragSlow ) )
        {
            cameraVelocity = cameraComp.velocity;
            cameraVelocity.z = 0f;

            transform.Translate( cameraVelocity * Time.deltaTime * cameraDrag );
            backGround.transform.Translate( ( cameraVelocity * Time.deltaTime * 0.9f ) / 2 );
            //print( "Camera Velocity: " + cameraVelocity * Time.deltaTime * 0.8f );
        }
        

        if( cameraVelocity == Vector3.zero )
        {
            doDragSlow = false;
        }

		// get current mouse coordinates
		mousePosOnScreen = Input.mousePosition;
		mousePosTopDown = ( Screen.height - mousePosOnScreen.y );
		mousePosition = Camera.main.ScreenToWorldPoint( mousePosOnScreen );
		mousePosition = new Vector3( mousePosition.x, mousePosition.y, 0 );
		
		cameraZoom = GetComponent<Camera>().orthographicSize;
		
		if( Input.GetKeyDown( KeyCode.P ) )
		{
			if( !paused )
			{
				paused = true;
			}
			else
			{
				paused = false;
				options = false;
				tutorial = false;
			}
		}
		
		if( !audioManager.isPlaying )
		{
			int music = Random.Range( 0, 9999 );
			music = music % 2;
			
			switch( music )
			{
			case 0:
				audioManager.clip = gameMusic_1;
				break;
			case 1:
				audioManager.clip = gameMusic_2;
				break;
			default:
				print( "music entry error in GUI script" );
				break;
			}
			audioManager.volume = 0f;
			StartCoroutine( musicEntry() );
		}
		else
		{
			musicIntroDone = true;
		}
		
//		print( mousePosition );
	
		if( Input.GetKeyDown( KeyCode.Alpha1 ) )
		{
			if( powerManager.currentMinerals >= generatorCost )
			{
				if( currentlyPickedUp )
				{
					DestroyPickedUp();
				}
				PickUpSolar();
			}
		}
		if( Input.GetKeyDown( KeyCode.Alpha2 ) )
		{
			if( powerManager.currentMinerals >= nodeCost )
			{
				if( currentlyPickedUp )
				{
					DestroyPickedUp();
				}
				PickUpNode();
			}
		}
		if( Input.GetKeyDown( KeyCode.Alpha3 ) )
		{
			if( powerManager.currentMinerals >= minerCost )
			{
				if( currentlyPickedUp )
				{
					DestroyPickedUp();
				}
				PickUpMiner();
			}
		}
		if( Input.GetKeyDown( KeyCode.Alpha4 ) )
		{
			if( powerManager.currentMinerals >= batteryCost )
			{
				if( currentlyPickedUp )
				{
					DestroyPickedUp();
				}
				PickUpStorage();
			}
		}
		if( Input.GetKeyDown( KeyCode.Alpha5 ) )
		{
			if( powerManager.currentMinerals >= repairStationCost )
			{
				if( currentlyPickedUp )
				{
					DestroyPickedUp();
				}
				PickUpRepair();
			}
		}
		if( Input.GetKeyDown( KeyCode.Alpha6 ) )
		{
			if( powerManager.currentMinerals >= turretCost )
			{
				if( currentlyPickedUp )
				{
					DestroyPickedUp();
				}
				PickUpTurret();
			}
		}
        //if( Input.GetKeyDown( KeyCode.Alpha7 ) )
        //{
        //    if( powerManager.currentMinerals >= missileLauncherCost )
        //    {
        //        if( currentlyPickedUp )
        //        {
        //            DestroyPickedUp();
        //        }
        //        PickUpMissile();
        //    }
        //}
	
		// check if you're holding a structure
		if( !placed )
		{ 
			// makes the game object follow the cursor's movements
			currentlyPickedUp.transform.position = mousePosition;
			
			
			// try to check for all the colliders within the power range
			// returns all colliders in that range
			
			if( currentlyPickedUp.layer == 11 ) // checks if what's currently picked up is a node
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll( mousePosition, 1f, StructureMask ); // then checks for any structure colliders around it
				GenericStructureScript currentStructScript = currentlyPickedUp.GetComponent<GenericStructureScript>();
				if( colliders.Length > currentStructScript.maxBeamsAllowed )
				{
					Collider2D[] tempColliders = new Collider2D[currentStructScript.maxBeamsAllowed];
					bool[] alreadyDone = new bool[colliders.Length];
					for( int allowedBeams = 0; allowedBeams < currentStructScript.maxBeamsAllowed; allowedBeams++ )
					{
						float closest = 99999f;
						Collider2D tempTar = null;
						
						for( int otherBeams = 0; otherBeams < colliders.Length; otherBeams++ )
						{
							Vector3 tempLoc = colliders[otherBeams].transform.position;
							float distance = Vector3.Distance( currentlyPickedUp.transform.position, tempLoc );
							if ( distance < closest )
							{
								for( int ad = 0; ad < tempColliders.Length; ad++ )
								{
									if( tempColliders[ad] == colliders[otherBeams] )
									{
										alreadyDone[otherBeams] = true;
										break;
									}
								}
								if( !alreadyDone[otherBeams] )
								{
									closest = distance;
									tempTar = colliders[otherBeams];
								}
							}
						}
						tempColliders[allowedBeams] = tempTar;
					}
					
					colliders = new Collider2D[tempColliders.Length];
					for( int col = 0; col < tempColliders.Length; col++ )
					{
						colliders[col] = tempColliders[col];
					}
				}
				beamsArr = new GameObject[colliders.Length];
				beamsGood = new bool[colliders.Length];
				DrawBeam( colliders, beamsArr );
				
				// every structure gets an energy beam range indicator
				if( energyRangeDisplay )
				{
					energyRangeIndicator.transform.position = currentlyPickedUp.transform.position;
				}
				else
				{
					energyRangeIndicator = (GameObject)Instantiate( energyRangeIndicatorPreFab, currentlyPickedUp.transform.position, Quaternion.identity );
					energyRangeIndicator.transform.localScale = new Vector2( 2.01f, 2.01f );
					energyRangeDisplay = true;
				}
			}
			else if( currentlyPickedUp.layer == 8 ) // if what's picked up is not a node
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll( mousePosition, 1f, NodeMask ); // then find all nodes
				GenericStructureScript currentStructScript = currentlyPickedUp.GetComponent<GenericStructureScript>();
				if( colliders.Length > currentStructScript.maxBeamsAllowed )
				{
					Collider2D[] tempColliders = new Collider2D[currentStructScript.maxBeamsAllowed];
					bool[] alreadyDone = new bool[colliders.Length];
					for( int allowedBeams = 0; allowedBeams < currentStructScript.maxBeamsAllowed; allowedBeams++ )
					{
						float closest = 99999f;
						Collider2D tempTar = null;
						
						for( int otherBeams = 0; otherBeams < colliders.Length; otherBeams++ )
						{
							Vector3 tempLoc = colliders[otherBeams].transform.position;
							float distance = Vector3.Distance( currentlyPickedUp.transform.position, tempLoc );
							if ( distance < closest )
							{
								for( int ad = 0; ad < tempColliders.Length; ad++ )
								{
									if( tempColliders[ad] == colliders[otherBeams] )
									{
										alreadyDone[otherBeams] = true;
										break;
									}
								}
								if( !alreadyDone[otherBeams] )
								{
									closest = distance;
									tempTar = colliders[otherBeams];
								}
							}
						}
						tempColliders[allowedBeams] = tempTar;
					}
					
					colliders = new Collider2D[tempColliders.Length];
					for( int col = 0; col < tempColliders.Length; col++ )
					{
						colliders[col] = tempColliders[col];
					}
				}
				beamsArr = new GameObject[colliders.Length];
				beamsGood = new bool[colliders.Length];
				DrawBeam( colliders, beamsArr );
				
				// every structure gets an energy beam range indicator
				if( energyRangeDisplay )
				{
					energyRangeIndicator.transform.position = currentlyPickedUp.transform.position;
				}
				else
				{
					energyRangeIndicator = (GameObject)Instantiate( energyRangeIndicatorPreFab, currentlyPickedUp.transform.position, Quaternion.identity );
					energyRangeIndicator.transform.localScale = new Vector2( 2.01f, 2.01f );
					energyRangeDisplay = true;
				}
				
				// now for the conditional range indicators
				if( currentlyPickedUp.gameObject.name == "MinerPreFab(Clone)" )
				{
					if( miningRangeDisplay )
					{
						miningRangeIndicator.transform.position = currentlyPickedUp.transform.position;
					}
					else
					{
						miningRangeIndicator = (GameObject)Instantiate( miningRangeIndicatorPreFab, currentlyPickedUp.transform.position, Quaternion.identity );
						MinerScript MinerTemp = currentlyPickedUp.GetComponent<MinerScript>();
						float rangeIndSizeTemp = MinerTemp.miningRange * 2 + 0.01f;
						miningRangeIndicator.transform.localScale = new Vector2( rangeIndSizeTemp, rangeIndSizeTemp );
						miningRangeDisplay = true;
					}
				}
				if( currentlyPickedUp.gameObject.name == "TurretPreFab(Clone)" )
				{
					if( turretRangeDisplay )
					{
						turretRangeIndicator.transform.position = currentlyPickedUp.transform.position;
					}
					else
					{
						turretRangeIndicator = (GameObject)Instantiate( turretRangeIndicatorPreFab, currentlyPickedUp.transform.position, Quaternion.identity );
						Turret_Basic TurretTemp = currentlyPickedUp.GetComponent<Turret_Basic>();
						float rangeIndSizeTemp = TurretTemp.range * 2 + 0.01f;
						turretRangeIndicator.transform.localScale = new Vector2( rangeIndSizeTemp, rangeIndSizeTemp );
						turretRangeDisplay = true;
					}
				}
				if( currentlyPickedUp.gameObject.name == "RepairStationPreFab(Clone)" )
				{
					if( repairRangeDisplay )
					{
						repairRangeIndicator.transform.position = currentlyPickedUp.transform.position;
					}
					else
					{
						repairRangeIndicator = (GameObject)Instantiate( repairRangeIndicatorPreFab, currentlyPickedUp.transform.position, Quaternion.identity );
						Turret_Healer RepairTemp = currentlyPickedUp.GetComponent<Turret_Healer>();
						float rangeIndSizeTemp = RepairTemp.range * 0.625f + 0.01f;
						repairRangeIndicator.transform.localScale = new Vector2( rangeIndSizeTemp, rangeIndSizeTemp );
						repairRangeDisplay = true;
					}
				}
			}
			
			
			// if there is a collider meaning asteroid or other structure that would overlap with this sprite, it changes color to red and left clicking will do nothing
			if( Physics2D.OverlapCircle( mousePosition, currentlyPickedCol.radius, EverythingMask ) )
			{
				currentlyPickedSprite.color = new Color( 1f, 0.5f, 0.5f, 0.9f );
			}
			else
			{
				// sets color back to normal and places object
				currentlyPickedSprite.color = new Color( 1f, 1f, 1f, 1f );
				if( Input.GetMouseButtonDown( 0 ) )
				{
					if( mousePosTopDown < y1 ) // if the mouse is on the lower part of the screen meaning over the GUI, it will delete what is has picked up and pick up a new thing
					{
						bool goodToPlace = false;
						string attemptingToPlace = currentlyPickedUp.gameObject.name;
						
						switch( attemptingToPlace )
						{
						case "MinerPreFab(Clone)":
							if( powerManager.currentMinerals >= minerCost )
							{
								powerManager.currentMinerals -= minerCost;
								goodToPlace = true;
							}
							break;
						case "EnergyStoragePreFab(Clone)":
							if( powerManager.currentMinerals >= batteryCost )
							{
								powerManager.currentMinerals -= batteryCost;
								goodToPlace = true;
							}
							break;
						case "MissilePreFab(Clone)":
							if( powerManager.currentMinerals >= missileLauncherCost )
							{
								powerManager.currentMinerals -= missileLauncherCost;
								goodToPlace = true;
							}
							break;
						case "RepairStationPreFab(Clone)":
							if( powerManager.currentMinerals >= repairStationCost )
							{
								powerManager.currentMinerals -= repairStationCost;
								goodToPlace = true;
							}
							break;
						case "TurretPreFab(Clone)":
							if( powerManager.currentMinerals >= turretCost )
							{
								powerManager.currentMinerals -= turretCost;
								goodToPlace = true;
							}
							break;
						case "SolarCollectorPreFab(Clone)":
							if( powerManager.currentMinerals >= generatorCost )
							{
								powerManager.currentMinerals -= generatorCost;
								goodToPlace = true;
							}
							break;
						case "EnergyNodePreFab(Clone)":
							if( powerManager.currentMinerals >= nodeCost )
							{
								powerManager.currentMinerals -= nodeCost;
								goodToPlace = true;
							}
							break;
						default: 
							break;
						}
						
						if( goodToPlace )
						{
							PlacePickedUp( beamsArr );
							powerManager.AddBeamsToRecord( beamsArr, beamsGood );
							if( currentlyPickedUp.name == "SolarCollectorPreFab(Clone)" )
							{
								powerManager.StartNewTree( currentlyPickedUp );
							}
							else if( currentlyPickedUp.name == "EnergyStoragePreFab(Clone)" )
							{
								powerManager.StartNewTree( currentlyPickedUp );
							}
							powerManager.AddToTree( currentlyPickedUp );
							
							if( Input.GetKey( KeyCode.LeftShift ) )
							{
								string tempName = currentlyPickedUp.gameObject.name;
								ClearPickedUp();
								
								switch( tempName )
								{
								case "MinerPreFab(Clone)":
									PickUpMiner();
									break;
								case "EnergyStoragePreFab(Clone)":
									PickUpStorage();
									break;
								case "MissilePreFab(Clone)":
									PickUpMissile();
									break;
								case "RepairStationPreFab(Clone)":
									PickUpRepair();
									break;
								case "TurretPreFab(Clone)":
									PickUpTurret();
									break;
								case "SolarCollectorPreFab(Clone)":
									PickUpSolar();
									break;
								case "EnergyNodePreFab(Clone)":
									PickUpNode();
									break;
								default: 
									break;
								}
							}
							else
							{
								ClearPickedUp();
							}
						}
					}
					else
					{
						DestroyPickedUp();
						DestroyBadBeams();
						DestroyUnplantedBeams();
					}
				}
			}
			if( Input.GetKeyDown( KeyCode.Escape ) )
			{
				DestroyPickedUp();
				DestroyBadBeams();
				DestroyUnplantedBeams();
			}
		}
		else
		{
			// while you have nothing picked up, select and deselect whatever you click on
			if( Input.GetMouseButtonDown( 0 ) )
			{

				if( mousePosTopDown < y1 )
				{
					// this shoots a cool little line to wherever you click.  Have to view it in 3D to see it
	//				Debug.DrawLine( camera.transform.position, mousePosition );
	
					Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
					RaycastHit2D hit = Physics2D.GetRayIntersection( ray, Mathf.Infinity );
					
					if( ( hit.collider != null ) && ( hit.transform.gameObject.name != "EnergyBeamPreFab(Clone)" ) )
					{
						currentSelection = hit.transform.gameObject;
	
						if( currentSelection.gameObject.name == "MinerPreFab(Clone)" )
						{
							if( miningRangeDisplay )
							{
								miningRangeIndicator.transform.position = currentSelection.transform.position;
							}
							else
							{
								miningRangeIndicator = (GameObject)Instantiate( miningRangeIndicatorPreFab, currentSelection.transform.position, Quaternion.identity );
								MinerScript MinerTemp = currentSelection.GetComponent<MinerScript>();
								float rangeIndSizeTemp = MinerTemp.miningRange * 2 + 0.01f;
								miningRangeIndicator.transform.localScale = new Vector2( rangeIndSizeTemp, rangeIndSizeTemp );
								miningRangeDisplay = true;
							}
							if( repairRangeIndicator != null )
							{
								Destroy( repairRangeIndicator.gameObject );
								repairRangeDisplay = false;
							}
							if( turretRangeIndicator != null )
							{
								Destroy( turretRangeIndicator.gameObject );
								turretRangeDisplay = false;
							}
						}
						if( currentSelection.gameObject.name == "TurretPreFab(Clone)" )
						{
							if( turretRangeDisplay )
							{
								turretRangeIndicator.transform.position = currentSelection.transform.position;
							}
							else
							{
								turretRangeIndicator = (GameObject)Instantiate( turretRangeIndicatorPreFab, currentSelection.transform.position, Quaternion.identity );
								Turret_Basic TurretTemp = currentSelection.GetComponent<Turret_Basic>();
								float rangeIndSizeTemp = TurretTemp.range * 2 + 0.01f;
								turretRangeIndicator.transform.localScale = new Vector2( rangeIndSizeTemp, rangeIndSizeTemp );
								turretRangeDisplay = true;
							}
							if( miningRangeIndicator != null )
							{
								Destroy( miningRangeIndicator.gameObject );
								miningRangeDisplay = false;
							}
							if( repairRangeIndicator != null )
							{
								Destroy( repairRangeIndicator.gameObject );
								repairRangeDisplay = false;
							}
						}
						if( currentSelection.gameObject.name == "RepairStationPreFab(Clone)" )
						{
							if( repairRangeDisplay )
							{
								repairRangeIndicator.transform.position = currentSelection.transform.position;
							}
							else
							{
								repairRangeIndicator = (GameObject)Instantiate( repairRangeIndicatorPreFab, currentSelection.transform.position, Quaternion.identity );
								Turret_Healer RepairTemp = currentSelection.GetComponent<Turret_Healer>();
								float rangeIndSizeTemp = RepairTemp.range * 0.625f + 0.01f;
								repairRangeIndicator.transform.localScale = new Vector2( rangeIndSizeTemp, rangeIndSizeTemp );
								repairRangeDisplay = true;
							}
							if( miningRangeIndicator != null )
							{
								Destroy( miningRangeIndicator.gameObject );
								miningRangeDisplay = false;
							}
							if( turretRangeIndicator != null )
							{
								Destroy( turretRangeIndicator.gameObject );
								turretRangeDisplay = false;
							}
						}
					}
					else
					{
						currentSelection = null;
                        minerUpgradeBool = false;
                        repairStationUpgradeBool = false;
                        solarBatteryUpgradeBool = false;
                        solarCollectorUpgradeBool = false;
                        turretUpgradeBool = false;
	
						if( miningRangeIndicator != null )
						{
							Destroy( miningRangeIndicator.gameObject );
							miningRangeDisplay = false;
						}
						if( repairRangeIndicator != null )
						{
							Destroy( repairRangeIndicator.gameObject );
							repairRangeDisplay = false;
						}
						if( turretRangeIndicator != null )
						{
							Destroy( turretRangeIndicator.gameObject );
							turretRangeDisplay = false;
						}
					}
				}
			}
			if( Input.GetKeyDown( KeyCode.Escape ) )
			{
				if( currentSelection != null )
				{
					currentSelection = null;
                    minerUpgradeBool = false;
                    repairStationUpgradeBool = false;
                    solarBatteryUpgradeBool = false;
                    solarCollectorUpgradeBool = false;
                    turretUpgradeBool = false;
					
					if( miningRangeIndicator != null )
					{
						Destroy( miningRangeIndicator.gameObject );
						miningRangeDisplay = false;
					}
					if( repairRangeIndicator != null )
					{
						Destroy( repairRangeIndicator.gameObject );
						repairRangeDisplay = false;
					}
					if( turretRangeIndicator != null )
					{
						Destroy( turretRangeIndicator.gameObject );
						turretRangeDisplay = false;
					}
				}
				else
				{
					if( !paused )
					{
						paused = true;
					}
					else
					{
						paused = false;
						options = false;
						tutorial = false;
					}
				}
			}
			if( Input.GetKey( KeyCode.E ) )
			{
				int posOrNegX = Random.Range( 1, 5000 );
				if( posOrNegX % 2 == 1 )
				{
					posOrNegX = -1;
				}
				else
				{
					posOrNegX = 1;
				}
				int posOrNegY = Random.Range( 1, 5000 );
				if( posOrNegY % 2 == 1 )
				{
					posOrNegY = -1;
				}
				else
				{
					posOrNegY = 1;
				}
				float randomPosX = 10f;
				float randomPosY = 10f;
				bool tooClose = true;
				while( tooClose )
				{
					randomPosX = (float)Random.Range( 0f, 20f );
					randomPosY = (float)Random.Range( 0f, 20f );
					
					float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
					if( distance >= 15 )
					{
						tooClose = false;
					}
				}
				Instantiate( enemyExploderPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, -.5f ), Quaternion.identity );
			}
			
			if( Input.GetKey( KeyCode.R ) )
			{
				int posOrNegX = Random.Range( 1, 5000 );
				if( posOrNegX % 2 == 1 )
				{
					posOrNegX = -1;
				}
				else
				{
					posOrNegX = 1;
				}
				int posOrNegY = Random.Range( 1, 5000 );
				if( posOrNegY % 2 == 1 )
				{
					posOrNegY = -1;
				}
				else
				{
					posOrNegY = 1;
				}
				float randomPosX = 10f;
				float randomPosY = 10f;
				bool tooClose = true;
				while( tooClose )
				{
					randomPosX = (float)Random.Range( 0f, 20f );
					randomPosY = (float)Random.Range( 0f, 20f );
					
					float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
					if( distance >= 15 )
					{
						tooClose = false;
					}
				}
				Instantiate( enemySwarmerPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, -.5f ), Quaternion.identity );
			}
			
			if( Input.GetKey( KeyCode.C ) )
			{
				int posOrNegX = Random.Range( 1, 5000 );
				if( posOrNegX % 2 == 1 )
				{
					posOrNegX = -1;
				}
				else
				{
					posOrNegX = 1;
				}
				int posOrNegY = Random.Range( 1, 5000 );
				if( posOrNegY % 2 == 1 )
				{
					posOrNegY = -1;
				}
				else
				{
					posOrNegY = 1;
				}
				float randomPosX = 10f;
				float randomPosY = 10f;
				bool tooClose = true;
				while( tooClose )
				{
					randomPosX = (float)Random.Range( 0f, 20f );
					randomPosY = (float)Random.Range( 0f, 20f );
					
					float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
					if( distance >= 15 )
					{
						tooClose = false;
					}
				}
				Instantiate( enemyCloaker, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, -.5f ), Quaternion.identity );
			}
			
			if( Input.GetKey( KeyCode.T ) )
			{
				int posOrNegX = Random.Range( 1, 5000 );
				if( posOrNegX % 2 == 1 )
				{
					posOrNegX = -1;
				}
				else
				{
					posOrNegX = 1;
				}
				int posOrNegY = Random.Range( 1, 5000 );
				if( posOrNegY % 2 == 1 )
				{
					posOrNegY = -1;
				}
				else
				{
					posOrNegY = 1;
				}
				float randomPosX = 10f;
				float randomPosY = 10f;
				bool tooClose = true;
				while( tooClose )
				{
					randomPosX = (float)Random.Range( 0f, 20f );
					randomPosY = (float)Random.Range( 0f, 20f );
					
					float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
					if( distance >= 15 )
					{
						tooClose = false;
					}
				}
				Instantiate( enemyTankPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, -.5f ), Quaternion.identity );
			}
			
			//This one is for the basic enemy. I cant figure out how to make a prefab right.
			if( Input.GetKey( KeyCode.B ) )
			{
				int posOrNegX = Random.Range( 1, 5000 );
				if( posOrNegX % 2 == 1 )
				{
					posOrNegX = -1;
				}
				else
				{
					posOrNegX = 1;
				}
				int posOrNegY = Random.Range( 1, 5000 );
				if( posOrNegY % 2 == 1 )
				{
					posOrNegY = -1;
				}
				else
				{
					posOrNegY = 1;
				}
				float randomPosX = 10f;
				float randomPosY = 10f;
				bool tooClose = true;
				while( tooClose )
				{
					randomPosX = (float)Random.Range( 0f, 20f );
					randomPosY = (float)Random.Range( 0f, 20f );
					
					float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
					if( distance >= 15 )
					{
						tooClose = false;
					}
				}
				Instantiate( enemyBasicPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, -.5f ), Quaternion.identity );
			}
			
			if( Input.GetKey( KeyCode.M ) )
			{
				powerManager.currentMinerals += 10;
			}
			
			if( Input.GetKeyDown( KeyCode.O ) )
			{
				Instantiate( mediumWormPreFab, new Vector3( 5f, 5f, -0.5f ), Quaternion.identity );
			}
		}
    }
    
    void LateUpdate()
    {
		// camera movement
        //if( Input.GetMouseButton( 1 ) )
        //{
        //    newCameraPosition.x = cameraDragSpeed * -Input.GetAxis( "Mouse X" );
        //    newCameraPosition.y = cameraDragSpeed * -Input.GetAxis( "Mouse Y" );
        //    Cursor.visible = false;
        //}
		if( Input.GetKey( KeyCode.A ) )
		{
			newCameraPosition.x -= cameraDragSpeed * 0.002f;
            newBackgroundPosition.x -= ( cameraDragSpeed * 0.002f ) / 2;
		}
		if( Input.GetKey( KeyCode.D ) )
		{
			newCameraPosition.x += cameraDragSpeed * 0.002f;
            newBackgroundPosition.x += ( cameraDragSpeed * 0.002f ) / 2;
		}
		if( Input.GetKey( KeyCode.W ) )
		{
			newCameraPosition.y += cameraDragSpeed * 0.002f;
            newBackgroundPosition.y += ( cameraDragSpeed * 0.002f ) / 2;
		}
		if( Input.GetKey( KeyCode.S ) )
		{
			newCameraPosition.y -= cameraDragSpeed * 0.002f;
            newBackgroundPosition.y -= ( cameraDragSpeed * 0.002f ) / 2;
		}
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			newCameraPosition.x = 0f;
			newCameraPosition.y = 0f;
            newBackgroundPosition.x = 0f;
            newBackgroundPosition.y = 0f;
		}
		
		transform.Translate( newCameraPosition );
        backGround.transform.Translate( newBackgroundPosition );

		if( Input.GetAxis( "Mouse ScrollWheel" ) < 0 ) // if you scroll wheel backwards
		{
			Camera.main.orthographicSize++;
		}
		if( Input.GetAxis( "Mouse ScrollWheel" ) > 0 ) // if you scroll wheel forwards
		{
			Camera.main.orthographicSize--;
		}
		Camera.main.orthographicSize = Mathf.Clamp( Camera.main.orthographicSize, 1.5f, 5f ); // make sure the zoom doesn't get closer than 1.5 or further than 5

    }

	private IEnumerator musicEntry()
	{
		musicIntroDone = false;
		audioManager.Play();
		
		for( int i = 0; i < 10; i++ )
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
	
	private IEnumerator doubleChecker()
	{
		yield return new WaitForSeconds( 1.5f );
		musicIntroDone = true;
	}
    
    void OnGUI()
	{
		float ry = mh * (float)1/8;
		float sy = mh * (float)2/8;
		float iy = mh * (float)3/8;
		float oy = mh * (float)4/8;
		float cy = mh * (float)5/8;
		float ey = mh * (float)6/8;
		float qy = mh * (float)7/8;
		
		float tx = Screen.width * (float)2/15;
		float ty = Screen.height * (float)1/15;
		float tw = Screen.width * (float)11/15;
		float th = Screen.height * (float)13/15;
		
		float opx = Screen.width * (float)6/15;
		float opw = Screen.width * (float)3/15;
		float opy = Screen.height * (float)6/15;
		float oph = Screen.height * (float)3/15;
		
		float optionHeight = mh * (float)1/10;
		
		if( GUI.Button( new Rect( px, py, pw, ph ), "Pause" ) )
		{
			if( !paused )
			{
                minerUpgradeBool = false;
                repairStationUpgradeBool = false;
                solarBatteryUpgradeBool = false;
                solarCollectorUpgradeBool = false;
                turretUpgradeBool = false;
				paused = true;
			}
			else
			{
				paused = false;
				options = false;
				tutorial = false;
			}
		}
		
		// Pause Menu
		GUI.BeginGroup( new Rect( mx, my, mw, mh ) );
		
		if( ( paused == true ) && ( tutorial == false ) && ( options == false ) )
		{
			GUI.Box( new Rect( 0, 0, mw, mh ), "Menu" );
			
			if( GUI.Button( new Rect( 10, ry, mw - 20, optionHeight ), "Resume" ) )
			{
				paused = false;
			}
			if( GUI.Button( new Rect( 10, sy, mw - 20, optionHeight ), "Save" ) )
			{
				
			}
			if( GUI.Button( new Rect( 10, iy, mw - 20, optionHeight ), "Instructions" ) )
			{
				tutorial = true;
			}
			if( GUI.Button( new Rect( 10, oy, mw - 20, optionHeight ), "Options" ) )
			{
				options = true;
			}
			if( GUI.Button( new Rect( 10, cy, mw - 20, optionHeight ), "Credits" ) )
			{
				
			}
			if( GUI.Button( new Rect( 10, ey, mw - 20, optionHeight ), "Main Menu" ) )
			{
				powerManager.CleanHouseForMainMenu();
			}
			if( GUI.Button( new Rect( 10, qy, mw - 20, optionHeight ), "Quit" ) )
			{
				Application.Quit();
			}
		}
		GUI.EndGroup();
		
		// Options menu
		GUI.BeginGroup( new Rect( opx, opy, opw, oph ) );
		
		if( ( paused == true ) && ( options == true ) )
		{
			GUI.Box( new Rect( 0, 0, opw, oph ), "Options" );
			
			GUI.Label( new Rect( opw/10, oph / 3 - 20, 200, 30 ), "Sound Effects Volume" );
			
			SFXVolume = GUI.HorizontalSlider( new Rect( opw/10, oph / 3, opw * 8 / 10, 20 ), SFXVolume, 0f, 100f );
			AudioListener.volume = SFXVolume/100;
			
			GUI.Label( new Rect( opw/10, oph * 2 / 3 - 20, 200, 30 ), "Background Music Volume" );
			
			BGMVolume = GUI.HorizontalSlider( new Rect( opw/10, oph * 2 / 3, opw * 8 / 10, 20 ), BGMVolume, 0f, 100f );
			if( musicIntroDone )
			{
				audioManager.volume = (float)BGMVolume/100;
			}
			
			if( GUI.Button( new Rect( 10, oph - 30, opw/5, 25 ), "Back" ) )
			{
				options = false;
			}
		}
		GUI.EndGroup();
		
		
		// Tutorial screens
		GUI.BeginGroup( new Rect( tx, ty, tw, th ) );
		
		if( ( paused == true ) && ( tutorial == true ) )
		{
			GUI.Box( new Rect( 0, 0, tw, th ), "" );
			
			GUI.Box( new Rect( 10, 10, 1179, 743 ), "" ); // these are the coordinates and width and height and stuff for the tutorial texture
			
			switch( tutorialPageNum )
			{
				case 1:
					GUI.DrawTexture( new Rect( 10, 10, (int)tw - 20, (int)th - 55 ), tutorialPage1 );
					break;
				case 2:
					GUI.DrawTexture( new Rect( 10, 10, (int)tw - 20, (int)th - 55 ), tutorialPage2 );
					break;
				case 3:
					GUI.DrawTexture( new Rect( 10, 10, (int)tw - 20, (int)th - 55 ), tutorialPage3 );
					break;
				case 4:
					GUI.DrawTexture( new Rect( 10, 10, (int)tw - 20, (int)th - 55 ), tutorialPage4 );
					break;
				case 5:
					GUI.DrawTexture( new Rect( 10, 10, (int)tw - 20, (int)th - 55 ), tutorialPage5 );
					break;
				case 6:
					GUI.DrawTexture( new Rect( 10, 10, (int)tw - 20, (int)th - 55 ), tutorialPage6 );
					break;
				default:
					print( "Tutorial page number error" );
					break;
			}
			
			if( GUI.Button( new Rect( 10, th - 30, tw/10, 25 ), "Back" ) )
			{
				tutorial = false;
			}
			if( GUI.Button( new Rect( 20 + tw/10, th - 30, tw/10, 25 ), "Previous" ) )
			{
				tutorialPageNum--;
				if( tutorialPageNum < 1 )
				{
					tutorialPageNum = 6;
				}
			}
			if( GUI.Button( new Rect( tw - tw/10 - 10, th - 30, tw/10, 25 ), "Next" ) )
			{
				tutorialPageNum++;
				if( tutorialPageNum > 6 )
				{
					tutorialPageNum = 1;
				}
			}
		}
		
		GUI.EndGroup();

		if(paused == true)
		{
			Time.timeScale = 0;
		}
		if(paused == false )
		{
			Time.timeScale = 1;
		}
		if( !paused )
		{
			if( currentSelection == null )
			{
				GUI.skin = mainSkin;
	
				// creates a group for GUI elements.  Basically the same thing as a div in HTML or something.  The group sets its global coordinates and then everything in
				// the group has coordinates relative to the BeginGroup coordinates
	
				//-----Solar Collector-----
				GUI.BeginGroup( new Rect( x3, y1, w1, h1 ), "");
				// if this button is clicked, it calls the function which instantiates the solar collector and sets the placed flag to false
				if( GUI.Button( new Rect( 0, 0, w1, h1 ), "" ) )
				{
					if( powerManager.currentMinerals >= generatorCost )
					{
						if( currentlyPickedUp )
						{
							DestroyPickedUp();
						}
						PickUpSolar();
					}
				}
                GUI.Label(new Rect(w1 / 2 - 46, h1 - 35, 100, 30), "Solar Generator");
				// this label puts the png icon of the solar collector in the GUI button
				GUI.Label( new Rect( w1 / 2 - 10, h1 / 3 - 16, 32, 32 ), new GUIContent( solar ) );
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( w1 / 2 - 19, h1 / 2 - 05, 50, 50 ), "" + generatorCost );
				GUI.EndGroup();
				GUI.skin = mainSkin;
				
				//-----Energy Node-----
				GUI.BeginGroup( new Rect( x4, y1, w1, h1 ), "");
				if( GUI.Button( new Rect( 0, 0, w1, h1 ), "" ) )
				{
					if( powerManager.currentMinerals >= nodeCost )
					{
						if( currentlyPickedUp )
						{
							DestroyPickedUp();
						}
						PickUpNode();
					}
				}
                GUI.Label(new Rect(w1 / 2 - 40, h1 - 35, 90, 30), "Energy Node");
				GUI.Label( new Rect( w1 / 2 - 2, h1 / 3 - 16, 32, 32 ), new GUIContent( node ) );
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( w1 / 2 - 21, h1 / 2 - 05, 50, 50 ), "" + nodeCost );
				GUI.EndGroup();
                GUI.skin = mainSkin;
	
				//-----Miner-----
				GUI.BeginGroup( new Rect( x5, y1, w1, h1 ), "");
				if( GUI.Button( new Rect( 0, 0, w1, h1 ), "" ) )
				{
					if( powerManager.currentMinerals >= minerCost )
					{
						if( currentlyPickedUp )
						{
							DestroyPickedUp();
						}
						PickUpMiner();
					}
				}
                GUI.Label(new Rect(w1 / 2 - 15, h1 - 35, 90, 30), "Miner");
				GUI.Label( new Rect( w1 / 2 - 5, h1 / 3 - 16, 32, 32 ), new GUIContent( miner ) );
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( w1 / 2 - 19, h1 / 2 - 05, 50, 50 ), "" + minerCost );
				GUI.EndGroup();
                GUI.skin = mainSkin;
	
				//-----Energy Storage-----
				GUI.BeginGroup( new Rect( x6, y1, w1, h1 ), "");
				if( GUI.Button( new Rect( 0, 0, w1, h1 ), "" ) )
				{
					if( powerManager.currentMinerals >= batteryCost )
					{
						if( currentlyPickedUp )
						{
							DestroyPickedUp();
						}
						PickUpStorage();
					}
				}
                GUI.Label(new Rect(w1 / 2 - 36, h1 - 35, 90, 30), "Solar Battery");
				GUI.Label( new Rect( w1 / 2 - 10, h1 / 3 - 16, 32, 32 ), new GUIContent( storage ) );
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( w1 / 2 - 19, h1 / 2 - 05, 50, 50 ), "" + batteryCost );
				GUI.EndGroup();
                GUI.skin = mainSkin;
	
				//-----Repair Station-----
				GUI.BeginGroup( new Rect( x7, y1, w1, h1 ), "");
				if( GUI.Button( new Rect( 0, 0, w1, h1 ), "" ) )
				{
					if( powerManager.currentMinerals >= repairStationCost )
					{
						if( currentlyPickedUp )
						{
							DestroyPickedUp();
						}
						PickUpRepair();
					}
				}
                GUI.Label(new Rect(w1 / 2 - 40, h1 - 35, 90, 30), "Repair Station");
				GUI.Label( new Rect( w1 / 2 - 15, h1 / 3 - 16, 32, 32 ), new GUIContent( healer ) );
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( w1 / 2 - 23, h1 / 2 - 05, 50, 50 ), "" + repairStationCost );
				GUI.EndGroup();
                GUI.skin = mainSkin;
	
				//-----Basic Turret-----
				GUI.BeginGroup( new Rect( x8, y1, w1, h1 ), "");
				if( GUI.Button( new Rect( 0, 0, w1, h1 ), "" ) )
				{
					if( powerManager.currentMinerals >= turretCost )
					{
						if( currentlyPickedUp )
						{
							DestroyPickedUp();
						}
						PickUpTurret();
					}
				}
                GUI.Label(new Rect(w1 / 2 - 37, h1 - 35, 90, 30), "Basic Turret");
				GUI.Label( new Rect( w1 / 2 - 8, h1 / 3 - 16, 32, 32 ), new GUIContent( turret ) );
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( w1 / 2 - 21, h1 / 2 - 05, 50, 50 ), "" + turretCost );
				GUI.EndGroup();
                GUI.skin = mainSkin;
	
				//-----Missile Defense-----
				GUI.BeginGroup( new Rect( x9, y1, w1, h1 ), "");
				if( GUI.Button( new Rect( 0, 0, w1, h1 ), "" ) )
				{
                    //if( powerManager.currentMinerals >= missileLauncherCost )
                    //{
                    //    if( currentlyPickedUp )
                    //    {
                    //        DestroyPickedUp();
                    //    }
                    //    PickUpMissile();
                    //}
				}
				GUI.Label( new Rect( w1 / 2 - 45, h1 - 35, 100, 30 ), "Missile Defense" );
				GUI.Label( new Rect( w1 / 2 - 8, h1 / 3 - 16, 32, 32 ), new GUIContent( missiles ) );
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( w1 / 2 - 21, h1 / 2 - 05, 50, 50 ), "" + missileLauncherCost );
				GUI.EndGroup();
                GUI.skin = mainSkin;
			}
			else
			{
				GUI.BeginGroup( new Rect( x3, y1, w3, h1 ), "" );
				GUI.Box( new Rect( 0, 0, w3, h1 ), "" );
				//=====Check What's Selected and Display Information Accordingly=====
	
				//=====Draw The Asteroid's Information GUI=====
				GUI.skin = structSelectSkin;
				
				AsteroidScript aster;
				int health;
				int beamsCount;
				int storedEnergy;
				float powerGeneration;
				switch( currentSelection.name )
				{
				case "Asteroid1v3(Clone)":
					aster = currentSelection.GetComponent<AsteroidScript>();
					//-----Asteroids Current Minerals / Starting Minerals -----
					GUI.Label( new Rect( w2 + 10, h1 / 2, 500, 30 ), "Mineral Count: " + aster.currentMins + " / " + aster.startingMins );
					//-----Graphic of the Asteroid-----
					GUI.skin = structSelectionTitleSkin;
					GUI.Label( new Rect( w1, h1/2-50 , 200, 100 ), "Asteroid" );
					GUI.skin = structSelectSkin;
					break;
				case "Asteroid2v3(Clone)":
					aster = currentSelection.GetComponent<AsteroidScript>();
					//-----Asteroids Current Minerals / Starting Minerals -----
					GUI.Label( new Rect( w2 + 10, h1 / 2, 500, 30 ), "Mineral Count: " + aster.currentMins + " / " + aster.startingMins );
					//-----Graphic of the Asteroid-----
					GUI.skin = structSelectionTitleSkin;
					GUI.Label( new Rect( w1, h1/2-50 , 200, 100 ), "Asteroid" );
					GUI.skin = structSelectSkin;
					break;
				case "Asteroid3v3(Clone)":
					aster = currentSelection.GetComponent<AsteroidScript>();
					//-----Asteroids Current Minerals / Starting Minerals -----
					GUI.Label( new Rect( w2 + 10, h1 / 2, 500, 30 ), "Mineral Count: " + aster.currentMins + " / " + aster.startingMins );
					//-----Graphic of the Asteroid-----
					GUI.skin = structSelectionTitleSkin;
					GUI.Label( new Rect( w1, h1/2-50 , 200, 100 ), "Asteroid" );
					GUI.skin = structSelectSkin;
					break;
                //case "Asteroid4(Clone)":
                //    aster = currentSelection.GetComponent<AsteroidScript>();
                //    //-----Asteroids Current Minerals / Starting Minerals -----
                //    GUI.Label( new Rect( w2 + 10, h1 / 2, 500, 30 ), "Mineral Count: " + aster.currentMins + " / " + aster.startingMins );
                //    //-----Graphic of the Asteroid-----
                //    GUI.skin = structSelectionTitleSkin;
                //    GUI.Label( new Rect( w1, h1/2-50 , 200, 100 ), "Asteroid" );
                //    GUI.skin = structSelectSkin;
                //    break;
				case "MinerPreFab(Clone)":
					health = powerManager.Trees.GetNodeHealth( currentSelection, powerManager.NodesRecord );
					beamsCount = powerManager.Trees.GetNodeAttachedBeams( currentSelection, powerManager.NodesRecord );
					storedEnergy = powerManager.Trees.GetNodeStoredPower( currentSelection, powerManager.NodesRecord );
					
					GUI.Label( new Rect( w2+70 , h1 / 2 - 40, 32, 32 ), new GUIContent( miner ) );
					GUI.Label( new Rect( w3 * (float)1/13, h1 / 2, w4, 50 ), "Energy: " + storedEnergy );
					GUI.Label( new Rect( w3 * (float)4/13, h1 / 2, w4, 50 ), "Health: " + health );
					GUI.Label( new Rect( w3 * (float)7/13, h1 / 2, w4, 50 ), "Beams: "  + beamsCount );
					
                    // salvage button
					if( GUI.Button( new Rect( w3 * (float)10/13, h1 / 2, w3 * (float)3/13, 50 ), "Salvage" ) )
					{
						MinerScript structScript = currentSelection.GetComponent<MinerScript>();
						if( !structScript.AbleToMine() )
						{
							if( structScript.transform.tag == "Bad Miner" )
							{
								GameObject[] badMiners = GameObject.FindGameObjectsWithTag( "Bad Miner" );
								for( int i = 0; i < badMiners.Length; i++ )
								{
                                    MinerScript tempScript = badMiners[i].GetComponent<MinerScript>();
									tempScript.StartCoroutine( tempScript.Die( true, minerCost ) );
								}
							}
							else
							{
                                structScript.StartCoroutine( structScript.Die( true, minerCost ) );
							}
						}
						else
						{
                            structScript.StartCoroutine( structScript.Die( true, minerCost ) );
						}
						if( miningRangeIndicator != null )
						{
							Destroy( miningRangeIndicator.gameObject );
							miningRangeDisplay = false;
						}
					}

                    // upgrade button
                    if( GUI.Button( new Rect( w3 * (float)10 / 13, 2, w3 * (float)3 / 13, 50 ), "Upgrade" ) )
                    {
                        MinerScript structScript = currentSelection.GetComponent<MinerScript>();
                        if( structScript.built )
                        {
                            minerUpgradeBool = !minerUpgradeBool;
                            if( ( miningRangeIndicator != null ) && ( minerUpgradeBool == true ) )
                            {
                                Destroy( miningRangeIndicator.gameObject );
                                miningRangeDisplay = false;
                            }
                        }
                    }
					break;
				case "EnergyStoragePreFab(Clone)":
					health = powerManager.Trees.GetNodeHealth( currentSelection, powerManager.NodesRecord );
					beamsCount = powerManager.Trees.GetNodeAttachedBeams( currentSelection, powerManager.NodesRecord );
					storedEnergy = powerManager.Trees.GetNodeStoredPower( currentSelection, powerManager.NodesRecord );
					
					GUI.Label( new Rect( w2+70 , h1 / 2 - 40, 32, 32 ), new GUIContent( storage ) );
					GUI.Label( new Rect( w3 * (float)1/13, h1 / 2, w4, 50 ), "Energy: " + storedEnergy );
					GUI.Label( new Rect( w3 * (float)4/13, h1 / 2, w4, 50 ), "Health: " + health );
					GUI.Label( new Rect( w3 * (float)7/13, h1 / 2, w4, 50 ), "Beams: "  + beamsCount );
					
					if( GUI.Button( new Rect( w3 * (float)10/13, h1 / 2, w3 * (float)3/13, 50 ), "Salvage" ) )
					{
						GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        structScript.StartCoroutine( structScript.Die( true, batteryCost ) );
					}

                    // upgrade button
                    if( GUI.Button( new Rect( w3 * (float)10 / 13, 2, w3 * (float)3 / 13, 50 ), "Upgrade" ) )
                    {
                        GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        if( structScript.built )
                        {
                            solarBatteryUpgradeBool = !solarBatteryUpgradeBool;
                        }
                    }
					break;
                //case "MissilePreFab(Clone)":
                //    health = powerManager.Trees.GetNodeHealth( currentSelection, powerManager.NodesRecord );
                //    beamsCount = powerManager.Trees.GetNodeAttachedBeams( currentSelection, powerManager.NodesRecord );
                //    storedEnergy = powerManager.Trees.GetNodeStoredPower( currentSelection, powerManager.NodesRecord );
					
                //    GUI.Label( new Rect( w2+70 , h1 / 2 - 40, 32, 32 ), new GUIContent( missiles ) );
                //    GUI.Label( new Rect( w3 * (float)1/13, h1 / 2, w4, 50 ), "Energy: " + storedEnergy );
                //    GUI.Label( new Rect( w3 * (float)4/13, h1 / 2, w4, 50 ), "Health: " + health );
                //    GUI.Label( new Rect( w3 * (float)7/13, h1 / 2, w4, 50 ), "Beams: "  + beamsCount );
					
                //    if( GUI.Button( new Rect( w3 * (float)10/13, h1 / 2, w3 * (float)3/13, 50 ), "Salvage" ) )
                //    {
                //        GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                //        structScript.Die( true, missileLauncherCost );
                //    }
                //    break;
				case "RepairStationPreFab(Clone)":
					health = powerManager.Trees.GetNodeHealth( currentSelection, powerManager.NodesRecord );
					beamsCount = powerManager.Trees.GetNodeAttachedBeams( currentSelection, powerManager.NodesRecord );
					storedEnergy = powerManager.Trees.GetNodeStoredPower( currentSelection, powerManager.NodesRecord );
					
					GUI.Label( new Rect( w2+70 , h1 / 2 - 40, 32, 32 ), new GUIContent( healer ) );
					GUI.Label( new Rect( w3 * (float)1/13, h1 / 2, w4, 50 ), "Energy: " + storedEnergy );
					GUI.Label( new Rect( w3 * (float)4/13, h1 / 2, w4, 50 ), "Health: " + health );
					GUI.Label( new Rect( w3 * (float)7/13, h1 / 2, w4, 50 ), "Beams: "  + beamsCount );
					
					if( GUI.Button( new Rect( w3 * (float)10/13, h1 / 2, w3 * (float)3/13, 50 ), "Salvage" ) )
					{
						GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
						structScript.Die( true, repairStationCost );
						if( repairRangeIndicator != null )
						{
							Destroy( repairRangeIndicator.gameObject );
                            structScript.StartCoroutine( structScript.Die( true, repairStationCost ) );
						}
					}

                    // upgrade button
                    if( GUI.Button( new Rect( w3 * (float)10 / 13, 2, w3 * (float)3 / 13, 50 ), "Upgrade" ) )
                    {
                        GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        if( structScript.built )
                        {
                            repairStationUpgradeBool = !repairStationUpgradeBool;
                            if( ( repairRangeIndicator != null ) && ( repairStationUpgradeBool == true ) )
                            {
                                Destroy( repairRangeIndicator.gameObject );
                                repairRangeDisplay = false;
                            }
                        }
                    }
					break;
				case "TurretPreFab(Clone)":
					health = powerManager.Trees.GetNodeHealth( currentSelection, powerManager.NodesRecord );
					beamsCount = powerManager.Trees.GetNodeAttachedBeams( currentSelection, powerManager.NodesRecord );
					storedEnergy = powerManager.Trees.GetNodeStoredPower( currentSelection, powerManager.NodesRecord );
					
					GUI.Label( new Rect( w2+70 , h1 / 2 - 40, 32, 32 ), new GUIContent( turret ) );
					GUI.Label( new Rect( w3 * (float)1/13, h1 / 2, w4, 50 ), "Energy: " + storedEnergy );
					GUI.Label( new Rect( w3 * (float)4/13, h1 / 2, w4, 50 ), "Health: " + health );
					GUI.Label( new Rect( w3 * (float)7/13, h1 / 2, w4, 50 ), "Beams: "  + beamsCount );
					
					if( GUI.Button( new Rect( w3 * (float)10/13, h1 / 2, w3 * (float)3/13, 50 ), "Salvage" ) )
					{
						GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        structScript.StartCoroutine( structScript.Die( true, turretCost ) );
						if( turretRangeIndicator != null )
						{
							Destroy( turretRangeIndicator.gameObject );
							turretRangeDisplay = false;
						}
					}
                    // upgrade button
                    if( GUI.Button( new Rect( w3 * (float)10 / 13, 2, w3 * (float)3 / 13, 50 ), "Upgrade" ) )
                    {
                        GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        if( structScript.built )
                        {
                            turretUpgradeBool = !turretUpgradeBool;
                            if( ( turretRangeIndicator != null ) && ( turretUpgradeBool == true ) )
                            {
                                Destroy( turretRangeIndicator.gameObject );
                                turretRangeDisplay = false;
                            }
                        }
                    }
					break;
				case "SolarCollectorPreFab(Clone)":
					health = powerManager.Trees.GetNodeHealth( currentSelection, powerManager.NodesRecord );
					beamsCount = powerManager.Trees.GetNodeAttachedBeams( currentSelection, powerManager.NodesRecord );
					storedEnergy = powerManager.Trees.GetNodeStoredPower( currentSelection, powerManager.NodesRecord );
					powerGeneration = powerManager.Trees.GetNodePowerGeneration( currentSelection, powerManager.NodesRecord );
					
					GUI.Label( new Rect( w2+70 , h1 / 2 - 40, 32, 32 ), new GUIContent( solar ) );
					GUI.Label( new Rect( w3 * (float)1/13, h1 / 2, w4, 50 ), "Energy: " + storedEnergy );
					GUI.Label( new Rect( w3 * (float)3/13, h1 / 2, w4, 50 ), "Health: " + health );
					GUI.Label( new Rect( w3 * (float)5/13, h1 / 2, w4, 50 ), "Beams: "  + beamsCount );
					GUI.Label( new Rect( w3 * (float)7/13, h1 / 2, w4, 50 ), "Power Generation: "  + powerGeneration );
					
					if( GUI.Button( new Rect( w3 * (float)10/13, h1 / 2, w3 * (float)3/13, 50 ), "Salvage" ) )
					{
						GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        structScript.StartCoroutine( structScript.Die( true, generatorCost ) );
					}

                    // upgrade button
                    if( GUI.Button( new Rect( w3 * (float)10 / 13, 2, w3 * (float)3 / 13, 50 ), "Upgrade" ) )
                    {
                        GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        if( structScript.built )
                        {
                            solarCollectorUpgradeBool = !solarCollectorUpgradeBool;
                        }
                    }
					break;
				case "EnergyNodePreFab(Clone)":
					health = powerManager.Trees.GetNodeHealth( currentSelection, powerManager.NodesRecord );
					beamsCount = powerManager.Trees.GetNodeAttachedBeams( currentSelection, powerManager.NodesRecord );
					storedEnergy = powerManager.Trees.GetNodeStoredPower( currentSelection, powerManager.NodesRecord );
					
					GUI.Label( new Rect( w2+70 , h1 / 2 - 40, 32, 32 ), new GUIContent( node ) );
					GUI.Label( new Rect( w3 * (float)1/13, h1 / 2, w4, 50 ), "Energy: " + storedEnergy );
					GUI.Label( new Rect( w3 * (float)4/13, h1 / 2, w4, 50 ), "Health: " + health );
					GUI.Label( new Rect( w3 * (float)7/13, h1 / 2, w4, 50 ), "Beams: "  + beamsCount );
					
					if( GUI.Button( new Rect( w3 * (float)10/13, h1 / 2, w3 * (float)3/13, 50 ), "Salvage" ) )
					{
						GenericStructureScript structScript = currentSelection.GetComponent<GenericStructureScript>();
                        structScript.StartCoroutine( structScript.Die( true, nodeCost ) );
					}
					break;
				default: 
					break;
				}
				GUI.EndGroup();

                float y2 = (float)Screen.height * 5f / 10f;
                
                // Upgrade windows
                GUI.skin = mainSkin;
                GUI.BeginGroup( new Rect( x5, y2, w2, h1 ), "" );
                if( minerUpgradeBool )
                {
                    GUI.Box( new Rect( 0, 0, w2, h1 ), "Choose an upgrade" );
                    if( GUI.Button( new Rect( 10, 25, w2 / 2 - 15, h1 - 35 ), miner ) )
                    {
                        
                    }
                    GUI.Label( new Rect( w2 / 4 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Miner V2" );
                    if( GUI.Button( new Rect( w2 / 2 + 5, 25, w2 / 2 - 15, h1 - 35 ), miner ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 * 3 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Asteroid Bore" );
                }
                else if( solarCollectorUpgradeBool )
                {
                    GUI.Box( new Rect( 0, 0, w2, h1 ), "Choose an upgrade" );
                    if( GUI.Button( new Rect( 10, 25, w2 / 2 - 15, h1 - 35 ), solar ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Collector V2" );
                    if( GUI.Button( new Rect( w2 / 2 + 5, 25, w2 / 2 - 15, h1 - 35 ), solar ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 * 3 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Shield Generator" );
                }
                else if( solarBatteryUpgradeBool )
                {
                    GUI.Box( new Rect( 0, 0, w2, h1 ), "Choose an upgrade" );
                    if( GUI.Button( new Rect( 10, 25, w2 / 2 - 15, h1 - 35 ), storage ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Battery V2" );
                    if( GUI.Button( new Rect( w2 / 2 + 5, 25, w2 / 2 - 15, h1 - 35 ), storage ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 * 3 - 40, h1 - 65, w2 / 2 - 15, 30 ), "EMP Emitter" );
                }
                else if( turretUpgradeBool )
                {
                    GUI.Box( new Rect( 0, 0, w2, h1 ), "Choose an upgrade" );
                    if( GUI.Button( new Rect( 10, 25, w2 / 2 - 15, h1 - 35 ), turret ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Long Range" );
                    if( GUI.Button( new Rect( w2 / 2 + 5, 25, w2 / 2 - 15, h1 - 35 ), turret ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 * 3 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Short Range" );
                }
                else if( repairStationUpgradeBool )
                {
                    GUI.Box( new Rect( 0, 0, w2, h1 ), "Choose an upgrade" );
                    if( GUI.Button( new Rect( 10, 25, w2 / 2 - 15, h1 - 35 ), healer ) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Repair Station V2" );
                    if( GUI.Button( new Rect( w2 / 2 + 5, 25, w2 / 2 - 15, h1 - 35 ), healer) )
                    {

                    }
                    GUI.Label( new Rect( w2 / 4 * 3 - 40, h1 - 65, w2 / 2 - 15, 30 ), "Something Else" );
                }
                GUI.EndGroup();
			}
			
			// border around the minimap
			GUI.BeginGroup( new Rect( x10, y1, w2, h1+1 ), "" );
			
			GUI.Box( new Rect( 0, 0, w2, 10 ), "" );
			GUI.Box( new Rect( 0, 0, 10, h1 ), "" );
			GUI.Box( new Rect( w2 - 10, 0, 10, h1 ), "" );
			GUI.Box( new Rect( 0, h1 - 9, w2, 10 ), "" );
			
			GUI.EndGroup();
		}
	}
	
	void DestroyPickedUp()
	{
		if( miningRangeIndicator != null )
		{
			Destroy( miningRangeIndicator.gameObject );
			miningRangeDisplay = false;
		}
		if( repairRangeIndicator != null )
		{
			Destroy( repairRangeIndicator.gameObject );
			repairRangeDisplay = false;
		}
		if( turretRangeIndicator != null )
		{
			Destroy( turretRangeIndicator.gameObject );
			turretRangeDisplay = false;
		}
		if( energyRangeIndicator != null )
		{
			Destroy( energyRangeIndicator.gameObject );
			energyRangeDisplay = false;
		}
		
		currentlyPickedCol = null;
		currentlyPickedSprite = null;
		Destroy( currentlyPickedUp.gameObject );
		currentlyPickedUp = null;
		placed = true;
	}

	void PlacePickedUp( GameObject[] beams )
	{
		if( miningRangeIndicator != null )
		{
			Destroy( miningRangeIndicator.gameObject );
			miningRangeDisplay = false;
		}
		if( repairRangeIndicator != null )
		{
			Destroy( repairRangeIndicator.gameObject );
			repairRangeDisplay = false;
		}
		if( turretRangeIndicator != null )
		{
			Destroy( turretRangeIndicator.gameObject );
			turretRangeDisplay = false;
		}
		if( energyRangeIndicator != null )
		{
			Destroy( energyRangeIndicator.gameObject );
			energyRangeDisplay = false;
		}
		
		placed = true;
		GenericStructureScript placedScript = currentlyPickedUp.GetComponent<GenericStructureScript>();
		placedScript.placed = true;

		currentlyPickedCol.enabled = true;
		for( int i = 0; i < beamsArr.Length; i++ )
		{
			if( beamsGood[i] )
			{
				beams[i].transform.tag = "Planted";
				beams[i].GetComponent<Collider2D>().enabled = true;
			}
		}
	}
	
	void ClearPickedUp()
	{
		currentlyPickedUp = null;
		currentlyPickedSprite = null;
		currentlyPickedCol = null;
	}

	void PickUpSolar()
	{
		placed = false;
		// instantiates the game object at the cursor's location
		// the parameters here are the preFab which is set in the inspector, the position, and the rotation.  Quaternion.identity just means give it the rotation it has by default
		// in other words, no rotation
		currentlyPickedUp = (GameObject)Instantiate( solarPreFab, mousePosition, Quaternion.identity );
		currentlyPickedCol = currentlyPickedUp.GetComponent<CircleCollider2D>();
		currentlyPickedSprite = currentlyPickedUp.GetComponent<SpriteRenderer>();
	}
	
	void PickUpNode()
	{
		placed = false;
		currentlyPickedUp = (GameObject)Instantiate( nodePreFab, mousePosition, Quaternion.identity );
		currentlyPickedCol = currentlyPickedUp.GetComponent<CircleCollider2D>();
		currentlyPickedSprite = currentlyPickedUp.GetComponent<SpriteRenderer>();
	}
	
	void PickUpMiner()
	{
		placed = false;
		currentlyPickedUp = (GameObject)Instantiate( minerPreFab, mousePosition, Quaternion.identity );
		currentlyPickedCol = currentlyPickedUp.GetComponent<CircleCollider2D>();
		currentlyPickedSprite = currentlyPickedUp.GetComponent<SpriteRenderer>();
	}
	
	void PickUpStorage()
	{
		placed = false;
		currentlyPickedUp = (GameObject)Instantiate( storagePreFab, mousePosition, Quaternion.identity );
		currentlyPickedCol = currentlyPickedUp.GetComponent<CircleCollider2D>();
		currentlyPickedSprite = currentlyPickedUp.GetComponent<SpriteRenderer>();
	}
	
	void PickUpRepair()
	{
		placed = false;
		currentlyPickedUp = (GameObject)Instantiate( healerPreFab, mousePosition, Quaternion.identity );
		currentlyPickedCol = currentlyPickedUp.GetComponent<CircleCollider2D>();
		currentlyPickedSprite = currentlyPickedUp.GetComponent<SpriteRenderer>();
	}
	
	void PickUpTurret()
	{
		placed = false;
		currentlyPickedUp = (GameObject)Instantiate( turretPreFab, mousePosition, Quaternion.identity );
		currentlyPickedCol = currentlyPickedUp.GetComponent<CircleCollider2D>();
		currentlyPickedSprite = currentlyPickedUp.GetComponent<SpriteRenderer>();
	}
	
	void PickUpMissile()
	{
		placed = false;
		currentlyPickedUp = (GameObject)Instantiate( missilePreFab, mousePosition, Quaternion.identity );
		currentlyPickedCol = currentlyPickedUp.GetComponent<CircleCollider2D>();
		currentlyPickedSprite = currentlyPickedUp.GetComponent<SpriteRenderer>();
	}
	
	void DrawBeam( Collider2D[] col, GameObject[] beamsArray )
	{
		DestroyUnplantedBeams();
		int removed = 0;
		for( int i = 0; i < col.Length; i++ )
		{
			bool toRemove = false;
			GenericStructureScript temp1 = col[i].gameObject.GetComponent<GenericStructureScript>();
			if( temp1.attachedBeamCount >= temp1.maxBeamsAllowed )
			{
				toRemove = true;
			}
			
			if( !toRemove )
			{
				Vector3 pos = new Vector3( ( col[i].transform.position.x + mousePosition.x ) / 2, ( col[i].transform.position.y + mousePosition.y ) / 2, 10f );
				Vector3 temp = col[i].transform.position - mousePosition;
				beamsArray[i] = (GameObject)Instantiate( energybeamPreFab, pos, Quaternion.identity );
				
				beamsArray[i].transform.rotation = Quaternion.FromToRotation( Vector3.up, temp );
				beamsArray[i].transform.localScale = new Vector3( beamsArray[i].transform.localScale.x, temp.magnitude, beamsArray[i].transform.localScale.z );
				
				RaycastHit2D hit = Physics2D.Linecast( mousePosition, col[i].transform.position, NotBeam );
	//			print( hit );
				
				if( hit.collider == col[i].GetComponent<Collider2D>() ) // this statement is drawing a line from the structure you just placed to the structure you just made a beam to
				{														// if the you follow the line and the first collider you hit is the target's, then it means there's nothing else
					beamsGood[i] = true;								// in between you. Otherwise it'll return the collider of a different structure or asteroid meaning the beam is bad
	//				Debug.Log( "good beam" );
					beamsArray[i].transform.tag = "Unplanted";
				}
				else
				{
					beamsGood[i] = false;
	//				print( "bad beam" );
					beamsArray[i].transform.tag = "Bad Beam";
				}
			}
			else
			{
				beamsArray[i] = null;
				removed++;
			}
		}
		
		GameObject[] tempCol = new GameObject[beamsArray.Length - removed];
		int iterator = 0;
		for( int a = 0; a < beamsArray.Length; a++ )
		{
			if( beamsArray[a] != null )
			{
				tempCol[iterator] = beamsArray[a];
				iterator++;
			}
		}
		beamsArray = new GameObject[tempCol.Length];
		for( int b = 0; b < tempCol.Length; b++ )
		{
			beamsArray[b] = tempCol[b];
		}
		
		DestroyBadBeams();
	}
	
	void DestroyUnplantedBeams()
	{
		GameObject[] oldBeams = GameObject.FindGameObjectsWithTag( "Unplanted" );
		for( int j = 0; j < oldBeams.Length; j++ )
		{
			Destroy( oldBeams[j] );
		}
	}
	
	void DestroyBadBeams()
	{
		GameObject[] badBeams = GameObject.FindGameObjectsWithTag( "Bad Beam" );
		for( int j = 0; j < badBeams.Length; j++ )
		{
			Destroy( badBeams[j] );
		}
	}
}

public static class VectorExtensions
{
    public static Vector3 FromMainScreenToWorld( this Vector3 v )
    {
        return Camera.main.ScreenToWorldPoint( v );
    }
}