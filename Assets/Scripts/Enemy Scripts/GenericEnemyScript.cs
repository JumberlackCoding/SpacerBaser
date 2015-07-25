using UnityEngine;
using System.Collections;

public class GenericEnemyScript : MonoBehaviour {

	//================FUNCTIONS=================
    [SerializeField]
	protected int maxHealth;
    [SerializeField]
    protected int _health;
    [SerializeField]
	protected int damage;
    [SerializeField]
	protected float range;
    [SerializeField]
    protected float movespeed;
    [SerializeField]
    protected float attackSpeed;
	
	public GameObject BeamPreFab;

    public ParticleSystem partSys;

    [SerializeField]
    protected GameObject healthBarBackPreFab;
    protected GameObject healthBarBackObj;
    [SerializeField]
    protected GameObject healthBarFrontPreFab;
    protected GameObject healthBarFrontObj;

	protected GameObject target = null;
	protected GameObject prevTarget = null;
	protected GameObject beamObj;
	
	protected Vector3 targetLocation;
	
	protected GenericStructureScript targetScript;
	
	protected float startTime;
	protected float rangeFromTarget;
	protected float totalDistance;
    protected float checkTimeDelay;
	
	protected bool inRange = false;
	protected bool haveTarget = false;
    protected bool startedAttacking = false;
    protected bool called = true;
    protected bool healthBarFrontMade = false;
    protected bool healthBarBackMade = false;


    public int health
    {
        // called when retrieving value
        get
        {
            return _health;
        }

        // called when setting value
        set
        {
            _health = value;
            // check if dead
            if( value <= 0 )
            {
                _health = 0;
                Die();
            }
        }
    }


	//================FUNCTIONS=================

    protected virtual IEnumerator CheckCloserTarget()
    {
        while( true )
        {
            yield return new WaitForSeconds( checkTimeDelay );

            target = AcquireTarget();
            if( target != prevTarget )
            {
                called = true;
            }
        }
    }

	protected virtual IEnumerator PulseAction()
	{
		float beamLife = 0.2f;
		
		while( true )
		{
			yield return new WaitForSeconds( attackSpeed );
			
			
			
			yield return new WaitForSeconds( beamLife );
			
			
			
		}
	}

	protected virtual void MoveToTarget()
	{
		rangeFromTarget = Vector3.Distance( transform.position, targetLocation );
		
		if( rangeFromTarget > range )
		{
//			inRange = false;
            float distanceCovered = ( Time.time - startTime ) * movespeed;
            transform.position = Vector2.Lerp( transform.position, targetLocation, ( movespeed * Time.deltaTime ) / ( totalDistance - distanceCovered ) );

            float angle = Mathf.Atan2( targetLocation.y - transform.position.y, targetLocation.x - transform.position.x ) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis( angle, Vector3.forward );

            //iTween.MoveTo( gameObject, iTween.Hash( "position", targetLocation, "speed", movespeed / , "delay", 0.1f ) );
		}

		else
		{
			inRange = true;
		}
	}
	
	protected virtual GameObject AcquireTarget()
	{
		// Set closest target to ~infinity
		float closest = 100000000;
		
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
			Vector2 tempLoc = allTogether[t].transform.position;
			float distance = Vector2.Distance( transform.position, tempLoc );
			GenericStructureScript tempScript = allTogether[t].GetComponent<GenericStructureScript>();
			
			if ( ( distance < closest ) && ( tempScript.placed ) )
			{
				closest = distance;
				tempTarget = allTogether[t];
			}
		}
		return tempTarget;
	}
	
	protected virtual void AttackTarget()
	{
			
		if (inRange == true)
		{
			// Show attack animation 
			// Remove "damage" from the "target" GameObjects health
		}
	}
	
	public virtual void TakeDamage( int damage )
	{
		health = health - damage;
	}
	
	public void DrawBeam()
	{
		if( ( target ) && ( health > 0 ) )
		{
			CircleCollider2D targetCol = target.GetComponent<CircleCollider2D>();
			beamObj = null;
			Vector3 posInsideAsteroid = new Vector3( targetCol.transform.position.x + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.x, targetCol.transform.position.y + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.y, -.3f );
			Vector3 pos = new Vector3( ( posInsideAsteroid.x + transform.position.x ) / 2, ( posInsideAsteroid.y + transform.position.y ) / 2, 4f );
			Vector3 temp = posInsideAsteroid - transform.position;
			temp.z = 0;
			beamObj = (GameObject)Instantiate( BeamPreFab, pos, Quaternion.identity );
			beamObj.transform.rotation = Quaternion.FromToRotation( new Vector3( Vector3.up.x, Vector3.up.y, Vector3.up.z ), temp );
			beamObj.transform.localScale = new Vector3( beamObj.transform.localScale.x, temp.magnitude, beamObj.transform.localScale.z );
		}
	}
	
	public void DestroyBeam()
	{
		if( beamObj != null )
		{
			Destroy( beamObj.gameObject );
		}
	}

    public virtual void Die()
	{
        partSys.Play();
        DestroyBeam();
        if( healthBarBackObj )
        {
            Destroy( healthBarBackObj, 0.1f );
        }
        if( healthBarFrontObj )
        {
            Destroy( healthBarFrontObj, 0.1f );
        }

        Destroy( gameObject, 0.2f );
	}
	
}






















