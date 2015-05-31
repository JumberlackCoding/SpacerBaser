using UnityEngine;
using System.Collections;

public class MinerScript : GenericStructureScript {
	
	public int miningRate = 10;
	public float miningRange = 0.3f;
	public GameObject MineralBeamPreFab;
	public LayerMask asteroidMask;
	
	private Collider2D[] asteroidColliders;
	
	void Start () {
		maxBeamsAllowed = 1;
		Initialize();
		powerConsumption = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if( !dead )
        {
            if( !built )
            {
                Build();
            }
            else
            {
                if( oneTimeCall )
                {
                    oneTimeCall = false;
                    asteroidColliders = Physics2D.OverlapCircleAll( transform.position, miningRange, asteroidMask ); // find all asteroids within mining range
                }
            }

            // handle health bars
            if( health < maxHealth )
            {
                if( !healthBarBackMade )
                {
                    healthBarBackObj = (GameObject)Instantiate( healthBarBackPreFab, new Vector3( transform.position.x, transform.position.y + 0.12f, transform.position.z - 0.5f ), Quaternion.identity );

                    healthBarBackMade = true;
                }
                if( !healthBarFrontMade )
                {
                    healthBarFrontObj = (GameObject)Instantiate( healthBarFrontPreFab, new Vector3( transform.position.x, transform.position.y + 0.12f, transform.position.z - 1.0f ), Quaternion.identity );
                    healthBarFrontMade = true;
                }

                if( healthBarBackObj )
                {
                    healthBarBackObj.transform.position = new Vector3( transform.position.x, transform.position.y + 0.12f, transform.position.z - 0.5f );
                }

                if( healthBarFrontObj )
                {
                    float healthPercent = (float)health / maxHealth;
                    healthBarFrontObj.transform.localScale = new Vector3( 0.2f * healthPercent, healthBarFrontObj.transform.localScale.y, healthBarFrontObj.transform.localScale.z );
                    healthBarFrontObj.transform.position = new Vector3( transform.position.x - 0.1f + ( 0.1f * healthPercent ), transform.position.y + 0.12f, transform.position.z - 1.0f );
                }
            }
            else
            {
                if( healthBarBackObj )
                {
                    Destroy( healthBarBackObj );
                    healthBarBackMade = false;
                }
                if( healthBarFrontObj )
                {
                    Destroy( healthBarFrontObj );
                    healthBarFrontMade = false;
                }
            }

            if( health <= 0 )
            {
                StartCoroutine( Die( false, 0 ) );
            }
        }
	}
	
	public bool AbleToMine()
	{
		bool able = false;
		if( asteroidColliders != null )
		{
			for( int i = 0; i < asteroidColliders.Length; i++ )
			{
				AsteroidScript asteroidSc = asteroidColliders[i].GetComponent<AsteroidScript>();
				if( asteroidSc.currentMins > 0 )
				{
					able = true;
					break;
				}
			}
		}
		
		if( built && !able )
		{
			transform.tag = "Bad Miner";
		}
		
		return able;
	}
	
	public override void StructureFunction()
	{
		if( asteroidColliders != null )
		{
			for( int i = 0; i < asteroidColliders.Length; i++ )
			{
				AsteroidScript asteroidSc = asteroidColliders[i].GetComponent<AsteroidScript>();
				
				if( asteroidSc.currentMins > 0 )
				{
					asteroidSc.currentMins -= miningRate;
					if( asteroidSc.currentMins < 0 )
					{
						asteroidSc.currentMins = 0;
					}
					powerManager.currentMinerals += miningRate;
					powerManager.mineralsForScore += miningRate;
				}
			}
		}
	}
	
	public void DrawBeams()
	{
		if( asteroidColliders != null )
		{
			GameObject[] beams = new GameObject[asteroidColliders.Length];
			
			for( int i = 0; i < asteroidColliders.Length; i++ )
			{
				AsteroidScript asteroidSc = asteroidColliders[i].GetComponent<AsteroidScript>();
				if( asteroidSc.currentMins > 0 )
				{
					Vector3 posInsideAsteroid = new Vector3( asteroidColliders[i].transform.position.x + Random.Range( -0.25f, 0.25f ) * asteroidColliders[i].transform.localScale.x, asteroidColliders[i].transform.position.y + Random.Range( -0.25f, 0.25f ) * asteroidColliders[i].transform.localScale.y, 4f );
					Vector3 pos = new Vector3( ( posInsideAsteroid.x + transform.position.x ) / 2, ( posInsideAsteroid.y + transform.position.y ) / 2, 4f );
					Vector3 temp = posInsideAsteroid - transform.position;
					temp.z = 0;
					beams[i] = (GameObject)Instantiate( MineralBeamPreFab, pos, Quaternion.identity );
					
					beams[i].transform.rotation = Quaternion.FromToRotation( new Vector3( Vector3.up.x, Vector3.up.y, Vector3.up.z ), temp );
					beams[i].transform.localScale = new Vector3( beams[i].transform.localScale.x, temp.magnitude, beams[i].transform.localScale.z );
				}
			}
		}
	}
	
	public void DestroyBeams()
	{
		GameObject[] beams = GameObject.FindGameObjectsWithTag( "MineralBeam" );
		for( int j = 0; j < beams.Length; j++ )
		{
			Destroy( beams[j].gameObject );
		}
	}
	
	public override IEnumerator Die( bool salvaged, int cost )
	{
        dead = true;
		if( salvaged )
		{
			if( built )
			{
				powerManager.currentMinerals += (int)( 0.6f * (float)cost );
			}
			else
			{
				powerManager.currentMinerals += cost;
			}
		}
		powerManager.RemoveFromEverything( gameObject );
		if( foreBuildBar != null )
		{
			Destroy( foreBuildBar.gameObject );
		}
		if( backBuildBar != null )
		{
			Destroy( backBuildBar.gameObject );
		}
		DestroyBeams();
        if( healthBarBackObj )
        {
            Destroy( healthBarBackObj );
        }
        if( healthBarFrontObj )
        {
            Destroy( healthBarFrontObj );
        }
        partSys.Play();

        yield return new WaitForSeconds( 0.2f );

		Destroy( gameObject );
	}
}
