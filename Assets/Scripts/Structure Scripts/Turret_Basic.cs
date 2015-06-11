using UnityEngine;
using System.Collections;

public class Turret_Basic : GenericStructureScript {

	private bool needScript = true;
	private bool fired = false;
	private bool closerTarget = false;
    private WormHeadScript targetWormScript;

	// Use this for initialization
	void Start () {
		Initialize();
		StartCoroutine( PulseAction() );
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
                if( ( target == null ) || ( closerTarget == true ) )
                {
                    inRange = false;
                    needScript = true;
                    target = AcquireTarget();
                    closerTarget = false;
                }
                CheckInRange();
                if( target != null )
                {
                    targetLocation = target.transform.position;
                    if( needScript )
                    {
                        if( ( target.name == "WURMHEAD(Clone)" ) || ( target.name == "WURMHEAD (Clone)" ) )
                        {
                            targetWormScript = target.GetComponent<WormHeadScript>();
                        }
                        else
                        {
                            targetScript = target.GetComponent<GenericEnemyScript>();
                        }
                        needScript = false;
                    }
                    closerTarget = CheckForCloserTarget();
                }
            }

            // handle health bars
            if( health < maxHealth )
            {
                if( !healthBarBackMade )
                {
                    healthBarBackObj = (GameObject)Instantiate( healthBarBackPreFab, new Vector3( transform.position.x, transform.position.y + 0.15f, transform.position.z - 0.5f ), Quaternion.identity );

                    healthBarBackMade = true;
                }
                if( !healthBarFrontMade )
                {
                    healthBarFrontObj = (GameObject)Instantiate( healthBarFrontPreFab, new Vector3( transform.position.x, transform.position.y + 0.15f, transform.position.z - 1.0f ), Quaternion.identity );
                    healthBarFrontMade = true;
                }

                if( healthBarBackObj )
                {
                    healthBarBackObj.transform.position = new Vector3( transform.position.x, transform.position.y + 0.15f, transform.position.z - 0.5f );
                }

                if( healthBarFrontObj )
                {
                    float healthPercent = (float)health / maxHealth;
                    healthBarFrontObj.transform.localScale = new Vector3( 0.2f * healthPercent, healthBarFrontObj.transform.localScale.y, healthBarFrontObj.transform.localScale.z );
                    healthBarFrontObj.transform.position = new Vector3( transform.position.x - 0.1f + ( 0.1f * healthPercent ), transform.position.y + 0.15f, transform.position.z - 1.0f );
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
        }
	}

	protected override IEnumerator PulseAction()
	{
		float beamLife = 0.2f;
		
		while( true )
		{
			if( built )
			{
				currentPower = powerManager.GetTurretPower( gameObject );
				
				if( inRange && ( target != null ) && !fired )
				{
					if( currentPower >= powerConsumption )
					{
						powerManager.ConsumeTurretPower( gameObject, powerConsumption );
                        if( targetScript )
                        {
                            targetScript.TakeDamage( damage );
                        }
                        else if( targetWormScript )
                        {
                            targetWormScript.TakeDamage( damage );
                        }
						DrawBeam();
						fired = true;
					}
				}
				
				yield return new WaitForSeconds( beamLife );
				
				if( beamObj && fired )
				{
					DestroyBeam();
					fired = false;
				}
				
				yield return new WaitForSeconds( attackSpeed - beamLife );
			}
			else
			{
				yield return new WaitForSeconds( 0.1f );
			}
		}
	}
	
	private bool CheckForCloserTarget()
	{
		bool yaynay = false;
		
		// Set closest distance to current target's position
		float closest = Vector3.Distance( transform.position, targetLocation );
		
		// Get all enemies
		GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        GameObject[] wurms = GameObject.FindGameObjectsWithTag( "Wurm" );
		
		// Loop through all the enemies
		for( int t = 0; t < enemies.Length; t++ )
		{
			Vector3 tempLoc = enemies[t].transform.position;
			float distance = Vector3.Distance( transform.position, tempLoc );
			
			if ( distance < closest )
			{
				yaynay = true;
				break;
			}
		}
        if( yaynay == false )
        {
            for( int t = 0; t < wurms.Length; t++ )
            {
                Vector3 tempLoc = wurms[t].transform.position;
                float distance = Vector3.Distance( transform.position, tempLoc );

                if( distance < closest )
                {
                    yaynay = true;
                    break;
                }
            }
        }
		
		return yaynay;
	}

    public override void DrawBeam()
    {
        if( target )
        {
            CircleCollider2D targetCol = target.GetComponent<CircleCollider2D>();
            beamObj = null;
            Vector3 posInsideAsteroid = new Vector3( targetCol.transform.position.x + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.x, targetCol.transform.position.y + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.y, 4f );
            Vector3 pos = new Vector3( ( posInsideAsteroid.x + transform.position.x ) / 2, ( posInsideAsteroid.y + transform.position.y ) / 2, 4f );
            Vector3 temp = posInsideAsteroid - transform.position;
            temp.z = 0;
            beamObj = (GameObject)Instantiate( BeamPreFab, pos, Quaternion.identity );

            beamObj.transform.rotation = Quaternion.FromToRotation( new Vector3( Vector3.up.x, Vector3.up.y, Vector3.up.z ), temp );
            beamObj.transform.localScale = new Vector3( beamObj.transform.localScale.x, temp.magnitude, beamObj.transform.localScale.z );

            TurretBeamScript beamScript = beamObj.GetComponent<TurretBeamScript>();
            beamScript.SetTargSource( target, transform );
        }
    }

	public bool AbleToAttack()
	{
		bool result = false;
		if( inRange && ( target != null ) )
		{
			result = true;
		}
		
		return result;
	}
}
