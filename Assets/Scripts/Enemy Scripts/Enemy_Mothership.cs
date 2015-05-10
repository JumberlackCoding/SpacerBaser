﻿using UnityEngine;
using System.Collections;

public class Enemy_Mothership : GenericEnemyScript {

    public GameObject SapperPreFab;
    public Sprite[] sprites;
    public GameObject[] DockingPoints;
    public GameObject SpawnPoint1;
    public GameObject SpawnPoint2;

    public int maxPowerStored;
    public int maxSappers;

    public float sapperChargeDelay;

    private SpriteRenderer spriteManager;

    private int powerStored;
    private int sapperCount;
    private int sapperCharges;
    private int sappersLaunched;

    private bool called = true;

    void Awake()
    {
        spriteManager = GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
        powerStored = 0;
        sapperCount = 0;
        sapperCharges = 0;
        sappersLaunched = 0;
        StartCoroutine( GetCharges() );
	}
	
	// Update is called once per frame
	void Update () {

        if( powerStored > maxPowerStored )
        {
            powerStored = maxPowerStored;
        }
        if( powerStored < 0 )
        {
            powerStored = 0;
        }

        // change sprite based on power stored
	    if( powerStored == maxPowerStored )
        {
            spriteManager.sprite = sprites[5];
        }
        else if( powerStored >= maxPowerStored * 4 / 5 )
        {
            spriteManager.sprite = sprites[4];
        }
        else if( powerStored >= maxPowerStored * 3 / 5 )
        {
            spriteManager.sprite = sprites[3];
        }
        else if( powerStored >= maxPowerStored * 2 / 5 )
        {
            spriteManager.sprite = sprites[2];
        }
        else if( powerStored >= maxPowerStored * 1 / 5 )
        {
            spriteManager.sprite = sprites[1];
        }
        else if( powerStored >= 0 )
        {
            spriteManager.sprite = sprites[0];
        }


        if( ( sapperCount < maxSappers ) && ( sapperCharges > 0 ) )
        {
            sapperCharges--;
            sapperCount++;

            Vector3 spawnpos;
            
            // alternate sides that launch sappers
            if( sappersLaunched % 2 == 0 )
            {
                spawnpos = SpawnPoint1.transform.position;
            }
            else
            {
                spawnpos = SpawnPoint2.transform.position;
            }
            Instantiate( SapperPreFab, spawnpos, transform.rotation );
            sappersLaunched++;
        }


        // usual enemy behaviour
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

    public void DropOffPower( int amount )
    {
        powerStored += amount;
    }

    IEnumerator GetCharges()
    {
        while( true )
        {
            yield return new WaitForSeconds( sapperChargeDelay );

            if( sapperCharges < maxSappers )
            {
                sapperCharges++;
            }
        }
    }

    public override IEnumerator Die()
    {
        partSys.Play();

        yield return new WaitForSeconds( 3f );

        Destroy( gameObject );
    }

}
