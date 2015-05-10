using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	
	public GameObject[] Asteroids;
	public GameObject SolarPreFab;
	public int numberOfAsteroids = 0;
	public int typeOfDistribution = 0;
	public float asteroidSeparation = 0.15f;
	private float minSize = 0.2f;
	private float maxSize = 0.7f;
	private float minDistance = 0.6f;
	private float maxDistance;
	
	void Awake()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag( "GameManager" );
		for( int i = 0; i < temp.Length; i++ )
		{
			if( temp[i] != gameObject )
			{
				Destroy( gameObject );
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad( gameObject );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnLevelWasLoaded( int level )
	{
		if( level == 2 )
		{
			CreateAsteroidField( numberOfAsteroids );
			
			//---Spawn a Starting Solar Collector---
			GameObject temp = (GameObject)Instantiate( SolarPreFab, Vector3.zero, Quaternion.identity );
			temp.GetComponent<Collider2D>().enabled = true;
			temp.tag = "FirstCollector";
			SolarCollectorScript tempScript = temp.GetComponent<SolarCollectorScript>();
			tempScript.built = true;
			tempScript.placed = true;
		}
	}
	
	void CreateAsteroidField( int astCount )
	{
		if( astCount <= 50 )
		{
			maxDistance = 8;
		}
		else if ( astCount <= 100 )
		{
			maxDistance = 15;
		}
		else if ( astCount <= 200 )
		{
			maxDistance = 20;
		}
		else if( astCount <= 300 )
		{
			maxDistance = 25;
		}
		else if( astCount <= 400 )
		{
			maxDistance = 30;
		}
		else if( astCount <= 500 )
		{
			maxDistance = 35;
		}
		else if( astCount <= 600 )
		{
			maxDistance = 40;
		}
		else if( astCount <= 700 )
		{
			maxDistance = 45;
		}
		else if( astCount <= 800 )
		{
			maxDistance = 50;
		}
		//---Initialize Uniform Variables---
		Vector3 uniform = Vector3.zero;


		//---Initialize Clustered Variables---
		Vector3 clusterCenter = Vector3.zero;
		Vector3 clusterCenter2 = Vector3.zero;
		int numberInCluster = 0;
		int clusterCounter = 0;


		//---Loop Through All Asteroids to be Created---
		for( int i = 0; i < astCount; i++ )
		{
			//---Initialize Position to Origin---
			Vector3 pos = Vector3.zero;
			//---Give Asteroid a Random Size---
			float size = Random.Range( minSize, maxSize );
			//---Give Asteroid a Random Texture---
			GameObject prefab = Asteroids[ Random.Range( 0, Asteroids.Length ) ];

			//==================================================
			//===============Uniform Distribution===============
			//==================================================
			if ( typeOfDistribution == 0 )
			{
				for( int j = 0; j < 100; j++ )
				{
					uniform = Random.insideUnitCircle * ( minDistance + ( maxDistance - minDistance ) * Random.value );
					
					if( !Physics2D.OverlapCircle( uniform, size + asteroidSeparation ) )
					{
						pos = uniform;
						break;
					}
				}
			}

			//==================================================
			//===============Cluster Distribution===============
			//==================================================
			else if ( typeOfDistribution == 1 )
			{
				//---Generate a New Cluster Center Every so many Asteroids
				if ( clusterCounter>=numberInCluster || i==0)
				{
					//---Randomize the Number of Asteroids in a Cluster---
					numberInCluster = (int) (5 * Random.value)+5;
					//---Randomize a New Cluster Center---
					for (int k=0; k < 10000; k++)
					{
						clusterCenter2 = clusterCenter;
						clusterCenter = Random.insideUnitCircle * ( minDistance + ( maxDistance - minDistance ) * Random.value );
						if (Vector3.Distance(clusterCenter2,clusterCenter)>10)
						{
							break;
						}
					}
					//Reset Cluster Counter
					clusterCounter = 0;
				}

				for( int j = 0; j < 10000; j++ )
				{
					Vector3 cluster = Random.insideUnitCircle * ( ( 10 - minDistance ) / 5 * Random.value );
					cluster = cluster + clusterCenter;
					if( !Physics2D.OverlapCircle( cluster, size  ) )
					{
						pos = cluster;
						clusterCounter ++;
						break;
					}
				}
			}

			//=========================================================
			//===============Vertical Belts Distribution===============
			//=========================================================
			else if ( typeOfDistribution == 2 )
			{
				//Change This Code to Vertical Belts Code
//				for( int j = 0; j < 100; j++ )
//				{
//					Vector3 temp = Random.insideUnitCircle * ( minDistance + ( maxDistance - minDistance ) * Random.value );
//					
//					if( !Physics2D.OverlapCircle( temp, size + asteroidSeparation ) )
//					{
//						pos = temp;
//						break;
//					}
//				}
			}

			//===========================================================
			//===============Horizontal Belts Distribution===============
			//===========================================================
			else if ( typeOfDistribution == 3 )
			{
				//Change This Code to Horizontal Belts Code
//				for( int j = 0; j < 100; j++ )
//				{
//				Vector3 temp = Random.insideUnitCircle * ( minDistance + ( maxDistance - minDistance ) * Random.value );
//				
//				if( !Physics2D.OverlapCircle( temp, size + asteroidSeparation ) )
//				{
//					pos = temp;
//					break;
//				}
//				}
			}

			//=================================================================
			//===============Place Asteroid in Generated Position==============
			//=================================================================
			
			if( pos != Vector3.zero )
			{
				float distance = Vector3.Distance( Vector3.zero, pos );
				
				if( distance > minDistance )
				{
					//---Create Instance of Asteroid---
					GameObject go = (GameObject)Instantiate( prefab, pos, Quaternion.identity );
					//---Random Rotation---
					go.transform.Rotate( new Vector3( Random.Range( -360f, 360f ), Random.Range( -360f, 360f ), Random.Range( -360f, 360f ) ) );
					//---Random Scale---
	//				go.transform.localScale = new Vector3( size, size, size );
				}
			}
		}
	}
}

