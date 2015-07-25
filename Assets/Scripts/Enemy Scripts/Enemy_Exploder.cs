using UnityEngine;
using System.Collections;

public class Enemy_Exploder : GenericEnemyScript {

	// Use this for initialization
	void Start() {
        StartCoroutine( CheckCloserTarget() );
	}
		
	// Update is called once per frame
	protected void Update() {
        if( target == null )
        {
            target = AcquireTarget();

            if( target == null )
            {
                targetLocation = transform.position;
                totalDistance = 0f;
                haveTarget = false;
                inRange = false;
            }
            else
            {
                called = true;
            }
        }
        if( target != null )
        {
			if( called )
			{
                haveTarget = true;
                targetLocation = target.transform.position;
                startTime = Time.time;
                totalDistance = Vector3.Distance( transform.position, targetLocation );
                targetScript = target.GetComponent<GenericStructureScript>();
				called = false;
            }
        }
        if( haveTarget )
        {
			float distanceFromTarget = Vector3.Distance( transform.position, targetLocation );
			if( distanceFromTarget > range )
			{
				inRange = false;
                checkTimeDelay = Random.Range( 6.8f, 7.21f );
			}
            else
            {
                checkTimeDelay = Random.Range( 1.8f, 2.21f );
            }
            if( inRange == false )
            {
                MoveToTarget();
            }
            if( inRange == true )
            {
                AttackTarget();
            }
        }

        // handle health bars
        if( health < maxHealth )
        {
            if( !healthBarBackMade )
            {
                healthBarBackObj = (GameObject)Instantiate( healthBarBackPreFab, new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z - 0.5f ), Quaternion.identity );

                healthBarBackMade = true;
            }
            if( !healthBarFrontMade )
            {
                healthBarFrontObj = (GameObject)Instantiate( healthBarFrontPreFab, new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z - 1.0f ), Quaternion.identity );
                healthBarFrontMade = true;
            }

            if( healthBarBackObj )
            {
                healthBarBackObj.transform.position = new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z - 0.5f );
            }

            if( healthBarFrontObj )
            {
                float healthPercent = (float)health / maxHealth;
                healthBarFrontObj.transform.localScale = new Vector3( 0.2f * healthPercent, healthBarFrontObj.transform.localScale.y, healthBarFrontObj.transform.localScale.z );
                healthBarFrontObj.transform.position = new Vector3( transform.position.x - 0.1f + ( 0.1f * healthPercent ), transform.position.y + 0.1f, transform.position.z - 1.0f );
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
	
	protected override void AttackTarget ()
	{
		// deal damage
		targetScript.TakeDamage( damage );
        Die();
	}
}
