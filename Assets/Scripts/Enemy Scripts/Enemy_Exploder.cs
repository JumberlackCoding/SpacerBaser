﻿using UnityEngine;
using System.Collections;

public class Enemy_Exploder : GenericEnemyScript {

	bool called;

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
        if( health <= 0 )
        {
            StartCoroutine( Die() );
        }
	}
	
	protected override void AttackTarget ()
	{
		// deal damage
		targetScript.TakeDamage( damage );
        StartCoroutine( Die() );
	}
}
