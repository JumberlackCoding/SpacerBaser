using UnityEngine;
using System.Collections;

public class Enemy_Swarmer : GenericEnemyScript {

	private bool fired = false;
	
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
			if( ( inRange == true ) && ( startedAttacking == false ) )
			{
				startedAttacking = true;
				AttackTarget();
			}
		}
		if( health <= 0 )
		{
            StartCoroutine( Die() );
		}
	}
	
	protected override IEnumerator PulseAction()
	{
		float beamLife = 0.2f;
		
		if( haveTarget && inRange && startedAttacking )
		{
			if( !fired )
			{
				targetScript.TakeDamage( damage );
				DrawBeam();
				fired = true;
			}
		}
		
		yield return new WaitForSeconds( beamLife );
		if( fired )
		{
			DestroyBeam();
			fired = false;
		}
		
		yield return new WaitForSeconds( attackSpeed - beamLife );
		startedAttacking = false;
	}
	
	protected override void AttackTarget ()
	{
		StartCoroutine( PulseAction() );	
	}
	
	public override IEnumerator Die()
	{
		if( beamObj )
		{
			DestroyBeam();
		}
        partSys.Play();

        yield return new WaitForSeconds( 0.2f );

        Destroy( gameObject );
	}
}
