using UnityEngine;
using System.Collections;

public class Turret_Healer : GenericStructureScript {

	private bool needScript = true;
	private bool fired = false;
	private bool CloserTarget = false;
	private GenericStructureScript targetAllyScript;
	
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
                if( ( target == null ) || ( CloserTarget == true ) )
                {
                    inRange = false;
                    needScript = true;
                    target = AcquireTarget();
                    CloserTarget = false;
                }
                CheckInRange();
                if( target != null )
                {
                    targetLocation = target.transform.position;
                    if( needScript )
                    {
                        targetAllyScript = target.GetComponent<GenericStructureScript>();
                        needScript = false;
                    }
                    CloserTarget = CheckForCloserTarget();
                }
            }

            // handle health bars
            if( health < maxHealth )
            {
                if( !healthBarBackMade )
                {
                    healthBarBackObj = (GameObject)Instantiate( healthBarBackPreFab, new Vector3( transform.position.x, transform.position.y + 0.2f, transform.position.z - 0.5f ), Quaternion.identity );

                    healthBarBackMade = true;
                }
                if( !healthBarFrontMade )
                {
                    healthBarFrontObj = (GameObject)Instantiate( healthBarFrontPreFab, new Vector3( transform.position.x, transform.position.y + 0.2f, transform.position.z - 1.0f ), Quaternion.identity );
                    healthBarFrontMade = true;
                }

                if( healthBarBackObj )
                {
                    healthBarBackObj.transform.position = new Vector3( transform.position.x, transform.position.y + 0.2f, transform.position.z - 0.5f );
                }

                if( healthBarFrontObj )
                {
                    float healthPercent = (float)health / maxHealth;
                    healthBarFrontObj.transform.localScale = new Vector3( 0.2f * healthPercent, healthBarFrontObj.transform.localScale.y, healthBarFrontObj.transform.localScale.z );
                    healthBarFrontObj.transform.position = new Vector3( transform.position.x - 0.1f + ( 0.1f * healthPercent ), transform.position.y + 0.2f, transform.position.z - 1.0f );
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
		float beamLife = 1.0f;
		
		while( true )
		{
			if( built )
			{
				currentPower = powerManager.GetRepairStationPower( gameObject );
				
				if( inRange && ( target != null ) && !fired )
				{
					if( currentPower >= powerConsumption )
					{
						if( targetAllyScript.health < targetAllyScript.maxHealth )
						{
							if( targetAllyScript.built == true )
							{
								if( targetAllyScript.gameObject != gameObject )
								{
									powerManager.ConsumeRepairStationPower( gameObject, powerConsumption );
									targetAllyScript.HealHealth( damage );
									DrawBeam();
									fired = true;
								}
							}
						}
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
	
	
	protected override GameObject AcquireTarget()
	{
	
		// Set closest target to max health and range
		int weakest = 100;
		
		// get all structures, nodes, and turrets and choose the closest
		GameObject[] structures = GameObject.FindGameObjectsWithTag( "Structure" );
		GameObject[] nodes = GameObject.FindGameObjectsWithTag( "Node" );
		GameObject[] turrets = GameObject.FindGameObjectsWithTag( "Turret" );
		GameObject[] firstCollector = GameObject.FindGameObjectsWithTag( "FirstCollector" );
		int tempSize = structures.Length + nodes.Length + turrets.Length + firstCollector.Length;
		GameObject[] allTogether = new GameObject[tempSize];
		
		for( int i = 0; i < structures.Length; i++ )
		{
			allTogether[i] = structures[i];
		}
		for( int j = 0; j < nodes.Length; j++ )
		{
			allTogether[structures.Length + j] = nodes[j];
		}
		for( int k = 0; k < turrets.Length; k++ )
		{
			allTogether[structures.Length + nodes.Length + k] = turrets[k];
		}
		for( int o = 0; o < firstCollector.Length; o ++ )
		{
			allTogether[structures.Length + nodes.Length + turrets.Length + o] = firstCollector[o];
		}
		
		GameObject tempTarget = null;
		
		// Loop through all the structures, nodes and turrets
		for( int t = 0; t < tempSize; t++ )
		{
			GenericStructureScript tempHealth = allTogether[t].GetComponent<GenericStructureScript>();
			int victimHealth = tempHealth.health;
			bool isBuilt = tempHealth.built;
			float distance = Vector3.Distance( transform.position, tempHealth.transform.position );
			
			if( ( victimHealth < weakest ) && ( distance < range ) && isBuilt )
			{
				weakest = victimHealth;
				tempTarget = allTogether[t];
			}
		}
		return tempTarget;
	}
	
	private bool CheckForCloserTarget()
	{
		bool yaynay = false;
		
		// Set closest target to ~infinity
		int weakest = targetAllyScript.health;
		
		// get all structures, nodes, and turrets and choose the closest
		GameObject[] structures = GameObject.FindGameObjectsWithTag( "Structure" );
		GameObject[] nodes = GameObject.FindGameObjectsWithTag( "Node" );
		GameObject[] turrets = GameObject.FindGameObjectsWithTag( "Turret" );
		GameObject[] firstCollector = GameObject.FindGameObjectsWithTag( "FirstCollector" );
		int tempSize = structures.Length + nodes.Length + turrets.Length + firstCollector.Length;
		GameObject[] allTogether = new GameObject[tempSize];
		
		for( int i = 0; i < structures.Length; i++ )
		{
			allTogether[i] = structures[i];
		}
		for( int j = 0; j < nodes.Length; j++ )
		{
			allTogether[structures.Length + j] = nodes[j];
		}
		for( int k = 0; k < turrets.Length; k++ )
		{
			allTogether[structures.Length + nodes.Length + k] = turrets[k];
		}
		for( int o = 0; o < firstCollector.Length; o ++ )
		{
			allTogether[structures.Length + nodes.Length + turrets.Length + o] = firstCollector[o];
		}
		
		// Loop through all the structures, nodes and turrets and check if any are in worse condition than the current target and make sure they're in range
		for( int t = 0; t < tempSize; t++ )
		{
			GenericStructureScript tempHealth = allTogether[t].GetComponent<GenericStructureScript>();
			int victimHealth = tempHealth.health;
			float distance = Vector3.Distance( transform.position, tempHealth.transform.position );
			
			if( ( victimHealth < weakest ) && ( distance < range ) )
			{
				yaynay = true;
				break;
			}
		}
		
		return yaynay;
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
	
	public override void StructureFunction()
	{
	}
}
