﻿using UnityEngine;
using System.Collections;

public class Enemy_Basic : GenericEnemyScript {

    public bool dead = false;

	private bool called = true;
	private bool fired = false;

	// Use this for initialization
	void Start() {
        
	}
	
	// Update is called once per frame
	protected void Update() {
		if( target == null )
		{
			targetLocation = transform.position;
			totalDistance = 0f;
			haveTarget = false;
			inRange = false;
			startedAttacking = false;
			called = true;
		}
		target = AcquireTarget();
		if( prevTarget != target )
		{
			prevTarget = target;
			called = true;
		}
		if( target != null )
		{
			if( called )
			{
				haveTarget = true;
				targetLocation = target.transform.position;
				startTime = Time.time;
				totalDistance = Vector2.Distance( transform.position, targetLocation );
				targetScript = target.GetComponent<GenericStructureScript>();
				called = false;
			}
		}
		if( haveTarget )
		{
			float distanceFromTarget = Vector2.Distance( transform.position, targetLocation );
			if( distanceFromTarget > range )
			{
				inRange = false;
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

        // check if dead
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
