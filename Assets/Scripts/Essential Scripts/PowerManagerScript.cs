﻿using UnityEngine;
using System.Collections;
using System;

public class PowerManagerScript : MonoBehaviour {

	public GUISkin powerSkin;
	public GUISkin mineralSkin;
	public GUISkin mainSkin;
    public GUISkin warningSkin;
	
	public LayerMask structures;
	public LayerMask beamsMask;
	
	public Material beamNormal;
	public Material beamPulsed;
	
	public Sprite Battery1;
	public Sprite Battery2;
	public Sprite Battery3;
	public Sprite Battery4;
	public Sprite Battery5;
	
	public bool BigBallDebug = false;
	
	public int maxPower;
	public int structureBuildRate;
	public int mineralGoal;
	public int difficulty;
	public int currentMinerals;
	public int mineralsForScore;
	public int enemyCount;

    public int SolarBatteryPowerRequest;
    public int MinerPowerRequest;
    public int RepairStationPowerRequest;
    public int TurretBasicPowerRequest;
	
	public int currentPower;
	public int powerRate;
	
	public Texture powerBar;
	public Texture backgroundBar;
	public Texture mineralBar;
	
	public GameObject BigRedBall;
	public GameObject BigBlueBall;
	
	public GameObject EnemyBasicPreFab;
	public GameObject EnemyExploderPreFab;
	public GameObject EnemySwarmerPreFab;
	public GameObject EnemyTankPreFab;
	public GameObject EnemyCloakerPreFab;
    public GameObject WurmMediumPreFab;
	
	public AudioClip clickingNoise;
	public AudioClip miningNoise;
	public AudioClip enemySpawn;
	public AudioClip bossSpawn;
	public AudioClip youLose;
	
	private GameObject InitialSolarCollector;
	
	private AudioSource audioManager;

    private int colorIntervals;

	private float powerPercent;
	private float mineralPercent;
	private float currPowFloat;
	private float x1;
	private float w2;
	private float h1;
	private float h2;
	private float y1;
	
	private float percentBasic;
	private float percentExploder;
	private float percentSwarmer;
	private float percentCloaker;
	private float percentTank;
//	private float percentMedic;
//	private float percentForceField;
//	private float percentMotherShip;
	
	private int[] mineralTriggers;
	private bool[] triggersTriggered;
	
	private GameGUIScript gameGUI;
	private bool gameGUIFound = false;
    private bool summoningEnemies = false;
	
	
	// for the top GUI
//	private float xx1;
	private float ww1;
	private float yy1;
	private float hh1;
	
	private bool gameGo = false;
	
	public PowerTreeNode[] Miners;
	public PowerTreeNode[] Generators;
	public PowerTreeNode[] Batteries;
	public PowerTreeNode[] RepairStations;
	public PowerTreeNode[] MissileLaunchers;
	public PowerTreeNode[] Turrets;
	public PowerTreeNode[] EnergyNodes;
	
	public PowerTree Trees;
	public EnergyBeamsRecord BeamsRecord = new EnergyBeamsRecord();
	public PowerNodeRecord NodesRecord = new PowerNodeRecord();
	
	void Awake()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag( "PowerManager" );
        audioManager = GetComponent<AudioSource>();
		for( int i = 0; i < temp.Length; i++ )
		{
			if( temp[i] != gameObject )
			{
				Destroy( gameObject );
			}
		}
        warningSkin.label.normal.textColor = new Color( 1.0f, 0.2f, 0.2f );
        colorIntervals = 0;
	}
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad( gameObject );
		
		Miners = new PowerTreeNode[0];
		Generators = new PowerTreeNode[0];
		Batteries = new PowerTreeNode[0];
		RepairStations = new PowerTreeNode[0];
		MissileLaunchers = new PowerTreeNode[0];
		Turrets = new PowerTreeNode[0];
		EnergyNodes = new PowerTreeNode[0];
		
		triggersTriggered = new bool[0];
		
		maxPower = 50;
		currentPower = 0;
		currentMinerals = 800;
		mineralsForScore = 0;
		mineralGoal = 30000;
		
		structureBuildRate = 1;
	}
	
	// Update is called once per frame
	void Update () {
        // set some variables for the GUI
        x1 = Screen.width * (float)( 1f / 50f );
        w2 = Screen.width * (float)( 10f / 50f );
        h1 = Screen.height * (float)( 10f / 50f );
        h2 = Screen.height * (float)( 20f / 50f );
        y1 = Screen.height * (float)( 15f / 19f );

        //		xx1 = 0f;
        ww1 = Screen.width * (float)1f/5f;
        yy1 = 0f;
        hh1 = Screen.height * (float)3f/19f;

		if( Input.GetMouseButtonDown( 0 ) )
		{
			audioManager.PlayOneShot( clickingNoise, 0.7f );
		}
        if( Input.GetKeyDown( KeyCode.I ) )
        {
            SummonEnemies();
        }
		if( Application.loadedLevel == 2 )
		{
			if( InitialSolarCollector == null )
			{
				InitialSolarCollector = GameObject.FindGameObjectWithTag( "FirstCollector" );
				if( InitialSolarCollector != null )
				{
					StartFirstTree( InitialSolarCollector );
				}
			}
		}
		maxPower = ( Generators.Length * 50 ) + ( Batteries.Length * 100 );
		
		if( mineralsForScore >= mineralGoal )
		{
			Application.LoadLevel( "ScoreScreen" );
			gameGo = false;
		}
		
		if( !gameGUIFound )
		{
			if( GameObject.Find( "Main Camera" ).GetComponent<GameGUIScript>() != null )
			{
				gameGUI = GameObject.Find( "Main Camera" ).GetComponent<GameGUIScript>();
			}
		}
		
		if( mineralTriggers != null )
		{
			for( int i = 0; i < mineralTriggers.Length; i++ )
			{
				if( mineralsForScore >= mineralTriggers[i] )
				{
					if( triggersTriggered[i] == false )
					{
						enemyCount += 5;
						SummonEnemies();
						triggersTriggered[i] = true;
						break;
					}
				}
			}
		}
		
		if( gameGo )
		{
			if( NodesRecord.nodesInRecord == 0 )
			{
				gameGo = false;
				audioManager.PlayOneShot( youLose );
				Application.LoadLevel( "LoseScreen" );
			}
		}
	}
	
	IEnumerator PulseFunction()
	{
		float delay = 0.7f;
		float beamLife = 0.2f;
		
		while( true )
		{
			/*--------Generators--------*/
			/*******GENERATE POWER*******/
			for( int t = 0; t < Generators.Length; t++ )
			{
				Generators[t].shouldFunction = Generators[t].CheckIfInAnyTree();
			}
			if( Generators.Length > 0 )
			{
				for( int genCount = 0; genCount < Generators.Length; genCount++ )
				{
					if( Generators[genCount].built )
					{
						if( ( Generators[genCount].shouldFunction ) && ( Generators[genCount].storedPower < Generators[genCount].maxStoredPower ) )
						{
							Generators[genCount].storedPower += Generators[genCount].powerGenerating;
							if( Generators[genCount].storedPower > Generators[genCount].maxStoredPower )
							{
								Generators[genCount].storedPower = Generators[genCount].maxStoredPower;
							}
						}
					}
					else
					{
						RequestPower( Generators[genCount], structureBuildRate );
						if( Generators[genCount].storedPower >= structureBuildRate )
						{
							SolarCollectorScript solarCollectorScript = Generators[genCount].nodeObject.GetComponent<SolarCollectorScript>();
							solarCollectorScript.currentPower += structureBuildRate;
							Generators[genCount].storedPower -= structureBuildRate;
						}
					}
				}
			}
			
			/*--------Batteries--------*/
			for( int t = 0; t < Batteries.Length; t++ )
			{
				Batteries[t].shouldFunction = Batteries[t].CheckIfInAnyTree();
			}
			for( int battCount = 0; battCount < Batteries.Length; battCount++ )
			{
				if( Batteries[battCount].built )
				{
					if( ( Batteries[battCount].shouldFunction ) && ( Batteries[battCount].storedPower < Batteries[battCount].maxStoredPower ) )
					{
						RequestPower( Batteries[battCount], SolarBatteryPowerRequest );
						SpriteRenderer battSprite = Batteries[battCount].nodeObject.GetComponent<SpriteRenderer>();
						if( Batteries[battCount].storedPower > 81 )
						{
							battSprite.sprite = Battery5;
						}
						else if( Batteries[battCount].storedPower > 61 )
						{
							battSprite.sprite = Battery4;
						}
						else if( Batteries[battCount].storedPower > 41 )
						{
							battSprite.sprite = Battery3;
						}
						else if( Batteries[battCount].storedPower > 21 )
						{
							battSprite.sprite = Battery2;
						}
						else
						{
							battSprite.sprite = Battery1;
						}
					}
				}
				else
				{
					RequestPower( Batteries[battCount], structureBuildRate );
					if( Batteries[battCount].storedPower >= structureBuildRate )
					{
						EnergyStorageScript batteryScript = Batteries[battCount].nodeObject.GetComponent<EnergyStorageScript>();
						batteryScript.currentPower += structureBuildRate;
						Batteries[battCount].storedPower -= structureBuildRate;
					}
				}
			}
			
			yield return new WaitForSeconds( delay/2 - beamLife/2 );
			
			/*--------Energy Nodes--------*/
			for( int t = 0; t < EnergyNodes.Length; t++ )
			{
				EnergyNodes[t].shouldFunction = EnergyNodes[t].CheckIfInAnyTree();
			}
			if( EnergyNodes.Length > 0 )
			{
				for( int nodeCount = 0; nodeCount < EnergyNodes.Length; nodeCount++ )
				{
					if( EnergyNodes[nodeCount].built )
					{
						// energy nodes don't do anything
					}
					else
					{
						RequestPower( EnergyNodes[nodeCount], structureBuildRate );
						if( EnergyNodes[nodeCount].storedPower >= structureBuildRate )
						{
							EnergyNodeScript energyNodesScript = EnergyNodes[nodeCount].nodeObject.GetComponent<EnergyNodeScript>();
							energyNodesScript.currentPower += structureBuildRate;
							EnergyNodes[nodeCount].storedPower -= structureBuildRate;
						}
					}
				}
			}
			
			/*--------Turrets--------*/ //TODO move turrets up to top priority in this set
			for( int t = 0; t < Turrets.Length; t++ )
			{
				Turrets[t].shouldFunction = Turrets[t].CheckIfInAnyTree();
			}
			if( Turrets.Length > 0 )
			{
				for( int turretCount = 0; turretCount < Turrets.Length; turretCount++ )
				{
					if( Turrets[turretCount].built )
					{
						if( Turrets[turretCount].shouldFunction )
						{
							if( Turrets[turretCount].storedPower < Turrets[turretCount].maxStoredPower )
							{
								RequestPower( Turrets[turretCount], TurretBasicPowerRequest );
							}
						}
					}
					else
					{
						RequestPower( Turrets[turretCount], structureBuildRate );
						if( Turrets[turretCount].storedPower >= structureBuildRate )
						{
							Turret_Basic turretScript = Turrets[turretCount].nodeObject.GetComponent<Turret_Basic>();
							turretScript.currentPower += structureBuildRate;
							Turrets[turretCount].storedPower -= structureBuildRate;
						}
					}
				}
			}
			
			/*--------Repair Stations--------*/
			for( int t = 0; t < RepairStations.Length; t++ )
			{
				RepairStations[t].shouldFunction = RepairStations[t].CheckIfInAnyTree();
			}
			if( RepairStations.Length > 0 )
			{
				for( int stationCount = 0; stationCount < RepairStations.Length; stationCount++ )
				{
					if( RepairStations[stationCount].built )
					{
						if( RepairStations[stationCount].shouldFunction )
						{
							if( RepairStations[stationCount].storedPower < RepairStations[stationCount].maxStoredPower )
							{
								RequestPower( RepairStations[stationCount], RepairStationPowerRequest );
							}
						}
					}
					else
					{
						RequestPower( RepairStations[stationCount], structureBuildRate );
						if( RepairStations[stationCount].storedPower >= structureBuildRate )
                        {
                            Turret_Healer stationScript = RepairStations[stationCount].nodeObject.GetComponent<Turret_Healer>();
                            stationScript.currentPower += structureBuildRate;
                            RepairStations[stationCount].storedPower -= structureBuildRate;
                        }
                    }
                }
            }
			
			/*--------Miners--------*/
			for( int t = 0; t < Miners.Length; t++ )
			{
				Miners[t].shouldFunction = Miners[t].CheckIfInAnyTree();
				if( Miners[t].shouldFunction )
				{
					MinerScript temp = Miners[t].nodeObject.GetComponent<MinerScript>();
					Miners[t].shouldFunction = temp.AbleToMine();
				}
            }
            bool beamSound = false;
			if( Miners.Length > 0 )
			{
				for( int minerCount = 0; minerCount < Miners.Length; minerCount++ )
				{
					if( Miners[minerCount].built )
					{
						if( Miners[minerCount].shouldFunction )
						{
							RequestPower( Miners[minerCount], MinerPowerRequest );
							SpriteRenderer mineSprite = Miners[minerCount].nodeObject.GetComponent<SpriteRenderer>();
							mineSprite.color = new Color( 1f, 1f, 1f );
							if( Miners[minerCount].storedPower >= Miners[minerCount].powerConsuming )
							{
								MinerScript minerScript = Miners[minerCount].nodeObject.GetComponent<MinerScript>();
								minerScript.StructureFunction();
								Miners[minerCount].storedPower -= Miners[minerCount].powerConsuming;
								minerScript.DrawBeams();
								beamSound = true;
							}
						}
						else
						{
							SpriteRenderer mineSprite = Miners[minerCount].nodeObject.GetComponent<SpriteRenderer>();
							mineSprite.color = new Color( 1f, 0.2f, 0.2f );
						}
					}
					else
					{
						RequestPower( Miners[minerCount], structureBuildRate );
						if( Miners[minerCount].storedPower >= structureBuildRate )
						{
							MinerScript minerScript = Miners[minerCount].nodeObject.GetComponent<MinerScript>();
							minerScript.currentPower += structureBuildRate;
							Miners[minerCount].storedPower -= structureBuildRate;
						}
					}
				}
				if( beamSound )
				{
				float volume = (float)1 / ( gameGUI.cameraZoom * 3 );
					audioManager.PlayOneShot( miningNoise, volume );
				}
			}
			
            yield return new WaitForSeconds( delay/2 +  beamLife/2 );
			
			/*--------Miners Continued--------*/
			if( Miners.Length > 0 )
			{
				for( int minerCount = 0; minerCount < Miners.Length; minerCount++ )
				{
					MinerScript minerScript = Miners[minerCount].nodeObject.GetComponent<MinerScript>();
					if( minerScript.built == true )
					{
						minerScript.DestroyBeams();
					}
				}
			}
			
            ///*--------Missile Launchers--------*/
            //for( int t = 0; t < MissileLaunchers.Length; t++ )
            //{
            //    MissileLaunchers[t].shouldFunction = MissileLaunchers[t].CheckIfInAnyTree();
            //}
            //if( MissileLaunchers.Length > 0 )
            //{
            //    for( int launcherCount = 0; launcherCount < MissileLaunchers.Length; launcherCount++ )
            //    {
            //        Turret_Missile launcherScript = MissileLaunchers[launcherCount].nodeObject.GetComponent<Turret_Missile>();
            //        if( MissileLaunchers[launcherCount].shouldFunction )
            //        {
            //            launcherScript.StructureFunction();
            //        }
            //    }
            //}
			
			yield return new WaitForSeconds( beamLife );
		}
	}
	
	public int GetTurretPower( GameObject interestedTurret )
	{
		PowerTreeNode temp = Trees.FindNode( interestedTurret, NodesRecord );
		
		if( temp != null )
		{
			return temp.storedPower;
		}
		else
		{
			return 0;
		}
	}
	
	public void ConsumeTurretPower( GameObject attackingTurret, int amount )
	{
		PowerTreeNode temp = Trees.FindNode( attackingTurret, NodesRecord );
		temp.storedPower -= amount;
	}
	
	public int GetRepairStationPower( GameObject interestedStation )
	{
		PowerTreeNode temp = Trees.FindNode( interestedStation, NodesRecord );
		
		if( temp != null )
		{
			return temp.storedPower;
		}
		else
		{
			return 0;
		}
	}
	
	public void ConsumeRepairStationPower( GameObject healingStation, int amount )
	{
		PowerTreeNode temp = Trees.FindNode( healingStation, NodesRecord );
		temp.storedPower -= amount;
	}
	
	public void SetPowerGeneration( GameObject collector, int amount )
	{
		PowerTreeNode generator = Trees.FindNode( collector, NodesRecord );
		generator.powerGenerating = amount;
	}
	
	public void UpdatePowerNode( GameObject nodeObjectToUpdate, bool doneBuilding, int maxPower, int powerGeneration, int powerConsumption )
	{
		PowerTreeNode temp = Trees.FindNode( nodeObjectToUpdate, NodesRecord );
		
		temp.built = doneBuilding;
		temp.maxStoredPower = maxPower;
		temp.powerConsuming = powerConsumption;
		temp.powerGenerating = powerGeneration;
		
		switch( nodeObjectToUpdate.name )
		{
		case "MinerPreFab(Clone)":
			AddMinerToList( nodeObjectToUpdate );
			
			break;
		case "EnergyStoragePreFab(Clone)":
			AddBatteryToList( nodeObjectToUpdate );
			
			break;
			
		case "EnergyNodePreFab(Clone)":
			AddEnergyNodeToList( nodeObjectToUpdate );
			
			break;
		case "MissilePreFab(Clone)":
			AddLauncherToList( nodeObjectToUpdate );
			
			break;
		case "RepairStationPreFab(Clone)":
			AddRepairStationToList( nodeObjectToUpdate );
			
			break;
		case "TurretPreFab(Clone)":
			AddTurretToList( nodeObjectToUpdate );
			
			break;
		case "SolarCollectorPreFab(Clone)":
			AddGeneratorToList( nodeObjectToUpdate );
			
			break;
		default: 
			break;
		}
	}
	
	public void SetMaxStoredPower( GameObject structure, int max )
	{
		PowerTreeNode battery = Trees.FindNode( structure, NodesRecord );
		battery.maxStoredPower = max;
	}
	
	PowerTreeNode FindGenerator( PowerTreeNode gen )
	{
		PowerTreeNode result = null;
		
		for( int i = 0; i < Generators.Length; i++ )
		{
			if( Generators[i] == gen )
			{
				result = gen;
				break;
			}
		}
		return result;
	}
	
	PowerTreeNode FindBattery( PowerTreeNode batt )
	{
		PowerTreeNode result = null;
		
		for( int i = 0; i < Batteries.Length; i++ )
		{
			if( Batteries[i] == batt )
			{
				result = batt;
				break;
			}
		}
		return result;
	}
	
	public void TotalPower()
	{
		currentPower = 0;
		for( int i = 0; i < Generators.Length; i++ )
		{
			currentPower += Generators[i].storedPower;
		}
		for( int j = 0; j < Batteries.Length; j++ )
		{
			currentPower += Batteries[j].storedPower;
		}
	}
	
	public void RequestPower( PowerTreeNode requestee, int amount )
	{
		int[] availableTrees = requestee.GetTrees();
		MeshRenderer[] beamMeshes = null;
		
		for( int bark = 0; bark < availableTrees.Length; bark++ )
		{
			int tree = availableTrees[bark];
			//if the first source has enough power to share, find the path to take upwards and store the beams and then break out of this loops since we have power and don't need to find another source
			if( ( Trees.roots[tree] != null ) && ( requestee.inTree[tree] != 2 ) )
			{
				if( Trees.roots[tree].storedPower >= amount )
				{
					if( requestee.storedPower <= ( requestee.maxStoredPower - amount ) )
					{
						if( ( Trees.roots[tree].nodeObject.name == "EnergyStoragePreFab(Clone)" ) && ( requestee.nodeObject.name != "EnergyStoragePreFab(Clone)" ) )
						{
							PowerTreeNode tempBatt = FindBattery( Trees.roots[tree] );
							if( tempBatt != null )
							{
								beamMeshes = requestee.ClimbTree( tree, BeamsRecord );
								StartCoroutine( BeamFlicker( beamMeshes ) );
								tempBatt.storedPower -= amount;
								requestee.storedPower += amount;
								break;
							}
						}
						else if( Trees.roots[tree].nodeObject.name == "SolarCollectorPreFab(Clone)" )
						{
							PowerTreeNode tempGen = FindGenerator( Trees.roots[tree] );
							if( tempGen != null )
							{
								beamMeshes = requestee.ClimbTree( tree, BeamsRecord );
								StartCoroutine( BeamFlicker( beamMeshes ) );
								tempGen.storedPower -= amount;
								requestee.storedPower += amount;
								if( ( requestee.nodeObject.name != "EnergyStoragePreFab(Clone)" ) || ( requestee.built == false ) )
								{
									break;
								}
							}
						}
					}
					else // This is for when a structure needs to draw less than normal amount of power because they just need topping off
					{
						int newAmount = requestee.maxStoredPower - requestee.storedPower;
						if( ( Trees.roots[tree].nodeObject.name == "EnergyStoragePreFab(Clone)" ) && ( requestee.nodeObject.name != "EnergyStoragePreFab(Clone)" ) )
						{
							PowerTreeNode tempBatt = FindBattery( Trees.roots[tree] );
							if( tempBatt != null )
							{
								beamMeshes = requestee.ClimbTree( tree, BeamsRecord );
								StartCoroutine( BeamFlicker( beamMeshes ) );
								tempBatt.storedPower -= newAmount;
								requestee.storedPower += newAmount;
								break;
							}
						}
						else if( Trees.roots[tree].nodeObject.name == "SolarCollectorPreFab(Clone)" )
						{
							PowerTreeNode tempGen = FindGenerator( Trees.roots[tree] );
							if( tempGen != null )
							{
								beamMeshes = requestee.ClimbTree( tree, BeamsRecord );
								StartCoroutine( BeamFlicker( beamMeshes ) );
								tempGen.storedPower -= newAmount;
								requestee.storedPower += newAmount;
								if( ( requestee.nodeObject.name != "EnergyStoragePreFab(Clone)" ) || ( requestee.built == false ) )
								{
									break;
								}
							}
						}
					}
				}
			}
		}
	}
	
	IEnumerator BeamFlicker( MeshRenderer[] meshes )
	{
		float delay = 0.2f;
		
		// make sprites flash
		for( int i = 0; i < meshes.Length; i++ )
		{
			if( meshes[i] != null )
			{
				meshes[i].material = beamPulsed;
			}
		}
		yield return new WaitForSeconds( delay );
		
		for( int i = 0; i < meshes.Length; i++ )
		{
			if( meshes[i] != null )
			{
				meshes[i].material = beamNormal;
			}
		}
	}
	
	public void CleanHouseForMainMenu()
	{
		gameGo = false;
		
		mineralsForScore = 0;
		
		StopAllCoroutines();
		
		Generators = new PowerTreeNode[0];
		Batteries = new PowerTreeNode[0];
		Miners = new PowerTreeNode[0];
		Turrets = new PowerTreeNode[0];
		RepairStations = new PowerTreeNode[0];
		EnergyNodes = new PowerTreeNode[0];
		MissileLaunchers = new PowerTreeNode[0];
		
		GameObject[] temp = GameObject.FindGameObjectsWithTag( "FirstCollector" );
		if( temp != null )
		{
			for( int i = 0; i < temp.Length; i++ )
			{
				Destroy( temp[i].gameObject );
			}
		}
		
		Trees.roots = new PowerTreeNode[0];
		Trees.size = 0;
		
		NodesRecord.powerNodes = new PowerTreeNode[0];
		NodesRecord.nodesInRecord = 0;
		
		BeamsRecord.energyBeams = new EnergyBeam[0];
		BeamsRecord.beamsInRecord = 0;
		
		Application.LoadLevel( "MainMenu" );
	}

    private void WurmSpawn()
    {
        bool shouldWurmSpawn = false;
        int wurmCount = 0;

        int temp = UnityEngine.Random.Range( 0, 10 );
        if( temp % 2 == 0 )
        {
            shouldWurmSpawn = true;
        }

        if( shouldWurmSpawn )
        {
            wurmCount = UnityEngine.Random.Range( 1, 4 );
        }

        for( int i = 0; i < wurmCount; i++ )
        {
            int posOrNegX = UnityEngine.Random.Range( 1, 5000 );
            if( posOrNegX % 2 == 1 )
            {
                posOrNegX = -1;
            }
            else
            {
                posOrNegX = 1;
            }
            int posOrNegY = UnityEngine.Random.Range( 1, 5000 );
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
            int minimumDistance = 6;
            int maximumDistance = 20;
            while( tooClose )
            {
                randomPosX = (float)UnityEngine.Random.Range( 0f, 28f );
                randomPosY = (float)UnityEngine.Random.Range( 0f, 28f );

                float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
                if( ( distance >= minimumDistance ) && ( distance <= maximumDistance ) )
                {
                    tooClose = false;
                }
            }
            Instantiate( WurmMediumPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, 0 ), Quaternion.identity );
        }
    }
	
	private void SummonEnemies()
	{
        summoningEnemies = true;
		audioManager.PlayOneShot( enemySpawn, 0.9f );
		
		int minimumDistance;
		int maximumDistance;
		
		int basicCount = (int)( enemyCount * percentBasic );
		int exploderCount = (int)( enemyCount * percentExploder );
		int swarmerCount = (int)( enemyCount * percentSwarmer ) * 2;
		int tankCount = (int)( enemyCount * percentTank );
		int cloakerCount = (int)( enemyCount * percentCloaker );
		
		// SPAWN BASIC ENEMIES
		for( int i = 0; i < basicCount; i++ )
		{
			int posOrNegX = UnityEngine.Random.Range( 1, 5000 );
			if( posOrNegX % 2 == 1 )
			{
				posOrNegX = -1;
			}
			else
			{
				posOrNegX = 1;
			}
			int posOrNegY = UnityEngine.Random.Range( 1, 5000 );
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
			minimumDistance = 23;
			maximumDistance = 26;
			while( tooClose )
			{
				randomPosX = (float)UnityEngine.Random.Range( 0f, 22f );
				randomPosY = (float)UnityEngine.Random.Range( 0f, 22f );
				
				float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
				if( ( distance >= minimumDistance ) && ( distance <= maximumDistance ) )
				{
					tooClose = false;	
				}
			}
			Instantiate( EnemyBasicPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, 0 ), Quaternion.identity );
		}
		
		// SPAWN EXPLODER ENEMIES
		for( int i = 0; i < exploderCount; i++ )
		{
			int posOrNegX = UnityEngine.Random.Range( 1, 5000 );
			if( posOrNegX % 2 == 1 )
			{
				posOrNegX = -1;
			}
			else
			{
				posOrNegX = 1;
			}
			int posOrNegY = UnityEngine.Random.Range( 1, 5000 );
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
			minimumDistance = 17;
			maximumDistance = 20;
			while( tooClose )
			{
				randomPosX = (float)UnityEngine.Random.Range( 0f, 20f );
				randomPosY = (float)UnityEngine.Random.Range( 0f, 20f );
				
				float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
				if( ( distance >= minimumDistance ) && ( distance <= maximumDistance ) )
                {
                    tooClose = false;	
                }
            }
            Instantiate( EnemyExploderPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, 0 ), Quaternion.identity );
        }
        
        // SPAWN SWARMER ENEMIES
		for( int i = 0; i < swarmerCount; i++ )
		{
			int posOrNegX = UnityEngine.Random.Range( 1, 5000 );
			if( posOrNegX % 2 == 1 )
			{
				posOrNegX = -1;
			}
			else
			{
				posOrNegX = 1;
			}
			int posOrNegY = UnityEngine.Random.Range( 1, 5000 );
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
			minimumDistance = 26;
			maximumDistance = 29;
			while( tooClose )
			{
				randomPosX = (float)UnityEngine.Random.Range( 0f, 29f );
				randomPosY = (float)UnityEngine.Random.Range( 0f, 29f );
				
				float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
				if( ( distance >= minimumDistance ) && ( distance <= maximumDistance ) )
                {
                    tooClose = false;	
                }
            }
            Instantiate( EnemySwarmerPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, 0 ), Quaternion.identity );
        }
        
        // SPAWN TANK ENEMIES
		for( int i = 0; i < tankCount; i++ )
		{
			int posOrNegX = UnityEngine.Random.Range( 1, 5000 );
			if( posOrNegX % 2 == 1 )
			{
				posOrNegX = -1;
			}
			else
			{
				posOrNegX = 1;
			}
			int posOrNegY = UnityEngine.Random.Range( 1, 5000 );
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
			minimumDistance = 12;
			maximumDistance = 15;
			while( tooClose )
			{
				randomPosX = (float)UnityEngine.Random.Range( 0f, 15f );
				randomPosY = (float)UnityEngine.Random.Range( 0f, 15f );
				
				float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
                if( distance >= minimumDistance )
                {
                    tooClose = false;	
                }
            }
            Instantiate( EnemyTankPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, 0 ), Quaternion.identity );
        }
        
        // SPAWN CLOAKER ENEMIES
		for( int i = 0; i < cloakerCount; i++ )
		{
			int posOrNegX = UnityEngine.Random.Range( 1, 5000 );
			if( posOrNegX % 2 == 1 )
			{
				posOrNegX = -1;
			}
			else
			{
				posOrNegX = 1;
			}
			int posOrNegY = UnityEngine.Random.Range( 1, 5000 );
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
			minimumDistance = 17;
			maximumDistance = 20;
			while( tooClose )
			{
				randomPosX = (float)UnityEngine.Random.Range( 0f, 20f );
				randomPosY = (float)UnityEngine.Random.Range( 0f, 20f );
				
				float distance = Vector3.Distance( new Vector3( randomPosX, randomPosY, 0 ), new Vector3( 0f, 0f, 0f ) );
                if( distance >= minimumDistance )
                {
                    tooClose = false;	
                }
            }
            Instantiate( EnemyCloakerPreFab, new Vector3( randomPosX * posOrNegX, randomPosY * posOrNegY, 0 ), Quaternion.identity );
        }
    }
    
    public void SetDifficulty( int difficulty )
	{
		int numAttacks = difficulty + 2;
		int variance = 0;
		if( mineralGoal <= 30000 )
		{
			variance = mineralGoal / 8;
		}
		else if( mineralGoal <= 80000 )
		{
			variance = mineralGoal / 20;
		}
		else if( mineralGoal <= 180000 )
		{
			variance = mineralGoal / 50;
		}
		else if( mineralGoal <= 400000 )
		{
			variance = mineralGoal / 100;
		}
		else if( mineralGoal <= 1000000 )
		{
			variance = mineralGoal / 225;
		}
//		print( "Variance: " + variance );
		mineralTriggers = new int[numAttacks];
		triggersTriggered = new bool[numAttacks];
		
		enemyCount = 10 + ( 3 * difficulty );

		switch( difficulty )
		{
			case -1:
				percentBasic = 0.5f;
				percentExploder = 0.0f;
				percentSwarmer = 0.0f;
				percentTank = 0.0f;
				percentCloaker = 0.0f;
				break;
			case 0:
				percentBasic = 0.5f;
				percentExploder = 0.1f;
				percentSwarmer = 0.1f;
				percentTank = 0.1f;
	            percentCloaker = 0.0f;
            	break;
			case 1:
				percentBasic = 0.0f;
				percentExploder = 0.0f;
				percentSwarmer = 1.3f;
				percentTank = 0.0f;
	            percentCloaker = 0.0f;
            	break;
			case 2:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.2f;
				percentTank = 0.2f;
	            percentCloaker = 0.2f;
            	break;
			case 3:
				percentBasic = 0.1f;
				percentExploder = 0.1f;
				percentSwarmer = 0.2f;
				percentTank = 0.3f;
	            percentCloaker = 0.3f;
            	break;
			case 4:
				percentBasic = 0.1f;
				percentExploder = 0.1f;
				percentSwarmer = 0.0f;
				percentTank = 0.4f;
	            percentCloaker = 0.4f;
            	break;
			case 5:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.0f;
				percentTank = 0.3f;
	            percentCloaker = 0.3f;
            	break;
			case 6:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.0f;
				percentTank = 0.3f;
				percentCloaker = 0.3f;
	            break;
	        case 7:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.0f;
				percentTank = 0.3f;
				percentCloaker = 0.3f;
	            break;
	        case 8:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.0f;
				percentTank = 0.3f;
				percentCloaker = 0.3f;
	            break;
	        case 9:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.0f;
				percentTank = 0.3f;
				percentCloaker = 0.3f;
	            break;
	        case 10:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.0f;
				percentTank = 0.3f;
				percentCloaker = 0.3f;
	            break;
	        default:
				percentBasic = 0.2f;
				percentExploder = 0.2f;
				percentSwarmer = 0.0f;
				percentTank = 0.3f;
				percentCloaker = 0.3f;
            break;
        }
        
        int minMultiplier = mineralGoal / ( numAttacks + 1 );
		
		for( int i = 1; i <= numAttacks; i++ )
		{
			mineralTriggers[i-1] = ( minMultiplier * i ) + UnityEngine.Random.Range( -variance, variance );
//			print( mineralTriggers[i-1] );
			triggersTriggered[i-1] = false;
		}
	}
	
	void OnLevelWasLoaded( int level )
	{
		if( level == 0 )
		{
			gameGo = false;
		}
		if( level == 2 )
		{
			StartCoroutine( PulseFunction() );	
			gameGo = true;
            WurmSpawn();
		}
	}
	
	void OnGUI()
	{
		if( gameGo )
		{
			if( !gameGUI.paused )
			{
				TotalPower();
				// power bar GUI
				if( maxPower > 0 )
				{
					powerPercent = (float)currentPower / maxPower;
				}
				else
				{
					powerPercent = 0;
				}
				powerPercent = Mathf.Round( powerPercent * 10000 ) / 10000;
				currPowFloat = currentPower;
				currPowFloat = Mathf.Round( currPowFloat * 100 ) / 100;
				
				mineralPercent = (float)mineralsForScore / mineralGoal;
				
				GUI.BeginGroup( new Rect( x1, y1, w2, h1 ), "" );
				powerSkin.label.fontSize = 25;
				GUI.skin = mainSkin;
				GUI.Box( new Rect( 0, 0, w2, h1 ), "" );
				GUI.skin = powerSkin;
				GUI.Label( new Rect( w2 / 2 -35 , h1 / 8 - 16, w2 - ( w2 / 3 - 60 ), 30 ), "Power" );
				powerSkin.label.fontSize = 20;
				
				GUI.Label( new Rect( w2/6+10, h1 / 3, 100, 30 ), "" + currPowFloat );
				GUI.Label( new Rect( w2 / 6 + 60, h1 / 3, 100, 30 ), "(" + powerPercent * 100 + "%)" );
				GUI.DrawTexture( new Rect( w2/6, h1 / 3 - 10, 2*w2 / 3, 5 ), backgroundBar );
				GUI.DrawTexture( new Rect( w2/6, h1 / 3 - 10, powerPercent * ( 2* w2 / 3 ) , 5), powerBar);
				
				//***************************************************************************************//
				//************** MINERALS ***************************************************************//
				
				GUI.skin = mineralSkin;
				mineralSkin.label.fontSize = 25;
				GUI.Label( new Rect( w2 / 2 - 50, h2 / 3 -25, w2 - ( w2 / 3 - 60 ), 30 ), "Minerals " );
				GUI.Label( new Rect( w2 / 2 - 25, h2 / 3 +5, 100, 30 ), "" + currentMinerals );
				
				GUI.EndGroup();
				
				
				
				//**** Top GUI ****//
				GUI.BeginGroup( new Rect( x1, yy1, ww1, hh1 ), "" );
				GUI.skin = mainSkin;
				GUI.Box( new Rect( 0, 0, ww1, hh1 ), "" );
				
				GUI.DrawTexture( new Rect( ww1/6, hh1 / 2 - 10, 2*w2 / 3, 5 ), backgroundBar );
				GUI.DrawTexture( new Rect( ww1/6, hh1 / 2 - 10,  mineralPercent * ( 2* w2 / 3 ) , 5), mineralBar);
				
				GUI.skin = mineralSkin;
				GUI.Label( new Rect( ww1/20, hh1 / 2 - 45, 300, 30 ), "Minerals Gathered" );
				GUI.Label( new Rect( ww1/20 + 220, hh1 / 2 - 45, 300, 30 ), "" + mineralsForScore );
				GUI.Label( new Rect( ww1/20, hh1 / 2 - 05, 250, 30 ), "Mineral Goal" );
				GUI.Label( new Rect( ww1/20 + 220, hh1 / 2 - 05, 300, 30 ), "" + mineralGoal );
				
				GUI.EndGroup();


                float wx = (float)Screen.width / 2f;
                float wy = (float)Screen.height / 15f * 3f;

                if( summoningEnemies )
                {
                    GUI.skin = warningSkin;

                    switch( colorIntervals )
                    {
                        case 0:
                            warningSkin.label.normal.textColor = new Color( 1f, warningSkin.label.normal.textColor.g + 0.007f, warningSkin.label.normal.textColor.b + 0.007f );
                            break;
                        case 1:
                            warningSkin.label.normal.textColor = new Color( 1f, warningSkin.label.normal.textColor.g - 0.007f, warningSkin.label.normal.textColor.b - 0.007f );
                            break;
                        case 2:
                            warningSkin.label.normal.textColor = new Color( 1f, warningSkin.label.normal.textColor.g + 0.007f, warningSkin.label.normal.textColor.b + 0.007f );
                            break;
                        case 3:
                            warningSkin.label.normal.textColor = new Color( 1f, warningSkin.label.normal.textColor.g - 0.007f, warningSkin.label.normal.textColor.b - 0.007f );
                            break;
                        case 4:
                            warningSkin.label.normal.textColor = new Color( 1f, warningSkin.label.normal.textColor.g + 0.007f, warningSkin.label.normal.textColor.b + 0.007f );
                            break;
                        case 5:
                            warningSkin.label.normal.textColor = new Color( 1f, warningSkin.label.normal.textColor.g - 0.007f, warningSkin.label.normal.textColor.b - 0.007f );
                            break;
                        case 6:
                            summoningEnemies = false;
                            colorIntervals = 0;
                            warningSkin.label.normal.textColor = new Color( 1.0f, 0.2f, 0.2f );
                            break;
                        default:
                            break;
                    }
                    
                    if( warningSkin.label.normal.textColor.g >= 1f )
                    {
                        colorIntervals++;
                    }
                    else if( warningSkin.label.normal.textColor.g <= 0.1f )
                    {
                        colorIntervals++;
                    }

                    GUI.Label( new Rect( wx - 225, wy, 450, 30 ), "WARNING: ENEMIES APPROACHING!!!!" );
                }
			}
		}
	}
	
	public void AddBeamsToRecord( GameObject[] beamsToAdd, bool[] beamsGood )
	{
		for( int i = 0; i < beamsToAdd.Length; i++ )
		{
			if( ( beamsToAdd[i] != null ) && ( beamsGood[i] == true ) )
			{
				float mag   = beamsToAdd[i].transform.localScale.y / 2;
				float theta = beamsToAdd[i].transform.eulerAngles.z - 90;
				
				// side2 is the position of the structure that the beam is coming from
				// side1 is the position of the structure that the beam is going to i.e. side1 = solar collector; side2 = energy node
				Vector3 side1 = new Vector3(  mag * Mathf.Cos(  theta * Mathf.PI / 180 ) + beamsToAdd[i].transform.position.x,  mag * ( Mathf.Sin( theta * Mathf.PI / 180 ) ) + beamsToAdd[i].transform.position.y, -2 );
				Vector3 side2 = new Vector3( -mag * Mathf.Cos(  theta * Mathf.PI / 180 ) + beamsToAdd[i].transform.position.x, -mag * ( Mathf.Sin( theta * Mathf.PI / 180 ) ) + beamsToAdd[i].transform.position.y, -2 );
				
				// get the collider of the structure that's at the point side1 and side2
				Collider2D colBeamTo   = Physics2D.OverlapCircle( side1, 0.1f, structures );
				Collider2D colBeamFrom = Physics2D.OverlapCircle( side2, 0.1f, structures );
				
				// convert that Collider2D into a Gameobject so I can compare treeNodes and add it to the tree
				GameObject endOne = colBeamTo.gameObject;  
				GameObject endTwo = colBeamFrom.gameObject;
				
				GenericStructureScript temp1 = endOne.GetComponent<GenericStructureScript>();
				GenericStructureScript temp2 = endTwo.GetComponent<GenericStructureScript>();
				
				temp1.attachedBeamCount++;
				temp2.attachedBeamCount++;
				
				BeamsRecord.AddBeamtoRecord( beamsToAdd[i], endOne, endTwo );
			}
		}
	}
	
	public void AddMinerToList( GameObject miner )
	{
		PowerTreeNode tempMiner = Trees.FindNode( miner, NodesRecord );
		if( tempMiner != null )
		{
			bool alreadyIn = false;
			if( Miners.Length > 0 )
			{
				for( int m = 0; m < Miners.Length; m++ )
				{
					if( Miners[m].nodeObject == miner )
					{
						alreadyIn = true;
					}
				}
				if( !alreadyIn )
				{
					PowerTreeNode[] temp = new PowerTreeNode[Miners.Length];
					for( int i = 0; i < temp.Length; i++ )
					{
						temp[i] = Miners[i];
					}
					Miners = new PowerTreeNode[temp.Length+1];
					for( int j = 0; j < temp.Length; j++ )
					{
						Miners[j] = temp[j];
					}
					Miners[Miners.Length-1] = Trees.FindNode( miner, NodesRecord );
				}
			}																		
			else
			{
				Miners = new PowerTreeNode[1];
				Miners[0] = Trees.FindNode( miner, NodesRecord );
			}
		}
	}
	
	public void AddGeneratorToList( GameObject generator )
	{
		PowerTreeNode tempGenerator = Trees.FindNode( generator, NodesRecord );
		GenericStructureScript SCSript = generator.GetComponent<GenericStructureScript>();
		if( tempGenerator != null )
		{
			bool alreadyIn = false;
			if( Generators.Length > 0 )
			{
				for( int m = 0; m < Generators.Length; m++ )
				{
					if( Generators[m].nodeObject == generator )
					{
						alreadyIn = true;
						break;
					}
				}
				if( !alreadyIn )
				{
					PowerTreeNode[] temp = new PowerTreeNode[Generators.Length];
					for( int i = 0; i < temp.Length; i++ )
					{
						temp[i] = Generators[i];
					}
					Generators = new PowerTreeNode[temp.Length+1];
					for( int j = 0; j < temp.Length; j++ )
					{
						Generators[j] = temp[j];
					}
					Generators[Generators.Length-1] = Trees.FindNode( generator, NodesRecord );
					Generators[Generators.Length-1].powerGenerating = SCSript.powerGeneration;
					Generators[Generators.Length-1].maxStoredPower= SCSript.maxStoredPower;
				}
			}
			else
			{
				Generators = new PowerTreeNode[1];
				Generators[0] = Trees.FindNode( generator, NodesRecord );
				Generators[0].powerGenerating = SCSript.powerGeneration;
				Generators[0].maxStoredPower = SCSript.maxStoredPower;
			}
		}
	}

    public void UpdateGenerator( GameObject generator )
    {
        PowerTreeNode tempGenerator = Trees.FindNode( generator, NodesRecord );
        GenericStructureScript SCSript = generator.GetComponent<GenericStructureScript>();
        if( tempGenerator != null )
        {
            if( Generators.Length > 0 )
            {
                for( int m = 0; m < Generators.Length; m++ )
                {
                    if( Generators[m].nodeObject == generator )
                    {
                        Generators[m].powerGenerating = SCSript.powerGeneration;
                        break;
                    }
                }
            }
        }
    }
	
	public void AddBatteryToList( GameObject battery )
	{
		PowerTreeNode tempBatt = Trees.FindNode( battery, NodesRecord );
		if( tempBatt != null )
		{
			bool alreadyIn = false;
			if( Batteries.Length > 0 )
			{
				for( int m = 0; m < Batteries.Length; m++ )
				{
					if( Batteries[m].nodeObject == battery )
					{
						alreadyIn = true;
					}
				}
				if( !alreadyIn )
				{
					PowerTreeNode[] temp = new PowerTreeNode[Batteries.Length];
					for( int i = 0; i < temp.Length; i++ )
					{
						temp[i] = Batteries[i];
					}
					Batteries = new PowerTreeNode[temp.Length+1];
					for( int j = 0; j < temp.Length; j++ )
					{
						Batteries[j] = temp[j];
					}
					Batteries[Batteries.Length-1] = Trees.FindNode( battery, NodesRecord );
				}
			}																	
			else
			{
				Batteries = new PowerTreeNode[1];
				Batteries[0] = Trees.FindNode( battery, NodesRecord );
			}
		}
	}
	
	public void AddLauncherToList( GameObject launcher )
	{
		PowerTreeNode tempLauncher = Trees.FindNode( launcher, NodesRecord );
		if( tempLauncher != null )
		{
			bool alreadyIn = false;
			if( MissileLaunchers.Length > 0 )
			{
				for( int m = 0; m < MissileLaunchers.Length; m++ )
				{
					if( MissileLaunchers[m].nodeObject == launcher )
					{
						alreadyIn = true;
					}
				}
				if( !alreadyIn )
				{
					PowerTreeNode[] temp = new PowerTreeNode[MissileLaunchers.Length];
					for( int i = 0; i < temp.Length; i++ )
					{
						temp[i] = MissileLaunchers[i];
					}
					MissileLaunchers = new PowerTreeNode[temp.Length+1];
					for( int j = 0; j < temp.Length; j++ )
					{
						MissileLaunchers[j] = temp[j];
					}
					MissileLaunchers[MissileLaunchers.Length-1] = Trees.FindNode( launcher, NodesRecord ); 
				}
			}																	
			else
			{
				MissileLaunchers = new PowerTreeNode[1];
				MissileLaunchers[0] = Trees.FindNode( launcher, NodesRecord );
			}
		}
	}
	
	public void AddTurretToList( GameObject turret )
	{
		PowerTreeNode tempTurret = Trees.FindNode( turret, NodesRecord );
		if( tempTurret != null )
		{
			bool alreadyIn = false;
			if( Turrets.Length > 0 )
			{
				for( int m = 0; m < Turrets.Length; m++ )
				{
					if( Turrets[m].nodeObject == turret )
					{
						alreadyIn = true;
					}
				}
				if( !alreadyIn )
				{
					PowerTreeNode[] temp = new PowerTreeNode[Turrets.Length];
					for( int i = 0; i < temp.Length; i++ )
					{
						temp[i] = Turrets[i];
					}
					Turrets = new PowerTreeNode[temp.Length+1];
					for( int j = 0; j < temp.Length; j++ )
					{
						Turrets[j] = temp[j];
					}
					Turrets[Turrets.Length-1] = Trees.FindNode( turret, NodesRecord ); 
				}
			}																		
			else
			{
				Turrets = new PowerTreeNode[1];
				Turrets[0] = Trees.FindNode( turret, NodesRecord );
			}
		}
	}
	
	public void AddEnergyNodeToList( GameObject node )
	{
		PowerTreeNode tempNode = Trees.FindNode( node, NodesRecord );
		if( tempNode != null )
		{
			bool alreadyIn = false;
			if( EnergyNodes.Length > 0 )
			{
				for( int m = 0; m < EnergyNodes.Length; m++ )
				{
					if( EnergyNodes[m].nodeObject == node )
					{
						alreadyIn = true;
					}
				}
				if( !alreadyIn )
				{
					PowerTreeNode[] temp = new PowerTreeNode[EnergyNodes.Length];
					for( int i = 0; i < temp.Length; i++ )
					{
						temp[i] = EnergyNodes[i];
					}
					EnergyNodes = new PowerTreeNode[temp.Length+1];
					for( int j = 0; j < temp.Length; j++ )
					{
						EnergyNodes[j] = temp[j];
					}
					EnergyNodes[EnergyNodes.Length-1] = Trees.FindNode( node, NodesRecord ); 
				}
			}																		
			else
			{
				EnergyNodes = new PowerTreeNode[1];
				EnergyNodes[0] = Trees.FindNode( node, NodesRecord );
			}
		}
	}
	
	public void AddRepairStationToList( GameObject repairStation )
	{
		PowerTreeNode tempRepair = Trees.FindNode( repairStation, NodesRecord );
		if( tempRepair != null )
		{
			bool alreadyIn = false;
			if( RepairStations.Length > 0 )
			{
				for( int m = 0; m < RepairStations.Length; m++ )
				{
					if( RepairStations[m].nodeObject == repairStation )
					{
						alreadyIn = true;
					}
				}
				if( !alreadyIn )
				{
					PowerTreeNode[] temp = new PowerTreeNode[RepairStations.Length];
					for( int i = 0; i < temp.Length; i++ )
					{
						temp[i] = RepairStations[i];
					}
					RepairStations = new PowerTreeNode[temp.Length+1];
					for( int j = 0; j < temp.Length; j++ )
					{
						RepairStations[j] = temp[j];
					}
					RepairStations[RepairStations.Length-1] = Trees.FindNode( repairStation, NodesRecord );
				}
			}																		
			else
			{
				RepairStations = new PowerTreeNode[1];
				RepairStations[0] = Trees.FindNode( repairStation, NodesRecord );
			}
		}
	}
	
	public void StartFirstTree( GameObject collectorPrime )
	{
		PowerTreeNode rootNode = new PowerTreeNode();
		rootNode.nodeObject = collectorPrime;
		SolarCollectorScript colScript = collectorPrime.gameObject.GetComponent<SolarCollectorScript>();
		colScript.built = true;
		colScript.health = 100;
		rootNode.built = true;
		
		int treeNumber = Trees.AddRoot( rootNode );
		int[] treeNumArray = new int[100];
		int[] treeNumArrayTemp = new int[0];
		treeNumArray[treeNumber] = 2;
		Trees.roots[treeNumber].SetRoot( treeNumber );
		NodesRecord.AddNodeToRecord( Trees.roots[treeNumber] );
		
		for( int u = 0; u < 100; u++ )
		{
			if( treeNumArray[u] == 2 )
			{
				if( treeNumArrayTemp.Length > 0 )
				{
					int[] temp = new int[treeNumArrayTemp.Length];
					for( int b = 0; b < temp.Length; b++ )
					{
						temp[b] = treeNumArrayTemp[b];
					}
					
					treeNumArrayTemp = new int[treeNumArrayTemp.Length+1];
					for( int v = 0; v < temp.Length; v++ )
					{
						treeNumArrayTemp[v] = temp[v];
					}
				}
				else
				{
					treeNumArrayTemp = new int[1];
				}
				treeNumArrayTemp[treeNumArrayTemp.Length-1] = u;
			}
		}
		Trees.RebuildTrees( treeNumArrayTemp, BeamsRecord, NodesRecord );
	}
	
	public void StartNewTree( GameObject rootObject )
	{
		PowerTreeNode rootNode = new PowerTreeNode();
		rootNode.nodeObject = rootObject;
		int treeNumber = Trees.AddRoot( rootNode );
		Trees.roots[treeNumber].SetRoot( treeNumber );
		NodesRecord.AddNodeToRecord( Trees.roots[treeNumber] );
		
		AddToTree( rootObject );
	}
	
	public void AddToTree( GameObject placingStructure )
	{
		// first we find all of the beams that are coming out of the structure we just placed
		Collider2D[] beamColliders = Physics2D.OverlapCircleAll( placingStructure.transform.position, 0.05f, beamsMask );
		GameObject[] beams = new GameObject[beamColliders.Length];
		for( int n = 0; n < beamColliders.Length; n++ )
		{
			beams[n] = beamColliders[n].gameObject;		// change the colliders into GameObjects so we can use them
		}

		int[] tempTouchedTrees = new int[100];
		for( int h = 0; h < 100; h++ )
		{
			tempTouchedTrees[h] = 0;
		}
		int[] touchedTrees = new int[0];
		
		// here we say, "check every beam in our entire record of beams.."
		for( int i = 0; i < BeamsRecord.beamsInRecord; i++ )
		{
			GameObject End1 = null;
			GameObject End2 = null;
			
			// "..and check every beam comes out of the structure we just placed.."
			for( int j = 0; j < beams.Length; j++ )
			{
				// "..and if they are the same beam, then we can use our BeamsRecord to quickly tell what that beam connects to"
				if( BeamsRecord.energyBeams[i].beamObject == beams[j] )
				{
					// get the GameObject for both structures that the beam connects to
					End1 = BeamsRecord.energyBeams[i].End1;
					End2 = BeamsRecord.energyBeams[i].End2;
					
					PowerTreeNode End1Node = null;
					PowerTreeNode End2Node = null;
					
					// find the node inside the PowerTree that corresponds to each structures' GameObject
					End1Node = Trees.FindNode( End1, NodesRecord );
					End2Node = Trees.FindNode( End2, NodesRecord );
					
					if( End1 == placingStructure )
					{
						if( End1Node != null )
						{
							for( int p = 0; p < 100; p++ )
							{
								if( ( End1Node.inTree[p] == 1 ) || ( End1Node.inTree[p] == 2 ) )
									tempTouchedTrees[p] = 1;
							}	
						}
						if( End2Node != null )
						{
							for( int p = 0; p < 100; p++ )
							{
								if( ( End2Node.inTree[p] == 1 ) || ( End2Node.inTree[p] == 2 ) )
								tempTouchedTrees[p] = 1;
							}	
						}
					}
					else if( End2 == placingStructure )
					{
						if( End2Node != null )
						{
							for( int p = 0; p < 100; p++ )
							{
								if( ( End2Node.inTree[p] == 1 ) || ( End2Node.inTree[p] == 2 ) )
									tempTouchedTrees[p] = 1;
							}
						}
						if( End1Node != null )
						{
							for( int p = 0; p < 100; p++ )
							{
								if( ( End1Node.inTree[p] == 1 ) || ( End1Node.inTree[p] == 2 ) )
									tempTouchedTrees[p] = 1;
							}	
						}
					}
					
					if( BigBallDebug )
					{
						Instantiate( BigRedBall , End1.transform.position, Quaternion.identity );
						Instantiate( BigBlueBall, End2.transform.position, Quaternion.identity );
					}
				}
			}
		}
		// this shortens the array from 100 1's and 0's to a short list of 1's with the length being the number of touched trees and the values being the touched trees
		for( int u = 0; u < 100; u++ )
		{
			if( tempTouchedTrees[u] == 1 )
			{
				if( touchedTrees.Length > 0 )
				{
					int[] temp = new int[touchedTrees.Length];
					for( int b = 0; b < temp.Length; b++ )
					{
						temp[b] = touchedTrees[b];
					}
					
					touchedTrees = new int[touchedTrees.Length+1];
					for( int v = 0; v < temp.Length; v++ )
					{
						touchedTrees[v] = temp[v];
					}
				}
				else
				{
					touchedTrees = new int[1];
				}
				touchedTrees[touchedTrees.Length-1] = u;
			}
		}
		Trees.RebuildTrees( touchedTrees, BeamsRecord, NodesRecord );
	}
	
	public void RemoveFromEverything( GameObject toBeRemoved )
	{
		bool needsRemoving = false;
		
		PowerTreeNode tempRemoved = Trees.FindNode( toBeRemoved, NodesRecord );
		int iterator = 0;
		int[] tempTouchedTrees = new int[100];
		int[] touchedTrees = new int[0];
		if( tempRemoved != null )
		{
			// This switch is to remove the powerNode from its structure-specific array -- doesn't apply to energy nodes because they don't have their own array
			switch( toBeRemoved.name )
			{
				case "MinerPreFab(Clone)":
					iterator = 0;
					for( int b = 0; b < Miners.Length; b++ )
					{
						if( tempRemoved == Miners[b] )
						{
							Miners[b] = null;
							needsRemoving = true;
						}
					}
					if( needsRemoving )
					{
						PowerTreeNode[] tempMiners = new PowerTreeNode[Miners.Length-1];
						for( int a = 0; a < Miners.Length; a++ )
						{
							if( Miners[a] != null )
							{
								tempMiners[iterator] = Miners[a];
								iterator++;
							}
						}
						Miners = new PowerTreeNode[tempMiners.Length];
						for( int c = 0; c < Miners.Length; c++ )
						{
							Miners[c] = tempMiners[c];
						}
					}
					break;
				case "EnergyStoragePreFab(Clone)":
					iterator = 0;
					for( int b = 0; b < Batteries.Length; b++ )
					{
						if( tempRemoved == Batteries[b] )
						{
							Batteries[b] = null;
							needsRemoving = true;
						}
					}
					if( needsRemoving )
					{
						PowerTreeNode[] tempBatteries = new PowerTreeNode[Batteries.Length-1];
						for( int a = 0; a < Batteries.Length; a++ )
						{
							if( Batteries[a] != null )
							{
								tempBatteries[iterator] = Batteries[a];
								iterator++;
							}
						}
						Batteries = new PowerTreeNode[tempBatteries.Length];
						for( int c = 0; c < Batteries.Length; c++ )
						{
							Batteries[c] = tempBatteries[c];
						}
					}
					break;
				case "EnergyNodePreFab(Clone)":
					iterator = 0;
					for( int b = 0; b < EnergyNodes.Length; b++ )
					{
						if( tempRemoved == EnergyNodes[b] )
						{
							EnergyNodes[b] = null;
							needsRemoving = true;
						}
					}
					if( needsRemoving )
					{
						PowerTreeNode[] tempEnergyNodes = new PowerTreeNode[EnergyNodes.Length-1];
						for( int a = 0; a < EnergyNodes.Length; a++ )
						{
							if( EnergyNodes[a] != null )
							{
								tempEnergyNodes[iterator] = EnergyNodes[a];
								iterator++;
							}
						}
						EnergyNodes = new PowerTreeNode[tempEnergyNodes.Length];
						for( int c = 0; c < EnergyNodes.Length; c++ )
						{
							EnergyNodes[c] = tempEnergyNodes[c];
						}
					}
					break;
				case "MissilePreFab(Clone)":
					iterator = 0;
					for( int b = 0; b < MissileLaunchers.Length; b++ )
					{
						if( tempRemoved == MissileLaunchers[b] )
						{
							MissileLaunchers[b] = null;
							needsRemoving = true;
						}
					}
					if( needsRemoving )
					{
						PowerTreeNode[] tempMissileLaunchers = new PowerTreeNode[MissileLaunchers.Length-1];
						for( int a = 0; a < MissileLaunchers.Length; a++ )
						{
							if( MissileLaunchers[a] != null )
							{
								tempMissileLaunchers[iterator] = MissileLaunchers[a];
								iterator++;
							}
						}
						MissileLaunchers = new PowerTreeNode[tempMissileLaunchers.Length];
						for( int c = 0; c < MissileLaunchers.Length; c++ )
						{
							MissileLaunchers[c] = tempMissileLaunchers[c];
						}
					}
					break;
				case "RepairStationPreFab(Clone)":
					iterator = 0;
		            for( int b = 0; b < RepairStations.Length; b++ )
					{
						if( tempRemoved == RepairStations[b] )
						{
							RepairStations[b] = null;
							needsRemoving = true;
						}
					}
					if( needsRemoving )
					{
						PowerTreeNode[] tempRepairStations = new PowerTreeNode[RepairStations.Length-1];
						for( int a = 0; a < RepairStations.Length; a++ )
						{
							if( RepairStations[a] != null )
							{
								tempRepairStations[iterator] = RepairStations[a];
								iterator++;
							}
						}
						RepairStations = new PowerTreeNode[tempRepairStations.Length];
						for( int c = 0; c < RepairStations.Length; c++ )
						{
							RepairStations[c] = tempRepairStations[c];
						}
					}
					break;
				case "TurretPreFab(Clone)":
					iterator = 0;
					for( int b = 0; b < Turrets.Length; b++ )
					{
						if( tempRemoved == Turrets[b] )
						{
							Turrets[b] = null;
							needsRemoving = true;
						}
					}
					if( needsRemoving )
					{
						PowerTreeNode[] tempTurrets = new PowerTreeNode[Turrets.Length-1];
						for( int a = 0; a < Turrets.Length; a++ )
						{
							if( Turrets[a] != null )
							{
								tempTurrets[iterator] = Turrets[a];
								iterator++;
							}
						}
						Turrets = new PowerTreeNode[tempTurrets.Length];
						for( int c = 0; c < Turrets.Length; c++ )
						{
							Turrets[c] = tempTurrets[c];
						}
					}
					break;
				case "SolarCollectorPreFab(Clone)":
					iterator = 0;
					for( int b = 0; b < Generators.Length; b++ )
					{
						if( tempRemoved == Generators[b] )
						{
							Generators[b] = null;
							needsRemoving = true;
						}
					}
					if( needsRemoving )
					{
						PowerTreeNode[] tempGenerators = new PowerTreeNode[Generators.Length-1];
						for( int a = 0; a < Generators.Length; a++ )
						{
							if( Generators[a] != null )
							{
								tempGenerators[iterator] = Generators[a];
								iterator++;
							}
						}
						Generators = new PowerTreeNode[tempGenerators.Length];
						for( int c = 0; c < Generators.Length; c++ )
						{
							Generators[c] = tempGenerators[c];
						}
					}
					break;
		        default: 
		            break;
	        }
			
			for( int p = 0; p < 100; p++ )
			{
				if( ( tempRemoved.inTree[p] == 1 ) || ( tempRemoved.inTree[p] == 2 ) )
					tempTouchedTrees[p] = 1;
			}
	
			for( int u = 0; u < 100; u++ )
			{
				if( tempTouchedTrees[u] == 1 )
				{
					if( touchedTrees.Length > 0 )
					{
						int[] temp = new int[touchedTrees.Length];
						for( int b = 0; b < temp.Length; b++ )
						{
							temp[b] = touchedTrees[b];
						}
						
						touchedTrees = new int[touchedTrees.Length+1];
						for( int v = 0; v < temp.Length; v++ )
						{
							touchedTrees[v] = temp[v];
						}
					}
					else
					{
						touchedTrees = new int[1];
					}
					touchedTrees[touchedTrees.Length-1] = u;
				}
			}
		}
		
		// We need to remove any beams attached to this structure before we annihilate its node in the PowerTree or else we will lose track of which ones touch this structure
		for( int e = 0; e < BeamsRecord.beamsInRecord; e++ )
		{
			if( BeamsRecord.energyBeams[e].End1 == toBeRemoved )
			{
				EnergyBeam byeBeam = BeamsRecord.energyBeams[e];
				GenericStructureScript tempSS = byeBeam.End2.GetComponent<GenericStructureScript>();
				tempSS.attachedBeamCount--;
				byeBeam.End1 = null;
				byeBeam.End2 = null;
				Destroy( byeBeam.beamObject );
			}
			else if( BeamsRecord.energyBeams[e].End2 == toBeRemoved )
			{
				EnergyBeam byeBeam = BeamsRecord.energyBeams[e];
				GenericStructureScript tempSS = byeBeam.End1.GetComponent<GenericStructureScript>();
				tempSS.attachedBeamCount--;
				byeBeam.End1 = null;
				byeBeam.End2 = null;
                Destroy( byeBeam.beamObject );
			}
		}
		
		// Next thing is to remove the node from the nodes record
		for( int p = 0; p < NodesRecord.nodesInRecord; p++ )
		{
			if( NodesRecord.powerNodes[p] == tempRemoved )
			{
				NodesRecord.powerNodes[p] = null;
				break;
			}
		}
		if( tempRemoved != null )
		{
			if( NodesRecord.nodesInRecord == 1 )
			{
				NodesRecord.powerNodes = new PowerTreeNode[0];
				NodesRecord.nodesInRecord = 0;
			}
			else
			{
				iterator = 0;
				PowerTreeNode[] tempNodes = new PowerTreeNode[NodesRecord.nodesInRecord-1];
				for( int a = 0; a < NodesRecord.nodesInRecord; a++ )
				{
					if( NodesRecord.powerNodes[a] != null )
					{
						tempNodes[iterator] = NodesRecord.powerNodes[a];
	                    iterator++;
	                }
	            }
	            NodesRecord.powerNodes = new PowerTreeNode[tempNodes.Length];
	            for( int b = 0; b < tempNodes.Length; b++ )
	            {
	            	NodesRecord.powerNodes[b] = tempNodes[b];
	            }
	            NodesRecord.nodesInRecord--;
	        }
        
	        // Now that we've removed the node from its structure-specific array, we need to remove it from the overall power tree
			for( int i = 0; i < Trees.size; i++ )
			{
				if( Trees.roots[i] == tempRemoved )
				{
					Trees.roots[i] = null;
				}
			}
			Trees.RebuildTrees( touchedTrees, BeamsRecord, NodesRecord );
            
            // after we remove what we need and rebuilt the trees we check the beams to make sure there aren't any left over
            CheckBeams();
		}
	}

    // this function should remove any beams that would have been left over
    void CheckBeams()
    {
        for( int i = 0; i < BeamsRecord.beamsInRecord; i++ )
        {
            if( ( BeamsRecord.energyBeams[i].End1 == null ) || ( BeamsRecord.energyBeams[i].End2 == null ) )
            {
                Destroy( BeamsRecord.energyBeams[i].beamObject );
            }
        }
    }
}


[System.Serializable]
public class PowerTree
{
	public PowerTreeNode[] roots = new PowerTreeNode[0];
	public int size = 0;
	
	public PowerTree()
	{
		
	}
	
	public int AddRoot( PowerTreeNode rootToBe )
	{
		int slotToAddRoot = -1;
		if( roots != null )
		{
			for( int m = 0; m < size; m++ )
			{
				if( roots[m] == null )
				{
					slotToAddRoot = m;
				}
			}
			if( slotToAddRoot == -1 )
			{
				PowerTreeNode[] temp = new PowerTreeNode[size];
				for( int i = 0; i < size; i++ )
				{
					temp[i] = roots[i];
				}
				
				size++;
				
				roots = new PowerTreeNode[size];
				for( int j = 0; j < size-1; j++ )
				{
					roots[j] = temp[j];
				}
				
				roots[size-1] = rootToBe;
				slotToAddRoot = size - 1;
			}
			else
			{
				roots[slotToAddRoot] = rootToBe;
			}
		}
		else
		{
			roots = new PowerTreeNode[size+1];
			roots[size] = rootToBe;
			size++;
			slotToAddRoot = size - 1;
		}
		return slotToAddRoot;
	}
	
	public PowerTreeNode FindNode( GameObject desiredObject, PowerNodeRecord nodeRecord )
	{
		PowerTreeNode answer = null;
		for( int i = 0; i < nodeRecord.nodesInRecord; i++ )
		{
			if( desiredObject == nodeRecord.powerNodes[i].nodeObject )
			{
				answer = nodeRecord.powerNodes[i];
				break;
			}
		}
		return answer;
	}
	
	public void RebuildTrees( int[] treesToRebuild, EnergyBeamsRecord beamRecord, PowerNodeRecord nodeRecord )
	{
		Queue treeQ = new Queue();
		Queue treeQChecker = new Queue();
		for( int i = 0; i < treesToRebuild.Length; i++ )
		{
			int thisTree = treesToRebuild[i];
			if( roots[thisTree] != null )
			{
				// first thing's first -- clear everything in the tree being rebuilt
				for( int n = 0; n < nodeRecord.nodesInRecord; n++ )
				{
					if( ( nodeRecord.powerNodes[n].inTree[thisTree] == 1 ) || ( nodeRecord.powerNodes[n].inTree[thisTree] == 2 ) )
					{
						PowerTreeNode temp = nodeRecord.powerNodes[n];
						
						// wipe away the children of this node in this tree before.  Then we'll rebuild it using the beam connections
						for( int j = 0; j < temp.numberofChildren[thisTree]; j++ )
						{
							temp.children[thisTree,j] = null;
						}
						temp.numberofChildren[thisTree] = 0;
						
						// now wipe away the parent
						temp.parent[thisTree] = null;
						
						// now reset the distance from the source
						temp.distanceFromRoot[thisTree] = 1000;
						
						// remove the nodes from the tree, they'll be re-added later if they're still in the tree
						temp.inTree[thisTree] = 0;
					}
				}
				
				// this is the starting point for the queue -- the source of the tree being rebuilt
				PowerTreeNode root = roots[thisTree];
				root.SetRoot( thisTree );
				root.distanceFromRoot[thisTree] = 0;
				
				treeQ.Enqueue( root );
				treeQChecker.Enqueue( root );
				PowerManagerScript tempPowerMan = GameObject.Find( "PowerManager" ).GetComponent<PowerManagerScript>();
				while( treeQ.Count > 0 )
				{
					PowerTreeNode temp = (PowerTreeNode)treeQ.Dequeue();
					if( temp != null )
					{
						bool alreadyExists = false;
						switch( temp.nodeObject.name ) // This switch it so add each structure to their structure-specific array if it isn't already in there -- nothing else
						{
							case "MinerPreFab(Clone)":
								for( int b = 0; b < tempPowerMan.Miners.Length; b++ )
								{
									if( temp == tempPowerMan.Miners[b] )
									{
										alreadyExists = true;
									}
								}
								if( !alreadyExists )
								{
									tempPowerMan.AddMinerToList( temp.nodeObject );
								}
								break;
							case "EnergyStoragePreFab(Clone)":
								for( int b = 0; b < tempPowerMan.Batteries.Length; b++ )
								{
									if( temp == tempPowerMan.Batteries[b] )
									{
										alreadyExists = true;
									}
								}
								if( !alreadyExists )
								{
									tempPowerMan.AddBatteryToList( temp.nodeObject );
								}
								break;
							case "EnergyNodePreFab(Clone)":
							for( int b = 0; b < tempPowerMan.EnergyNodes.Length; b++ )
							{
								if( temp == tempPowerMan.EnergyNodes[b] )
								{
									alreadyExists = true;
								}
							}
							if( !alreadyExists )
							{
								tempPowerMan.AddEnergyNodeToList( temp.nodeObject );
							}
							break;
							case "MissilePreFab(Clone)":
								for( int b = 0; b < tempPowerMan.MissileLaunchers.Length; b++ )
								{
									if( temp == tempPowerMan.MissileLaunchers[b] )
									{
										alreadyExists = true;
									}
								}
								if( !alreadyExists )
								{
									tempPowerMan.AddLauncherToList( temp.nodeObject );
								}
								break;
							case "RepairStationPreFab(Clone)":
								for( int b = 0; b < tempPowerMan.RepairStations.Length; b++ )
								{
									if( temp == tempPowerMan.RepairStations[b] )
									{
										alreadyExists = true;
									}
								}
								if( !alreadyExists )
								{
									tempPowerMan.AddRepairStationToList( temp.nodeObject );
								}
								break;
							case "TurretPreFab(Clone)":
								for( int b = 0; b < tempPowerMan.Turrets.Length; b++ )
								{
									if( temp == tempPowerMan.Turrets[b] )
									{
										alreadyExists = true;
									}
								}
								if( !alreadyExists )
								{
									tempPowerMan.AddTurretToList( temp.nodeObject );
								}
								break;
							case "SolarCollectorPreFab(Clone)":
								for( int b = 0; b < tempPowerMan.Generators.Length; b++ )
								{
									if( temp == tempPowerMan.Generators[b] )
									{
										alreadyExists = true;
									}
								}
								if( !alreadyExists )
								{
									tempPowerMan.AddGeneratorToList( temp.nodeObject );
								}
								break;
							default: 
								break;
						}
						
						// find all beams that have one end attached to the structure being placed
 						EnergyBeam[] attachedBeams = beamRecord.FindMultipleBeams( temp.nodeObject );
						for( int x = 0; x < attachedBeams.Length; x++ )
						{
							if( temp.numberofChildren[thisTree] < 6 )
							{
								if( attachedBeams[x].End1 == temp.nodeObject ) // find the end that is our temp node
								{
									// this is where we run through each node and check to make sure that the number of beams, what they're attached to, and the children
									// array all line up
									
									PowerTreeNode End2Node = FindNode( attachedBeams[x].End2, nodeRecord ); // check if another structure attached via energy beam has a powernode already and find it
									if( End2Node != null )	// if it does exist, then we need to add it as a child to our temp node since it's attached by energy beam
									{
										temp.numberofChildren[thisTree]++;
										temp.children[thisTree,temp.numberofChildren[thisTree]-1] = End2Node;
										if( temp.children[thisTree,temp.numberofChildren[thisTree]-1].inTree[thisTree] != 2 ) // this checks to make sure it's not a root.. we don't want to overwrite a 2 with a 1 for inTree
										{
											temp.children[thisTree,temp.numberofChildren[thisTree]-1].SetTree( thisTree );	// if it's a 2 already, then it's already in our tree so there's no need to replace it with a 1
										}
										if( End2Node.distanceFromRoot[thisTree] > ( temp.distanceFromRoot[thisTree] + 1 ) )	// this compares distances from the source
										{
											temp.children[thisTree,temp.numberofChildren[thisTree]-1].SetParent( temp, thisTree );	// if our temp node is closer to the source by more than 1, it'll set the temp node as
										}											// the parent for attached child
									}
									else
									{
										temp.AddChild( attachedBeams[x].End2, thisTree, nodeRecord );
										temp.children[thisTree,temp.numberofChildren[thisTree]-1].SetParent( temp, thisTree );
									}
								}
								else if( attachedBeams[x].End2 == temp.nodeObject )
								{
									PowerTreeNode End1Node = FindNode( attachedBeams[x].End1, nodeRecord );
									if( End1Node != null )
									{
										temp.numberofChildren[thisTree]++;
										temp.children[thisTree,temp.numberofChildren[thisTree]-1] = End1Node;
										if( temp.children[thisTree,temp.numberofChildren[thisTree]-1].inTree[thisTree] != 2 )
										{
											temp.children[thisTree,temp.numberofChildren[thisTree]-1].SetTree( thisTree );
										}
										if( End1Node.distanceFromRoot[thisTree] > ( temp.distanceFromRoot[thisTree]+ 1 ) )
										{
											temp.children[thisTree,temp.numberofChildren[thisTree]-1].SetParent( temp, thisTree );
										}
									}
									else
									{
										temp.AddChild( attachedBeams[x].End1, thisTree, nodeRecord );
										temp.children[thisTree,temp.numberofChildren[thisTree]-1].SetParent( temp, thisTree );
									}
								}
							}
						}
						
						for( int k = 0; k < temp.numberofChildren[thisTree]; k++ )
						{
							if( !treeQChecker.Contains( temp.children[thisTree,k] ) )
							{
								treeQChecker.Enqueue( temp.children[thisTree,k] );
								treeQ.Enqueue( temp.children[thisTree,k] );
							}
						}
					}
				}
				treeQChecker.Clear();
			}
		}
	}
	
	public GameObject GetNodeObject( PowerTreeNode current )
	{
		return current.nodeObject;
	}
	
	public void SetNodeObject( GameObject obj, PowerTreeNode current )
	{
		current.nodeObject = obj;
	}
	
	public int GetNodeStoredPower( GameObject requestedObejct, PowerNodeRecord nodeRecord )
	{
		int powerStored = 0;
		PowerTreeNode temp = FindNode( requestedObejct, nodeRecord );
		if( temp != null )
		{
			powerStored = temp.storedPower;
		}
		return powerStored;
	}
	
	public int GetNodeAttachedBeams( GameObject requestedObejct, PowerNodeRecord nodeRecord )
	{
		int attachedBeams = 0;
		PowerTreeNode temp = FindNode( requestedObejct, nodeRecord );
		if( temp != null )
		{
			GenericStructureScript tempSc = temp.nodeObject.GetComponent<GenericStructureScript>();
			attachedBeams = tempSc.attachedBeamCount;
		}
		return attachedBeams;
	}
	
	public int GetNodeHealth( GameObject requestedObject, PowerNodeRecord nodeRecord )
	{
		int health = 0;
		PowerTreeNode temp = FindNode( requestedObject, nodeRecord );
		if( temp != null )
		{
			GenericStructureScript tempSc = temp.nodeObject.GetComponent<GenericStructureScript>();
			health = tempSc.health;
		}
		return health;
	}
	
	public float GetNodePowerGeneration( GameObject requestedObejct, PowerNodeRecord nodeRecord )
	{
		float gen = 0;
		PowerTreeNode temp = FindNode( requestedObejct, nodeRecord );
		if( temp != null )
		{
			GenericStructureScript tempSc = temp.nodeObject.GetComponent<GenericStructureScript>();
			gen = tempSc.powerGeneration;
		}
		return gen;
	}

    public void UpdateNode( GameObject nodeObj, PowerNodeRecord nodeRec, int powerGen, int storedPow, bool shouldFunction, bool built )
    {
        PowerTreeNode disNode = FindNode( nodeObj, nodeRec );
        disNode.powerGenerating = powerGen;
        disNode.storedPower = storedPow;
        disNode.shouldFunction = shouldFunction;
        disNode.built = built;
    }
}


public class PowerTreeNode
{
	public GameObject nodeObject = null;
	public PowerTreeNode[,] children = new PowerTreeNode[100,6];
	public PowerTreeNode[] parent = new PowerTreeNode[100];
	
	public int powerGenerating = 0;
	public int storedPower = 0;
	public int maxStoredPower = 10;
	public int powerConsuming = 0;
	public int powerToBuild = 20;
	public int[] numberofChildren = new int[100];
	public int[] inTree = new int[100];
	public int[] distanceFromRoot = new int[100];
	public bool tried = false;
	public bool shouldFunction = false;
	public bool built = false;
	
	public PowerTreeNode()
	{
		for( int i = 0; i < 100; i++ )
		{
			distanceFromRoot[i] = 99999;
		}
	}
	
	public PowerTreeNode( GameObject newNode )
	{
		nodeObject = newNode;
		for( int i = 0; i < 100; i++ )
		{
			distanceFromRoot[i] = 99999;
		}
	}
	
	public void AddChild( GameObject newNode, int treeNumber, PowerNodeRecord nodeRecord )
	{
		numberofChildren[treeNumber]++;
		children[treeNumber,numberofChildren[treeNumber]-1] = new PowerTreeNode( newNode );
		children[treeNumber,numberofChildren[treeNumber]-1].SetTree( treeNumber );
		nodeRecord.AddNodeToRecord( children[treeNumber,numberofChildren[treeNumber]-1] );
	}
	
	public void SetParent( PowerTreeNode toBeParent, int treeNumber )
	{
		parent[treeNumber] = toBeParent;
		distanceFromRoot[treeNumber] = parent[treeNumber].distanceFromRoot[treeNumber] + 1;
	}
	
	public void SetTree( int treeNumber )
	{
		inTree[treeNumber] = 1;
	}
	
	public void SetRoot( int treeNumber )
	{
		inTree[treeNumber] = 2;
	}
	
	public bool CheckIfInTree( int treeNumber )
	{
		bool result = false;
		
		if( inTree[treeNumber] == 1 )
		{
			result = true;
		}
		return result;
	}
	
	public bool CheckIfInAnyTree()
	{
		bool result = false;
		
		for( int i = 0; i < 100; i++ )
		{
			if( ( inTree[i] == 1 ) || ( inTree[i] == 2 ) )
			{
				result = true;
				break;
	        }
        }
        return result;
    }
    
    public int[] GetTrees()
    {
    	int[] inTrees = new int[0];
    	int[] inTreesTemp = new int[0];
    	
    	for( int i = 0; i < 100; i++ )
    	{
    		if( ( inTree[i] == 1 ) || ( inTree[i] == 2 ) )
    		{
	    		if( inTrees.Length > 0 )
	    		{
					inTreesTemp = new int[inTrees.Length];
	    			for( int j = 0; j < inTrees.Length; j++ )
	    			{
	    				inTreesTemp[j] = inTrees[j];
	    			}
	    			inTrees = new int[inTrees.Length+1];
	    			for( int k = 0; k < inTreesTemp.Length; k++ )
	    			{
	    				inTrees[k] = inTreesTemp[k];
	    			}
	    		}
	    		else
	    		{
	    			inTrees = new int[1];
	    		}
	    		inTrees[inTrees.Length-1] = i;
    		}
    	}
    	
    	// now we have an array that may look something like { 0, 1, 2, 3, 4 }.  We need to sort it based on distance from source
    	int[] inTreeSorted = new int[inTrees.Length];
    	for( int te = 0; te < inTrees.Length; te++ )
    	{
    		inTreeSorted[te] = -1;
    	}
		for( int i = 0; i < inTrees.Length; i++ )
		{
			int minimum = 99999;
			int tree = -1;
	    	bool[] alreadyDone = new bool[inTrees.Length];
	    	
	    	
    		for( int j = 0; j < inTrees.Length; j++ )
    		{
    			for( int k = 0; k < inTreeSorted.Length; k++ )
    			{
    				if( inTrees[j] == inTreeSorted[k] )
    				{
    					alreadyDone[j] = true;
    					break;
    				}
    			}
    			if( ( distanceFromRoot[inTrees[j]] < minimum ) && ( !alreadyDone[j] ) )
    			{
    				minimum = distanceFromRoot[inTrees[j]];
    				tree = inTrees[j];
    			}
    		}
    		inTreeSorted[i] = tree;
		}
    	return inTreeSorted;
    }
    
    public MeshRenderer[] ClimbTree( int tree, EnergyBeamsRecord beamRecord )
    {
    	MeshRenderer[] meshes = new MeshRenderer[0];
    	PowerTreeNode[] climbingNodes = new PowerTreeNode[1];
    	EnergyBeam[] beams = new EnergyBeam[0];
    	PowerTreeNode tempNode = this;
    	climbingNodes[0] = this;
    	for( int i = 0; i < distanceFromRoot[tree]; i++ ) // loop through exactly enough times to reach the source and store each node
    	{
			PowerTreeNode[] temp = new PowerTreeNode[climbingNodes.Length];
			for( int j = 0; j < climbingNodes.Length; j++ )
			{
				temp[j] = climbingNodes[j];
			}
			
			climbingNodes = new PowerTreeNode[climbingNodes.Length+1];
			for( int k = 0; k < temp.Length; k++ )
			{
				climbingNodes[k] = temp[k];
			}
			
			climbingNodes[climbingNodes.Length-1] = tempNode.parent[tree];
			tempNode = tempNode.parent[tree];
    	}
    	
    	// after we have our array of nodes that is our trail back to the source, we can connect every 2 of them to get the beams
    	if( climbingNodes.Length == 1 )
    	{
    		Debug.Log( "Climbing to source from source... This should never happen" ); // TODO fix the issue with this. This shouldn't printed but it is
    	}
    	else
    	{
    	   	for( int i = 0; i <= climbingNodes.Length-2; i++ )
    		{
    			for( int bcount = 0; bcount < beamRecord.beamsInRecord; bcount++ )
    			{
    				if( ( beamRecord.energyBeams[bcount].End1 == climbingNodes[i].nodeObject ) && ( beamRecord.energyBeams[bcount].End2 == climbingNodes[i+1].nodeObject ) ) // if the 2 ends of the beam are
    				{																																		// in our climbing array meaning one is the parent of the previous
						EnergyBeam[] tempBeam = new EnergyBeam[beams.Length];																				// then add that beam to our list
						for( int b = 0; b < beams.Length; b++ )
						{
							tempBeam[b] = beams[b];
						}
						beams = new EnergyBeam[beams.Length+1];
						for( int c = 0; c < tempBeam.Length; c++ )
						{
							beams[c] = tempBeam[c];
						}
    					beams[beams.Length-1] = beamRecord.energyBeams[bcount];
    					break;
    				}
					else if( ( beamRecord.energyBeams[bcount].End2 == climbingNodes[i].nodeObject ) && ( beamRecord.energyBeams[bcount].End1 == climbingNodes[i+1].nodeObject ) )
					{
						EnergyBeam[] tempBeam = new EnergyBeam[beams.Length];
						for( int b = 0; b < beams.Length; b++ )
						{
							tempBeam[b] = beams[b];
						}
						beams = new EnergyBeam[beams.Length+1];
						for( int c = 0; c < tempBeam.Length; c++ )
						{
							beams[c] = tempBeam[c];
						}
						beams[beams.Length-1] = beamRecord.energyBeams[bcount];
						break;
					}
    			}
    		}
    	}
    	
    	// now we have our array of beams, lets get an array of sprites and return it so they can flash
    	meshes = new MeshRenderer[beams.Length];
    	
    	for( int s = 0; s < meshes.Length; s++ )
    	{
    		if( beams[s] != null )
    		{
    			if( beams[s].beamObject != null )
    			{
    				meshes[s] = beams[s].beamObject.GetComponent<MeshRenderer>();
				}
			}
    	}
    	
    	return meshes;
    }
	
	public bool GetChild( GameObject child, int treeNum )
	{
		bool yayNay = false;
		for( int i = 0; i < numberofChildren[treeNum]; i++ )
		{
			if( children[treeNum,i].nodeObject == child )
			{
				if( children[treeNum,i].CheckIfInTree( treeNum ) )
				{
					yayNay = true;
				}
			}
		}
		return yayNay;
	}
}


public class PowerNodeRecord
{
	public int nodesInRecord = 0;
	public PowerTreeNode[] powerNodes = new PowerTreeNode[0];
	
	public PowerNodeRecord()
	{
		
	}
	
	public void AddNodeToRecord( PowerTreeNode nodeToAdd )
	{
		if( nodesInRecord > 0 )
		{
			PowerTreeNode[] temp = new PowerTreeNode[powerNodes.Length];
			for( int i = 0; i < temp.Length; i++ )
			{
				temp[i] = powerNodes[i];
			}
			
			nodesInRecord++;
			powerNodes = new PowerTreeNode[nodesInRecord];
			for( int j = 0; j < nodesInRecord-1; j++ )
			{
				powerNodes[j] = temp[j];
			}
		}
		else
		{
			nodesInRecord++;
			powerNodes = new PowerTreeNode[nodesInRecord];
		}
		
		powerNodes[nodesInRecord-1] = nodeToAdd;
	}
}


public class EnergyBeamsRecord
{
	public int beamsInRecord = 0;
	public EnergyBeam[] energyBeams = new EnergyBeam[0];
	
	public EnergyBeamsRecord()
	{
		
	}
	
	public void AddBeamtoRecord( GameObject beamObject, GameObject End1, GameObject End2 )
	{
		if( beamsInRecord > 0 )
		{
			EnergyBeam[] temp = new EnergyBeam[energyBeams.Length];
			for( int i = 0; i < temp.Length; i++ )
			{
				temp[i] = energyBeams[i];
			}
			
			beamsInRecord++;
			energyBeams = new EnergyBeam[beamsInRecord];
			for( int j = 0; j < beamsInRecord-1; j++ )
			{
				energyBeams[j] = temp[j];
			}
		}
		else
		{
			beamsInRecord++;
			energyBeams = new EnergyBeam[beamsInRecord];
		}
		
		energyBeams[beamsInRecord-1] = new EnergyBeam( beamObject, End1, End2 );
	}
	
	public EnergyBeam[] FindMultipleBeams( GameObject desiredObject )
	{
		EnergyBeam[] results = new EnergyBeam[0];
		for( int u = 0 ; u < beamsInRecord; u++ )
		{
			if( energyBeams[u].End1 == desiredObject )
			{
				EnergyBeam[] temp = new EnergyBeam[results.Length];
				for( int i = 0; i < results.Length; i++ )
				{
					temp[i] = results[i];
				}
				results = new EnergyBeam[temp.Length+1];
				for( int j = 0; j < temp.Length; j++ )
				{
					results[j] = temp[j];
				}
				results[results.Length-1] = energyBeams[u];
			}
			else if( energyBeams[u].End2 == desiredObject )
			{
				EnergyBeam[] temp = new EnergyBeam[results.Length];
				for( int i = 0; i < results.Length; i++ )
				{
					temp[i] = results[i];
				}
				results = new EnergyBeam[temp.Length+1];
				for( int j = 0; j < temp.Length; j++ )
				{
					results[j] = temp[j];
				}
				results[results.Length-1] = energyBeams[u];
			}
		}
		return results;
	}
}


public class EnergyBeam
{
	public GameObject beamObject;
	public GameObject End1;
	public GameObject End2;
	public bool End1Tried;
	public bool End2Tried;
	
	public EnergyBeam()
	{
		beamObject = null;
		End1 = null;
		End2 = null;
		End1Tried = false;
		End2Tried = false;
	}
	
	public EnergyBeam( GameObject beam )
	{
		beamObject = beam;
		End1 = null;
		End2 = null;
		End1Tried = false;
		End2Tried = false;
	}
	
	public EnergyBeam( GameObject beam, GameObject firstEnd, GameObject secondEnd )
	{
		beamObject = beam;
		End1 = firstEnd;
		End2 = secondEnd;
		End1Tried = false;
		End2Tried = false;
	}
}