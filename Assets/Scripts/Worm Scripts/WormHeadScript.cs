using UnityEngine;
using System.Collections;

public class WormHeadScript : MonoBehaviour {
	
	public float wormFeedSpeed;
	public float moveSpeed;
	public float willingTravelDistance;
	
	public int maxHealth;
	public int mineralConsumption;

    public GameObject wormPart2;
    public GameObject wormPart3;
    public GameObject wormPart4;
    public GameObject headPoint;
    public GameObject feedPoint;

    public ParticleSystem rockParticleEmitter;

    public LayerMask asteroidMask;
	
	private int health;
	
	public float rangeFromTarget;
    private float totalDistance;
    private float startTime;
    public float range;

    public GameObject WurmPreFab;
	
	private GameObject targetAsteroid;

    private AsteroidScript targetScript;
	
	private Vector3 targetLocation;
    private Vector3 p1StartPos;

    private int minsAte;

    [SerializeField]
    private int reproduceAmnt;

	private bool begunFeeding = false;
	private bool haveTarget = false;
    private bool targetSet = false;
    private bool inRange = false;
    private bool called = true;
	
	
	// Use this for initialization
	void Start () {
        health = maxHealth;
        //targetAsteroid = AcquireTarget();
        called = true;
        minsAte = 0;
        gameObject.name = "WURMHEAD (Clone)";
	}
	
	// Update is called once per frame
	void Update () {
        if( !haveTarget )
        {
            targetAsteroid = AcquireTarget();
            called = true;
        }

        RaycastHit2D temp = Physics2D.Linecast( headPoint.transform.position, feedPoint.transform.position, asteroidMask );
        if( temp.collider != null )
        {
            if( temp.collider.gameObject == targetAsteroid )
            {
                inRange = true;
            }
            else
            {
                inRange = false;
            }
        }

        if( targetAsteroid != null )
        {
            if( called )
            {
                haveTarget = true;
                targetLocation = targetAsteroid.transform.position;
                startTime = Time.time;
                totalDistance = Vector2.Distance( transform.position, targetLocation );
                targetScript = targetAsteroid.GetComponent<AsteroidScript>();
                range = targetAsteroid.transform.localScale.x;
                called = false;
                targetSet = true;
                p1StartPos = transform.position;
            }
        }
        if( targetScript != null )
        {
            if( targetScript.currentMins <= targetScript.startingMins / 2 )
            {
                targetAsteroid = AcquireTarget();
                targetSet = false;
                called = true;
            }
        }
        if( targetSet )
        {
            if( inRange == false )
            {
                MoveToTarget();
            }
            if( ( inRange == true ) && ( !begunFeeding ) )
            {
                begunFeeding = true;
                StartCoroutine( Feed() );
            }
        }

        if( minsAte >= reproduceAmnt )
        {
            minsAte = 0;
            Instantiate( WurmPreFab, transform.position, Quaternion.identity );
        }

        if( health <= 0 )
        {
            Die();
        }
	}
	
	protected virtual IEnumerator Feed()
    {
        targetScript.currentMins -= mineralConsumption;
        minsAte += mineralConsumption;
        rockParticleEmitter.Play();
		yield return new WaitForSeconds( wormFeedSpeed );
        begunFeeding = false;
    }
    
	protected virtual void MoveToTarget()
	{
		if( !inRange )
		{
            float distanceCovered = ( Time.time - startTime ) * moveSpeed;
            float distanceMoved = distanceCovered / totalDistance;

            transform.position = Vector2.Lerp( p1StartPos, targetLocation, distanceMoved );
            transform.position = new Vector3( transform.position.x, transform.position.y, -0.5f );
            
            float angleRad = Mathf.Atan2( targetLocation.y - transform.position.y, targetLocation.x - transform.position.x );
            float angleDeg = angleRad * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis( angleDeg, Vector3.forward );
        }
    }
    
	protected virtual GameObject AcquireTarget()
	{
        totalDistance = 0f;
        haveTarget = false;
        inRange = false;

		// get all astgeroids
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag( "Asteroid" );
		GameObject[] asteroids2 = new GameObject[asteroids.Length];
		
		GameObject tempTarget = null;
		
		bool targetsValid = false;
		
		// Loop through all asteroids and check if the asteroid is within the distance willing to travel
        int count = 0;
		for( int t = 0; t < asteroids.Length; t++ )
		{
			Vector3 tempLoc = asteroids[t].transform.position;
			float distance = Vector3.Distance( transform.position, tempLoc );
			AsteroidScript tempScript = asteroids[t].GetComponent<AsteroidScript>();
            
            if ( ( distance < willingTravelDistance ) && ( tempScript.currentMins > tempScript.startingMins / 2 ) )
            {
                asteroids2[t] = asteroids[t];
                targetsValid = true;
                count++;
            }
        }

        if( targetsValid )
        {
            GameObject[] astTemp = new GameObject[count];
            int iterator = 0;
            for( int i = 0; i < asteroids2.Length; i++ )
            {
                if( asteroids2[i] != null )
                {
                    astTemp[iterator] = asteroids2[i];
                    iterator++;
                }
            }

            // now pick a random one to go to
            tempTarget = astTemp[(int)Random.Range( 0, 99999 ) % count];
            haveTarget = true;
        }

        if( tempTarget == null )
        {
            float closest = 99999f;
            for( int t = 0; t < asteroids.Length; t++ )
            {
                Vector3 tempLoc = asteroids[t].transform.position;
                float distance = Vector2.Distance( transform.position, tempLoc );
                AsteroidScript tempScript = asteroids[t].GetComponent<AsteroidScript>();

                if( ( distance < closest ) && ( tempScript.currentMins > tempScript.startingMins / 2 ) )
                {
                    closest = distance;
                    tempTarget = asteroids[t];
                }
            }
        }
        return tempTarget;
    }
    
	public virtual void TakeDamage( int damage )
	{
		health -= damage;
    }
    
	public virtual void Die()
	{
		Destroy( gameObject );
    }
}
